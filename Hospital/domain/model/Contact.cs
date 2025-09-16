using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Contact:Person
    {
       
        private Person name;
        private Patient id_patient;
        private Person cellphone;
        private string relation;

        internal Person Name1 { get => name; set => name = value; }
        internal Patient Id_patient{ get => id_patient; set => id_patient = value; }
        internal Person Cellphone { get => cellphone; set => cellphone = value; }
        internal string Relation { get => relation; set => relation = value; }
    }
}
