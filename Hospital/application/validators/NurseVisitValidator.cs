using System;
using System.Text.RegularExpressions;
using Hospital.domain.model;

namespace Hospital.application.validators
{
    public class NurseVisitValidator
    {
        public ValidationResult Validate(Visit visit)
        {
            if (visit == null) return ValidationResult.Fail("Visita nula.");

            // Presión arterial: si existe, formato "SYS/DIA"
            var pressureProp = visit.GetType().GetProperty("Pressure");
            if (pressureProp != null)
            {
                var p = pressureProp.GetValue(visit) as string;
                if (!string.IsNullOrWhiteSpace(p) && !Regex.IsMatch(p.Trim(), @"^\d{2,3}/\d{2,3}$"))
                    return ValidationResult.Fail("Presión arterial inválida. Formato esperado: 'SYS/DIA' (ej. 120/80).");
            }

            // Temperatura: si existe, rango 30-45
            var tempProp = visit.GetType().GetProperty("Temperature");
            if (tempProp != null)
            {
                var tempObj = tempProp.GetValue(visit);
                if (tempObj != null && decimal.TryParse(tempObj.ToString(), out var t))
                {
                    if (t < 30m || t > 45m) return ValidationResult.Fail("Temperatura fuera de rango razonable (30.0 - 45.0 °C).");
                }
            }

            // Pulso: si existe, rango 30-220
            var pulseProp = visit.GetType().GetProperty("Pulse");
            if (pulseProp != null)
            {
                var pObj = pulseProp.GetValue(visit);
                if (pObj != null && int.TryParse(pObj.ToString(), out var pulse))
                {
                    if (pulse < 30 || pulse > 220) return ValidationResult.Fail("Pulso fuera de rango razonable (30 - 220).");
                }
            }

            // Oxígeno: si existe, 0-100
            var oxProp = visit.GetType().GetProperty("Oxygen");
            if (oxProp != null)
            {
                var oObj = oxProp.GetValue(visit);
                if (oObj != null && int.TryParse(oObj.ToString(), out var ox))
                {
                    if (ox < 0 || ox > 100) return ValidationResult.Fail("Saturación de oxígeno inválida. Debe estar entre 0 y 100.");
                }
            }

            return ValidationResult.Success();
        }
    }
}