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
        Width = 420;
        Height = 260;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var lblUsername = new Label() { Text = "Usuario:", Left = 12, Top = 60, Width = 110 };
        txtUsername = new TextBox() { Left = 130, Top = 58, Width = 260};

        var lblPassword = new Label() { Text = "Contraseña:", Left = 12, Top = 100, Width = 110 };
        txtPassword = new TextBox() { Left = 130, Top = 98, Width = 260, UseSystemPasswordChar = true };

        var lblRole = new Label() { Text = "Rol:", Left = 12, Top = 140, Width = 110 };
        txtRole = new TextBox() { Left = 130, Top = 138, Width = 260 };

        btnRegister = new Button() { Text = "Registrar", Left = 130, Top = 180, Width = 120 };
        btnRegister.Click += BtnRegister_Click;

        // Cambiado a Regresar para consistencia; cierra el form
        btnClose = new Button() { Text = "Regresar", Left = 270, Top = 180, Width = 120 };
        btnClose.Click += (_, _) => Close();

        Controls.AddRange(new Control[] {
            lblUsername, txtUsername,
            lblPassword, txtPassword,
            lblRole, txtRole,
            btnRegister, btnClose
        });
    }

    private void BtnRegister_Click(object? sender, EventArgs e)
    {
        var user = new User
        {
            Name_user = txtUsername.Text?.Trim() ?? string.Empty,
            Password = txtPassword.Text ?? string.Empty,
            Rol = txtRole.Text ?? string.Empty
        };

        // Si quiere, puede mapear txtName a propiedades internas de Person si es necesario.
        try
        {
            _useCase.Register(user);
            MessageBox.Show("Empleado registrado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al registrar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}