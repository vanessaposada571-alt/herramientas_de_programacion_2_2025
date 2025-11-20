using System;
using Hospital.domain.model;

namespace Hospital.application.validators
{
    /// <summary>
    /// Validaciones mínimas para la creación/actualización de entradas de historia clínica.
    /// Lanza ArgumentException si la validación falla.
    /// </summary>
    public class ClinicalHistoryValidator
    {
        public void ValidateCreateOrUpdate(Doctor doctor, Patient patient, DateTime attentionDate, string content)
        {
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (string.IsNullOrWhiteSpace(patient.Id_patient))
                throw new ArgumentException("El paciente debe tener una cédula válida (Id_patient).", nameof(patient));
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("El contenido de la historia clínica no puede estar vacío.", nameof(content));

            // Atención: no permitir fechas excesivamente en el futuro
            if (attentionDate > DateTime.Now.AddMinutes(5))
                throw new ArgumentException("La fecha de atención no puede ser una fecha futura.", nameof(attentionDate));

            // Validación ligera del doctor: si existe Id_doctor se asume válido; se pueden ampliar reglas.
            if (string.IsNullOrWhiteSpace(doctor.Id_doctor))
                throw new ArgumentException("El médico debe tener un identificador (Id_doctor).", nameof(doctor));
        }

        public void ValidatePatientKey(string patientId)
        {
            if (string.IsNullOrWhiteSpace(patientId))
                throw new ArgumentException("La cédula del paciente es obligatoria.", nameof(patientId));
        }

        public void ValidateDateKey(string dateKey)
        {
            if (string.IsNullOrWhiteSpace(dateKey))
                throw new ArgumentException("La subclave de fecha es obligatoria.", nameof(dateKey));
            // No estrictamente necesario parsear aquí, pero se recomienda usar un formato ISO consistente.
        }
    }
}