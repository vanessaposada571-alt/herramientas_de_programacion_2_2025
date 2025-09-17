using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.services
{
    internal class C_Patient
    {
        private Patient_port patient_Port;
        public void Create (Patient patient)
        {
            if (patient_Port.FindById_patient(patient) == null)
            {
                throw new Exception("El paciente no existe");
            }
            if (patient_Port.FindByIdSure(patient) == null)
            {
                throw new Exception("El paciente no tiene seguro");
            }

            patient_Port.Save(patient);
        }
    }
}
