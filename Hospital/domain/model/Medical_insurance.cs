using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    public class Medical_insurance
    {
        private int idSure;
        private string company_name;
        private string policy_number;
        private bool policy_status;  // true = activa, false = inactiva
        private DateTime effective_Date;
        private long copayment;
        private long insurance_cost;

        // Relación al paciente (ID guardado); nombre es solo visual en UI
        private string patientId;
        private string patientName;

        public int IdSure { get => idSure; set => idSure = value; }
        public string Company_name { get => company_name; set => company_name = value; }
        public string Policy_number { get => policy_number; set => policy_number = value; }
        public bool Policy_status { get => policy_status; set => policy_status = value; }
        public DateTime Effective_Date { get => effective_Date; set => effective_Date = value; }
        public long Copayment { get => copayment; set => copayment = value; }
        public long Insurance_cost { get => insurance_cost; set => insurance_cost = value; }

        // Nuevo: PatientId se persiste (o se usa para relacionar desde el servicio)
        public string PatientId { get => patientId; set => patientId = value; }

        // Nuevo: solo visual en la UI, no debe guardarse en la tabla de pólizas
        public string PatientName { get => patientName; set => patientName = value; }
    }
}

