using Hospital.domain.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.services
{
    internal class Create_user
    {
        public void Create (User user)
        {
            user.Rol = "";
            user.Name_user = "";
            user.Password = "";
        }
    }
}
