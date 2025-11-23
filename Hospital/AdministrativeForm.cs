using System;
using System.Drawing;
using System.Net.Mail;
using System.Windows.Forms;
using Hospital.domain.model;

namespace Hospital
{
    public class AdministrativeForm : Form
    {
        private readonly application.usecases.AdministrativeUseCase _useCase;

        private Label lblIdPatient;
        private TextBox txtIdPatient;
        private Label lblName;
        private TextBox txtName;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblCellphone;
        private TextBox txtCellphone;
        private Label lblBirth;
        private DateTimePicker dtpBirth;
        private Label lblGender;
        private ComboBox cmbGender;
        private Label lblDirection;
        private TextBox txtDirection;

        // Controles nuevos para contacto de emergencia
        private Label lblContactName;
        private TextBox txtContactName;
        private Label lblContactRelation;
        private TextBox txtContactRelation;
        private Label lblContactPhone;
        private TextBox txtContactPhone;

        private Button btnRegister;
        private Button btnUpdate;
        private Button btnGet;

        public AdministrativeForm(application.usecases.AdministrativeUseCase useCase)
        {
            _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Text = "Administración de Pacientes";
            Size = new Size(520, 460);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            int leftCol = 20;
            int labelWidth = 120;
            int inputLeft = leftCol + labelWidth + 10;
            int y = 20;
            int rowHeight = 30;
            int inputWidth = 350;

            lblIdPatient = new Label { Text = "Id paciente:", Left = leftCol, Top = y, Width = labelWidth };
            txtIdPatient = new TextBox { Left = inputLeft, Top = y - 4, Width = inputWidth };
            y += rowHeight;

            lblName = new Label { Text = "Nombre:", Left = leftCol, Top = y, Width = labelWidth };
            txtName = new TextBox { Left = inputLeft, Top = y - 4, Width = inputWidth };
            y += rowHeight;

            lblEmail = new Label { Text = "Email:", Left = leftCol, Top = y, Width = labelWidth };
            txtEmail = new TextBox { Left = inputLeft, Top = y - 4, Width = inputWidth };
            y += rowHeight;

            lblCellphone = new Label { Text = "Teléfono:", Left = leftCol, Top = y, Width = labelWidth };
            txtCellphone = new TextBox { Left = inputLeft, Top = y - 4, Width = inputWidth };
            y += rowHeight;

            lblBirth = new Label { Text = "Nacimiento:", Left = leftCol, Top = y, Width = labelWidth };
            dtpBirth = new DateTimePicker { Left = inputLeft, Top = y - 4, Width = 200, Format = DateTimePickerFormat.Short };
            y += rowHeight;

            lblGender = new Label { Text = "Género:", Left = leftCol, Top = y, Width = labelWidth };
            cmbGender = new ComboBox { Left = inputLeft, Top = y - 4, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbGender.Items.AddRange(new[] { "No especificado", "Masculino", "Femenino" });
            cmbGender.SelectedIndex = 0;
            y += rowHeight;

            lblDirection = new Label { Text = "Dirección:", Left = leftCol, Top = y, Width = labelWidth };
            txtDirection = new TextBox { Left = inputLeft, Top = y - 4, Width = inputWidth };
            y += rowHeight + 6;

            // Controles de contacto de emergencia (agregados)
            lblContactName = new Label { Text = "Nombre Contacto:", Left = leftCol, Top = y, Width = labelWidth };
            txtContactName = new TextBox { Left = inputLeft, Top = y - 4, Width = inputWidth };
            y += rowHeight;

            lblContactRelation = new Label { Text = "Relación Contacto:", Left = leftCol, Top = y, Width = labelWidth };
            txtContactRelation = new TextBox { Left = inputLeft, Top = y - 4, Width = inputWidth };
            y += rowHeight;

            lblContactPhone = new Label { Text = "Teléfono Contacto:", Left = leftCol, Top = y, Width = labelWidth };
            txtContactPhone = new TextBox { Left = inputLeft, Top = y - 4, Width = inputWidth };
            y += rowHeight + 6;

            btnRegister = new Button { Text = "Registrar", Left = leftCol + 10, Top = y, Width = 120 };
            btnUpdate = new Button { Text = "Actualizar", Left = leftCol + 150, Top = y, Width = 120 };
            btnGet = new Button { Text = "Obtener por Id", Left = leftCol + 290, Top = y, Width = 120 };

            btnRegister.Click += BtnRegister_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnGet.Click += BtnGet_Click;

            Controls.AddRange(new Control[]
            {
                lblIdPatient, txtIdPatient,
                lblName, txtName,
                lblEmail, txtEmail,
                lblCellphone, txtCellphone,
                lblBirth, dtpBirth,
                lblGender, cmbGender,
                lblDirection, txtDirection,
                // controls de contacto
                lblContactName, txtContactName,
                lblContactRelation, txtContactRelation,
                lblContactPhone, txtContactPhone,
                btnRegister, btnUpdate, btnGet
            });
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var patient = BuildPatientFromInputs();
            try
            {
                _useCase.RegisterPatient(patient);
                MessageBox.Show("Paciente registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var patient = BuildPatientFromInputs();
            try
            {
                _useCase.UpdatePatient(patient);
                MessageBox.Show("Paciente actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGet_Click(object sender, EventArgs e)
        {
            var query = new Patient { Id_patient = txtIdPatient.Text?.Trim() ?? "" };
            try
            {
                var found = _useCase.GetPatientById(query);
                if (found == null)
                {
                    MessageBox.Show("No se encontró el paciente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                txtIdPatient.Text = found.Id_patient ?? "";
                txtName.Text = found.Name ?? "";
                txtEmail.Text = found.Email?.ToString() ?? "";
                txtCellphone.Text = found.Cellphone != 0 ? found.Cellphone.ToString() : "";
                dtpBirth.Value = found.Birth == default ? DateTime.Today : found.Birth;
                txtDirection.Text = found.Direction ?? "";

                if (found.Gender1 != null)
                    cmbGender.SelectedIndex = found.Gender1.Gender ? 1 : 2;
                else
                    cmbGender.SelectedIndex = 0;

                // Si hay contacto, rellenar los campos nuevos
                if (found.Contact != null)
                {
                    var contactName = found.Contact.Name1?.Name ?? found.Contact.Name ?? string.Empty;
                    txtContactName.Text = contactName;
                    txtContactRelation.Text = found.Contact.Relation ?? string.Empty;
                    var contactPhone = found.Contact.Cellphone != null ? found.Contact.Cellphone.Cellphone.ToString() : string.Empty;
                    txtContactPhone.Text = contactPhone;
                }
                else
                {
                    txtContactName.Text = "";
                    txtContactRelation.Text = "";
                    txtContactPhone.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar paciente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // AdministrativeForm
            // 
            ClientSize = new Size(648, 422);
            Name = "AdministrativeForm";
            Load += AdministrativeForm_Load;
            ResumeLayout(false);

        }

        private Patient BuildPatientFromInputs()
        {
            var p = new Patient();

            p.Id_patient = txtIdPatient.Text?.Trim() ?? "";
            p.Name = txtName.Text?.Trim() ?? "";

            var emailText = txtEmail.Text?.Trim();
            if (!string.IsNullOrEmpty(emailText))
            {
                try
                {
                    p.Email = new MailAddress(emailText);
                }
                catch
                {
                    p.Email = null;
                }
            }

            if (long.TryParse(txtCellphone.Text?.Trim(), out var cell))
                p.Cellphone = cell;

            p.Birth = dtpBirth.Value;
            p.Direction = txtDirection.Text?.Trim() ?? "";

            p.Gender1 = new Person { Gender = cmbGender.SelectedIndex == 1 };

            // Construir contacto de emergencia a partir de los nuevos campos
            var contactName = txtContactName.Text?.Trim();
            var contactRelation = txtContactRelation.Text?.Trim();
            var contactPhoneText = txtContactPhone.Text?.Trim();

            if (!string.IsNullOrWhiteSpace(contactName) ||
                !string.IsNullOrWhiteSpace(contactRelation) ||
                !string.IsNullOrWhiteSpace(contactPhoneText))
            {
                var contact = new Contact
                {
                    Relation = contactRelation ?? string.Empty,
                    Name1 = new Person { Name = contactName ?? string.Empty },
                    Cellphone = new Person()
                };

                if (long.TryParse(contactPhoneText, out var contactPhone))
                {
                    contact.Cellphone.Cellphone = contactPhone;
                }
                // si no parsea, dejar cellphone en 0; el validador lo detectará y retornará error si falta
                p.Contact = contact;
            }

            return p;
        }

        private void AdministrativeForm_Load(object sender, EventArgs e)
        {

        }
    }
}