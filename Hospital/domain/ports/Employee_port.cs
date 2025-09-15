using Hospital.domain.model;
using Hospital.domain.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.ports
{
    internal interface Employee_port
    {
        public User FindByPassword(User user);
        public Person FindById(User user);
        public User FindByName_user(User user);
        public void Save(User user);
        public void Update(User user);
    }
}