using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class person
    {
        private String name;
        private long cedula;
        private MailAddress correo;
        private long telefono;

        public person() { }

        public string Name { get => name; set => name = value; }
        public long Cedula { get => cedula; set => cedula = value; }
        public MailAddress Correo { get => correo; set => correo = value; }
        public long Telefono { get => telefono; set => telefono = value; }
    }
}
