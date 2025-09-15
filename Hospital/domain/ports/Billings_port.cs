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
        public Billings FindById_billings(Billings billings);
        public Patient FindById_Patient(Billings billings);
        public Medical_insurance FindByPolicy_number(Billings billings);
        public Medical_insurance FindByInsurance_cost(Billings billings);
        public void Save(Billings billings);
        public void Update(Billings billings);
    }
}
