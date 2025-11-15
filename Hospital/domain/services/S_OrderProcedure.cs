using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
 public class S_OrderProcedure
 {
 private Order_procedure_port orderProcedurePort;

 public Order_procedure FindByNumOrder(Order_procedure orderProcedure)
 {
 return orderProcedurePort.FindByNumOrder(orderProcedure);
 }
 }
}
