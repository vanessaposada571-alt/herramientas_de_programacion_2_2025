using System;
using System.Windows.Forms;
using System.Drawing;
using Hospital.application.usecases;
using Hospital.domain.services;
using Hospital.domain.ports;
using Hospital.application.validators;
using Hospital.infraestructure.adapters.output;
using Hospital;
using Hospital.application.forms;
using System.Collections.Generic;
using Hospital.domain.model;

public class Program
{
    private static Form selectionWindow;

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Instanciar ports y usecases
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

        // ======================================================
        //    CREAR Y MOSTRAR EL FORMULARIO DE SELECCIÓN
        // ======================================================

        int selected;
        selectionWindow = CreateSelectionForm(out selected);

        // ======================================================
        //                ABRIR FORMULARIO SELECCIONADO
        // ======================================================

        switch (selected)
        {
            case 1: // Recursos Humanos - Menú Principal
                using (var hr = new MainMenuForm(
                    hrUseCase,
                    administrativeUseCase,
                    miUseCase,
                    patientPort,
                    insurancePort,
                    selectionWindow)) // PASA EL FORMULARIO ANTERIOR
                {
                    selectionWindow.Hide();
                    hr.ShowDialog();
                }
                break;

            case 2: // Facturación
                using (var billing = new BillingForm(patientPort, insurancePort))
                {
                    selectionWindow.Hide();
                    billing.ShowDialog();
                }
                break;
        }
    }

    // ==================================================================
    //     ESTE MÉTODO CREA LA VENTANA DE SELECCIÓN (NO LA EJECUTA)
    // ==================================================================
    public static Form CreateSelectionForm(out int result)
    {
        int selection = 0;
        var dlg = new Form()
        {
            Text = "",
            StartPosition = FormStartPosition.CenterScreen,
            Width = 500,
            Height = 300,
            FormBorderStyle = FormBorderStyle.None,
            BackColor = Color.White,
        };

        // Panel superior (encabezado)
        var header = new Panel()
        {
            BackColor = Color.FromArgb(30, 60, 100),
            Dock = DockStyle.Top,
            Height = 70,
        };

        var title = new Label()
        {
            Text = "Bienvenido al Sistema del Hospital PB",
            AutoSize = false,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),
        };

        header.Controls.Add(title);
        dlg.Controls.Add(header);

        // Crear botones modernos
        Button CreateModernButton(string text, int top)
        {
            var btn = new Button()
            {
                Text = text,
                Width = 300,
                Height = 45,
                Top = top,
                Left = (dlg.Width - 300) / 2,
                BackColor = Color.FromArgb(50, 120, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 12),
            };
            btn.FlatAppearance.BorderSize = 0;

            btn.Region = Region.FromHrgn(
                WinAPI.CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 20, 20));

            return btn;
        }

        var btnHR = CreateModernButton(" Recursos Humanos ", 100);
        var btnBilling = CreateModernButton(" Facturación ", 160);

        // Asignar eventos
        btnHR.Click += (_, _) => { selection = 1; dlg.Close(); };
        btnBilling.Click += (_, _) => { selection = 2; dlg.Close(); };

        dlg.Controls.Add(btnHR);
        dlg.Controls.Add(btnBilling);

        // Botón cerrar arriba a la derecha
        var btnClose = new Button()
        {
            Text = "X",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            FlatStyle = FlatStyle.Flat,
            Width = 40,
            Height = 40,
            Top = 5,
            Left = dlg.Width - 50,
            Font = new Font("Segoe UI", 12, FontStyle.Bold)
        };
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.Click += (_, _) => { dlg.Close(); };

        header.Controls.Add(btnClose);

        // Mostrar el diálogo aquí para capturar la selección antes de retornar
        dlg.ShowDialog();
        result = selection;
        return dlg;
    }

    // Clase WinAPI (botones redondeados)
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