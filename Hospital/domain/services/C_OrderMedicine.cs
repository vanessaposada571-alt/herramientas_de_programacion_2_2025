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
    internal class C_OrderMedicine
    {
        private Order_medicine_port orderMedicinePort;
        public void Create(Order_medicine orderMedicine)
        {

            if (orderMedicinePort.FindByNumOrder(orderMedicine) != null)
            {
                throw new Exception("Ya existe el número de orden");
            }
            if (orderMedicinePort.FindByIdMedicine(orderMedicine) = null)
            {
                throw new Exception("No se tiene el medicamento");
            }
            if (orderMedicinePort.FindByItem(orderMedicine) = null)
            {
                throw new Exception("No existe el número de orden");
            }

            orderMedicinePort.Save(orderMedicine);
        }
    }
}
