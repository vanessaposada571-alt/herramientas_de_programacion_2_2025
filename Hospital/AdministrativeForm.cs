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
        private Form _mainMenu;

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
        private TextBox txtGender;
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
        private Button btnBack; // nuevo

        public AdministrativeForm(application.usecases.AdministrativeUseCase useCase, Form mainMenu)
        {
            _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
            _mainMenu = mainMenu;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            
            Text = "Administración de Pacientes";
            Size = new Size(720, 720); 
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.FromArgb(30, 60, 100);
            Font = new Font("Segoe UI", 10);

            
            Panel card = new Panel
            {
                Left = 20,
                Top = 20,
                Width = 660,   
                Height = 630,  
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(card);

           
            Label title = new Label
            {
                Text = "Registro y Gestión de Pacientes",
                Font = new Font("Segoe UI Semibold", 15, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 60, 100),
                AutoSize = false,
                Width = card.Width,
                Height = 45,
                Top = 10,
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(title);

          
            Label CreateLabel(string text, int top)
                => new Label { Text = text, Left = 25, Top = top, Width = 180 };

            TextBox CreateTextBox(int top)
                => new TextBox { Left = 210, Top = top - 4, Width = 410 };

            int y = 70;
            int row = 38;

            
            card.Controls.Add(lblIdPatient = CreateLabel("Id Paciente:", y));
            card.Controls.Add(txtIdPatient = CreateTextBox(y));
            y += row;

            card.Controls.Add(lblName = CreateLabel("Nombre:", y));
            card.Controls.Add(txtName = CreateTextBox(y));
            y += row;

            card.Controls.Add(lblEmail = CreateLabel("Correo:", y));
            card.Controls.Add(txtEmail = CreateTextBox(y));
            y += row;

            card.Controls.Add(lblCellphone = CreateLabel("Teléfono:", y));
            card.Controls.Add(txtCellphone = CreateTextBox(y));
            y += row;

            card.Controls.Add(lblBirth = CreateLabel("Fecha Nacimiento:", y));
            dtpBirth = new DateTimePicker
            {
                Left = 210,
                Top = y - 4,
                Width = 160,
                Format = DateTimePickerFormat.Short
            };
            card.Controls.Add(dtpBirth);
            y += row;

            card.Controls.Add(lblGender = CreateLabel("Género:", y));
            card.Controls.Add(txtGender = CreateTextBox(y));
            y += row;

            card.Controls.Add(lblDirection = CreateLabel("Dirección:", y));
            card.Controls.Add(txtDirection = CreateTextBox(y));
            y += row + 12;

            var contactTitle = new Label
            {
                Text = "Contacto de Emergencia",
                Left = 22,
                Top = y,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 80, 140),
                AutoSize = true
            };
            card.Controls.Add(contactTitle);
            y += row;

            card.Controls.Add(lblContactName = CreateLabel("Nombre Contacto:", y));
            card.Controls.Add(txtContactName = CreateTextBox(y));
            y += row;

            card.Controls.Add(lblContactRelation = CreateLabel("Relación:", y));
            card.Controls.Add(txtContactRelation = CreateTextBox(y));
            y += row;

            card.Controls.Add(lblContactPhone = CreateLabel("Teléfono Contacto:", y));
            card.Controls.Add(txtContactPhone = CreateTextBox(y));
            y += row + 20;

            
            Button CreateButton(string text, int left)
            {
                return new Button
                {
                    Text = text,
                    Left = left,
                    Top = y,
                    Width = 180,
                    Height = 40,
                    BackColor = Color.FromArgb(70, 130, 180),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI Semibold", 10)
                };
            }

            btnRegister = CreateButton("Registrar", 25);
            btnUpdate = CreateButton("Actualizar", 225);
            btnGet = CreateButton("Buscar por ID", 425);

            card.Controls.Add(btnRegister);
            card.Controls.Add(btnUpdate);
            card.Controls.Add(btnGet);

            y += 60; 

            
            btnBack = new Button
            {
                Text = "⟵ Regresar al menú principal",
                Left = 25,
                Top = y,
                Width = 250,
                Height = 45,
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11)
            };

            btnBack.Click += (_, _) =>
            {
                this.Close();
                _mainMenu.Show();
            };

            card.Controls.Add(btnBack);

            btnRegister.Click += BtnRegister_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnGet.Click += BtnGet_Click;
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

                
                txtGender.Text = found.Gender1?.Gender ?? "";

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

            // Asignar género desde el TextBox al wrapper Person (Gender1.Gender)
            p.Gender1 = new Person { Gender = txtGender.Text?.Trim() ?? string.Empty };

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
    }
}