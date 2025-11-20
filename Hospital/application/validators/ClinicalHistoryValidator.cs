using System;
using System.Text.RegularExpressions;
using Hospital.domain.model;

namespace Hospital.application.validators
{                           
    public class ClinicalHistoryValidator
    {
        private static readonly Regex DoctorIdRegex = new Regex(@"^\d{1,10}$", RegexOptions.Compiled);

        /// <summary>
        /// Valida los campos mínimos de una entrada de historia clínica.
        /// Diagnosis puede ser vacía (por ejemplo, cuando se solicita una ayuda diagnóstica).
        /// </summary>
        public void ValidateCreateOrUpdate(Doctor doctor, Patient patient, ClinicalHistoryEntry entry)
        {
            if (doctor == null) throw new ArgumentNullException(nameof(doctor));
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            if (string.IsNullOrWhiteSpace(patient.Id_patient))
                throw new ArgumentException("El paciente debe tener una cédula válida (Id_patient).", nameof(patient));

            var docId = doctor.Id_doctor ?? entry.DoctorId ?? string.Empty;
            if (!DoctorIdRegex.IsMatch(docId))
                throw new ArgumentException("La cédula del médico debe contener sólo dígitos y máximo 10 caracteres.", nameof(doctor));

            // Fecha no puede ser futura (pequeña tolerancia)
            if (entry.AttentionDate > DateTime.Now.AddMinutes(5))
                throw new ArgumentException("La fecha de atención no puede ser una fecha futura.", nameof(entry.AttentionDate));

            if (string.IsNullOrWhiteSpace(entry.Reason))
                throw new ArgumentException("El motivo de la consulta no puede estar vacío.", nameof(entry.Reason));
            if (string.IsNullOrWhiteSpace(entry.Symptoms))
                throw new ArgumentException("La sintomatología no puede estar vacía.", nameof(entry.Symptoms));

            // Diagnosis: opcional (dejar validación adicional al flujo que maneja órdenes/resultados)
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
        }
    }
}