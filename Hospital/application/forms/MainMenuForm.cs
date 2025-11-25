using System;
using System.Windows.Forms;
using Hospital.application.usecases;
using Hospital.domain.model;
using Hospital.domain.ports;
using Hospital.infraestructure.adapters.output;

namespace Hospital.application.forms
{
    public class MainMenuForm : Form
    {
        private readonly HumanResourcesUseCase _hrUseCase;
        private readonly AdministrativeUseCase _administrativeUseCase;
        private readonly MedicalInsuranceUseCase _miUseCase;
        private readonly Patient_port _patientPort;
        private readonly Medical_insurance_port _insurancePort;

        public MainMenuForm(
            HumanResourcesUseCase hrUseCase,
            AdministrativeUseCase administrativeUseCase,
            MedicalInsuranceUseCase miUseCase,
            Patient_port patientPort,
            Medical_insurance_port insurancePort)
        {
            _hrUseCase = hrUseCase;
            _administrativeUseCase = administrativeUseCase;
            _miUseCase = miUseCase;
            _patientPort = patientPort;
            _insurancePort = insurancePort;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Text = "Seleccionar módulo";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 420;
            Height = 220;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var btnHR = new Button { Text = "Recursos Humanos", Left = 20, Top = 40, Width = 120 };
            var btnPatient = new Button { Text = "Registrar Paciente", Left = 150, Top = 40, Width = 140 };
            var btnPolicy = new Button { Text = "Pólizas", Left = 300, Top = 40, Width = 80 };
            var btnConsult = new Button { Text = "Consulta Paciente", Left = 120, Top = 90, Width = 160 };

            btnHR.Click += (_, _) =>
            {
                using var f = new HumanResourcesForm(_hrUseCase);
                f.ShowDialog(this);
            };

            btnPatient.Click += (_, _) =>
            {
                using var f = new AdministrativeForm(_administrativeUseCase);
                f.ShowDialog(this);
            };

            btnPolicy.Click += (_, _) =>
            {
                using var f = new MedicalInsuranceForm(_miUseCase);
                f.ShowDialog(this);
            };

            btnConsult.Click += (_, _) => ShowPatientConsultation();

            Controls.Add(btnHR);
            Controls.Add(btnPatient);
            Controls.Add(btnPolicy);
            Controls.Add(btnConsult);
        }

        private void ShowPatientConsultation()
        {
            // pedir Id paciente
            var patientId = Prompt.ShowDialog("Ingrese Id del paciente:", "Buscar paciente");
            if (string.IsNullOrWhiteSpace(patientId)) return;

            var probe = new Patient { Id_patient = patientId.Trim() };
            var found = _patientPort.FindById_patient(probe);
            if (found == null)
            {
                MessageBox.Show("No se encontró el paciente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var consultForm = new PatientConsultationForm(found, _insurancePort);
            consultForm.ShowDialog(this);
        }

        // simple InputBox helper
        internal static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                using var prompt = new Form()
                {
                    Width = 360,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterParent,
                    Text = caption,
                    MinimizeBox = false,
                    MaximizeBox = false
                };
                var textLabel = new Label() { Left = 12, Top = 12, Text = text, Width = 320 };
                var inputBox = new TextBox() { Left = 12, Top = 36, Width = 320 };
                var confirmation = new Button() { Text = "Aceptar", Left = 180, Width = 75, Top = 68, DialogResult = DialogResult.OK };
                var cancel = new Button() { Text = "Cancelar", Left = 260, Width = 75, Top = 68, DialogResult = DialogResult.Cancel };
                confirmation.Click += (_, _) => { prompt.Close(); };
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(inputBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);
                prompt.AcceptButton = confirmation;
                return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : string.Empty;
            }
        }
    }
}