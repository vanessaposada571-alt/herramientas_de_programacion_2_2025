using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Billings: Patient //Facturacion
    {
        private Patient patient;
        private Doctor doctor;
        private Order order;

        internal Patient Patient { get => patient; set => patient = value; }
        internal Doctor Doctor { get => doctor; set => doctor = value; }
        internal Order Order { get => order; set => order = value; }
    }
}
