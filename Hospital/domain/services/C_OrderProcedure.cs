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

            /*if (orderProcedurePort.FindBySpecialist(orderProcedure) == false )
            {
                throw new Exception("No requiere atención de especialista");
            }*/
            if (orderProcedurePort.FindByNumOrder(orderProcedure) != null)
            {
                throw new Exception("Ya existe el número de orden");
            }
            if (orderProcedurePort.FindByIdProcedure(orderProcedure) != null)
            {
                throw new Exception("El procedimiento con ese ID ya existe");
            }
            if (orderProcedurePort.FindByItem(orderProcedure) != null)
            {
                throw new Exception("Ya existe un procedimiento asignado");
            }

            orderProcedurePort.Save(orderProcedure);

        }
    }
}
