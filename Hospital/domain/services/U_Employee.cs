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
    internal class U_Employee
    {
        private Employee_port employee_Port;
        public void Update(User user, string list)
        {
            if (employee_Port.FindById(user) == null)
            {
                throw new Exception("El empleado no existe en el sistema");
            
            }
            employee_Port.Update(user);
        }
    }
}
