using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.services
{
    public class C_Patient
    {
        private readonly Patient_port patient_Port;

        public C_Patient(Patient_port patientPort)
        {
            patient_Port = patientPort ?? throw new ArgumentNullException(nameof(patientPort));
        }

        public void Create(Patient patient)
        {
            

            patient_Port.Save(patient);
        }
    }
}
