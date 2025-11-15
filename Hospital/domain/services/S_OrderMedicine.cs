using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
 public class S_OrderMedicine
 {
 private Order_medicine_port orderMedicinePort;

 public Order_medicine FindByNumOrder(Order_medicine orderMedicine)
 {
 return orderMedicinePort.FindByNumOrder(orderMedicine);
 }
 }
}
