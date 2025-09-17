using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Visit 
    {
        private Patient id_patient;
        private Doctor id_doctor;
        private Order_procedure id_procedure;
        private Vital_data vital_data;


        internal Patient Id_patient { get => id_patient; set => id_patient = value; }
        internal Doctor Id_doctor { get => id_doctor; set => id_doctor = value; }
        internal Order_procedure Id_procedure { get => id_procedure; set => id_procedure = value; }
        internal Vital_data Vital_data { get => vital_data; set => vital_data = value; }
    }
}
