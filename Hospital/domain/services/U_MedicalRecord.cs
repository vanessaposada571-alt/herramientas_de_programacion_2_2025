using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Hospital.domain.services
{
    internal class Create_MedicalRecord
    {
        private MedicalRecord_port medicalrecord_Port;

        public void Update(Medical_record medical_Record)
        {
            if (medicalrecord_Port.FindById_RegistroMedico(medical_Record) == null)
            {
                throw new Exception("No existe una orden creada con ese ID");
            }

            medicalrecord_Port.Update(medical_Record);
        }
    }
}