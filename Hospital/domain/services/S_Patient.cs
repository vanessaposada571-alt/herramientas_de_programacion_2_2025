using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.services
{
    public class S_Patient
    {
        private Patient_port patient_Port;

        public S_Patient()
        {
        }

        public S_Patient(Patient_port patientPort)
        {
        }

        public S_Patient(infraestructure.adapters.output.SQLPatientPort patientPort)
        {
            patient_Port = new Hospital.infraestructure.adapters.output.SQLPatientPort();
        }
        public Patient Search(Patient patient)
        {
            if (patient_Port.FindById_patient(patient) == null)
            {
                throw new Exception("El paciente no existe");
            }
            return patient_Port.Search(patient);
        }
    }
}
