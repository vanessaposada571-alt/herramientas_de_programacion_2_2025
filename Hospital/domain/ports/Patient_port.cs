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
        public Patient FindByName1(Patient patient);
        public Patient FindByContact(Patient patient);
        public Patient FindByEmail1(Patient patient);
        public Patient FindByCellphone1(Patient patient);
        public Patient FindByGender1(Patient patient);
        public Patient FindByIdSure(Patient patient);
        public Patient FindByBirth1(Patient patient);
        public Patient FindByDirection1(Patient patient);

        public void Save(Patient patient);
        public void Update(Patient patient);
        public  void Delete(Patient patient);
        public  void Search(Patient patient);
    }
}
