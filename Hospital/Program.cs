using System;
using System.Windows.Forms;
using Hospital.application.usecases;
using Hospital.domain.services;
using Hospital.application.validators;
using Hospital.infraestructure.adapters.output;
using Hospital;
using Hospital.application.forms;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Instanciar ports y usecases (reutilizar lógica anterior)
        var employeePort = new SqlEmployeePort();
        var hrUseCase = new HumanResourcesUseCase(
            new C_Employee(employeePort),
            new U_Employee(),
            new S_Employee()
        );

        var patientPort = new SQLPatientPort();
        var selectPatient = new S_Patient(patientPort);
        var createPatient = new C_Patient(patientPort);
        var updatePatient = new U_Patient(patientPort);
        var patientValidator = new PatientValidator();

        var administrativeUseCase = new AdministrativeUseCase(
            createPatient,
            updatePatient,
            selectPatient,
            patientValidator
        );

        var insurancePort = new SQLMedicalInsurancePort();
        var createInsurance = new C_Medical_insurance(insurancePort, patientPort);
        var updateInsurance = new U_Medical_insurance(insurancePort);
        var selectInsurance = new S_Medical_insurance(insurancePort, patientPort);

        var miUseCase = new MedicalInsuranceUseCase(
            createInsurance,
            updateInsurance,
            selectInsurance,
            new MedicalInsuranceValidator()
        );

        // Crear formulario lanzador con opción adicional "Facturación"
        var launcher = new Form
        {
            Text = "Inicio - Seleccione opción",
            Width = 420,
            Height = 180,
            StartPosition = FormStartPosition.CenterScreen,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false
        };

        var btnMainMenu = new Button { Text = "Menú principal", Left = 20, Top = 20, Width = 160, Height = 36 };
        var btnBilling = new Button { Text = "Facturación", Left = 200, Top = 20, Width = 160, Height = 36 };
        var btnExit = new Button { Text = "Salir", Left = 20, Top = 70, Width = 340, Height = 36 };

        btnMainMenu.Click += (_, _) =>
        {
            using var main = new MainMenuForm(
                hrUseCase,
                administrativeUseCase,
                miUseCase,
                patientPort,
                insurancePort);
            main.ShowDialog(launcher);
        };

        btnBilling.Click += (_, _) =>
        {
            using var billing = new BillingForm((Patient_port)patientPort, (Medical_insurance_port)insurancePort);
            billing.ShowDialog(launcher);
        };

        btnExit.Click += (_, _) => launcher.Close();

        launcher.Controls.Add(btnMainMenu);
        launcher.Controls.Add(btnBilling);
        launcher.Controls.Add(btnExit);

        Application.Run(launcher);
    }
}