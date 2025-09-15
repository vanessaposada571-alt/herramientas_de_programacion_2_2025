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
    internal class Create_billings
    {
        private Billings_port billings_Port;
        public void Create(Billings billings)
        {
            
            Medical_insurance.policy_number = "";
            Medical_insurance.insurance_cost = "";

            if (billings_port.FindById_billings(billings) != null)
            {
                throw new Exception("Ya existe una factura con ese numero");
            }

            billings_port.Save(billings);
        }
    }
}


if (employee_Port.FindByName_user(user) != null)
{
    throw new Exception("Ya existe un empleado registrado con ese nombre");
}
if (employee_Port.FindById(user) != null)
{
    throw new Exception("Ya existe un empleado registrado con ese documento");
}

