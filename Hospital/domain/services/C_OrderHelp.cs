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
    internal class C_OrderHelp
    {
        private Order_help_port orderHelpPort;
        public void Create(Order_help orderHelp)
        {

            if (orderHelpPort.FindByNumOrderA(orderHelp) != null)
            {
                throw new Exception("Ya existe el número de orden");
            }
            if (orderHelpPort.FindByIdHelp(orderHelp) != null)
            {
                throw new Exception("Ya existe el número de ayuda");
            }
            if (orderHelpPort.FindByIdSpecialist(orderHelp) == null)
            {
                throw new Exception("El especialista no existe");
            }

            orderHelpPort.Save(orderHelp);
        }

    }
}