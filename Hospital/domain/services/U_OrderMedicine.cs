using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
 public class U_OrderMedicine
 {
 private Order_medicine_port orderMedicinePort;

 public void Update(Order_medicine orderMedicine)
 {
 if (orderMedicinePort.FindByNumOrder(orderMedicine) == null)
 {
 throw new Exception("La orden de medicina no existe");
 }

 orderMedicinePort.Update(orderMedicine);
 }
 }
}
