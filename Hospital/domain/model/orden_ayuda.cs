using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class orden_ayuda
    {
        private long num_orden;
        private long id_ayuda;
        private int cantidad;
        private bool asistencia;
        private long id_especial;
        private long item;

        public long Num_orden { get => num_orden; set => num_orden = value; }
        public long Id_ayuda { get => id_ayuda; set => id_ayuda = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        public bool Asistencia { get => asistencia; set => asistencia = value; }
        public long Id_especial { get => id_especial; set => id_especial = value; }
        public long Item { get => item; set => item = value; }
    }
}
