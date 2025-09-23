using Hospital.domain.model;
using Hospital.domain.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.ports
{
    internal interface Visit_port
    {
        public Order FindByNumOrder(Visit visit);
        
        public void Save(Visit visit);
        public void Update(Visit visit);
        public void Search(Visit visit);


    }
}

