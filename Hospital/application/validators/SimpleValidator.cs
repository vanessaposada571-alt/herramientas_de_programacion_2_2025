using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.application.validators
{
    internal class SimpleValidator
    {
        public string StringNotNullOrEmpty(string value, string fieldName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception($"el campo {fieldName} no puede ser nulo o vacio");
            }
            return value;
        }

        public int IntNotNullOrEmpty(string value, string fieldName)
        {
            StringNotNullOrEmpty(value, fieldName);
            if (!int.TryParse(value, out int result))
            {
                throw new Exception($"el campo {fieldName} debe ser un numero entero");
            }
            return result;
        }
        public long LongNotNullOrEmpty(string value, string fieldName)
        {
            StringNotNullOrEmpty(value, fieldName);
            if (!long.TryParse(value, out long result))
            {
                throw new Exception($"el campo {fieldName} debe ser un numero entero largo");
            }
            return result;
        }

        public ulong ULongNotNullOrEmpty(string value, string fieldName)
        {
            StringNotNullOrEmpty(value, fieldName);
            if (!ulong.TryParse(value, out ulong result))
            {
                throw new Exception($"el campo {fieldName} debe ser un numero entero largo sin signo");
            }
            return result;
        }
    }
}