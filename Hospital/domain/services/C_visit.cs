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
    internal class C_Visit
    {
        private Visit_port visit_Port;
        public void Create(Visit visit)
        {

            if (visit_Port.FindById_patient(visit) == null)
            {
                throw new Exception("El paciente no existe");
            }
            if (visit_Port.FindById_doctor(visit) == null)
            {
                throw new Exception("El doctor ingresado no existe");
            }
            if (visit_Port.FindById_procedure(visit) == null)
            {
                throw new Exception("El ID de procedimiento no existe");
            }
            if (visit_Port.FindByPressure(visit) == null)
            {
                throw new Exception("Debes ingresar la presión del paciente");
            }

            if (visit_Port.FindByTemperature(visit) == null)
            {
                throw new Exception("Debes ingresar la temperatura del paciente");
            }

            if (visit_Port.FindByPulse(visit) == null)
            {
                throw new Exception("Debes ingresar el pulso del paciente");
            }

            if (visit_Port.FindByBlood_oxygen_level(visit) == null)
            {
                throw new Exception("Debes ingresar el nivel de oxígeno en sangre del paciente");
            }
            visit_Port.Save(visit);
        }
    }
}


