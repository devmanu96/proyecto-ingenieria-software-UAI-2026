using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI.Venta
{
    public partial class FormMockTarjetaUI : FormBaseObserver
    {
        public Cobro CobroGenerado { get; private set; }
        private decimal montoRestante;

        // El constructor recibe el monto que falta pagar
        public FormMockTarjetaUI(decimal montoAPagar)
        {
            InitializeComponent();
            montoRestante = montoAPagar;

            this.Load += FormMockTarjetaUI_Load;
            rbCredito.CheckedChanged += rbCredito_CheckedChanged;
            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void FormMockTarjetaUI_Load(object? sender, EventArgs e)
        {
            txtMonto.Text = montoRestante.ToString("C2");
            txtMonto.ReadOnly = true; // El monto lo dicta el sistema

            cmbCuotas.Items.AddRange(new object[] { 1, 3, 6, 9, 12 });
            cmbCuotas.SelectedIndex = 0;
            cmbCuotas.Enabled = false; // Por defecto arranca en Débito
            rbDebito.Checked = true;
        }

        private void rbCredito_CheckedChanged(object? sender, EventArgs e)
        {
            // Solo habilitamos las cuotas si es Tarjeta de Crédito
            cmbCuotas.Enabled = rbCredito.Checked;
            if (!rbCredito.Checked)
            {
                cmbCuotas.SelectedIndex = 0; // Si vuelve a débito, fuerza 1 cuota
            }
        }

        private void btnAceptar_Click(object? sender, EventArgs e)
        {
            // Validaciones básicas de UI
            if (string.IsNullOrWhiteSpace(txtTitular.Text) || string.IsNullOrWhiteSpace(txtNumero.Text))
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_DatosTarjetaVacios", "Complete los datos obligatorios (Titular y Número)."),
                                GestorIdioma.GetInstance.TraducirMensaje("msg_Atencion", "Atención"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Enmascaramos la tarjeta (Mock)
            string numTarjeta = txtNumero.Text.Trim();
            string ultimos4 = numTarjeta.Length >= 4 ? numTarjeta.Substring(numTarjeta.Length - 4) : numTarjeta;

            // Instanciamos el objeto de dominio anémico
            CobroGenerado = new Cobro
            {
                MontoCobrado = montoRestante,
                MetodoPago = rbCredito.Checked ? "Tarjeta Crédito" : "Tarjeta Débito",
                NumeroTarjeta = $"XXXX-XXXX-XXXX-{ultimos4}",
                Cuotas = rbCredito.Checked ? Convert.ToInt32(cmbCuotas.SelectedItem) : 1,
                FechaCobro = DateTime.Now
            };

            // Indicamos que todo salió bien y cerramos el modal
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Respetamos tu patrón Observer para traducciones
        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            this.Text = GestorIdioma.GetInstance.TraducirMensaje("FormMockTarjetaUI", "Pago con Tarjeta");
            lblMonto.Text = GestorIdioma.GetInstance.TraducirMensaje("lblMontoTarjeta", "Monto a cobrar:");
            lblTitular.Text = GestorIdioma.GetInstance.TraducirMensaje("lblTitular", "Nombre del Titular:");
            lblNumero.Text = GestorIdioma.GetInstance.TraducirMensaje("lblNumeroTarjeta", "Número de Tarjeta:");
            lblVencimiento.Text = GestorIdioma.GetInstance.TraducirMensaje("lblVencimiento", "Vencimiento:");
            lblCVV.Text = GestorIdioma.GetInstance.TraducirMensaje("lblCVV", "Código CVV:");
            grpTipo.Text = GestorIdioma.GetInstance.TraducirMensaje("grpTipoTarjeta", "Tipo de Tarjeta");
            lblCuotas.Text = GestorIdioma.GetInstance.TraducirMensaje("lblCuotas", "Cuotas:");
            btnAceptar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnAceptar", "Aceptar");
            btnCancelar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnCancelar", "Cancelar");
            rbDebito.Text = GestorIdioma.GetInstance.TraducirMensaje("pago_Debito", "Débito");
            rbCredito.Text = GestorIdioma.GetInstance.TraducirMensaje("pago_Credito", "Crédito");
        }
    }
}