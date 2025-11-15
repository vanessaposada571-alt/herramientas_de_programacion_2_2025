using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Hospital.domain.model;
using Hospital.domain.services;

namespace Hospital.application.usecases
{
    /// <summary>
    /// UseCase para que el personal de enfermería registre visitas:
    /// - Signos vitales: presión arterial, temperatura, pulso, saturación de O2.
    /// - Medicamentos administrados, procedimientos realizados, pruebas aplicadas.
    /// - Observaciones.
    /// </summary>
    public class NurseVisitUseCase
    {
        private readonly C_Visit _createVisit;
        private readonly S_OrderMedicine _selectMedicine;
        private readonly S_OrderProcedure _selectProcedure;
        private readonly S_OrderHelp _selectHelp;

        public NurseVisitUseCase(
            C_Visit createVisit,
            S_OrderMedicine selectMedicine,
            S_OrderProcedure selectProcedure,
            S_OrderHelp selectHelp)
        {
            _createVisit = createVisit ?? throw new ArgumentNullException(nameof(createVisit));
            _selectMedicine = selectMedicine ?? throw new ArgumentNullException(nameof(selectMedicine));
            _selectProcedure = selectProcedure ?? throw new ArgumentNullException(nameof(selectProcedure));
            _selectHelp = selectHelp ?? throw new ArgumentNullException(nameof(selectHelp));
        }

        /// <summary>
        /// Registra una visita realizada por la enfermera.
        /// Se asume que el parámetro <paramref name="visit"/> contiene los signos vitales en sus propiedades
        /// y que las colecciones de medicamentos/procedimientos/pruebas administradas están provistas en los parámetros.
        /// </summary>
        public void RegisterVisit(
            Visit visit,
            IEnumerable<Order_medicine>? administeredMedicines = null,
            IEnumerable<Order_procedure>? performedProcedures = null,
            IEnumerable<Order_help>? performedTests = null,
            string? observations = null)
        {
            if (visit == null) throw new ArgumentNullException(nameof(visit));

            // Validar signos vitales (si están presentes en el objeto Visit)
            ValidateVitals(visit);

            // Verificar que las órdenes referenciadas por medicamentos/procedimientos/pruebas existan
            if (administeredMedicines != null)
            {
                foreach (var med in administeredMedicines)
                {
                    if (med == null) throw new ArgumentException("Medicamento administrado inválido.");
                    var exists = _selectMedicine.FindByNumOrder(med);
                    if (exists == null)
                        throw new InvalidOperationException($"La orden de medicina (NumOrder={med.NumOrder}) no existe.");
                }
            }

            if (performedProcedures != null)
            {
                foreach (var proc in performedProcedures)
                {
                    if (proc == null) throw new ArgumentException("Procedimiento realizado inválido.");
                    var exists = _selectProcedure.FindByNumOrder(proc);
                    if (exists == null)
                        throw new InvalidOperationException($"La orden de procedimiento (NumOrderP={proc.NumOrderP}) no existe.");
                }
            }

            if (performedTests != null)
            {
                foreach (var test in performedTests)
                {
                    if (test == null) throw new ArgumentException("Prueba realizada inválida.");
                    var exists = _selectHelp.FindByNumOrder(test);
                    if (exists == null)
                        throw new InvalidOperationException($"La orden de ayuda diagnóstica (NumOrderA={test.NumOrderA}) no existe.");
                }
            }

            // Agregar observaciones si fue provista (se asume que Visit tiene algún campo para comentarios; si no, el adaptador/port puede gestionar)
            if (!string.IsNullOrWhiteSpace(observations))
            {
                // Intenta asignar a una propiedad comúnmente usada; si la clase Visit no tiene, esto no compilará y habrá que mapear según modelo real.
                try
                {
                    var obsProp = visit.GetType().GetProperty("Observations");
                    if (obsProp != null && obsProp.CanWrite)
                        obsProp.SetValue(visit, observations);
                }
                catch
                {
                    // Si no existe la propiedad, ignorar: la información puede guardarse en otro lado por el adaptador.
                }
            }

            // En el modelo de dominio C_Visit se encargará de persistir/validar relación con orden/paciente/doctor.
            _createVisit.Create(visit);
        }

        /// <summary>
        /// Valida los signos vitales contenidos en <paramref name="visit"/>.
        /// Se esperan (nombres de propiedad comunes): Pressure (string "SYS/DIA"), Temperature (decimal/double), Pulse (int),
        /// Oxygen (int). Si alguna no existe se omite la validación específica.
        /// </summary>
        private void ValidateVitals(Visit visit)
        {
            // PRESIÓN ARTERIAL: formato "S/D" con números (ej. "120/80")
            var pressureProp = visit.GetType().GetProperty("Pressure");
            if (pressureProp != null)
            {
                var pressureVal = pressureProp.GetValue(visit) as string;
                if (string.IsNullOrWhiteSpace(pressureVal) || !Regex.IsMatch(pressureVal.Trim(), @"^\d{2,3}/\d{2,3}$"))
                    throw new ArgumentException("Presión arterial inválida. Formato esperado: 'SYS/DIA' (ej. 120/80).");
            }

            // TEMPERATURA: valor numérico en grados Celsius (rango razonable 30.0 - 45.0)
            var tempProp = visit.GetType().GetProperty("Temperature");
            if (tempProp != null)
            {
                var tempObj = tempProp.GetValue(visit);
                if (tempObj != null && decimal.TryParse(tempObj.ToString(), out var temp))
                {
                    if (temp < 30m || temp > 45m)
                        throw new ArgumentException("Temperatura fuera de rango razonable (30.0 - 45.0 °C).");
                }
            }

            // PULSO: entero (rango razonable 30 - 220)
            var pulseProp = visit.GetType().GetProperty("Pulse");
            if (pulseProp != null)
            {
                var pulseObj = pulseProp.GetValue(visit);
                if (pulseObj != null && int.TryParse(pulseObj.ToString(), out var pulse))
                {
                    if (pulse < 30 || pulse > 220)
                        throw new ArgumentException("Pulso fuera de rango razonable (30 - 220 latidos por minuto).");
                }
            }

            // OXÍGENO: entero porcentaje 0 - 100
            var oxProp = visit.GetType().GetProperty("Oxygen");
            if (oxProp != null)
            {
                var oxObj = oxProp.GetValue(visit);
                if (oxObj != null && int.TryParse(oxObj.ToString(), out var ox))
                {
                    if (ox < 0 || ox > 100)
                        throw new ArgumentException("Saturación de oxígeno inválida. Debe estar entre 0 y 100.");
                }
            }
        }

        /// <summary>
        /// Genera un resumen imprimible de la visita con signos vitales y lista de items administrados.
        /// </summary>
        public string PrintVisitReport(Visit visit,
            IEnumerable<Order_medicine>? administeredMedicines = null,
            IEnumerable<Order_procedure>? performedProcedures = null,
            IEnumerable<Order_help>? performedTests = null,
            string? observations = null)
        {
            if (visit == null) throw new ArgumentNullException(nameof(visit));

            var sb = new StringBuilder();
            sb.AppendLine("REGISTRO DE VISITA");
            sb.AppendLine($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine();

            // Paciente / orden (intentar extraer propiedades comunes)
            string patientId = TryGetPropertyAsString(visit, "PatientId") ?? TryGetPropertyAsString(visit, "Id_patient") ?? string.Empty;
            string patientName = TryGetPropertyAsString(visit, "PatientName") ?? string.Empty;
            string visitNum = TryGetPropertyAsString(visit, "NumVisit") ?? TryGetPropertyAsString(visit, "NumOrder") ?? string.Empty;

            sb.AppendLine("=== Paciente / Orden ===");
            if (!string.IsNullOrEmpty(patientName)) sb.AppendLine($"Nombre: {patientName}");
            if (!string.IsNullOrEmpty(patientId)) sb.AppendLine($"Cédula: {patientId}");
            if (!string.IsNullOrEmpty(visitNum)) sb.AppendLine($"Orden/Visita: {visitNum}");
            sb.AppendLine();

            // Signos vitales
            sb.AppendLine("=== Signos vitales ===");
            sb.AppendLine($"Presión arterial: {TryGetPropertyAsString(visit, "Pressure") ?? "N/A"}");
            sb.AppendLine($"Temperatura: {TryGetPropertyAsString(visit, "Temperature") ?? "N/A"}");
            sb.AppendLine($"Pulso: {TryGetPropertyAsString(visit, "Pulse") ?? "N/A"}");
            sb.AppendLine($"Saturación O2: {TryGetPropertyAsString(visit, "Oxygen") ?? "N/A"}");
            sb.AppendLine();

            // Administrados
            sb.AppendLine("=== Medicamentos administrados ===");
            if (administeredMedicines != null && administeredMedicines.Any())
            {
                foreach (var m in administeredMedicines)
                    sb.AppendLine($"- Orden {m.NumOrder}, Medicamento ID {m.IdMedicine}, Dosis: {m.Dose}, Costo (item): {m.Item:C}");
            }
            else sb.AppendLine("Ninguno");

            sb.AppendLine();
            sb.AppendLine("=== Procedimientos realizados ===");
            if (performedProcedures != null && performedProcedures.Any())
            {
                foreach (var p in performedProcedures)
                    sb.AppendLine($"- Orden {p.NumOrderP}, Procedimiento ID {p.IdProcedure}, Monto: {p.Amount:C}");
            }
            else sb.AppendLine("Ninguno");

            sb.AppendLine();
            sb.AppendLine("=== Pruebas realizadas ===");
            if (performedTests != null && performedTests.Any())
            {
                foreach (var t in performedTests)
                    sb.AppendLine($"- Orden {t.NumOrderA}, Prueba ID {t.IdHelp}, Monto: {t.Amount:C}");
            }
            else sb.AppendLine("Ninguno");

            sb.AppendLine();
            sb.AppendLine("=== Observaciones ===");
            sb.AppendLine(string.IsNullOrWhiteSpace(observations) ? "Ninguna" : observations);

            return sb.ToString();
        }

        // Helper para leer propiedades por reflexión sin lanzar excepción
        private static string? TryGetPropertyAsString(object obj, string propName)
        {
            try
            {
                var p = obj.GetType().GetProperty(propName);
                if (p == null) return null;
                var v = p.GetValue(obj);
                return v?.ToString();
            }
            catch { return null; }
        }
    }
}