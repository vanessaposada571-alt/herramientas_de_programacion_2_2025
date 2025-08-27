using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class orden
    {
        private orden_ayuda orden_Ayuda;
        private orden_medicamento orden_Medicamento;
        private orden_procedimiento orden_Procedimiento;

        internal orden_ayuda Orden_Ayuda { get => orden_Ayuda; set => orden_Ayuda = value; }
        internal orden_medicamento Orden_Medicamento { get => orden_Medicamento; set => orden_Medicamento = value; }
        internal orden_procedimiento Orden_Procedimiento { get => orden_Procedimiento; set => orden_Procedimiento = value; }
    }
}
