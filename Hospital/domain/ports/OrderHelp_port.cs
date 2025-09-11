using Hospital.domain.model;
using Hospital.domain.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.ports
{
    internal interface Order_help_port
    {
        public Order_help FindByNumOrderA(Order_help orderHelp);
        public Order_help FindByIdHelp(Order_help orderHelp);
        public Order_help FindBySpecialist(Order_help orderHelp);
        public void Save(Order_help orderHelp);
        public void Update(Order_help orderHelp);
        public void Delete(Order_help orderHelp);
    }
}