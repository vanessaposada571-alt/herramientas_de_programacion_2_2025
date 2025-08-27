using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class usuario
    {
        private String rol;
        private string nombre_usuario;
        private string constraseña;

        public string Rol { get => rol; set => rol = value; }
        public string Nombre_usuario { get => nombre_usuario; set => nombre_usuario = value; }
        public string Constraseña { get => constraseña; set => constraseña = value; }
    }
}
