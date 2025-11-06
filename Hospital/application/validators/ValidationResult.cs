// Facade wrapper that re-exports the shared ValidationResult into the validators namespace
using Hospital.Common;

namespace Hospital.application.validators
{
 public sealed class ValidationResult
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
