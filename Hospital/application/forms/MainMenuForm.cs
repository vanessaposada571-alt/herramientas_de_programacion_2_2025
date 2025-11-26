using System;
using System.Drawing;
using System.Windows.Forms;
using Hospital.application.usecases;
using Hospital.domain.model;
using Hospital.domain.ports;
using Hospital;

namespace Hospital.application.forms
{
    public class MainMenuForm : Form
    {
        private readonly HumanResourcesUseCase _hrUseCase;
        private readonly AdministrativeUseCase _administrativeUseCase;
        private readonly MedicalInsuranceUseCase _miUseCase;
        private readonly Patient_port _patientPort;
        private readonly Medical_insurance_port _insurancePort;

        // ❌ Eliminado: este formulario es el menú principal; no regresa a nadie
        // private readonly Form _formAnterior;

        public MainMenuForm(
            HumanResourcesUseCase hrUseCase,
            AdministrativeUseCase administrativeUseCase,
            MedicalInsuranceUseCase miUseCase,
            Patient_port patientPort,
            Medical_insurance_port insurancePort,
            Form selectionWindow)
        {
            _hrUseCase = hrUseCase;
            _administrativeUseCase = administrativeUseCase;
            _miUseCase = miUseCase;
            _patientPort = patientPort;
            _insurancePort = insurancePort;

            InitializeModernUI();
        }

        private void InitializeModernUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            Width = 520;
            Height = 400;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            var header = new Panel()
            {
                BackColor = Color.FromArgb(30, 60, 100),
                Dock = DockStyle.Top,
                Height = 70,
            };

            var title = new Label()
            {
                Text = "Menú Principal - Hospital PB",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold)
            };

            var btnClose = new Button()
            {
                Text = "X",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Width = 40,
                Height = 40,
                Top = 5,
                Left = this.Width - 50,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (_, _) => this.Close();

            header.Controls.Add(btnClose);
            header.Controls.Add(title);
            Controls.Add(header);

            Button CreateModernButton(string text, int top)
            {
                var btn = new Button()
                {
                    Text = text,
                    Width = 320,
                    Height = 45,
                    Top = top,
                    Left = (this.Width - 320) / 2,
                    BackColor = Color.FromArgb(50, 120, 200),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };

                btn.FlatAppearance.BorderSize = 0;

                btn.Region = System.Drawing.Region.FromHrgn(
                    WinAPI.CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 20, 20));

                return btn;
            }

            var btnHR = CreateModernButton("Recursos Humanos", 100);
            var btnPatient = CreateModernButton("Registrar Paciente", 155);
            var btnPolicy = CreateModernButton("Pólizas Médicas", 210);
            var btnConsult = CreateModernButton("Consulta de Pacientes", 265);

            btnHR.Click += (_, _) =>
            {
                using var f = new HumanResourcesForm(_hrUseCase);
                f.ShowDialog(this);
            };

            btnPatient.Click += (_, _) =>
            {
                using var f = new AdministrativeForm(_administrativeUseCase, this);
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
            var patientId = Prompt.ShowDialog("Ingrese Id del paciente:", "Buscar paciente");
            if (string.IsNullOrWhiteSpace(patientId)) return;

            var probe = new Patient { Id_patient = patientId.Trim() };
            var found = _patientPort.FindById_patient(probe);

            if (found == null)
            {
                MessageBox.Show("No se encontró el paciente.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var f = new PatientConsultationForm(found, _insurancePort);
            f.ShowDialog(this);
        }

        internal static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                using var prompt = new Form()
                {
                    Width = 480,
                    Height = 220,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterParent,
                    Text = caption,
                    BackColor = Color.White
                };

                var header = new Panel()
                {
                    BackColor = Color.FromArgb(30, 60, 100),
                    Dock = DockStyle.Top,
                    Height = 45,
                };

                var lblTitle = new Label()
                {
                    Text = caption,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };

                header.Controls.Add(lblTitle);

                var textLabel = new Label()
                {
                    Left = 20,
                    Top = 60,
                    Text = text,
                    Width = 340,
                    Font = new Font("Segoe UI", 10)
                };

                var inputBox = new TextBox()
                {
                    Left = 20,
                    Top = 90,
                    Width = 340,
                    Font = new Font("Segoe UI", 11)
                };

                var confirmation = new Button()
                {
                    Text = "Buscar",
                    Left = 140,
                    Width = 120,
                    Top = 125,
                    Height = 40,
                    DialogResult = DialogResult.OK,
                    BackColor = Color.FromArgb(50, 120, 200),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };

                var cancel = new Button()
                {
                    Text = "Cancelar",
                    Left = 270,
                    Width = 120,
                    Top = 125,
                    Height = 40,
                    DialogResult = DialogResult.Cancel,
                    BackColor = Color.FromArgb(180, 50, 50),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };

                confirmation.Click += (_, _) => { prompt.Close(); };
                cancel.FlatAppearance.BorderSize = 0;
                confirmation.FlatAppearance.BorderSize = 0;

                prompt.Controls.Add(header);
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(inputBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : string.Empty;
            }
        }
    }

    public static class WinAPI
    {
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );
    }
}