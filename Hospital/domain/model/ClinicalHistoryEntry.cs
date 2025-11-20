
using System;

namespace Hospital.domain.model
{
    /// <summary>
    /// Entrada de historia clínica que se almacena como valor en el diccionario NoSQL.
    /// La subclave del diccionario será la fecha de atención en formato ISO (ej. yyyy-MM-ddTHH:mm:ss).
    /// </summary>
    public class ClinicalHistoryEntry
    {
        // Fecha de atención (también se usa como subclave)
        public DateTime AttentionDate { get; set; }

        // Cédula del médico que atendió (máximo 10 dígitos)
        public string DoctorId { get; set; }

        // Motivo de la consulta
        public string Reason { get; set; }

        // Sintomatología
        public string Symptoms { get; set; }

        // Diagnóstico (puede estar vacío en el momento de solicitar ayudas diagnósticas)
        public string Diagnosis { get; set; }

        // Información adicional / notas libres
        public string Notes { get; set; }
    }
}