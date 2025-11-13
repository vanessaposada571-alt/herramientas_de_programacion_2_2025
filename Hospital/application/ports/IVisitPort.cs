csharp Hospital\application\ports\IVisitPort.cs
using Hospital.domain.model;
using System.Collections.Generic;

namespace Hospital.application.ports
{
    public interface IVisitPort
    {
        // Persistir visita de enfermería
        void Save(Visit visit);

        // Verificar existencia por número de orden/visita
        Visit FindByNumOrder(Visit visit);

        // Obtener visitas por paciente
        IEnumerable<Visit> GetByPatient(string patientId);
    }
}