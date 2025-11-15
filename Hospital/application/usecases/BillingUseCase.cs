using System;
using System.Linq;
using System.Text;
using Hospital.domain.model;
using Hospital.domain.services;
using Hospital.application.validators;

namespace Hospital.application.usecases
{
    public class BillingUseCase
    {
        private readonly C_Billings _createBilling;
        private readonly PatientValidator _validator;

        public BillingUseCase(C_Billings createBilling, PatientValidator validator)
        {
            _createBilling = createBilling ?? throw new ArgumentNullException(nameof(createBilling));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// Genera y persiste una factura para el paciente y la orden proporcionados.
        /// Parámetro opcional previousCopayTotalThisYear permite que el adaptador/servicio
        /// entregue el total de copagos del año para aplicar la regla de exención anual.
        /// </summary>
        public Billings CreateBilling(Patient patient, Order order, long previousCopayTotalThisYear = 0L)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (order == null) throw new ArgumentNullException(nameof(order));

            // Validación básica del paciente (reutiliza el validador existente)
            var validationResult = _validator.Validate(patient);
            if (!validationResult.IsValid)
                throw new ArgumentException(validationResult.ErrorMessage);

            // Validar contacto de emergencia: se requiere al menos un contacto (según modelos sólo hay un Contact)
            var contact = patient.Contact;
            if (contact == null)
                throw new ArgumentException("El paciente debe tener un contacto de emergencia.");

            // Nombre de contacto: separar nombres y apellidos
            var fullName = contact.Name1?.Name ?? contact.Name1?.ToString() ?? string.Empty;
            var nameParts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var firstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
            var lastName = nameParts.Length > 1 ? string.Join(' ', nameParts.Skip(1)) : string.Empty;

            // Relación con el paciente
            var relationship = contact.Relation ?? string.Empty;
            if (string.IsNullOrWhiteSpace(relationship))
                throw new ArgumentException("La relación del contacto de emergencia con el paciente es obligatoria.");

            // Teléfono de emergencia: intentar obtener de Contact.Cellphone (Person.Cellphone -> long)
            string phoneStr = string.Empty;
            try
            {
                var phoneLong = contact.Cellphone?.Cellphone ?? 0L;
                phoneStr = phoneLong > 0 ? phoneLong.ToString() : string.Empty;
            }
            catch
            {
                phoneStr = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(phoneStr) || phoneStr.Length != 10 || !phoneStr.All(char.IsDigit))
                throw new ArgumentException("Número de teléfono de emergencia inválido. Debe contener exactamente 10 dígitos numéricos.");

            // Calcular totales de la orden con los campos disponibles en los modelos:
            // - Order_help.Amount (ayudas diagnósticas u otros)
            // - Order_procedure.Amount (procedimientos)
            // - Order_medicine.Item (si se usa como costo en este modelo)
            long total = 0L;
            if (order.NumOrderA1 != null) total += order.NumOrderA1.Amount;
            if (order.NumOrderP1 != null) total += order.NumOrderP1.Amount;
            if (order.NumOrder1 != null) total += order.NumOrder1.Item;

            // Información de seguro
            var insurance = patient.IdSure;
            bool hasActivePolicy = (insurance != null) && insurance.Policy_status;
            long copayApplied = 0L;
            long patientPays = 0L;
            long insurerPays = 0L;

            const long STANDARD_COPAY = 50_000L;
            const long YEARLY_COPAY_LIMIT = 1_000_000L;

            if (hasActivePolicy)
            {
                // Si el adaptador ya calcula previousCopayTotalThisYear puede pasarlo para evaluar la exención anual
                if (previousCopayTotalThisYear >= YEARLY_COPAY_LIMIT)
                {
                    copayApplied = 0L; // exento por alcanzar el tope
                }
                else
                {
                    copayApplied = STANDARD_COPAY;
                }

                patientPays = Math.Min(total, copayApplied);
                insurerPays = Math.Max(0L, total - patientPays);
            }
            else
            {
                // póliza inactiva o ausente -> paciente paga todo
                patientPays = total;
                insurerPays = 0L;
                copayApplied = 0L;
            }

            // Construir la entidad Billings reutilizando las propiedades del modelo existente
            var billings = new Billings
            {
                Id_patient1 = patient,
                Patient_name = patient,
                Order = order,
                Amount = total,
                Insurance_cost = insurance // si necesita otro campo, ajustar en adaptadores
            };

            // Asignar company/policy en el objeto Billings si se desea conservar la referencia
            if (insurance != null)
            {
                billings.Company_name = insurance;
                billings.Policy_number = insurance;
            }

            // Persistir usando el servicio de dominio creado (C_Billings)
            _createBilling.Create(billings);

            // Para uso posterior en impresión, se puede almacenar patientPays/insurerPays/copayApplied en Insurance_cost o campos adicionales.
            // Dado que el modelo actual no tiene campos explícitos para estos, se deja la lógica de presentación en PrintInvoice.

            return billings;
        }

        /// <summary>
        /// Genera una representación textual de la factura con los datos solicitados.
        /// Usa la entidad Billings y la información disponible en los modelos.
        /// </summary>
        public string PrintInvoice(Billings billings)
        {
            if (billings == null) throw new ArgumentNullException(nameof(billings));

            var sb = new StringBuilder();
            var patient = billings.Id_patient1 ?? billings.Patient_name ?? new Patient();

            // Nombre paciente (se intenta extraer desde Person.Name o Person.Name1)
            string patientFullName = patient.Name1?.Name ?? patient.Name ?? string.Empty;

            // Cédula
            string cedula = patient.Id_patient ?? string.Empty;

            // Edad (intentando extraer Birth1.Birth o Birth)
            DateTime birth = DateTime.MinValue;
            try
            {
                birth = patient.Birth1 != null ? patient.Birth1.Birth : patient.Birth;
            }
            catch { birth = patient.Birth; }
            int age = 0;
            if (birth != DateTime.MinValue)
            {
                var today = DateTime.Today;
                age = today.Year - birth.Year;
                if (birth > today.AddYears(-age)) age--;
            }

            // Contacto de emergencia
            var contact = patient.Contact;
            string ecFirstName = contact?.Name1?.Name?.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
            string ecLastName = string.Join(' ', (contact?.Name1?.Name?.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)) ?? Array.Empty<string>());
            string ecRelation = contact?.Relation ?? string.Empty;
            string ecPhone = string.Empty;
            try
            {
                ecPhone = contact?.Cellphone?.Cellphone > 0 ? contact.Cellphone.Cellphone.ToString() : string.Empty;
            }
            catch { ecPhone = string.Empty; }

            // Seguro
            var insurance = patient.IdSure;
            string insuranceCompany = insurance?.Company_name ?? string.Empty;
            string policyNumber = insurance?.Policy_number ?? string.Empty;
            DateTime policyEnd = insurance != null ? insurance.Effective_Date : DateTime.MinValue;
            int daysRemaining = policyEnd != DateTime.MinValue ? Math.Max(0, (policyEnd.Date - DateTime.Today).Days) : 0;
            string policyEndStr = policyEnd != DateTime.MinValue ? policyEnd.ToString("dd/MM/yyyy") : string.Empty;

            // Orden / Médico tratante
            string treatingDoctor = billings.Order?.NumOrderP1 != null ? "[procedimiento asociado]" : string.Empty;
            // (si el modelo tuviera nombre del médico, extraerlo aquí)

            // Totales
            long total = billings.Amount;

            sb.AppendLine($"FACTURA ID: {billings.Id_billings}");
            sb.AppendLine($"Fecha: {DateTime.Now:dd/MM/yyyy}");
            sb.AppendLine();
            sb.AppendLine("=== Datos del Paciente ===");
            sb.AppendLine($"Nombre: {patientFullName}");
            sb.AppendLine($"Edad: {age}");
            sb.AppendLine($"Cédula: {cedula}");
            sb.AppendLine();
            sb.AppendLine("=== Contacto de Emergencia ===");
            sb.AppendLine($"Nombre (Nombres): {ecFirstName}");
            sb.AppendLine($"Apellido(s): {ecLastName}");
            sb.AppendLine($"Relación: {ecRelation}");
            sb.AppendLine($"Teléfono de emergencia: {ecPhone}");
            sb.AppendLine();
            sb.AppendLine("=== Seguro Médico ===");
            sb.AppendLine($"Compañía: {insuranceCompany}");
            sb.AppendLine($"Número de póliza: {policyNumber}");
            sb.AppendLine($"Días de vigencia de la póliza: {daysRemaining}");
            sb.AppendLine($"Fecha de finalización de la póliza: {policyEndStr}");
            sb.AppendLine();
            sb.AppendLine("=== Información de Facturación ===");
            sb.AppendLine($"Médico tratante: {treatingDoctor}");
            sb.AppendLine($"Total servicios: {total:C}");
            sb.AppendLine();
            sb.AppendLine("=== Detalle clínico de órdenes ===");

            // Detalle extraído desde la orden (si existe)
            var ord = billings.Order;
            if (ord != null)
            {
                if (ord.NumOrder1 != null)
                    sb.AppendLine($"- Medicamento (ID/Item): {ord.NumOrder1.IdMedicine} - Costo estimado: {ord.NumOrder1.Item:C}");

                if (ord.NumOrderP1 != null)
                    sb.AppendLine($"- Procedimiento (ID): {ord.NumOrderP1.IdProcedure} - Costo: {ord.NumOrderP1.Amount:C}");

                if (ord.NumOrderA1 != null)
                    sb.AppendLine($"- Ayuda/Examen (ID): {ord.NumOrderA1.IdHelp} - Costo: {ord.NumOrderA1.Amount:C}");
            }

            return sb.ToString();
        }
    }
}