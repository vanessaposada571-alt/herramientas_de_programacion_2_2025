using System;
using Hospital.application.validators;
using Hospital.domain.model;
using Hospital.domain.services;

namespace Hospital.application.usecases
{
    /// <summary>
    /// Casos de uso para crear/actualizar órdenes (medicamento/procedimiento/ayuda diagnóstica).
    /// Ahora delega la persistencia/consulta a los servicios de dominio en domain\services.
    /// </summary>
    public class OrderUseCase
    {
        private readonly C_Order _createOrder;
        private readonly U_Order _updateOrder;
        private readonly S_Order _selectOrder;
        private readonly OrderValidator _validator;

        public OrderUseCase(C_Order createOrder, U_Order updateOrder, S_Order selectOrder, OrderValidator validator)
        {
            _createOrder = createOrder ?? throw new ArgumentNullException(nameof(createOrder));
            _updateOrder = updateOrder ?? throw new ArgumentNullException(nameof(updateOrder));
            _selectOrder = selectOrder ?? throw new ArgumentNullException(nameof(selectOrder));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public void CreateOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            _validator.ValidateForCreateOrUpdate(order);

            // Delegar la comprobación de unicidad y la persistencia al servicio de dominio
            _createOrder.Create(order);
        }

        public void UpdateOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            _validator.ValidateForCreateOrUpdate(order);

            // Delegar la existencia y actualización al servicio de dominio
            _updateOrder.Update(order);
        }

        public Order GetByIdOrder(Order probe)
        {
            if (probe == null) throw new ArgumentNullException(nameof(probe));
            return _selectOrder.Select(probe);
        }
    }
}