using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Order_medicine
    {
        private long num_orderM; //numero de orden de medicina
        private long id_medicine;
        private String dose; //dosis
        private int duration_treat; //duracion de tratamiento
        private long item;

        public long Num_order { get => num_order; set => num_order = value; }
        public long Id_medicine { get => id_medicine; set => id_medicine = value; }
        public string Dose { get => dose; set => dose = value; }
        public int Duration_treat { get => duration_treat; set => duration_treat = value; }
        public long Item { get => item; set => item = value; }
    }
}
