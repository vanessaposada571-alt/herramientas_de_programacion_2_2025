using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class paciente:person
    {
        private long id;
        private string seguro;
        private string genero;
        private contacto contacto;

        public long Id { get => id; set => id = value; }
        public string Seguro { get => seguro; set => seguro = value; }
        public string Genero { get => genero; set => genero = value; }
        internal contacto Contacto { get => contacto; set => contacto = value; }
    }
}
