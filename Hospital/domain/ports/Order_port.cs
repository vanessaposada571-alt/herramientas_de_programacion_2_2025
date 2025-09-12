using Hospital.domain.model;
using Hospital.domain.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.ports
{
    internal interface Order_port
    {
        public Order FindByorder_Help(Order orderGrl);
        public Order FindByorder_Medicine(Order orderGrl);
        public Order FindByorder_Procedure(Order orderGrl);
        public void Save(Order orderGrl);
        public void Update(Order orderGrl);
    }
}
