using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Hospital.domain.services
{
    public class C_Employee
    {
        private readonly Employee_port employee_Port;

        public C_Employee(Employee_port employeePort)
        {
            employee_Port = employeePort ?? throw new ArgumentNullException(nameof(employeePort));
        }

        public void Create(User user)
        {

            if (employee_Port.FindById1(user) != null)
            {
                throw new Exception("Ya existe un empleado registrado con ese documento");
            }
            
            employee_Port.Save(user);
        }
    }
}

