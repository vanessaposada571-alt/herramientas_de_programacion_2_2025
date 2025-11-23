using Hospital.application.validators;
using Hospital.domain.model;
using Hospital.domain.services;

namespace Hospital.application.usecases
{
    public class AdministrativeUseCase
    {   
        private readonly C_Patient _createPatient;
        private readonly U_Patient _updatePatient;
        private readonly S_Patient _selectPatient;
        private readonly PatientValidator _validator;

        public AdministrativeUseCase(
            C_Patient createPatient,
            U_Patient updatePatient,
            S_Patient selectPatient,
            PatientValidator validator)
        {
            _createPatient = createPatient;
            _updatePatient = updatePatient;
            _selectPatient = selectPatient;
            _validator = validator;
        }

        public void RegisterPatient(Patient patient)
        {
            var result = _validator.Validate(patient);
            if (!result.IsValid)
                throw new System.ArgumentException(result.ErrorMessage);

            _createPatient.Create(patient);
        }

        public void UpdatePatient(Patient patient)
        {
            var result = _validator.Validate(patient);
            if (!result.IsValid)
                throw new System.ArgumentException(result.ErrorMessage);

            _updatePatient.Update(patient);
        }

        public Patient GetPatientById(Patient patient)
        {
            return _selectPatient.Search(patient);
        }
    }
}