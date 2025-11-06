using Hospital.domain.model;
using Hospital.domain.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.ports
{
    internal interface Patient_port
    {
        public Patient FindById_patient(Patient patient);
        public Patient FindByContact(Patient patient);
        public Patient FindByIdSure(Patient patient);

        public void Save(Patient patient);
        public void Update(Patient patient);
        public  void Delete(Patient patient);
        public  Patient Search(Patient patient);
    }
}
