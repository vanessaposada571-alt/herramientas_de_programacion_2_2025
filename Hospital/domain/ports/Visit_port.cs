using Hospital.domain.model;
using Hospital.domain.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.ports
{
    internal interface Visit_port
    {
        public Patient FindById_patient(Visit visit);
        public Doctor FindById_doctor(Visit visit);
        public Order_procedure FindById_procedure(Visit visit);
        public Vital_data FindByPressure(Visit visit);
        public Vital_data FindByTemperature(Visit visit);
        public Vital_data FindByPulse(Visit visit);
        public Vital_data FindByBlood_oxygen_level(Visit visit);

        public void Save(Visit visit);
        public void Update(Visit visit);
        public void Search(Visit visit);


    }
}

