using Hospital.domain.model;
using Hospital.domain.ports;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Hospital.domain.services
{
    internal class C_Billings
    {
        private Billings_port billings_Port;
        public void Create(Billings billings)
        {
            
            // Medical_insurance.policy_number = "";
            //Medical_insurance.insurance_cost = "";

            if (billings_Port.FindById_billings(billings) != null)
            {
                throw new Exception("Ya existe una factura con ese ID.");
            }

            if (billings_Port.FindByNumOrder(billings) == null) // La orden trae el paciente
            {
                throw new Exception("La orden no existe");
            }
            billings_Port.Save(billings);
        }
    }
}
