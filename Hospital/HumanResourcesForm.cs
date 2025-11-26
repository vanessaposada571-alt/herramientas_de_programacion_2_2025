using System;
using System.Windows.Forms;
using Hospital.application.usecases;
using Hospital.domain.model;

public class HumanResourcesForm : Form
{
    private readonly HumanResourcesUseCase _useCase;
    private TextBox txtUsername;
    private TextBox txtPassword;
    private TextBox txtRole;
    private Button btnRegister;
    private Button btnClose;

    public HumanResourcesForm(HumanResourcesUseCase useCase)
    {
        _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
        InitializeComponent();
    }

    private void InitializeComponent()
    {
       
        Text = "Recursos Humanos";
        StartPosition = FormStartPosition.CenterScreen;
        Width = 520;   
        Height = 350;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        BackColor = Color.FromArgb(245, 248, 255); 

      
        var header = new Panel()
        {
            Dock = DockStyle.Top,
            Height = 65,
            BackColor = Color.FromArgb(30, 60, 120)
        };

        var lblHeader = new Label()
        {
            Text = "Registro de Empleados",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };

        header.Controls.Add(lblHeader);
        Controls.Add(header);

      
        var lblUsername = new Label()
        {
            Text = "Usuario:",
            Left = 40,
            Top = 100,
            Width = 110,
            Font = new Font("Segoe UI Semibold", 11)
        };
        txtUsername = new TextBox()
        {
            Left = 160,
            Top = 98,
            Width = 300
        };

        var lblPassword = new Label()
        {
            Text = "Contraseña:",
            Left = 40,
            Top = 145,
            Width = 110,
            Font = new Font("Segoe UI Semibold", 11)
        };
        txtPassword = new TextBox()
        {
            Left = 160,
            Top = 143,
            Width = 300,
            UseSystemPasswordChar = true
        };

        var lblRole = new Label()
        {
            Text = "Rol:",
            Left = 40,
            Top = 190,
            Width = 110,
            Font = new Font("Segoe UI Semibold", 11)
        };
        txtRole = new TextBox()
        {
            Left = 160,
            Top = 188,
            Width = 300
        };

      
        btnRegister = new Button()
        {
            Text = "Registrar",
            Left = 160,
            Top = 240,
            Width = 135,
            Height = 40,
            BackColor = Color.FromArgb(40, 120, 200),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat
        };
        btnRegister.FlatAppearance.BorderSize = 0;
        btnRegister.Click += BtnRegister_Click;

        btnClose = new Button()
        {
            Text = "Regresar",
            Left = 325,
            Top = 240,
            Width = 135,
            Height = 40,
            BackColor = Color.FromArgb(180, 60, 60),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat
        };
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.Click += (_, _) => Close();

      
        Controls.Add(lblUsername);
        Controls.Add(txtUsername);

        Controls.Add(lblPassword);
        Controls.Add(txtPassword);

        Controls.Add(lblRole);
        Controls.Add(txtRole);

        Controls.Add(btnRegister);
        Controls.Add(btnClose);
    }

    private void BtnRegister_Click(object? sender, EventArgs e)
    {
        var user = new User
        {
            Name_user = txtUsername.Text?.Trim() ?? string.Empty,
            Password = txtPassword.Text ?? string.Empty,
            Rol = txtRole.Text ?? string.Empty
        };

        try
        {
            _useCase.Register(user);
            MessageBox.Show("Empleado registrado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
        catch (Exception)
        {
            MessageBox.Show("Error al registrar el usuario: " + user.Name_user,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}