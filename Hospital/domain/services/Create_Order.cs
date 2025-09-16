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
        public void Create(Order orderGrl)
        {

            if (orderPort.FindByorder_Help(orderGrl) == null )
            {
                throw new Exception("No requiere ayuda diagnóstica");
            }
            if (orderPort.FindByorder_Medicine(orderGrl) == null)
            {
                throw new Exception("No requiere medicamento");
            }
            if (orderPort.FindByorder_Procedure(orderGrl) == null)
            {
                throw new Exception("No requiere Procedimiento");
            }

            orderPort.Save(orderGrl);
        }
    }
}
