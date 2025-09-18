using Hospital.domain.model;
using Hospital.domain.ports;
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
        public User FindById1(User user);
        public User FindByName_user(User user);
        public User FindByEmail1(User user);
        public User FindByCellphone1(User user);
        public User FindByBirth1(User user);
        public User FindByName1(User user);
        public User FindByDirection1(User user);
        public User FindByRol(User user);
        public void Save(User user);
        public void Update(User user);
        public void Delete(User user);
        public void Search(User user);
    }
}

