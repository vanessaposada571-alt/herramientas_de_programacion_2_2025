using Hospital.domain.model;
using Hospital.domain.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.ports
{
    internal interface Billings_port
    {
        public Person FindByName(Billings billings);
        public Doctor FindByDoctor(Billings billings);
        public Patient FindByPolicyNumber(Billings billings);
        public void Save(Billings billings);
        public void Update(Billings billings);
    }
}
