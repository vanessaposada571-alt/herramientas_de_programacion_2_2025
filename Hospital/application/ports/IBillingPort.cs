csharp Hospital\application\ports\IBillingPort.cs
using Hospital.domain.model;
using System.Collections.Generic;

namespace Hospital.application.ports
{
    public interface IBillingPort
    {
        // Persistir factura
        void SaveInvoice(Billings billings);

        // Consulta total de copagos pagados por paciente en un año (para regla de exención)
        decimal GetPatientCopayTotalForYear(string patientId, int year);

        // Consultas auxiliares
        Billings FindById(Billings billings);
        Billings FindByOrderId(string orderId);
    }
}