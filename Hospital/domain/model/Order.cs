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
        private Order_procedure NumOrder;

        public int IDOrder { get => IdOrder; set => IdOrder = value; }
        internal Order_help NumOrderA { get => order_Help; set => order_Help = value; }
        internal Order_medicine NumOrder { get => order_Medicine; set => order_Medicine = value; }
        internal Order_procedure NumOrder { get => order_Procedure; set => order_Procedure = value; }
    }
}
