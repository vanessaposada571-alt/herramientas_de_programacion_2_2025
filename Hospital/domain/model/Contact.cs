using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Contact:Person
    {
        private Person name;
        private Person cedula;
        private Person mail;
        private Person telefono;

        internal Person Name1 { get => name; set => name = value; }
        internal Person Cedula { get => cedula; set => cedula = value; }
        internal Person Mail { get => mail; set => mail = value; }
        internal Person Telefono { get => telefono; set => telefono = value; }
    }
}
