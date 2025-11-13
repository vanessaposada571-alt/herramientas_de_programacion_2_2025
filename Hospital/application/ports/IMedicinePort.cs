csharp Hospital\application\ports\IMedicinePort.cs
using Hospital.domain.model;

namespace Hospital.application.ports
{
    public interface IMedicinePort
    {
        Order_medicine FindByNumOrder(Order_medicine query);
        void Save(Order_medicine medicine);
        void Update(Order_medicine medicine);
        void Delete(Order_medicine medicine);
    }
}