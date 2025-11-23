using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Hospital.application.usecases;
using Hospital.domain.model;

namespace Hospital
{
    public class BillingForm : Form
    {
        private readonly BillingUseCase _useCase;

        private Label lblPatientName;
        private TextBox txtPatientName;
        private Label lblAge;
        private NumericUpDown numAge;
        private Label lblCedula;
        private TextBox txtCedula;

        private Label lblDoctorName;
        private TextBox txtDoctorName;

        private Label lblInsuranceCompany;
        private TextBox txtInsuranceCompany;
        private Label lblPolicyNumber;
        private TextBox txtPolicyNumber;
        private Label lblPolicyDays;
        private NumericUpDown numPolicyDays;
        private Label lblPolicyEnd;
        private DateTimePicker dtpPolicyEnd;

        // Listas para órdenes / medicamentos / procedimientos
        private Label lblDiagnostics;
        private ListBox lstDiagnostics;
        private TextBox txtAddDiagnostic;
        private Button btnAddDiagnostic;
        private Button btnRemoveDiagnostic;

        private Label lblMedicines;
        private ListBox lstMedicines;
        private TextBox txtAddMedicine;
        private Button btnAddMedicine;
        private Button btnRemoveMedicine;

        private Label lblProcedures;
        private ListBox lstProcedures;
        private TextBox txtAddProcedure;
        private Button btnAddProcedure;
        private Button btnRemoveProcedure;

        private Label lblAmount;
        private TextBox txtAmount;

        private Button btnSave;
        private Button btnUpdate;
        private Button btnFind;
        private Button btnClose;

        public BillingForm(BillingUseCase useCase)
        {
            _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Facturación";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(820, 620);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            int left = 12;
            int top = 12;
            int labelW = 140;
            int inputLeft = left + labelW + 8;
            int inputW = 620;
            int rowH = 30;

            // Paciente
            lblPatientName = new Label { Text = "Nombre paciente:", Left = left, Top = top, Width = labelW };
            txtPatientName = new TextBox { Left = inputLeft, Top = top - 4, Width = inputW };
            top += rowH;

            lblAge = new Label { Text = "Edad:", Left = left, Top = top, Width = labelW };
            numAge = new NumericUpDown { Left = inputLeft, Top = top - 6, Width = 80, Minimum = 0, Maximum = 150 };
            lblCedula = new Label { Text = "Cédula:", Left = inputLeft + 100, Top = top, Width = 60 };
            txtCedula = new TextBox { Left = inputLeft + 160, Top = top - 4, Width = 200 };
            top += rowH;

            // Médico
            lblDoctorName = new Label { Text = "Nombre médico:", Left = left, Top = top, Width = labelW };
            txtDoctorName = new TextBox { Left = inputLeft, Top = top - 4, Width = inputW };
            top += rowH;

            // Seguro
            lblInsuranceCompany = new Label { Text = "Compañía seguro:", Left = left, Top = top, Width = labelW };
            txtInsuranceCompany = new TextBox { Left = inputLeft, Top = top - 4, Width = 360 };
            lblPolicyNumber = new Label { Text = "Nº Póliza:", Left = inputLeft + 380, Top = top, Width = 70 };
            txtPolicyNumber = new TextBox { Left = inputLeft + 460, Top = top - 4, Width = 180 };
            top += rowH;

            lblPolicyDays = new Label { Text = "Días vigencia:", Left = left, Top = top, Width = labelW };
            numPolicyDays = new NumericUpDown { Left = inputLeft, Top = top - 6, Width = 80, Minimum = 0, Maximum = 3650 };
            lblPolicyEnd = new Label { Text = "Fecha fin póliza:", Left = inputLeft + 100, Top = top, Width = 120 };
            dtpPolicyEnd = new DateTimePicker { Left = inputLeft + 230, Top = top - 6, Width = 200, Format = DateTimePickerFormat.Short };
            top += rowH + 6;

            // Amount
            lblAmount = new Label { Text = "Importe:", Left = left, Top = top, Width = labelW };
            txtAmount = new TextBox { Left = inputLeft, Top = top - 4, Width = 200 };
            top += rowH + 6;

            // Órdenes diagnósticas
            lblDiagnostics = new Label { Text = "Órdenes (Ayuda diagnóstica):", Left = left, Top = top, Width = 220 };
            lstDiagnostics = new ListBox { Left = inputLeft, Top = top - 4, Width = 360, Height = 120 };
            txtAddDiagnostic = new TextBox { Left = inputLeft + 370, Top = top - 4, Width = 220 };
            btnAddDiagnostic = new Button { Text = "Agregar", Left = inputLeft + 370, Top = top + 30, Width = 100 };
            btnRemoveDiagnostic = new Button { Text = "Quitar", Left = inputLeft + 490, Top = top + 30, Width = 100 };
            btnAddDiagnostic.Click += (_, _) => AddToList(lstDiagnostics, txtAddDiagnostic);
            btnRemoveDiagnostic.Click += (_, _) => RemoveSelectedFromList(lstDiagnostics);
            top += 130;

            // Medicamentos
            lblMedicines = new Label { Text = "Medicamentos:", Left = left, Top = top, Width = 220 };
            lstMedicines = new ListBox { Left = inputLeft, Top = top - 4, Width = 360, Height = 120 };
            txtAddMedicine = new TextBox { Left = inputLeft + 370, Top = top - 4, Width = 220 };
            btnAddMedicine = new Button { Text = "Agregar", Left = inputLeft + 370, Top = top + 30, Width = 100 };
            btnRemoveMedicine = new Button { Text = "Quitar", Left = inputLeft + 490, Top = top + 30, Width = 100 };
            btnAddMedicine.Click += (_, _) => AddToList(lstMedicines, txtAddMedicine);
            btnRemoveMedicine.Click += (_, _) => RemoveSelectedFromList(lstMedicines);
            top += 130;

            // Procedimientos
            lblProcedures = new Label { Text = "Procedimientos:", Left = left, Top = top, Width = 220 };
            lstProcedures = new ListBox { Left = inputLeft, Top = top - 4, Width = 360, Height = 120 };
            txtAddProcedure = new TextBox { Left = inputLeft + 370, Top = top - 4, Width = 220 };
            btnAddProcedure = new Button { Text = "Agregar", Left = inputLeft + 370, Top = top + 30, Width = 100 };
            btnRemoveProcedure = new Button { Text = "Quitar", Left = inputLeft + 490, Top = top + 30, Width = 100 };
            btnAddProcedure.Click += (_, _) => AddToList(lstProcedures, txtAddProcedure);
            btnRemoveProcedure.Click += (_, _) => RemoveSelectedFromList(lstProcedures);
            top += 150;

            // Botones
            btnSave = new Button { Text = "Registrar", Left = left + 8, Top = top, Width = 120 };
            btnUpdate = new Button { Text = "Actualizar", Left = left + 140, Top = top, Width = 120 };
            btnFind = new Button { Text = "Buscar por cédula", Left = left + 272, Top = top, Width = 140 };
            btnClose = new Button { Text = "Cerrar", Left = left + 420, Top = top, Width = 120 };

            btnSave.Click += BtnSave_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnFind.Click += BtnFind_Click;
            btnClose.Click += (_, _) => Close();

            Controls.AddRange(new Control[]
            {
                lblPatientName, txtPatientName,
                lblAge, numAge, lblCedula, txtCedula,
                lblDoctorName, txtDoctorName,
                lblInsuranceCompany, txtInsuranceCompany, lblPolicyNumber, txtPolicyNumber,
                lblPolicyDays, numPolicyDays, lblPolicyEnd, dtpPolicyEnd,
                lblAmount, txtAmount,
                lblDiagnostics, lstDiagnostics, txtAddDiagnostic, btnAddDiagnostic, btnRemoveDiagnostic,
                lblMedicines, lstMedicines, txtAddMedicine, btnAddMedicine, btnRemoveMedicine,
                lblProcedures, lstProcedures, txtAddProcedure, btnAddProcedure, btnRemoveProcedure,
                btnSave, btnUpdate, btnFind, btnClose
            });
        }

        private void AddToList(ListBox list, TextBox input)
        {
            var text = input.Text?.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                list.Items.Add(text);
                input.Clear();
            }
        }

        private void RemoveSelectedFromList(ListBox list)
        {
            while (list.SelectedItems.Count > 0)
                list.Items.Remove(list.SelectedItems[0]);
        }

        private Billings BuildBillingFromInputs()
        {
            var b = new Billings();

            // Identificadores / datos de paciente
            // Se intenta reutilizar las propiedades del modelo disponible en el proyecto.
            try
            {
                // Si existe propiedad pública Id_patient (compatibilidad con otras partes del código)
                var pi = typeof(Billings).GetProperty("Id_patient");
                if (pi != null)
                    pi.SetValue(b, txtCedula.Text?.Trim() ?? string.Empty);
                else
                {
                    // intentar campo/backing si existe (código defensivo)
                    var f = typeof(Billings).GetField("id_patient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (f != null)
                        f.SetValue(b, txtCedula.Text?.Trim() ?? string.Empty);
                }
            }
            catch { /* ignorar incompatibilidades y seguir */ }

            // Monto
            if (long.TryParse(txtAmount.Text?.Trim(), out var amt))
                b.Amount = amt;

            // No confiamos en la forma exacta de Order en el dominio;
            // mantenemos los listados en la UI para presentar/almacenar si se implementa la persistencia adecuada.
            // Guardamos las listas en propiedades dinámicas si existen (compatibilidad limitada).
            try
            {
                // Si existe una propiedad 'Order' que soporte lista, omitir (dejar para adaptador).
                // No forzamos conversiones complejas aquí.
            }
            catch { }

            return b;
        }

        private void PopulateFromBilling(object found)
        {
            if (found == null) return;

            try
            {
                // Intentar mapear campos comunes desde el objeto encontrado
                var dyn = (dynamic)found;
                try { txtPatientName.Text = dyn.Name ?? dyn.Patient_name ?? string.Empty; } catch { }
                try { txtCedula.Text = dyn.Id_patient ?? dyn.Id_patient1?.Id?.ToString() ?? string.Empty; } catch { }
                try { numAge.Value = dyn.Age ?? 0; } catch { }
                try { txtDoctorName.Text = dyn.Doctor ?? string.Empty; } catch { }
                try { txtInsuranceCompany.Text = dyn.Company_name ?? string.Empty; } catch { }
                try { txtPolicyNumber.Text = dyn.Policy_number ?? string.Empty; } catch { }
                try { numPolicyDays.Value = dyn.PolicyDays ?? 0; } catch { }
                try { if (dyn.PolicyEnd != null) dtpPolicyEnd.Value = dyn.PolicyEnd; } catch { }

                // Órdenes/medicamentos/procedimientos: intentar leer colecciones
                lstDiagnostics.Items.Clear();
                lstMedicines.Items.Clear();
                lstProcedures.Items.Clear();

                try
                {
                    if (dyn.Orders != null)
                    {
                        foreach (var o in dyn.Orders)
                            lstDiagnostics.Items.Add(o?.ToString() ?? string.Empty);
                    }
                }
                catch { }

                try
                {
                    if (dyn.Medicines != null)
                    {
                        foreach (var m in dyn.Medicines)
                            lstMedicines.Items.Add(m?.ToString() ?? string.Empty);
                    }
                }
                catch { }

                try
                {
                    if (dyn.Procedures != null)
                    {
                        foreach (var p in dyn.Procedures)
                            lstProcedures.Items.Add(p?.ToString() ?? string.Empty);
                    }
                }
                catch { }
            }
            catch { /* ignore */ }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var b = BuildBillingFromInputs();
            try
            {
                ((dynamic)_useCase).Register(b);
                MessageBox.Show("Facturación registrada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var b = BuildBillingFromInputs();
            try
            {
                ((dynamic)_useCase).Update(b);
                MessageBox.Show("Facturación actualizada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            // Buscar facturación por cédula del paciente (si proporcionada)
            var probe = new Billings();
            try
            {
                // Intentar colocar la cédula en el probe de forma defensiva
                var pi = typeof(Billings).GetProperty("Id_patient");
                if (pi != null)
                    pi.SetValue(probe, txtCedula.Text?.Trim() ?? string.Empty);
                else
                {
                    var f = typeof(Billings).GetField("id_patient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (f != null)
                        f.SetValue(probe, txtCedula.Text?.Trim() ?? string.Empty);
                }
            }
            catch { /* ignore */ }

            try
            {
                var found = ((dynamic)_useCase).GetByProbe(probe);
                if (found == null)
                {
                    MessageBox.Show("No se encontró facturación.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                PopulateFromBilling(found);
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                MessageBox.Show("El caso de uso de facturación no expone el método de consulta esperado. Ajuste según su implementación.", "Operación no soportada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}