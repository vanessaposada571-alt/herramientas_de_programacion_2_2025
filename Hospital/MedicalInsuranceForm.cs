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
            Width = 620;
            Height = 560;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.FromArgb(30, 60, 100);

            var header = new Label()
            {
                Text = "GESTIÓN DE SEGURO MÉDICO",
                Dock = DockStyle.Top,
                Height = 80,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                BackColor = Color.FromArgb(25, 45, 75)
            };
            Controls.Add(header);

            var panel = new Panel()
            {
                Left = 25,
                Top = 100,
                Width = 560,
                Height = 420,
                BackColor = Color.FromArgb(235, 240, 255),
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(panel);

           
            var lblCompany = new Label { Text = "Nombre compañía:", Left = 20, Top = 20, Width = 150, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            txtCompanyName = new TextBox { Left = 180, Top = 17, Width = 340 };

            var lblPolicy = new Label { Text = "N° Póliza:", Left = 20, Top = 70, Width = 150, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            txtPolicyNumber = new TextBox { Left = 180, Top = 67, Width = 200 };

            btnLoad = new Button
            {
                Text = "Cargar",
                Left = 390,
                Top = 63,
                Width = 130,
                Height = 35,
                BackColor = Color.FromArgb(40, 90, 170),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnLoad.Click += BtnLoad_Click;

            var lblPatient = new Label { Text = "ID Paciente:", Left = 20, Top = 120, Width = 150, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            txtPatientId = new TextBox { Left = 180, Top = 117, Width = 200 };

            btnSearchByPatient = new Button
            {
                Text = "Buscar",
                Left = 390,
                Top = 113,
                Width = 130,
                Height = 35,
                BackColor = Color.FromArgb(40, 90, 170),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSearchByPatient.Click += BtnSearchByPatient_Click;

            lblPatientName = new Label
            {
                Text = "",
                Left = 180,
                Top = 150,
                Width = 330,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Italic)
            };

            var lblStatus = new Label { Text = "Activa:", Left = 20, Top = 190, Width = 150, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            chkPolicyStatus = new CheckBox { Left = 180, Top = 190, Width = 20, Checked = true };

            var lblEffective = new Label { Text = "Fecha vigencia:", Left = 20, Top = 240, Width = 150, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            dtpEffectiveDate = new DateTimePicker
            {
                Left = 180,
                Top = 237,
                Width = 200,
                Format = DateTimePickerFormat.Short
            };

            
            btnSave = new Button
            {
                Text = "Registrar",
                Left = 40,
                Top = 320,
                Width = 140,
                Height = 45,
                BackColor = Color.FromArgb(25, 140, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;

            btnUpdate = new Button
            {
                Text = "Actualizar",
                Left = 210,
                Top = 320,
                Width = 140,
                Height = 45,
                BackColor = Color.FromArgb(255, 160, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnUpdate.Click += BtnUpdate_Click;

            btnBack = new Button
            {
                Text = "Regresar",
                Left = 380,
                Top = 320,
                Width = 140,
                Height = 45,
                BackColor = Color.FromArgb(180, 50, 50),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnBack.Click += (_, _) => Close();

            
            panel.Controls.AddRange(new Control[]
            {
                lblCompany, txtCompanyName,
                lblPolicy, txtPolicyNumber, btnLoad,
                lblPatient, txtPatientId, btnSearchByPatient,
                lblPatientName,
                lblStatus, chkPolicyStatus,
                lblEffective, dtpEffectiveDate,
                btnSave, btnUpdate, btnBack
            });
        }

        private void BtnSearchByPatient_Click(object? sender, EventArgs e)
        {
            try
            {
                var patientId = txtPatientId.Text?.Trim();
                if (string.IsNullOrWhiteSpace(patientId))
                {
                    MessageBox.Show("Ingrese Id del paciente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var mi = _useCase.GetByPatientId(patientId);
                if (mi == null)
                {
                    MessageBox.Show("No se encontró póliza para este paciente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _loadedInsurance = mi;

                txtCompanyName.Text = mi.Company_name ?? "";
                txtPolicyNumber.Text = mi.Policy_number ?? "";
                chkPolicyStatus.Checked = mi.Policy_status;
                dtpEffectiveDate.Value = mi.Effective_Date == default ? DateTime.Today : mi.Effective_Date;

                lblPatientName.Text = mi.PatientName ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message);
            }
        }

        private void BtnLoad_Click(object? sender, EventArgs e)
        {
            try
            {
                var policy = txtPolicyNumber.Text?.Trim();
                if (string.IsNullOrWhiteSpace(policy))
                {
                    MessageBox.Show("Ingrese número de póliza.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _loadedInsurance = _useCase.GetByPolicyNumber(policy);

                if (_loadedInsurance == null)
                {
                    MessageBox.Show("No existe esta póliza.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                txtCompanyName.Text = _loadedInsurance.Company_name ?? "";
                chkPolicyStatus.Checked = _loadedInsurance.Policy_status;
                dtpEffectiveDate.Value = _loadedInsurance.Effective_Date == default ? DateTime.Today : _loadedInsurance.Effective_Date;

                var patient = _useCase.GetPatientByInsuranceId(_loadedInsurance.IdSure);
                txtPatientId.Text = patient?.Id_patient ?? "";
                lblPatientName.Text = patient?.Name ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar: " + ex.Message);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var model = MapFormToModel();
                _useCase.Register(model);
                MessageBox.Show("Póliza registrada correctamente.", "Éxito");
                _loadedInsurance = model;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message);
            }
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (_loadedInsurance == null)
            {
                MessageBox.Show("Cargue primero la póliza.", "Aviso");
                return;
            }

            try
            {
                var model = MapFormToModel();
                model.IdSure = _loadedInsurance.IdSure;

                _useCase.Update(model);

                MessageBox.Show("Póliza actualizada correctamente.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
        }

        private Medical_insurance MapFormToModel()
        {
            return new Medical_insurance
            {
                Company_name = txtCompanyName.Text.Trim(),
                Policy_number = txtPolicyNumber.Text.Trim(),
                Policy_status = chkPolicyStatus.Checked,
                Effective_Date = dtpEffectiveDate.Value.Date,
                PatientId = txtPatientId.Text.Trim()
            };
        }
    }
}
