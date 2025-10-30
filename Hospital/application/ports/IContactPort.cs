using Hospital.domain.model;

namespace Hospital.application.ports
{
    public interface IContactPort
    {
        // Verifica existencia del paciente asociado (usado por C_Contact)
        Patient FindById_patient(Contact contact);

        // Operaciones de persistencia para contacto de emergencia
        void Save(Contact contact);
        void Update(Contact contact);
        void Delete(Contact contact);

        // Consulta
        Contact FindById(Contact contact);
    }
}