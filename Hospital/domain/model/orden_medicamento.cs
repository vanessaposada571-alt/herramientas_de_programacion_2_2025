using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class orden_medicamento
    {
        private long nro_orden;
        private long id_medicamento;
        private String dosis;
        private int duracion_trat;
        private long item;

        public long Nro_orden { get => nro_orden; set => nro_orden = value; }
        public long Id_medicamento { get => id_medicamento; set => id_medicamento = value; }
        public string Dosis { get => dosis; set => dosis = value; }
        public int Duracion_trat { get => duracion_trat; set => duracion_trat = value; }
        public long Item { get => item; set => item = value; }
    }
}
