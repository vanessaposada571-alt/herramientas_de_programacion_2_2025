using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class datos_vitales
    {
        private int presion;
        private int temperatura;
        private int pulso;
        private int niv_oxi_sang;

        public int Presion { get => presion; set => presion = value; }
        public int Temperatura { get => temperatura; set => temperatura = value; }
        public int Pulso { get => pulso; set => pulso = value; }
        public int Niv_oxi_sang { get => niv_oxi_sang; set => niv_oxi_sang = value; }
    }
}
