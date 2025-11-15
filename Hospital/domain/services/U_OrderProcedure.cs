using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
 public class U_OrderProcedure
 {
 private Order_procedure_port orderProcedurePort;

 public void Update(Order_procedure orderProcedure)
 {
 if (orderProcedurePort.FindByNumOrder(orderProcedure) == null)
 {
 throw new Exception("La orden de procedimiento no existe");
 }

 orderProcedurePort.Update(orderProcedure);
 }
 }
}
