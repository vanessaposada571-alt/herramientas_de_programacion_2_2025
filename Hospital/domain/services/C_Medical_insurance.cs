using Hospital.domain.model;
using Hospital.domain.ports;
using System;

namespace Hospital.domain.services
{
    public class C_Medical_insurance
    {
        private readonly Medical_insurance_port insurancePort;
        private readonly Patient_port patientPort;

        public C_Medical_insurance(Medical_insurance_port insurancePort, Patient_port patientPort)
        {
            this.insurancePort = insurancePort ?? throw new ArgumentNullException(nameof(insurancePort));
            this.patientPort = patientPort ?? throw new ArgumentNullException(nameof(patientPort));
        }

        public void Create(Medical_insurance insurance)
        {
            if (insurance == null) throw new ArgumentNullException(nameof(insurance));

            // Validar que venga el identificador del paciente
            if (string.IsNullOrWhiteSpace(insurance.PatientId))
            {
                throw new Exception("La póliza debe especificar el Id del paciente (PatientId).");
            }

            // Verificar existencia del paciente
            var probePatient = new Patient { Id_patient = insurance.PatientId };
            var existingPatient = patientPort.FindById_patient(probePatient);
            if (existingPatient == null)
            {
                throw new Exception("No existe el paciente especificado.");
            }

            // Validaciones de unicidad de póliza
            if (insurancePort.FindByPolicy_number(insurance) != null)
            {
                throw new Exception("Ya existe una póliza con ese número");
            }
            if (insurancePort.FindByIdSure(insurance) != null)
            {
                throw new Exception("Ya existe el id del seguro");
            }

            // Guardar póliza
            insurancePort.Save(insurance);

            // Asociar la póliza en el paciente y actualizar
            // Se asume que insurancePort.Save ha poblado insurance.IdSure (o la implementación debe devolver el id)
            existingPatient.IdSure = insurance; // relacionar toda la entidad (según el model actual)
            existingPatient.PolicyNumber = insurance.Policy_number;
            patientPort.Update(existingPatient);
        }
    }
}
