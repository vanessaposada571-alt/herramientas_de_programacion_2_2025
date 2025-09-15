using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Visit
    {
        private Patient patient;
        private Doctor doctor;
        private Order_procedure procedure;
        private Vital_data vital_data;
        private String observations;

        public string Observations { get => observations; set => observations = value; }
        internal Patient Patient { get => patient; set => patient = value; }
        internal Doctor Doctor { get => doctor; set => doctor = value; }
        internal Order_procedure Procedure { get => procedure; set => procedure = value; }
        internal Vital_data Vital_data { get => vital_data; set => vital_data = value; }
    }
}
