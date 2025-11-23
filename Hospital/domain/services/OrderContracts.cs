using Hospital.domain.model;

namespace Hospital.domain.services
{
    public interface IC_Order
    {
        void Create(Order order);
    }

    public interface IU_Order
    {
        void Update(Order order);
    }

    public interface IS_Order
    {
        Order Select(Order probe);
    }
}
