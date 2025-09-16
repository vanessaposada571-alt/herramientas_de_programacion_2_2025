using Hospital.domain.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Appointment 
    {
        private string id_appointment;
        private Patient id_Patient;
        private Doctor  id_doctor;
        private DateTime Date;
        private string Reason;

        public string Id_appointment { get => id_appointment; set => id_appointment = value; }
        public DateTime Date1 { get => Date; set => Date = value; }
        public string Reason1 { get => Reason; set => Reason = value; }
        internal Patient Id_Patient { get => id_Patient; set => id_Patient = value; }
        internal Doctor Id_doctor { get => id_doctor; set => id_doctor = value; }
    }
}


