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
            if (patient_Port.FindById_patient(patient) != null)
            {
                throw new Exception("El paciente con ese Id ya existe");
            }
            if (patient_Port.FindByDirection1(patient) == null)
            {
                throw new Exception("Tiene que agregar una dirección");
            }
            if (patient_Port.FindByContact(patient) == null)
            {
                throw new Exception("El paciente tiene que tener un contacto de emergencia");
            }
            if (patient_Port.FindByEmail1(patient) == null)
            {
                throw new Exception("El paciente debe contar con un email");
            }
            if (patient_Port.FindByCellphone1(patient) == null)
            {
                throw new Exception("Agrega un numero de telefono");
            }
            if (patient_Port.FindByIdSure(patient) == null)
            {
                throw new Exception("El paciente no tiene seguro");
            }
            if (patient_Port.FindByBirth1(patient) == null)
            {
                throw new Exception("Debe agregar la fecha de nacimiento");
            }

            patient_Port.Save(patient);
        }
    }
}
