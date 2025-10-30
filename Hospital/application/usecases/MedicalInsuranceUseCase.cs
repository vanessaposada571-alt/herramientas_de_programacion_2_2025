using System;
using Hospital.application.validators;
using Hospital.application.ports;
using Hospital.domain.model;
using Hospital.domain.services;

namespace Hospital.application.usecases
{
    public class MedicalInsuranceUseCase
    {
        private readonly C_Medical_insurance _createInsurance;
        private readonly U_Medical_insurance _updateInsurance;
        private readonly S_Medical_insurance _selectInsurance;
        private readonly MedicalInsuranceValidator _validator;
        private readonly IMedicalInsurancePort _port;

        public MedicalInsuranceUseCase(
            C_Medical_insurance createInsurance,
            U_Medical_insurance updateInsurance,
            S_Medical_insurance selectInsurance,
            MedicalInsuranceValidator validator,
            IMedicalInsurancePort port)
        {
            _createInsurance = createInsurance;
            _updateInsurance = updateInsurance;
            _selectInsurance = selectInsurance;
            _validator = validator;
            _port = port;
        }

        public void Register(Medical_insurance insurance)
        {
            var result = _validator.Validate(insurance);
            if (!result.IsValid)
                throw new ArgumentException(result.ErrorMessage);

            _createInsurance.Create(insurance);
        }

        public void Update(Medical_insurance insurance)
        {
            var result = _validator.Validate(insurance);
            if (!result.IsValid)
                throw new ArgumentException(result.ErrorMessage);

            _updateInsurance.Update(insurance);
        }

        public Medical_insurance GetByPolicyNumber(string policyNumber)
        {
            var probe = new Medical_insurance { Policy_number = policyNumber };
            return _selectInsurance.Select(probe);
        }
    }
}
