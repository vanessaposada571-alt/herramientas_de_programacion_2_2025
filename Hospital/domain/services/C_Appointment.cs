using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.services
{
    internal class C_Appointment
    {
        private Appointment_port appointment_Port;

        public void Create(Appointment appointment)
        {
            // 1. Validar que el Id del paciente no esté vacío
            if (appointment_Port.FindById_patient(appointment) == null)
            {
                throw new Exception("El paciente no existe");
            }
            
            appointment_Port.Save(appointment); // 3. Guardar cita
        }
    }
}
