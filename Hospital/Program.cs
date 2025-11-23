using System;
using System.Windows.Forms;
using Hospital.application.usecases;
using Hospital.domain.services;
using Hospital.application.validators;
using Hospital.infraestructure.adapters.output;
using Hospital;
 
static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
 
        int choice = ShowSelectionDialog();
        if (choice == 1)
        {
            var employeePort = new SqlEmployeePort();
            var hrUseCase = new HumanResourcesUseCase(
                new C_Employee(employeePort),
                new U_Employee(),
                new S_Employee()
            );
 
            Application.Run(new HumanResourcesForm(hrUseCase));
        }
        else if (choice == 2)
        {
            var patientPort = new SQLPatientPort();
            var createPatient = new C_Patient(patientPort);
            var updatePatient = new U_Patient();
            var selectPatient = new S_Patient();
            var validator = new PatientValidator();
 
            var administrativeUseCase = new AdministrativeUseCase(
                createPatient,
                updatePatient,
                selectPatient,
                validator
            );
 
            Application.Run(new AdministrativeForm(administrativeUseCase));
        }
        else if (choice == 3)
        {
            // Construir use case de facturación y abrir BillingForm
            var billingUseCase = new BillingUseCase(
                new C_Billings(),
                new PatientValidator()
            );
 
            Application.Run(new BillingForm(billingUseCase));
        }
    }
 
    // Muestra un diálogo simple para seleccionar el formulario
    private static int ShowSelectionDialog()
    {
        using var dlg = new Form()
        {
            Text = "Seleccionar módulo",
            StartPosition = FormStartPosition.CenterScreen,
            Width = 420,
            Height = 180,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false
        };
 
        var btnHR = new Button() { Text = "Recursos Humanos", Left = 20, Top = 50, Width = 120 };
        var btnPatient = new Button() { Text = "Registrar Paciente", Left = 150, Top = 50, Width = 140 };
        var btnBilling = new Button() { Text = "Facturación", Left = 300, Top = 50, Width = 80 };
 
        int result = 0;
        btnHR.Click += (_, _) => { result = 1; dlg.Close(); };
        btnPatient.Click += (_, _) => { result = 2; dlg.Close(); };
        btnBilling.Click += (_, _) => { result = 3; dlg.Close(); };
 
        dlg.Controls.Add(btnHR);
        dlg.Controls.Add(btnPatient);
        dlg.Controls.Add(btnBilling);
 
        dlg.ShowDialog();
        return result;
    }
}