using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.services
{
    internal class S_Order
    {
        private Order_port order_Port;
        public void Search(Order order)
        {
            if (order_Port.FindByIDOrder(order) == null)
            {
                throw new Exception("La orden no existe");
            }

            order_Port.Search(order);
        }
    }
}
