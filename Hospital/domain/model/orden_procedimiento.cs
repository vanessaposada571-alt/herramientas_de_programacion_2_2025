using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class orden_procedimiento
    {
        private long num_orden;
        private long id_procedim;
        private int cantidad;
        private String frecuencia;
        private bool especialista;
        private long id_especial;
        private long item;

        public long Num_orden { get => num_orden; set => num_orden = value; }
        public long Id_procedim { get => id_procedim; set => id_procedim = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        public string Frecuencia { get => frecuencia; set => frecuencia = value; }
        public bool Especialista { get => especialista; set => especialista = value; }
        public long Id_especial { get => id_especial; set => id_especial = value; }
        public long Item { get => item; set => item = value; }
    }
}
