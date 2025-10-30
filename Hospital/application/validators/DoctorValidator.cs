using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Hospital.application.validators
{
    internal class DoctorValidator: SimpleValidator
    {
        public string ValidateSpecialty(string specialty)
        {
            return StringNotNullOrEmpty(specialty, "specialty");
        }

        public string ValidateIdDoctor(string idDoctor)
        {
            return StringNotNullOrEmpty(idDoctor, "idDoctor");
        }

        private string StringNotNullOrEmpty(string value, string fieldName)
        {
            if (string.IsNullOrEmpty(value))
                throw new Exception($"El campo {fieldName} no puede estar vacío.");
            return value;
        }
    }
}