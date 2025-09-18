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
    internal class C_Order
    {
        private Order_port orderPort;
        public void Create(Order orderGrl)
        {

            if (orderPort.FindByNumOrderA1(orderGrl) == null )
            {
                throw new Exception("No requiere ayuda diagnóstica");
            }
            if (orderPort.FindByNumOrder(orderGrl) == null)
            {
                throw new Exception("No requiere medicamento");
            }
            if (orderPort.FindByNumOrderP1(orderGrl) == null)
            {
                throw new Exception("No requiere Procedimiento");
            }

            orderPort.Save(orderGrl);
        }
    }
}
