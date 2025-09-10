using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Patient:Person
    {
        private long id_patient; //Id del paciente
        private string insurance; //Seguro medico
        private string gender;
        private Contact contact;

        public long Id_patient { get => id_patient; set => id_patient = value; }
        public string Insurance { get => insurance; set => insurance = value; }
        public string Gender { get => gender; set => gender = value; }
        internal Contact Contact { get => contact; set => contact = value; }
    }
}
