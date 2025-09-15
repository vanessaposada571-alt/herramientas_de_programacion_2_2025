using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Billings : Patient //Facturacion
    {
        private int id_billings;
        private Patient id_patient;
        private Patient patient_name;
        private medical_insurance company_name;
        private medical_insurance policy_number;
        private Order order;
        private long amount;
        private medical_insurance insurance_cost;

        public int Id_billings { get => id_billings; set => id_billings = value; }
        public long Amount { get => amount; set => amount = value; }
        internal Patient Id_patient1 { get => id_patient; set => id_patient = value; }
        internal Patient Patient_name { get => patient_name; set => patient_name = value; }
        internal medical_insurance Company_name { get => company_name; set => company_name = value; }
        internal medical_insurance Policy_number { get => policy_number; set => policy_number = value; }
        internal Order Order { get => order; set => order = value; }
        internal medical_insurance Insurance_cost { get => insurance_cost; set => insurance_cost = value; }
    }
}

  
