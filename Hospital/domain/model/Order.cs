using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Order
    {
        private int IdOrder;
        private Order_help NumOrderA;
        private Order_medicine NumOrder;
        private Order_procedure NumOrderP;

        public int IdOrder1 { get => IdOrder; set => IdOrder = value; }
        internal Order_help NumOrderA1 { get => NumOrderA; set => NumOrderA = value; }
        internal Order_medicine NumOrder1 { get => NumOrder; set => NumOrder = value; }
        internal Order_procedure NumOrderP1 { get => NumOrderP; set => NumOrderP = value; }
    }
}
