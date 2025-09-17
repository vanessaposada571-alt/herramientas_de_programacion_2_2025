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
            if (visit_Port.FindById_doctor(visit == null)
            {
                throw new Exception("El doctor ingresado no existe");
            }
            if (visit_Port.FindById_procedure(visit) == null)
            {
                throw new Exception("El ID de procedimiento no existe");
            }
            if (visit_Port.FindByPressure(visit) < 60 || visit_Port.FindByPressure(visit) > 180)
            {
                throw new Exception("La presión ingresada no es válida");
            }

            if (visit_Port.FindByTemperature(visit) < 35 || visit_Port.FindByTemperature(visit) > 42)
            {
                throw new Exception("La temperatura ingresada no es válida");
            }

            if (visit_Port.FindByPulse(visit) < 40 || visit_Port.FindByPulse(visit) > 180)
            {
                throw new Exception("El pulso ingresado no es válido");
            }

            if (visit_Port.FindByBlood_oxygen_level(visit) < 70 || visit_Port.FindByBlood_oxygen_level(visit) > 100)
            {
                throw new Exception("El nivel de oxígeno en sangre no es válido");
            }
            visit_Port.Save(visit);
        }
    }
}


