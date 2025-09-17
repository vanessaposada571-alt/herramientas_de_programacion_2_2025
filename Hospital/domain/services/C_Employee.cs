using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Hospital.domain.services
{
    internal class C_Employee
    {
        private Employee_port employee_Port;
        public void Create(User user)
        {

            if (employee_Port.FindByName_user(user) != null)
            {
                throw new Exception("Ya existe un empleado registrado con ese nombre");
            }
            if (employee_Port.FindById(user) != null)
            {
                throw new Exception("Ya existe un empleado registrado con ese documento");
            }

            employee_Port.Save(user);
        }
    }
}
