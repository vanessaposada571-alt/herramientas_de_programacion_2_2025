using Hospital.domain.model;
using Hospital.domain.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.ports
{
    internal interface MedicalRecord_Port
    {
        public Medical_record FindById_RegistroMedico(Medical_record medical_Record);
        public Patient FindById_patient(Medical_record medical_Record);
        public Doctor FindById_doctor(Medical_record medical_Record);
        public Order FindByIdOrder(Medical_record medical_Record);
        public void Save(Medical_record medical_Record);
        public void Update(Medical_record medical_Record);
        public void Delete(Medical_record medical_Record);
        public void Search(Medical_record medical_Record);
    }
}