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
            if (appointment_Port.FindById_patient(appointment) != null)
            {
                throw new Exception("El Id del paciente es obligatorio");
            }
            // 2. Validar que el Id del doctor no esté vacío
            if (appointment_Port.FindbyId_doctor(appointment) != null)
            {
                throw new Exception("El Id del doctor es obligatorio");
            }
            // 3. Validar que no haya cita duplicada (paciente + fecha/hora)
            if (appointment_Port.FindByPatientAndDate(appointment) != null)
            {
                throw new Exception("El paciente ya tiene una cita en esa fecha y hora");
            }
            
            appointment_Port.Save(appointment); // 4. Guardar cita
            appointment_Port.Update(appointment); // 5. Actualizar cita
            appointment_Port.Delete(appointment); // 6. Eliminar cita
        }
    }
}
