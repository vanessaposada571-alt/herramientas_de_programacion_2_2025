using System;
using System.Collections.Generic;
using Hospital.domain.model;

namespace Hospital.application.ports
{
    /// <summary>
    /// Puerto de persistencia para la historia clínica.
    /// La implementación concreta puede mapear esto a una base NoSQL.
    /// Estructura esperada (no estructurada):
    ///   - clave principal: cédula del paciente (Patient.Id_patient)
    ///   - subclave: fecha de atención (ISO / cadena)
    ///   - valor: texto libre / JSON / documento no estructurado
    /// </summary>
    public interface IClinicalHistoryPort
    {
        /// <summary>
        /// Obtiene todo el diccionario de historia clínica para un paciente.
        /// Devuelve un diccionario: fecha(string) -> contenido(string).
        /// Si no existe, se debe devolver un diccionario vacío.
        /// </summary>
        Dictionary<string, string> GetHistoryByPatientId(string patientId);

        /// <summary>
        /// Obtiene una entrada concreta por fecha (clave de subregistro).
        /// Devuelve null si no existe.
        /// </summary>
        string GetEntry(string patientId, string dateKey);

        /// <summary>
        /// Crea o guarda una entrada nueva para el paciente en la fecha indicada.
        /// Si ya existe la clave, la implementación puede sobrescribirla o fallar según la política.
        /// </summary>
        void SaveEntry(string patientId, string dateKey, string content, Doctor author);

        /// <summary>
        /// Actualiza una entrada existente. Debe lanzar excepción si no existe.
        /// </summary>
        void UpdateEntry(string patientId, string dateKey, string content, Doctor author);

        /// <summary>
        /// Elimina una entrada por fecha; si no existe puede ser no-op o lanzar excepción según implementación.
        /// </summary>
        void DeleteEntry(string patientId, string dateKey);
    }
}