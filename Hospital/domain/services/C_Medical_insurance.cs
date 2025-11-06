using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
 public class C_Medical_insurance
 {
 private Medical_insurance_port insurancePort;

 public void Create(Medical_insurance insurance)
 {
 if (insurancePort.FindByPolicy_number(insurance) != null)
 {
 throw new Exception("Ya existe una póliza con ese número");
 }
 if (insurancePort.FindByIdSure(insurance) != null)
 {
 throw new Exception("Ya existe el id del seguro");
 }
 insurancePort.Save(insurance);
 }
 }
}
