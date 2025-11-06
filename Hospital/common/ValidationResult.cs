namespace Hospital.Common
{
 // Shared ValidationResult used across the application
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
