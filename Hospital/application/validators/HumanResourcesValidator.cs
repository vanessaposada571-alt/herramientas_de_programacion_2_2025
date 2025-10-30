using Hospital.domain.model;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Hospital.application.validators
{
    public class HumanResourcesValidator
    {
        public ValidationResult Validate(User user)
        {
            // Validación de nombre completo
            if (string.IsNullOrWhiteSpace(user.Name1?.Name))
                return ValidationResult.Fail("El nombre completo es obligatorio.");

            // Validación de número de cédula
            if (user.Id1 == null || user.Id1.Id <= 0)
                return ValidationResult.Fail("El número de cédula es obligatorio y debe ser mayor a cero.");

            /* if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return ValidationResult.Fail("El correo electrónico no es válido.");*/

            // Validación de correo electrónico
            if (user.Email1?.Email == null || !IsValidEmail(user.Email1.Email.Address))
                return ValidationResult.Fail("El correo electrónico no es válido.");

            // Validación de número de teléfono
            if (user.Cellphone1 == null || user.Cellphone1.Cellphone.ToString().Length < 5 || user.Cellphone1.Cellphone.ToString().Length > 10)
                return ValidationResult.Fail("El número de teléfono debe tener entre 5 y 10 dígitos.");

            // Validación de fecha de nacimiento
            if (user.Birth1 == null || user.Birth1.Birth > DateTime.Now || user.Birth1.Birth < DateTime.Now.AddYears(-150))
                return ValidationResult.Fail("La fecha de nacimiento no es válida.");

            // Validación de dirección
            if (user.Direction1?.Direction != null && user.Direction1.Direction.Length > 30)
                return ValidationResult.Fail("La dirección debe tener máximo 30 caracteres.");

            // Validación de rol
            if (string.IsNullOrWhiteSpace(user.Rol))
                return ValidationResult.Fail("El rol es obligatorio.");

            // Validación de nombre de usuario
            if (string.IsNullOrWhiteSpace(user.Name_user) || user.Name_user.Length > 15)
                return ValidationResult.Fail("El nombre de usuario debe ser único y máximo 15 caracteres.");

            // Validación de contraseña
            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 6)
                return ValidationResult.Fail("La contraseña debe tener mínimo 6 caracteres.");

            return ValidationResult.Success();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
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