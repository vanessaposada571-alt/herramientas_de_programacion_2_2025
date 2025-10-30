using Hospital.domain.model;
using System;
using System.Text.RegularExpressions;

namespace Hospital.application.validators
{
    public class EmergencyContactValidator
    {
        public ValidationResult Validate(Contact contact)
        {
            if (contact == null)
                return ValidationResult.Fail("Contacto de emergencia nulo.");

            // Nombre del contacto (puede estar en Name1.Name o en Name heredado)
            var nombre = contact.Name1?.Name ?? contact.Name;
            if (string.IsNullOrWhiteSpace(nombre))
                return ValidationResult.Fail("El nombre del contacto de emergencia es obligatorio.");

            // Relación con el paciente
            if (string.IsNullOrWhiteSpace(contact.Relation))
                return ValidationResult.Fail("La relación con el paciente es obligatoria.");

            // Teléfono de emergencia: se espera en contact.Cellphone (tipo Person) -> Person.Cellphone (long)
            var telefonoStr = contact.Cellphone != null ? contact.Cellphone.Cellphone.ToString() : string.Empty;
            if (string.IsNullOrWhiteSpace(telefonoStr))
                return ValidationResult.Fail("El número de teléfono de emergencia es obligatorio.");

            // Solo dígitos y exactamente 10 caracteres
            if (!Regex.IsMatch(telefonoStr, @"^\d{10}$"))
                return ValidationResult.Fail("El número de teléfono de emergencia debe contener exactamente 10 dígitos y solo números.");

            return ValidationResult.Success();
        }

        // Separa un nombre completo en "nombres" y "apellidos" (último token como apellidos)
        public (string FirstNames, string LastName) SplitFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return (string.Empty, string.Empty);

            var parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
                return (parts[0], string.Empty);

            var lastName = parts[^1];
            var firstNames = string.Join(" ", parts, 0, parts.Length - 1);
            return (firstNames, lastName);
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; }
        public string ErrorMessage { get; }

        private ValidationResult(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        public static ValidationResult Success() => new ValidationResult(true, null);
        public static ValidationResult Fail(string errorMessage) => new ValidationResult(false, errorMessage);
    }
}