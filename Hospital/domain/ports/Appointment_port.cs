using Hospital.domain.model;
using Hospital.domain.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.ports
{
    internal interface Appointment_port
    {
        public Appointment FindById_patient(Appointment appointment); // buscar cita por Id

        void Save(Appointment appointment);   // registrar cita nueva
        void Update(Appointment appointment); // actualizar cita existente
        void Delete(Appointment appointment);
    }
    
    
}