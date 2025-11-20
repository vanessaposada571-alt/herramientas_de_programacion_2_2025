using System;
using System.Collections.Generic;
using Hospital.application.ports;
using Hospital.application.validators;
using Hospital.domain.model;

namespace Hospital.application.usecases
{
    /// <summary>
    /// Casos de uso para que los médicos creen, actualicen y consulten la historia clínica.
    /// La persistencia se delega en IClinicalHistoryPort (NoSQL/dicciónario).
    /// </summary>
    public class ClinicalHistoryUseCase
    {
        private readonly IClinicalHistoryPort _historyPort;
        private readonly ClinicalHistoryValidator _validator;

        public ClinicalHistoryUseCase(IClinicalHistoryPort historyPort, ClinicalHistoryValidator validator)
        {
            _historyPort = historyPort ?? throw new ArgumentNullException(nameof(historyPort));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// Crea una entrada de historia clínica para un paciente en la fecha de atención indicada.
        /// La subclave de fecha se recomienda en formato ISO: yyyy-MM-ddTHH:mm:ss (o similar).
        /// </summary>
        public void CreateEntry(Doctor doctor, Patient patient, DateTime attentionDate, string content)
        {
            _validator.ValidateCreateOrUpdate(doctor, patient, attentionDate, content);

            var patientId = patient.Id_patient.Trim();
            var dateKey = attentionDate.ToString("s"); // formato sortable: yyyy-MM-ddTHH:mm:ss

            // Guardar (el puerto decidirá política de sobrescritura)
            _historyPort.SaveEntry(patientId, dateKey, content, doctor);
        }

        /// <summary>
        /// Actualiza una entrada existente de la historia clínica.
        /// </summary>
        public void UpdateEntry(Doctor doctor, Patient patient, DateTime attentionDate, string content)
        {
            _validator.ValidateCreateOrUpdate(doctor, patient, attentionDate, content);

            var patientId = patient.Id_patient.Trim();
            var dateKey = attentionDate.ToString("s");

            // Actualizar (se espera que el puerto valide existencia o lance excepción)
            _historyPort.UpdateEntry(patientId, dateKey, content, doctor);
        }

        /// <summary>
        /// Obtiene toda la historia clínica (diccionario fecha->contenido) de un paciente.
        /// </summary>
        public Dictionary<string, string> GetPatientHistory(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            _validator.ValidatePatientKey(patient.Id_patient);

            var patientId = patient.Id_patient.Trim();
            return _historyPort.GetHistoryByPatientId(patientId) ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Obtiene una entrada específica por fecha.
        /// </summary>
        public string GetEntry(Patient patient, DateTime attentionDate)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            _validator.ValidatePatientKey(patient.Id_patient);

            var patientId = patient.Id_patient.Trim();
            var dateKey = attentionDate.ToString("s");
            return _historyPort.GetEntry(patientId, dateKey);
        }

        /// <summary>
        /// Elimina una entrada de historia clínica.
        /// </summary>
        public void DeleteEntry(Patient patient, DateTime attentionDate)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            _validator.ValidatePatientKey(patient.Id_patient);

            var patientId = patient.Id_patient.Trim();
            var dateKey = attentionDate.ToString("s");
            _historyPort.DeleteEntry(patientId, dateKey);
        }
    }
}