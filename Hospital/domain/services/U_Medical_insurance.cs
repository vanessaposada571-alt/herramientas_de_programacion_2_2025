using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
 public class U_Medical_insurance
 {
 private Medical_insurance_port insurancePort;

 public void Update(Medical_insurance insurance)
 {
 if (insurancePort.FindByIdSure(insurance) == null)
 {
 throw new Exception("La póliza no existe");
 }
 insurancePort.Update(insurance);
 }
 }
}
