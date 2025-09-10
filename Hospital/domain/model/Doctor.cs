using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Doctor: Patient
    {
        private Patient paciente;
        private Order orden;

        internal Patient Paciente { get => paciente; set => paciente = value; }
        internal Order Orden { get => orden; set => orden = value; }
    }
}
