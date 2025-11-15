using Hospital.domain.model;

namespace Hospital.application.ports
{
    public interface IHelpPort
    {
        Order_help FindByNumOrder(Order_help query);
        void Save(Order_help help);
        void Update(Order_help help);
        void Delete(Order_help help);
    }
}