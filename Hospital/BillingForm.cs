using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Hospital.application.usecases;
using Hospital.domain.model;
using Hospital.domain.ports;
using Hospital.infraestructure.adapters.output;

namespace Hospital
{
    public class BillingForm : Form
    {
        private readonly BillingUseCase _useCase;

        // Controles inicializados en InitializeComponent; usar null-forgiving para silenciar advertencias de analysis de nullability
        private Label lblPatientName = null!;
        private TextBox txtPatientName = null!;
        private Label lblAge = null!;
        private NumericUpDown numAge = null!;
        private Label lblCedula = null!;
        private TextBox txtCedula = null!;

        private Label lblDoctorName = null!;
        private TextBox txtDoctorName = null!;

        private Label lblInsuranceCompany = null!;
        private TextBox txtInsuranceCompany = null!;
        private Label lblPolicyNumber = null!;
        private TextBox txtPolicyNumber = null!;
        private Label lblPolicyDays = null!;
        private NumericUpDown numPolicyDays = null!;
        private Label lblPolicyEnd = null!;
        private DateTimePicker dtpPolicyEnd = null!;

        // Listas para órdenes / medicamentos / procedimientos
        private Label lblDiagnostics = null!;
        private ListBox lstDiagnostics = null!;
        private TextBox txtAddDiagnostic = null!;
        private Button btnAddDiagnostic = null!;
        private Button btnRemoveDiagnostic = null!;

        private Label lblMedicines = null!;
        private ListBox lstMedicines = null!;
        private TextBox txtAddMedicine = null!;
        private Button btnAddMedicine = null!;
        private Button btnRemoveMedicine = null!;

        private Label lblProcedures = null!;
        private ListBox lstProcedures = null!;
        private TextBox txtAddProcedure = null!;
        private Button btnAddProcedure = null!;
        private Button btnRemoveProcedure = null!;

        private Label lblAmount = null!;
        private TextBox txtAmount = null!;

        private Button btnSave = null!;
        private Button btnUpdate = null!;
        private Button btnFind = null!;
        private Button btnClose = null!;
        private Button btnCalculate = null!;
        private Button btnBack = null!;
        private Button btnSaveInvoice = null!;
        private Label lblPreviousCopay = null!;
        private NumericUpDown numPreviousCopay = null!;

        private Patient_port patientPort;
        private Medical_insurance_port insurancePort;

        // Constructor que provee el caso de uso (opcional)
        public BillingForm(Patient_port patientPort, BillingUseCase useCase)
        {
            _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
            this.patientPort = patientPort;
            this.insurancePort = null!; // puede establecerse después si se desea
            InitializeComponent();
        }

        // Constructor usado por Program.cs: recibe puertos
        public BillingForm(Patient_port patientPort, Medical_insurance_port insurancePort)
        {
            this.patientPort = patientPort ?? throw new ArgumentNullException(nameof(patientPort));
            this.insurancePort = insurancePort ?? throw new ArgumentNullException(nameof(insurancePort));
            _useCase = null!; // no siempre disponible
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Ventana principal
            Text = "Facturación – Hospital PB";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 900;
            Height = 700;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.FromArgb(245, 248, 255);

            // Panel principal
            var mainPanel = new Panel
            {
                Left = 20,
                Top = 20,
                Width = 840,
                Height = 620,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(mainPanel);

            int labelW = 140, inputW = 220, rowH = 32, col1 = 20, col2 = 180, col3 = 440, y = 20;

            // Datos del paciente
            var header = new Label
            {
                Text = "FACTURACIÓN DE SERVICIOS",
                ForeColor = Color.FromArgb(30, 60, 100),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(230, 235, 250)
            };
            Controls.Add(header);

            mainPanel.Controls.AddRange(new Control[]
            {
                new Label { Text = "Nombre paciente:", Left = col1, Top = y, Width = labelW },
                txtPatientName = new TextBox { Left = col2, Top = y - 2, Width = inputW },
                new Label { Text = "Edad:", Left = col3, Top = y, Width = 50 },
                numAge = new NumericUpDown { Left = col3 + 60, Top = y - 2, Width = 60, Minimum = 0, Maximum = 150 },
            });
            y += rowH;

            mainPanel.Controls.AddRange(new Control[]
            {
                new Label { Text = "Cédula:", Left = col1, Top = y, Width = labelW },
                txtCedula = new TextBox { Left = col2, Top = y - 2, Width = inputW },
                new Label { Text = "Médico tratante:", Left = col3, Top = y, Width = 120 },
                txtDoctorName = new TextBox { Left = col3 + 130, Top = y - 2, Width = inputW }
            });
            txtCedula.KeyDown += TxtCedula_KeyDown;
            txtCedula.Leave += TxtCedula_Leave;
            y += rowH;

            mainPanel.Controls.AddRange(new Control[]
            {
                new Label { Text = "Compañía seguro:", Left = col1, Top = y, Width = labelW },
                txtInsuranceCompany = new TextBox { Left = col2, Top = y - 2, Width = inputW },
                new Label { Text = "Nº Póliza:", Left = col3, Top = y, Width = 70 },
                txtPolicyNumber = new TextBox { Left = col3 + 80, Top = y - 2, Width = 120 }
            });
            y += rowH;

            mainPanel.Controls.AddRange(new Control[]
            {
                new Label { Text = "Días vigencia:", Left = col1, Top = y, Width = labelW },
                numPolicyDays = new NumericUpDown { Left = col2, Top = y - 2, Width = 80, Minimum = 0, Maximum = 3650 },
                new Label { Text = "Fecha fin póliza:", Left = col3, Top = y, Width = 120 },
                dtpPolicyEnd = new DateTimePicker { Left = col3 + 130, Top = y - 2, Width = 120, Format = DateTimePickerFormat.Short }
            });
            y += rowH + 10;

            // Listas y controles de órdenes, medicamentos y procedimientos
            int listH = 90;

            // Órdenes diagnósticas
            mainPanel.Controls.AddRange(new Control[]
            {
                lblDiagnostics = new Label { Text = "Órdenes diagnósticas:", Left = col1, Top = y, Width = labelW + 40 },
                lstDiagnostics = new ListBox { Left = col2, Top = y, Width = inputW, Height = listH },
                txtAddDiagnostic = new TextBox { Left = col2 + inputW + 10, Top = y, Width = 120 },
                btnAddDiagnostic = new Button { Text = "Cargar", Left = col2 + inputW + 135, Top = y, Width = 60 },
                btnRemoveDiagnostic = new Button { Text = "Borrar", Left = col2 + inputW + 200, Top = y, Width = 60 }
            });
            btnAddDiagnostic.Click += (_, _) => AddToList(lstDiagnostics, txtAddDiagnostic);
            btnRemoveDiagnostic.Click += (_, _) => RemoveSelectedFromList(lstDiagnostics);
            y += listH + 10;

            // Medicamentos
            mainPanel.Controls.AddRange(new Control[]
            {
                lblMedicines = new Label { Text = "Medicamentos (Nombre;Costo;Dosis):", Left = col1, Top = y, Width = labelW + 80 },
                lstMedicines = new ListBox { Left = col2, Top = y, Width = inputW, Height = listH },
                txtAddMedicine = new TextBox { Left = col2 + inputW + 10, Top = y, Width = 120 },
                btnAddMedicine = new Button { Text = "Cargar", Left = col2 + inputW + 135, Top = y, Width = 60 },
                btnRemoveMedicine = new Button { Text = "Borrar", Left = col2 + inputW + 200, Top = y, Width = 60 }
            });
            btnAddMedicine.Click += (_, _) => AddToList(lstMedicines, txtAddMedicine);
            btnRemoveMedicine.Click += (_, _) => RemoveSelectedFromList(lstMedicines);
            y += listH + 10;

            // Procedimientos
            mainPanel.Controls.AddRange(new Control[]
            {
                lblProcedures = new Label { Text = "Procedimientos (Nombre;Costo):", Left = col1, Top = y, Width = labelW + 60 },
                lstProcedures = new ListBox { Left = col2, Top = y, Width = inputW, Height = listH },
                txtAddProcedure = new TextBox { Left = col2 + inputW + 10, Top = y, Width = 120 },
                btnAddProcedure = new Button { Text = "Cargar", Left = col2 + inputW + 135, Top = y, Width = 60 },
                btnRemoveProcedure = new Button { Text = "Borrar", Left = col2 + inputW + 200, Top = y, Width = 60 }
            });
            btnAddProcedure.Click += (_, _) => AddToList(lstProcedures, txtAddProcedure);
            btnRemoveProcedure.Click += (_, _) => RemoveSelectedFromList(lstProcedures);
            y += listH + 20;

            // Importe y copago
            mainPanel.Controls.AddRange(new Control[]
            {
                new Label { Text = "Importe (total):", Left = col1, Top = y, Width = labelW },
                txtAmount = new TextBox { Left = col2, Top = y - 2, Width = 120, ReadOnly = true },
                new Label { Text = "Copagos acumulados (año):", Left = col3, Top = y, Width = 180 },
                numPreviousCopay = new NumericUpDown { Left = col3 + 190, Top = y - 2, Width = 120, Minimum = 0, Maximum = 10_000_000, Increment = 50_000 }
            });
            y += rowH + 10;

            // Botones de acción
            mainPanel.Controls.AddRange(new Control[]
            {
                btnSave = new Button { Text = "Registrar", Left = col1, Top = y, Width = 110 },
                btnUpdate = new Button { Text = "Actualizar", Left = col1 + 120, Top = y, Width = 110 },
                btnFind = new Button { Text = "Buscar por cédula", Left = col1 + 240, Top = y, Width = 140 },
                btnCalculate = new Button { Text = "Calcular cobros", Left = col1 + 390, Top = y, Width = 120 },
                btnSaveInvoice = new Button { Text = "Guardar factura", Left = col1 + 520, Top = y, Width = 140 },
                btnBack = new Button { Text = "Atrás", Left = col1 + 670, Top = y, Width = 80 }
            });
            btnSave.Click += BtnSave_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnFind.Click += BtnFind_Click;
            btnCalculate.Click += BtnCalculate_Click;
            btnSaveInvoice.Click += BtnSaveInvoice_Click;
            btnBack.Click += (_, _) => Close();
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
            var item = list.SelectedItem;
            if (item != null)
                list.Items.Remove(item);
        }

        private Billings BuildBillingFromInputs()
        {
            var b = new Billings();

            // Identificadores / datos de paciente
            try
            {
                var pi = typeof(Billings).GetProperty("Id_patient");
                if (pi != null)
                    pi.SetValue(b, txtCedula.Text?.Trim() ?? string.Empty);
                else
                {
                    var f = typeof(Billings).GetField("id_patient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (f != null)
                        f.SetValue(b, txtCedula.Text?.Trim() ?? string.Empty);
                }
            }
            catch { /* ignorar incompatibilidades y seguir */ }

            if (long.TryParse(txtAmount.Text?.Trim(), out var amt))
                b.Amount = amt;

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

                // órdenes/medicamentos/procedimientos: intentar leer colecciones
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
                if (_useCase != null)
                {
                    ((dynamic)_useCase).Register(b);
                    MessageBox.Show("Facturación registrada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // No hay caso de uso inyectado: mostrar resumen y dejar que el adaptador realice persistencia si se implementa
                    MessageBox.Show("Facturación preparada (no hay caso de uso inyectado). Revise los datos antes de guardar en la BD.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
                if (_useCase != null)
                {
                    ((dynamic)_useCase).Update(b);
                    MessageBox.Show("Facturación actualizada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Actualización preparada (no hay caso de uso inyectado).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            var ced = txtCedula.Text?.Trim();
            SearchPatientByCedula(ced);
        }

        private void TxtCedula_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                var ced = txtCedula.Text?.Trim();
                SearchPatientByCedula(ced);
            }
        }

        private void TxtCedula_Leave(object? sender, EventArgs e)
        {
            var ced = txtCedula.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(ced))
                SearchPatientByCedula(ced);
        }

        private void SearchPatientByCedula(string ced)
        {
            if (string.IsNullOrWhiteSpace(ced))
            {
                MessageBox.Show("Ingrese la cédula para buscar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Patient found = null;
            try
            {
                // Intentar llamar a FindByIdNumber si el adaptador concreto lo expone
                if (patientPort is infraestructure.adapters.output.SQLPatientPort sqlPatient)
                {
                    found = sqlPatient.FindByIdNumber(new Patient { Id_patient = ced });
                }
                else
                {
                    // Intentar usar los métodos de la interfaz como fallback
                    var probe = new Patient { Id_patient = ced };
                    found = patientPort.Search(probe) ?? patientPort.FindById_patient(probe);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar paciente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (found == null)
            {
                MessageBox.Show("No se encontró paciente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Poblar datos básicos
            try { txtPatientName.Text = found.Name1?.Name ?? found.Name ?? string.Empty; } catch { txtPatientName.Text = string.Empty; }
            try
            {
                var birth = found.Birth1 != null ? found.Birth1.Birth : found.Birth;
                if (birth != default)
                {
                    var today = DateTime.Today;
                    var age = today.Year - birth.Year;
                    if (birth > today.AddYears(-age)) age--;
                    numAge.Value = Math.Min(Math.Max(age, 0), 150);
                }
            }
            catch { }

            try { txtCedula.Text = found.Id_patient ?? string.Empty; } catch { }

            // Doctor tratante: si el paciente tuviera referencia se debe mapear; por ahora intentar leer contacto especial o dejar vacío
            try { txtDoctorName.Text = string.Empty; } catch { }

            // Obtener información de póliza
            try
            {
                Medical_insurance mi = null;
                if (found.IdSure != null && insurancePort != null)
                {
                    try { mi = insurancePort.FindByIdSure(found.IdSure); } catch { mi = null; }
                }

                // Si no tenemos idSure, intentar por número de póliza
                if (mi == null && !string.IsNullOrWhiteSpace(found.PolicyNumber) && insurancePort != null)
                {
                    try { mi = insurancePort.FindByPolicy_number(new Medical_insurance { Policy_number = found.PolicyNumber }); } catch { mi = null; }
                }

                if (mi != null)
                {
                    txtInsuranceCompany.Text = mi.Company_name ?? string.Empty;
                    txtPolicyNumber.Text = mi.Policy_number ?? string.Empty;

                    // Fecha fin: supondremos Effective_Date es inicio y la póliza tiene duración implícita, si no existe usaremos Effective_Date
                    DateTime end = mi.Effective_Date != default ? mi.Effective_Date : DateTime.MinValue;
                    if (end != DateTime.MinValue)
                    {
                        dtpPolicyEnd.Value = end;
                        var days = Math.Max(0, (end.Date - DateTime.Today).Days);
                        numPolicyDays.Value = Math.Min(Math.Max(days, 0), 3650);
                    }
                    else
                    {
                        dtpPolicyEnd.Value = DateTime.Today;
                        numPolicyDays.Value = 0;
                    }
                }
                else
                {
                    txtInsuranceCompany.Text = string.Empty;
                    txtPolicyNumber.Text = found.PolicyNumber ?? string.Empty;
                    dtpPolicyEnd.Value = DateTime.Today;
                    numPolicyDays.Value = 0;
                }
            }
            catch { }

            // limpiar listas de órdenes existentes (si hubieran sido pobladas)
            lstDiagnostics.Items.Clear();
            lstMedicines.Items.Clear();
            lstProcedures.Items.Clear();

            // No mostrar popup repetido cuando se dispara desde Leave; solo si se vino por botón mostrar mensaje
            // Mostrar breve indicación en el título de la ventana
            this.Text = "Facturación - Paciente cargado";
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            // Calcular el total a partir de las listas (intentar parsear costes incluidos en las cadenas)
            long total = 0;

            foreach (var it in lstMedicines.Items)
            {
                total += ParseLastNumber(it?.ToString() ?? string.Empty);
            }
            foreach (var it in lstProcedures.Items)
            {
                total += ParseLastNumber(it?.ToString() ?? string.Empty);
            }
            foreach (var it in lstDiagnostics.Items)
            {
                total += ParseLastNumber(it?.ToString() ?? string.Empty);
            }

            // Determinar si existe póliza activa
            bool hasActivePolicy = false;
            long previousCopay = (long)numPreviousCopay.Value;
            try
            {
                // si hay texto en txtPolicyNumber intentar obtener póliza
                if (!string.IsNullOrWhiteSpace(txtPolicyNumber.Text) && insurancePort != null)
                {
                    var mi = insurancePort.FindByPolicy_number(new Medical_insurance { Policy_number = txtPolicyNumber.Text.Trim() });
                    if (mi != null)
                        hasActivePolicy = mi.Policy_status;
                }
            }
            catch { }

            const long STANDARD_COPAY = 50_000L;
            const long YEARLY_COPAY_LIMIT = 1_000_000L;

            long copayApplied = 0;
            long patientPays = 0;
            long insurerPays = 0;

            if (hasActivePolicy)
            {
                if (previousCopay >= YEARLY_COPAY_LIMIT)
                {
                    copayApplied = 0; // exento
                }
                else
                {
                    copayApplied = STANDARD_COPAY;
                }

                patientPays = Math.Min(total, copayApplied);
                insurerPays = Math.Max(0L, total - patientPays);
            }
            else
            {
                patientPays = total;
                insurerPays = 0;
                copayApplied = 0;
            }

            txtAmount.Text = patientPays.ToString();

            var summary = $"Total servicios: {total:C}\nCopago aplicado: {copayApplied:C}\nA cargo paciente: {patientPays:C}\nA cargo aseguradora: {insurerPays:C}";
            MessageBox.Show(summary, "Resumen de facturación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private long ParseLastNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;
            long val = 0;
            var parts = s.Split(new[] { ';', ',', '-', '|' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = parts.Length - 1; i >= 0; i--)
            {
                var p = parts[i].Trim();
                p = p.Replace("$", string.Empty).Replace(".", string.Empty).Replace(" ", string.Empty);
                if (long.TryParse(p, out val)) return val;
                if (decimal.TryParse(p, out var d)) return (long)Math.Round(d);
            }
            var digits = System.Text.RegularExpressions.Regex.Matches(s, "\\d+");
            if (digits.Count > 0)
            {
                var last = digits[digits.Count - 1].Value;
                if (long.TryParse(last, out val)) return val;
            }
            return 0;
        }

        private string GenerateInvoiceTextFromUi()
        {
            // Build a simple textual invoice from the current UI state
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("FACTURA");
            sb.AppendLine($"Fecha: {DateTime.Now:yyyy-MM-dd HH:mm}");
            sb.AppendLine();
            sb.AppendLine("=== Datos del paciente ===");
            sb.AppendLine($"Nombre: {txtPatientName.Text}");
            sb.AppendLine($"Cédula: {txtCedula.Text}");
            sb.AppendLine($"Edad: {numAge.Value}");
            sb.AppendLine();
            sb.AppendLine("=== Seguro médico ===");
            sb.AppendLine($"Compañía: {txtInsuranceCompany.Text}");
            sb.AppendLine($"Número póliza: {txtPolicyNumber.Text}");
            sb.AppendLine($"Fecha fin póliza: {dtpPolicyEnd.Value:yyyy-MM-dd}");
            sb.AppendLine();
            sb.AppendLine("=== Detalle clínico de órdenes ===");

            long total = 0;

            if (lstMedicines.Items.Count > 0)
            {
                sb.AppendLine("Medicamentos:");
                foreach (var it in lstMedicines.Items)
                {
                    var line = it?.ToString() ?? string.Empty;
                    sb.AppendLine(" - " + line);
                    total += ParseLastNumber(line);
                }
            }

            if (lstProcedures.Items.Count > 0)
            {
                sb.AppendLine("Procedimientos:");
                foreach (var it in lstProcedures.Items)
                {
                    var line = it?.ToString() ?? string.Empty;
                    sb.AppendLine(" - " + line);
                    total += ParseLastNumber(line);
                }
            }

            if (lstDiagnostics.Items.Count > 0)
            {
                sb.AppendLine("Ayudas diagnósticas:");
                foreach (var it in lstDiagnostics.Items)
                {
                    var line = it?.ToString() ?? string.Empty;
                    sb.AppendLine(" - " + line);
                    total += ParseLastNumber(line);
                }
            }

            sb.AppendLine();
            sb.AppendLine($"Total servicios (estimado): {total:C}");

            // Reuse calculation logic to determine copay
            bool hasActivePolicy = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(txtPolicyNumber.Text) && insurancePort != null)
                {
                    var mi = insurancePort.FindByPolicy_number(new Medical_insurance { Policy_number = txtPolicyNumber.Text.Trim() });
                    if (mi != null) hasActivePolicy = mi.Policy_status;
                }
            }
            catch { }

            const long STANDARD_COPAY = 50_000L;
            const long YEARLY_COPAY_LIMIT = 1_000_000L;
            long previousCopay = (long)numPreviousCopay.Value;
            long copayApplied = 0;
            long patientPays = 0;
            long insurerPays = 0;

            if (hasActivePolicy)
            {
                copayApplied = previousCopay >= YEARLY_COPAY_LIMIT ? 0 : STANDARD_COPAY;
                patientPays = Math.Min(total, copayApplied);
                insurerPays = Math.Max(0L, total - patientPays);
            }
            else
            {
                patientPays = total;
                insurerPays = 0;
                copayApplied = 0;
            }

            sb.AppendLine();
            sb.AppendLine("=== Resumen de cobros ===");
            sb.AppendLine($"Copago aplicado: {copayApplied:C}");
            sb.AppendLine($"A cargo paciente: {patientPays:C}");
            sb.AppendLine($"A cargo aseguradora: {insurerPays:C}");

            sb.AppendLine();
            sb.AppendLine("Médico tratante: " + txtDoctorName.Text);

            return sb.ToString();
        }

        private void BtnSaveInvoice_Click(object? sender, EventArgs e)
        {
            try
            {
                var invoiceText = GenerateInvoiceTextFromUi();
                var invoicesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Invoices");
                Directory.CreateDirectory(invoicesDir);
                var ced = string.IsNullOrWhiteSpace(txtCedula.Text) ? "unknown" : txtCedula.Text.Trim();
                var fileName = $"invoice_{ced}_{DateTime.Now:yyyyMMddHHmmss}.txt";
                var path = Path.Combine(invoicesDir, fileName);
                File.WriteAllText(path, invoiceText);
                MessageBox.Show($"Factura guardada en: {path}", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar factura: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}