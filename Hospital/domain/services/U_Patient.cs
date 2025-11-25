using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
    public class U_Patient
    {
        private readonly Patient_port patient_Port;

        public U_Patient(Patient_port patientPort)
        {
            patient_Port = patientPort ?? throw new ArgumentNullException(nameof(patientPort));
        }

        public void Update(Patient patient)
        {
            if (patient_Port.FindById_patient(patient) == null)
            {
                throw new Exception("El paciente no existe");
            }

            patient_Port.Update(patient);
        }
    }
}
