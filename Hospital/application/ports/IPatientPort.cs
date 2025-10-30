using Hospital.domain.model;

namespace Hospital.application.ports
{
    public interface IPatientPort
    {
        Patient FindById_patient(Patient patient);
        Contact FindByContact(Patient patient);
        Medical_insurance FindByIdSure(Patient patient);
        void Save(Patient patient);
        void Update(Patient patient);
        Patient FindById(Patient patient);
    }
}