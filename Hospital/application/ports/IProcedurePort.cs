using Hospital.domain.model;

namespace Hospital.application.ports
{
    public interface IProcedurePort
    {
        Order_procedure FindByNumOrder(Order_procedure query);
        void Save(Order_procedure procedure);
        void Update(Order_procedure procedure);
        void Delete(Order_procedure procedure);
    }
}
