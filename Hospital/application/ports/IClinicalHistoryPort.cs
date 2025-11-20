using System;
using System.Collections.Generic;
using Hospital.domain.model;

namespace Hospital.application.ports
{
    /// <summary>
    /// Puerto de persistencia para la historia clínica (NoSQL / diccionario).
    /// Clave principal: cédula del paciente (Patient.Id_patient)
    /// Subclave: fecha de atención (string ISO)
    /// Valor: objeto ClinicalHistoryEntry
    /// </summary>
    public interface IClinicalHistoryPort
    {
        // Devuelve diccionario: fechaClave -> entrada estructurada
        Dictionary<string, ClinicalHistoryEntry> GetHistoryByPatientId(string patientId);

        ClinicalHistoryEntry GetEntry(string patientId, string dateKey);

        // Guardar/actualizar con el objeto estructurado
        void SaveEntry(string patientId, string dateKey, ClinicalHistoryEntry entry);
        void UpdateEntry(string patientId, string dateKey, ClinicalHistoryEntry entry);
        void DeleteEntry(string patientId, string dateKey);
    }
}