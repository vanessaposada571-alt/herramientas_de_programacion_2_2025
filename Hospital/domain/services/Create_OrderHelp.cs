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
    internal class Create_OrderHelp
    {
        private Order_help_port orderHelpPort;
        public void Create(Order_help orderHelp, string list)
        {
            orderHelp.Num_orderA = "";
            orderHelp.Id_help = "";
            orderHelp.Amount = "";
            orderHelp.Assistance = "true";
            orderHelp.Id_specialist = "";
            orderHelp.Item = "";

            if (Order_help_port.FindByNumOrderA(orderHelp) != null)
            {
                throw new Exception("Ya existe el número de orden");
            }
            if (Order_help_port.FindByIdHelp(orderHelp) != null)
            {
                throw new Exception("Ya existe el número de ayuda");
            }
            if (Order_help_port.FindBySpecialist(orderHelp) = null)
            {
                throw new Exception("El especialista no existe");
            }

            Order_help_port.Save(orderHelp);
        }

    }
}