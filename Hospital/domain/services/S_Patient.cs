using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.services
{
    internal class Search_Patient
    {
        private Patient_port patient_Port;
        public void Search(Patient patient)
        {
            if (patient_Port.FindById_patient(patient) == null)
            {
                throw new Exception("El paciente no existe")
            }
            patient_Port.Search(patient);
        }
    }
}
