using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class facturacion: paciente
    {
        private paciente paciente;
        private medico medico;
        private orden orden;

        internal paciente Paciente { get => paciente; set => paciente = value; }
        internal medico Medico { get => medico; set => medico = value; }
        internal orden Orden { get => orden; set => orden = value; }
    }
}
