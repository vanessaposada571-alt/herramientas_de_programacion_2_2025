
using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.domain.model;

namespace Hospital.application.validators
{
    /// <summary>
    /// Validaciones de reglas de negocio para órdenes según especificación:
    /// - Número de orden máximo 6 dígitos.
    /// - Si hay una ayuda diagnóstica no puede convivir con medicamentos/procedimientos en la misma orden.
    /// - Los ítems dentro de la orden deben ser únicos (no repetir el mismo número de ítem).
    /// - Campos mínimos requeridos para cada subtipo.
    /// </summary>
    public class OrderValidator
    {
        private const int MAX_ORDER_NUMBER = 999999;

        public void ValidateForCreateOrUpdate(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            if (order.IdOrder1 <= 0 || order.IdOrder1 > MAX_ORDER_NUMBER)
                throw new ArgumentException($"El número de orden debe ser positivo y como máximo {MAX_ORDER_NUMBER} (6 dígitos).", nameof(order));

            var items = new List<long>();

            if (order.NumOrder1 != null)
            {
                // medicamento
                if (order.NumOrder1.IdMedicine <= 0)
                    throw new ArgumentException("Medicamento: Id del medicamento inválido.", nameof(order));
                if (order.NumOrder1.Item <= 0)
                    throw new ArgumentException("Medicamento: el número de ítem debe ser mayor que 0.", nameof(order));
                items.Add(order.NumOrder1.Item);
            }

            if (order.NumOrderP1 != null)
            {
                // procedimiento
                if (order.NumOrderP1.IdProcedure <= 0)
                    throw new ArgumentException("Procedimiento: Id del procedimiento inválido.", nameof(order));
                if (order.NumOrderP1.Item <= 0)
                    throw new ArgumentException("Procedimiento: el número de ítem debe ser mayor que 0.", nameof(order));
                items.Add(order.NumOrderP1.Item);
            }

            if (order.NumOrderA1 != null)
            {
                // ayuda diagnóstica
                if (order.NumOrderA1.IdHelp <= 0)
                    throw new ArgumentException("Ayuda diagnóstica: Id inválido.", nameof(order));
                if (order.NumOrderA1.Item <= 0)
                    throw new ArgumentException("Ayuda diagnóstica: el número de ítem debe ser mayor que 0.", nameof(order));
                items.Add(order.NumOrderA1.Item);
            }

            // Regla: cuando se receta una ayuda diagnostica no puede recetarse procedimiento ni medicamento
            if (order.NumOrderA1 != null && (order.NumOrder1 != null || order.NumOrderP1 != null))
            {
                throw new ArgumentException("Una orden que contiene una ayuda diagnóstica no puede contener medicamentos ni procedimientos.");
            }

            // Regla: no puede existir dos elementos en la misma orden con el mismo ítem
            var duplicates = items.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (duplicates.Any())
            {
                throw new ArgumentException($"Los números de ítem dentro de la orden deben ser únicos. Ítems duplicados: {string.Join(',', duplicates)}");
            }

            // Más reglas de secuencia (ítems empezando en 1, etc.) pueden añadirse si se envía un conjunto completo de ítems.
        }
    }
}