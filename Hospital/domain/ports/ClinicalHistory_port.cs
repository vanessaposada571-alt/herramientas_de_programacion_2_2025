
using System;
using System.Collections.Generic;
using Hospital.domain.model;
using Hospital.domain.services;

namespace Hospital.domain.ports
{
    /// <summary>
    /// Puerto de dominio para la historia clínica (interfaz entre usecases y servicios/implementaciones NoSQL).
    /// Mantiene la convención de los demás ports del proyecto.
    /// </summary>
    public interface ClinicalHistory_port
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