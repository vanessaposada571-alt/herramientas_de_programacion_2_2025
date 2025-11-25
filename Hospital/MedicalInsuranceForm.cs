using System;
using System.Windows.Forms;
using Hospital.application.usecases;
using Hospital.domain.model;

namespace Hospital.application.forms
{
    public class MedicalInsuranceForm : Form
    {
        private readonly MedicalInsuranceUseCase _useCase;

        private TextBox txtCompanyName;
        private TextBox txtPolicyNumber;
        private TextBox txtPatientId;
        private Label lblPatientName; // visual
        private CheckBox chkPolicyStatus;
        private DateTimePicker dtpEffectiveDate;
        private Button btnSave;
        private Button btnLoad;
        private Button btnUpdate;
        private Button btnSearchByPatient;
        private Button btnBack; // regresar al menú

        private Medical_insurance? _loadedInsurance;

        public MedicalInsuranceForm(MedicalInsuranceUseCase useCase)
        {
            _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Text = "Póliza - Seguro Médico";
            Width = 420;
            Height = 360;
            StartPosition = FormStartPosition.CenterParent;

            var lblCompany = new Label { Text = "Nombre compañía:", Left = 12, Top = 16, Width = 120 };
            txtCompanyName = new TextBox { Left = 140, Top = 12, Width = 250 };

            var lblPolicy = new Label { Text = "Nº Póliza:", Left = 12, Top = 52, Width = 120 };
            txtPolicyNumber = new TextBox { Left = 140, Top = 48, Width = 180 };

            btnLoad = new Button { Text = "Cargar", Left = 330, Top = 46, Width = 60 };
            btnLoad.Click += BtnLoad_Click;

            var lblPatient = new Label { Text = "Id Paciente:", Left = 12, Top = 88, Width = 120 };
            txtPatientId = new TextBox { Left = 140, Top = 84, Width = 180 };

            btnSearchByPatient = new Button { Text = "Buscar", Left = 330, Top = 82, Width = 60 };
            btnSearchByPatient.Click += BtnSearchByPatient_Click;

            lblPatientName = new Label { Text = string.Empty, Left = 140, Top = 110, Width = 250 };

            var lblStatus = new Label { Text = "Activa:", Left = 12, Top = 140, Width = 120 };
            chkPolicyStatus = new CheckBox { Left = 140, Top = 138, Width = 20, Checked = true };

            var lblEffective = new Label { Text = "Vigencia (fecha):", Left = 12, Top = 176, Width = 120 };
            dtpEffectiveDate = new DateTimePicker { Left = 140, Top = 172, Width = 200, Format = DateTimePickerFormat.Short };

            btnSave = new Button { Text = "Registrar", Left = 60, Top = 220, Width = 100 };
            btnSave.Click += BtnSave_Click;

            btnUpdate = new Button { Text = "Actualizar", Left = 180, Top = 220, Width = 100 };
            btnUpdate.Click += BtnUpdate_Click;

            btnBack = new Button { Text = "Regresar", Left = 300, Top = 220, Width = 100 };
            btnBack.Click += (_, _) => Close();

            Controls.Add(lblCompany);
            Controls.Add(txtCompanyName);
            Controls.Add(lblPolicy);
            Controls.Add(txtPolicyNumber);
            Controls.Add(btnLoad);
            Controls.Add(lblPatient);
            Controls.Add(txtPatientId);
            Controls.Add(btnSearchByPatient);
            Controls.Add(lblPatientName);
            Controls.Add(lblStatus);
            Controls.Add(chkPolicyStatus);
            Controls.Add(lblEffective);
            Controls.Add(dtpEffectiveDate);
            Controls.Add(btnSave);
            Controls.Add(btnUpdate);
            Controls.Add(btnBack);
        }

        private void BtnSearchByPatient_Click(object? sender, EventArgs e)
        {
            try
            {
                var patientId = txtPatientId.Text?.Trim();
                if (string.IsNullOrWhiteSpace(patientId))
                {
                    MessageBox.Show("Ingrese Id del paciente para buscar la póliza.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var mi = _useCase.GetByPatientId(patientId);
                if (mi == null)
                {
                    MessageBox.Show("No se encontró póliza asociada al paciente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _loadedInsurance = mi;

                // Mapear al formulario
                txtCompanyName.Text = mi.Company_name ?? string.Empty;
                txtPolicyNumber.Text = mi.Policy_number ?? string.Empty;
                txtPatientId.Text = patientId;
                lblPatientName.Text = mi.PatientName ?? string.Empty;
                chkPolicyStatus.Checked = mi.Policy_status;
                dtpEffectiveDate.Value = mi.Effective_Date == default ? DateTime.Today : mi.Effective_Date;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar por paciente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLoad_Click(object? sender, EventArgs e)
        {
            try
            {
                var policy = txtPolicyNumber.Text?.Trim();
                if (string.IsNullOrWhiteSpace(policy))
                {
                    MessageBox.Show("Ingrese número de póliza para cargar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _loadedInsurance = _useCase.GetByPolicyNumber(policy);
                if (_loadedInsurance == null)
                {
                    MessageBox.Show("No se encontró la póliza.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Mapear al formulario (solo campos requeridos)
                txtCompanyName.Text = _loadedInsurance.Company_name ?? string.Empty;
                txtPolicyNumber.Text = _loadedInsurance.Policy_number ?? string.Empty;
                chkPolicyStatus.Checked = _loadedInsurance.Policy_status;
                dtpEffectiveDate.Value = _loadedInsurance.Effective_Date == default ? DateTime.Today : _loadedInsurance.Effective_Date;

                // Obtener paciente asociado (si existe) para mostrar Id y Nombre (solo visual)
                var patient = _useCase.GetPatientByInsuranceId(_loadedInsurance.IdSure);
                if (patient != null)
                {
                    txtPatientId.Text = patient.Id_patient ?? string.Empty;
                    lblPatientName.Text = patient.Name ?? patient.Name1?.Name ?? string.Empty;
                }
                else
                {
                    txtPatientId.Text = string.Empty;
                    lblPatientName.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var model = MapFormToModel();
                _useCase.Register(model);
                MessageBox.Show("Póliza registrada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _loadedInsurance = model;
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_loadedInsurance == null)
                {
                    MessageBox.Show("Cargue primero la póliza a actualizar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var model = MapFormToModel();
                model.IdSure = _loadedInsurance.IdSure; // conservar id para update
                _useCase.Update(model);
                MessageBox.Show("Póliza actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _loadedInsurance = model;
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Medical_insurance MapFormToModel()
        {
            var m = new Medical_insurance
            {
                Company_name = txtCompanyName.Text?.Trim() ?? string.Empty,
                Policy_number = txtPolicyNumber.Text?.Trim() ?? string.Empty,
                Policy_status = chkPolicyStatus.Checked,
                Effective_Date = dtpEffectiveDate.Value.Date,
                PatientId = txtPatientId.Text?.Trim() ?? string.Empty // importante: solo ID
            };

            return m;
        }
    }
}