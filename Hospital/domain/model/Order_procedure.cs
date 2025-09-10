using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Order_procedure
    {
        private long num_orderP;//numero de orden de procedimiento
        private long id_procedure;
        private int amount;
        private String frequency;
        private bool specialist;
        private long id_specialist;
        private long item;

        public long Num_order { get => num_order; set => num_order = value; }
        public long Id_procedure { get => id_procedure; set => id_procedure = value; }
        public int Amount { get => amount; set => amount = value; }
        public string Frequency { get => frequency; set => frequency = value; }
        public bool Specialist { get => specialist; set => specialist = value; }
        public long Id_specialist { get => id_specialist; set => id_specialist = value; }
        public long Item { get => item; set => item = value; }
    }
}
