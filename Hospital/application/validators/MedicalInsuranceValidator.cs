using System;
using Hospital.domain.model;

namespace Hospital.application.validators
{
    public class MedicalInsuranceValidator
    {
        public ValidationResult Validate(Medical_insurance insurance)
        {
            if (insurance == null)
                return ValidationResult.Fail("Seguro médico nulo.");

            // Nombre de la compañía
            if (string.IsNullOrWhiteSpace(insurance.Company_name))
                return ValidationResult.Fail("El nombre de la compañía del seguro es obligatorio.");
            if (insurance.Company_name.Length > 150)
                return ValidationResult.Fail("El nombre de la compañía es demasiado largo (máximo 150 caracteres).");

            // Número de póliza
            if (string.IsNullOrWhiteSpace(insurance.Policy_number))
                return ValidationResult.Fail("El número de póliza es obligatorio.");
            if (insurance.Policy_number.Length > 60)
                return ValidationResult.Fail("El número de póliza es demasiado largo (máximo 60 caracteres).");

            // Vigencia: fecha de finalización (Effective_Date) obligatoria y no anterior a hoy
            if (insurance.Effective_Date == default(DateTime))
                return ValidationResult.Fail("La fecha de finalización de la póliza es obligatoria y debe ser válida (dd/mm/yyyy).");
            if (insurance.Effective_Date < DateTime.Today)
                return ValidationResult.Fail("La vigencia de la póliza debe ser hoy o una fecha futura.");

            return ValidationResult.Success();
        }
    }
}
