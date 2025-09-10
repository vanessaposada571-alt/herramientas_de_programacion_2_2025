using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Order_help
    {
        private long num_orderA;//numero de orden de ayuda
        private long id_help;
        private int amount;
        private bool assistance;
        private long id_specialist;
        private long item;

        public long Num_orderA { get => num_orderA; set => num_orderA = value; }
        public long Id_help { get => id_help; set => id_help = value; }
        public int Amount { get => amount; set => amount = value; }
        public bool Assistance { get => assistance; set => assistance = value; }
        public long Id_specialist { get => id_specialist; set => id_specialist = value; }
        public long Item { get => item; set => item = value; }
    }
}
