using Hospital.domain.model;
using Hospital.domain.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.ports
{
    internal interface Contact_port
    {
        public Person FindByName1(Contact contact);
        public Patient FindById_patient(Contact contact);
        public Person FindByCellphone(Contact contact);
        public void Save(Contact contact);
        public void Delete(Contact contact);
        public void Update(Contact contact);
        public void Search(Contact contact);
    }
}
