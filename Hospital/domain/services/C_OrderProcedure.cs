using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Hospital.domain.services
{
    internal class C_OrderProcedure
    {
        private Order_procedure_port orderProcedurePort;
        public void Create(Order_procedure orderProcedure)
        {

            if (orderProcedurePort.FindByNumOrder(orderProcedure) != null) // se trae la orden, la orden trae el procedimiento (si existe o no)
            {
                throw new Exception("Ya existe el número de orden");
            }

            orderProcedurePort.Save(orderProcedure);

        }
    }
}
