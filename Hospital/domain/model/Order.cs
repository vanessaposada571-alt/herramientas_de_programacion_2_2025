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
        private Order_medicine orden_Medicine;
        private Order_procedure orden_Procedure;

        internal Order_help order_Help { get => order_Help; set => order_Help = value; }
        internal Order_medicine orden_Medicine { get => orden_Medicine; set => orden_Medicine = value; }
        internal Order_procedure orden_Procedure { get => orden_Procedure; set => orden_Procedure = value; }
    }
}
