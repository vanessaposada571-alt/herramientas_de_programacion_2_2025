using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Patient:Person
    {
        private string id_patient; //Id del paciente
        private Medical_insurance idSure; //Seguro medico
        private string policyNumber; // Número de Poliza
        private string gender;
        private Contact contact;

        public string Id_patient { get => id_patient; set => id_patient = value; }
        public Medical_insurance IdSure { get => idSure; set => idSure = value; }
        public string PolicyNumber { get => policyNumber; set => policyNumber = value; }
        public string Gender { get => gender; set => gender = value; }
        internal Contact Contact { get => contact; set => contact = value; }
    }
}
