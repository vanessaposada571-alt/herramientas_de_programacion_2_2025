using System;
using System.Windows.Forms;
using Hospital.domain.model;
using Hospital.domain.ports;

namespace Hospital.application.forms
{
    public class PatientConsultationForm : Form
    {
        private readonly Patient _patient;
        private readonly Medical_insurance_port _insurancePort;

        private Label lblId;
        private Label lblName;
        private Label lblEmail;
        private Label lblCellphone;
        private Label lblBirth;
        private Label lblGender;
        private Label lblDirection;

        private Label lblContactName;
        private Label lblContactPhone;
        private Label lblContactRelation;

        private Label lblPolicyId;
        private Label lblPolicyNumber;
        private Label lblPolicyCompany;
        private Label lblPolicyEffective;

        private Button btnBack;

        public PatientConsultationForm(Patient patient, Medical_insurance_port insurancePort)
        {
            _patient = patient ?? throw new ArgumentNullException(nameof(patient));
            _insurancePort = insurancePort;
            InitializeComponents();
            MapData();
        }

        private void InitializeComponents()
        {
            Text = "Consulta Paciente";
            Width = 640;
            Height = 480;
            StartPosition = FormStartPosition.CenterParent;

            int leftLabel = 12, leftValue = 140;
            int top = 12, step = 26;

            Controls.Add(new Label { Text = "Id paciente:", Left = leftLabel, Top = top, Width = 120 });
            lblId = new Label { Left = leftValue, Top = top, Width = 460 }; top += step;

            Controls.Add(new Label { Text = "Nombre:", Left = leftLabel, Top = top, Width = 120 });
            lblName = new Label { Left = leftValue, Top = top, Width = 460 }; top += step;

            Controls.Add(new Label { Text = "Email:", Left = leftLabel, Top = top, Width = 120 });
            lblEmail = new Label { Left = leftValue, Top = top, Width = 460 }; top += step;

            Controls.Add(new Label { Text = "Teléfono:", Left = leftLabel, Top = top, Width = 120 });
            lblCellphone = new Label { Left = leftValue, Top = top, Width = 460 }; top += step;

            Controls.Add(new Label { Text = "Fecha nacimiento:", Left = leftLabel, Top = top, Width = 120 });
            lblBirth = new Label { Left = leftValue, Top = top, Width = 460 }; top += step;

            Controls.Add(new Label { Text = "Género:", Left = leftLabel, Top = top, Width = 120 });
            lblGender = new Label { Left = leftValue, Top = top, Width = 460 }; top += step;

            Controls.Add(new Label { Text = "Dirección:", Left = leftLabel, Top = top, Width = 120 });
            lblDirection = new Label { Left = leftValue, Top = top, Width = 460 }; top += step + 8;

            // Contacto de emergencia
            Controls.Add(new Label { Text = "Contacto (nombre):", Left = leftLabel, Top = top, Width = 120 });
            lblContactName = new Label { Left = leftValue, Top = top, Width = 460 }; top += step;

            Controls.Add(new Label { Text = "Contacto (teléfono):", Left = leftLabel, Top = top, Width = 120 });
            lblContactPhone = new Label { Left = leftValue, Top = top, Width = 460 }; top += step;

            Controls.Add(new Label { Text = "Relación:", Left = leftLabel, Top = top, Width = 120 });
            lblContactRelation = new Label { Left = leftValue, Top = top, Width = 460 }; top += step + 8;

            // Póliza
            Controls.Add(new Label { Text = "Id Póliza:", Left = leftLabel, Top = top, Width = 120 });
            lblPolicyId = new Label { Left = leftValue, Top = top, Width = 460 }; top += step;

            Controls.Add(new Label { Text = "Nº Póliza:", Left = leftLabel, Top = top, Width = 120 });
            lblPolicyNumber = new Label { Left = leftValue, Top = top, Width = 460 }; top += step;

            Controls.Add(new Label { Text = "Compañía:", Left = leftLabel, Top = top, Width = 120 });
            lblPolicyCompany = new Label { Left = leftValue, Top = top, Width = 460 }; top += step;

            Controls.Add(new Label { Text = "Vigencia:", Left = leftLabel, Top = top, Width = 120 });
            lblPolicyEffective = new Label { Left = leftValue, Top = top, Width = 460 }; top += step + 12;

            btnBack = new Button { Text = "Regresar", Left = 520, Top = top, Width = 80 };
            btnBack.Click += (_, _) => Close();

            Controls.Add(lblId);
            Controls.Add(lblName);
            Controls.Add(lblEmail);
            Controls.Add(lblCellphone);
            Controls.Add(lblBirth);
            Controls.Add(lblGender);
            Controls.Add(lblDirection);
            Controls.Add(lblContactName);
            Controls.Add(lblContactPhone);
            Controls.Add(lblContactRelation);
            Controls.Add(lblPolicyId);
            Controls.Add(lblPolicyNumber);
            Controls.Add(lblPolicyCompany);
            Controls.Add(lblPolicyEffective);
            Controls.Add(btnBack);
        }

        private void MapData()
        {
            // Id y nombre
            lblId.Text = _patient.Id_patient ?? string.Empty;
            lblName.Text = _patient.Name ?? _patient.Name1?.Name ?? string.Empty;

            // Email: intentar múltiples fuentes (Email wrapper o propiedad directa)
            string email = string.Empty;
            try
            {
                if (_patient.Email1?.Email?.Address != null)
                    email = _patient.Email1.Email.Address;
                else if (_patient.Email != null)
                    email = _patient.Email.Address;
            }
            catch
            {
                email = string.Empty;
            }
            lblEmail.Text = email;

            lblCellphone.Text = _patient.Cellphone1?.Cellphone != 0 ? _patient.Cellphone1.Cellphone.ToString() : string.Empty;
            lblBirth.Text = _patient.Birth != default ? _patient.Birth.ToShortDateString() : string.Empty;
            lblGender.Text = _patient.Gender1?.Gender ?? string.Empty;
            lblDirection.Text = _patient.Direction1?.Direction ?? _patient.Direction ?? string.Empty;

            if (_patient.Contact != null)
            {
                lblContactName.Text = _patient.Contact.Name1?.Name ?? _patient.Contact.Name ?? string.Empty;
                lblContactPhone.Text = _patient.Contact.Cellphone?.Cellphone != 0 ? _patient.Contact.Cellphone.Cellphone.ToString() : string.Empty;
                lblContactRelation.Text = _patient.Contact.Relation ?? string.Empty;
            }

            // Póliza: intentar por IdSure primero, si no usar PolicyNumber como fallback
            Medical_insurance mi = null;
            if (_patient.IdSure != null && _insurancePort != null && _patient.IdSure.IdSure != 0)
            {
                mi = _insurancePort.FindByIdSure(new Medical_insurance { IdSure = _patient.IdSure.IdSure });
            }
            else if (!string.IsNullOrWhiteSpace(_patient.PolicyNumber) && _insurancePort != null)
            {
                mi = _insurancePort.FindByPolicy_number(new Medical_insurance { Policy_number = _patient.PolicyNumber });
            }

            if (mi != null)
            {
                lblPolicyId.Text = mi.IdSure != 0 ? mi.IdSure.ToString() : string.Empty;
                lblPolicyNumber.Text = mi.Policy_number ?? string.Empty;
                lblPolicyCompany.Text = mi.Company_name ?? string.Empty;
                lblPolicyEffective.Text = mi.Effective_Date != default ? mi.Effective_Date.ToShortDateString() : string.Empty;
            }
            else
            {
                lblPolicyId.Text = string.Empty;
                lblPolicyNumber.Text = _patient.PolicyNumber ?? string.Empty;
                lblPolicyCompany.Text = string.Empty;
                lblPolicyEffective.Text = string.Empty;
            }
        }
    }
}