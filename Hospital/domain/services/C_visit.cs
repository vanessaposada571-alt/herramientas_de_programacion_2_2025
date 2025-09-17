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
    internal class C_visit
    {
        private Visit_port visit_Port;
        public void Create(Visit visit)
        {

            if (visit_Port.FindById_patient(visit) == null)
            {
                throw new Exception("El paciente no existe");
            }
            if (visit_Port.FindById)
            {
                throw new Exception("El doctor ingresado no existe");
            }
            if (visit_Port.FindById_procedure(visit) == null)
            {
                throw new Exception("El ID de procedimiento no existe");
            }

            if (visit.Vital_data.Pressure < 60 || visit.Vital_datal.Presure > 180)
            {
                throw new Exception("La presión ingresada no es válida");
            }

            if (visit.Vital_data.temperature < 35 || visit.Vital_data.Temperature > 42)
            {
                throw new Exception("La temperatura ingresada no es válida");
            }

            if (visit.Vital_data.Pulse < 40 || visit.Vital_data.Pulse > 180)
            {
                throw new Exception("El pulso ingresado no es válido");
            }

            if (visit.Vital_data.Blood_oxygen_level < 70 || visit.Vital_data.Blood_oxygen_level > 100)
            {
                throw new Exception("El nivel de oxígeno en sangre no es válido");
            }
            visit_Port.Save(visit);
        }
    }
}


