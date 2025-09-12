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
    internal class Create_Order
    {
        private Order_port orderPort;
        public void Create(Order orderGrl, string list)
        {
            orderGrl.order_Help = "";
            orderGrl.order_Medicine = "";
            orderGrl.order_Procedure = "";

            if (orderPort.FindByorder_Help(orderGrl) == false)
            {
                throw new Exception("No requiere ayuda diagnóstica");
            }
            if (orderPort.FindByorder_Medicine(orderGrl) == false)
            {
                throw new Exception("No requiere medicamento");
            }
            if (orderPort.FindByorder_Procedure(orderGrl) == false)
            {
                throw new Exception("No requiere Procedimiento");
            }

            orderPort.Save(orderGrl);
        }
    }
}
