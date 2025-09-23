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
    internal class C_MedicalRecord
    {
        private MedicalRecord_Port medicalrecord_Port;

        public void Create(Medical_record medical_Record)
        {
            if (medicalrecord_Port.FindById_RegistroMedico(medical_Record) != null)
            {
                throw new Exception("Ya existe una orden creada con ese ID");
            }
            if(medicalrecord_Port.FindByIdOrder(medical_Record) == null) // De la orden se trae el paciente y el doctor
            {
                throw new Exception("La orden no existe");
            }

            medicalrecord_Port.Save(medical_Record);
        }
    }
}