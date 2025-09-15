using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Order
    {
        private Order_help order_Help;
        private Order_medicine order_Medicine;
        private Order_procedure order_Procedure;

        internal Order_help order_Help { get => order_Help; set => order_Help = value; }
        internal Order_medicine order_Medicine { get => order_Medicine; set => order_Medicine = value; }
        internal Order_procedure order_Procedure { get => order_Procedure; set => order_Procedure = value; }
    }
}
