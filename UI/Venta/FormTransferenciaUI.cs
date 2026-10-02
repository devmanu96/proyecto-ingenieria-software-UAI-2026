using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI.Venta
{
    public partial class FormTransferenciaUI : FormBaseObserver
    {
        // Propiedad pública para que PuntoVentaUI pueda extraer el cobro armado
        public Cobro CobroGenerado { get; private set; }

        private decimal montoRestante;

        // El constructor recibe el dinero que falta pagar
        public FormTransferenciaUI(decimal montoAPagar)
        {
            InitializeComponent();
            montoRestante = montoAPagar;

            this.Load += FormTransferenciaUI_Load;
            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void FormTransferenciaUI_Load(object? sender, EventArgs e)
        {
            // Mostrar el monto bloqueado
            txtMonto.Text = montoRestante.ToString("C2");
            txtMonto.ReadOnly = true;

            // Cargar las opciones de billeteras virtuales
            cmbBanco.Items.AddRange(new object[] {
                "Mercado Pago",
                "MODO",
                "Personal Pay",
                "Cuenta DNI",
                "Ualá",
                "Naranja X",
                "Transferencia Bancaria"
            });

            cmbBanco.SelectedIndex = 0;
        }

        private void btnAceptar_Click(object? sender, EventArgs e)
        {
            // 1. Armamos el objeto de dominio anémico uniendo el método con la billetera
            CobroGenerado = new Cobro
            {
                MontoCobrado = montoRestante,
                MetodoPago = $"Transferencia - {cmbBanco.SelectedItem}",
                Cuotas = 1,
                FechaCobro = DateTime.Now
                // El Número de Comprobante queda nulo aquí, 
                // se lo asignará automáticamente el GestorVentas al confirmar todo.
            };

            // 2. Indicamos a la ventana principal que todo salió bien
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Traducciones heredadas de FormBaseObserver
        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            this.Text = GestorIdioma.GetInstance.TraducirMensaje("FormTransferenciaUI", "Pago por Transferencia");
            lblMonto.Text = GestorIdioma.GetInstance.TraducirMensaje("lblMontoTransf", "Monto a cobrar:");
            lblBanco.Text = GestorIdioma.GetInstance.TraducirMensaje("lblBanco", "Entidad Origen (Banco / Billetera):");
            btnAceptar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnAceptar", "Aceptar");
            btnCancelar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnCancelar", "Cancelar");
        }
    }
}