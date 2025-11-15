using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
 public class U_OrderHelp
 {
 private Order_help_port orderHelpPort;

 public void Update(Order_help orderHelp)
 {
 if (orderHelpPort.FindByNumOrder(orderHelp) == null)
 {
 throw new Exception("La orden de ayuda diagnóstica no existe");
 }

 orderHelpPort.Update(orderHelp);
 }
 }
}
