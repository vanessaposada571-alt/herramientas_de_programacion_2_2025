using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class User : Person
    {
        private string rol;
        private string name_user;
        private string password;

        public string Rol { get => rol; set => rol = value; }
        public string Name_user { get => name_user; set => name_user = value; }
        public string Password { get => password; set => password = value; }
    }
}
