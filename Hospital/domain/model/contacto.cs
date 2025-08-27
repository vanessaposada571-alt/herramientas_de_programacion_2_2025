using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class contacto:person
    {
        private person name;
        private person cedula;
        private person mail;
        private person telefono;

        internal person Name1 { get => name; set => name = value; }
        internal person Cedula1 { get => cedula; set => cedula = value; }
        internal person Mail { get => mail; set => mail = value; }
        internal person Telefono1 { get => telefono; set => telefono = value; }
    }
}
