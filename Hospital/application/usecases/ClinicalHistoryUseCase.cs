using System;
using System.Collections.Generic;
using Hospital.application.validators;
using Hospital.domain.model;
using Hospital.domain.services;

namespace Hospital.application.usecases
{
    /// <summary>
    /// Casos de uso para que los médicos creen, actualicen y consulten la historia clínica.
    /// Consume directamente el servicio de dominio `C_ClinicalHistory`.
    /// </summary>
    public class ClinicalHistoryUseCase
    {
        private readonly C_ClinicalHistory _historyService;
        private readonly ClinicalHistoryValidator _validator;

        public ClinicalHistoryUseCase(C_ClinicalHistory historyService, ClinicalHistoryValidator validator)
        {
            _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public void CreateEntry(Doctor doctor, Patient patient, ClinicalHistoryEntry entry)
        {
            _validator.ValidateCreateOrUpdate(doctor, patient, entry);

            var patientId = patient.Id_patient.Trim();
            var dateKey = entry.AttentionDate.ToString("s"); // yyyy-MM-ddTHH:mm:ss

            if (string.IsNullOrWhiteSpace(entry.DoctorId))
                entry.DoctorId = doctor?.Id_doctor;

            _historyService.SaveEntry(patientId, dateKey, entry);
        }

        public void UpdateEntry(Doctor doctor, Patient patient, ClinicalHistoryEntry entry)
        {
            _validator.ValidateCreateOrUpdate(doctor, patient, entry);

            var patientId = patient.Id_patient.Trim();
            var dateKey = entry.AttentionDate.ToString("s");

            if (string.IsNullOrWhiteSpace(entry.DoctorId))
                entry.DoctorId = doctor?.Id_doctor;

            _historyService.UpdateEntry(patientId, dateKey, entry);
        }

        public Dictionary<string, ClinicalHistoryEntry> GetPatientHistory(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            _validator.ValidatePatientKey(patient.Id_patient);

            var patientId = patient.Id_patient.Trim();
            return _historyService.GetHistoryByPatientId(patientId) ?? new Dictionary<string, ClinicalHistoryEntry>();
        }

        public ClinicalHistoryEntry GetEntry(Patient patient, DateTime attentionDate)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            _validator.ValidatePatientKey(patient.Id_patient);

            var patientId = patient.Id_patient.Trim();
            var dateKey = attentionDate.ToString("s");
            return _historyService.GetEntry(patientId, dateKey);
        }

        public void DeleteEntry(Patient patient, DateTime attentionDate)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            _validator.ValidatePatientKey(patient.Id_patient);

            var patientId = patient.Id_patient.Trim();
            var dateKey = attentionDate.ToString("s");
            _historyService.DeleteEntry(patientId, dateKey);
        }
    }
}