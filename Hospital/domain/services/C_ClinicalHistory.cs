
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Hospital.domain.model;

namespace Hospital.domain.services
{
    /// <summary>
    /// Servicio de dominio que actúa como adaptador in-memory para la historia clínica (simula NoSQL).
    /// Estructura: pacienteId -> (dateKey -> ClinicalHistoryEntry)
    /// Política: SaveEntry sobrescribe si ya existe la clave.
    /// </summary>
    public class C_ClinicalHistory
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ClinicalHistoryEntry>> _store
            = new();

        public Dictionary<string, ClinicalHistoryEntry> GetHistoryByPatientId(string patientId)
        {
            if (string.IsNullOrWhiteSpace(patientId)) return new Dictionary<string, ClinicalHistoryEntry>();
            if (_store.TryGetValue(patientId, out var dict))
                return new Dictionary<string, ClinicalHistoryEntry>(dict);
            return new Dictionary<string, ClinicalHistoryEntry>();
        }

        public ClinicalHistoryEntry GetEntry(string patientId, string dateKey)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(dateKey)) return null;
            if (_store.TryGetValue(patientId, out var dict) && dict.TryGetValue(dateKey, out var entry))
                return entry;
            return null;
        }

        public void SaveEntry(string patientId, string dateKey, ClinicalHistoryEntry entry)
        {
            if (string.IsNullOrWhiteSpace(patientId)) throw new ArgumentNullException(nameof(patientId));
            if (string.IsNullOrWhiteSpace(dateKey)) throw new ArgumentNullException(nameof(dateKey));
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            var dict = _store.GetOrAdd(patientId, _ => new ConcurrentDictionary<string, ClinicalHistoryEntry>());
            dict[dateKey] = entry; // sobrescribe si existe
        }

        public void UpdateEntry(string patientId, string dateKey, ClinicalHistoryEntry entry)
        {
            if (string.IsNullOrWhiteSpace(patientId)) throw new ArgumentNullException(nameof(patientId));
            if (string.IsNullOrWhiteSpace(dateKey)) throw new ArgumentNullException(nameof(dateKey));
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            if (!_store.TryGetValue(patientId, out var dict) || !dict.ContainsKey(dateKey))
                throw new KeyNotFoundException("La entrada especificada no existe.");

            dict[dateKey] = entry;
        }

        public void DeleteEntry(string patientId, string dateKey)
        {
            if (string.IsNullOrWhiteSpace(patientId)) throw new ArgumentNullException(nameof(patientId));
            if (string.IsNullOrWhiteSpace(dateKey)) throw new ArgumentNullException(nameof(dateKey));

            if (_store.TryGetValue(patientId, out var dict))
            {
                dict.TryRemove(dateKey, out _);
            }
        }
    }
}