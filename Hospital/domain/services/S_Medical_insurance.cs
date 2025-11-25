using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
    public class S_Medical_insurance
    {
        private readonly Medical_insurance_port insurancePort;
        private readonly Patient_port patientPort;

        public S_Medical_insurance(Medical_insurance_port insurancePort, Patient_port patientPort)
        {
            this.insurancePort = insurancePort ?? throw new ArgumentNullException(nameof(insurancePort));
            this.patientPort = patientPort ?? throw new ArgumentNullException(nameof(patientPort));
        }

        public Medical_insurance Select(Medical_insurance probe)
        {
            return insurancePort.FindByPolicy_number(probe);
        }

        // Nuevo: obtener póliza a partir del PatientId (ya existía SelectByPatientId en iteraciones previas)
        public Medical_insurance SelectByPatientId(string patientId)
        {
            if (string.IsNullOrWhiteSpace(patientId)) return null;
            var patientProbe = new Patient { Id_patient = patientId };
            var patient = patientPort.FindById_patient(patientProbe);
            if (patient == null) return null;

            if (patient.IdSure != null && patient.IdSure.IdSure != 0)
            {
                var miProbe = new Medical_insurance { IdSure = patient.IdSure.IdSure };
                var mi = insurancePort.FindByIdSure(miProbe);
                if (mi != null)
                {
                    mi.PatientId = patient.Id_patient;
                    mi.PatientName = patient.Name ?? patient.Name1?.Name ?? string.Empty;
                }
                return mi;
            }

            if (!string.IsNullOrWhiteSpace(patient.PolicyNumber))
            {
                var miProbe = new Medical_insurance { Policy_number = patient.PolicyNumber };
                var mi = insurancePort.FindByPolicy_number(miProbe);
                if (mi != null)
                {
                    mi.PatientId = patient.Id_patient;
                    mi.PatientName = patient.Name ?? patient.Name1?.Name ?? string.Empty;
                }
                return mi;
            }

            return null;
        }

        // Nuevo: obtener paciente por InsuranceId
        public Patient GetPatientByInsuranceId(int insuranceId)
        {
            if (insuranceId == 0) return null;
            var probe = new Patient { IdSure = new Medical_insurance { IdSure = insuranceId } };
            return patientPort.FindByIdSure(probe);
        }

        // Nuevo: obtener paciente por Id
        public Patient GetPatientById(string patientId)
        {
            if (string.IsNullOrWhiteSpace(patientId)) return null;
            var probe = new Patient { Id_patient = patientId };
            return patientPort.FindById_patient(probe);
        }
    }
}
