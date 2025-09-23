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
        public Order FindByNumOrder(Order_help orderHelp);

        public void Save(Order_help orderHelp);
        public void Update(Order_help orderHelp);
        public void Search(Order_help orderHelp);
        public void Delete(Order_help orderHelp);
    }
}