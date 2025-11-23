using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    public class Medical_record
    {
        private int Id_RM;
        private Patient id_patient;
        private Doctor id_doctor;
        private Order id_order;

        public int Id_RegistroMedico { get => Id_RM; set => Id_RM = value; }
        internal Patient Id_patient { get => id_patient; set => id_patient = value; }
        internal Doctor Id_doctor { get => id_doctor;set => id_doctor = value; }
        internal Order IdOrder { get => id_order; set => id_order = value; }
    }
}
