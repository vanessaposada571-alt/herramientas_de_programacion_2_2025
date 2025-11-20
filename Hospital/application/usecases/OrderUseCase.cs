
using System;
using Hospital.application.validators;
using Hospital.domain.model;
using Hospital.domain.ports;

namespace Hospital.application.usecases
{
    /// <summary>
    /// Casos de uso para crear/actualizar órdenes (medicamento/procedimiento/ayuda diagnóstica).
    /// Usa el puerto de dominio Order_port para persistencia SQL.
    /// </summary>
    public class OrderUseCase
    {
        private readonly Order_port _orderPort;
        private readonly OrderValidator _validator;

        public OrderUseCase(Order_port orderPort, OrderValidator validator)
        {
            _orderPort = orderPort ?? throw new ArgumentNullException(nameof(orderPort));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public void CreateOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            _validator.ValidateForCreateOrUpdate(order);

            // Comprobar unicidad de número de orden (si el puerto lo soporta)
            var existing = _orderPort.FindByIDOrder(order);
            if (existing != null)
                throw new ArgumentException($"Ya existe una orden con el identificador {order.IdOrder1}.");

            _orderPort.Save(order);
        }

        public void UpdateOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            _validator.ValidateForCreateOrUpdate(order);

            // Se delega al puerto la existencia y actualización
            _orderPort.Update(order);
        }
    }
}