using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    public class Patient:Person
    {
        private string id_patient; //Id del paciente
        private Medical_insurance idSure; //Seguro medico
        private string policyNumber; // Número de Poliza
        private Contact contact;

        // Backing fields for wrapper Person objects used in the app
        private Person _name1;
        private Person _email1;
        private Person _cellphone1;
        private Person _birth1;
        private Person _direction1;
        private Person _gender1;

        public string Id_patient { get => id_patient; set => id_patient = value; }
        public Medical_insurance IdSure { get => idSure; set => idSure = value; }
        public string PolicyNumber { get => policyNumber; set => policyNumber = value; }
        public Contact Contact { get => contact; set => contact = value; }

        // Wrapper properties kept for backward compatibility with existing validators/usecases
        internal Person Name1 { get => _name1; set => _name1 = value; }
        internal Person Email1 { get => _email1; set => _email1 = value; }
        internal Person Cellphone1 { get => _cellphone1; set => _cellphone1 = value; }
        internal Person Birth1 { get => _birth1; set => _birth1 = value; }
        internal Person Direction1 { get => _direction1; set => _direction1 = value; }
        public Person Gender1 { get => _gender1; set => _gender1 = value; }
    }
}
