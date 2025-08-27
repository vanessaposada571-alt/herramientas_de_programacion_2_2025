using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class medico: paciente
    {
        private paciente paciente;
        private orden orden;

        internal paciente Paciente { get => paciente; set => paciente = value; }
        internal orden Orden { get => orden; set => orden = value; }
    }
}
