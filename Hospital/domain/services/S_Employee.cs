using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
 public class S_Employee
 {
 private Employee_port employee_Port;

 public User Select(User user)
 {
 var found = employee_Port.FindById1(user);
 if (found == null)
 {
 throw new Exception("El empleado no existe");
 }
 return found;
 }
 }
}
