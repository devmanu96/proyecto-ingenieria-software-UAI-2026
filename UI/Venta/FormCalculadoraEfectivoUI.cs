using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI.Venta
{
    public partial class FormCalculadoraEfectivoUI : FormBaseObserver
    {
        public Cobro CobroGenerado { get; private set; }
        private decimal montoRestante;

        public FormCalculadoraEfectivoUI(decimal restante)
        {
            InitializeComponent();
            montoRestante = restante;
            this.KeyPreview = true;

            // 2. Suscribimos el evento para capturar el teclado
            this.KeyDown += FormCalculadoraEfectivoUI_KeyDown;

            this.Load += FormCalculadoraEfectivoUI_Load;
            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnBorrar.Click += btnBorrar_Click;

            // Asignar evento a todos los botones numéricos
            AsignarEventosTeclado();
        }

        private void FormCalculadoraEfectivoUI_Load(object? sender, EventArgs e)
        {
            lblSaldo.Text = $"Saldo a cubrir: {montoRestante:C2}";
            txtIngreso.Text = "0";
        }

        private void AsignarEventosTeclado()
        {
            Button[] botonesTeclado = { btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btnComa };
            foreach (var btn in botonesTeclado)
            {
                btn.Click += TeclaPulsada;
            }
        }
        private void FormCalculadoraEfectivoUI_KeyDown(object sender, KeyEventArgs e)
        {
            // 1. Números del teclado principal (D0-D9) y teclado numérico (NumPad0-NumPad9)
            if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))
            {
                // Extrae solo el número (ej: "NumPad7" se convierte en "7")
                string digito = e.KeyCode.ToString().Replace("NumPad", "").Replace("D", "");

                // Busca dinámicamente el botón correspondiente (ej: "btn7") y le hace clic
                Button? btn = this.Controls.Find("btn" + digito, true).FirstOrDefault() as Button;
                btn?.PerformClick();

                e.Handled = true; // Evita el sonido de alerta de Windows
            }
            // 2. Coma o Punto (Teclado normal o teclado numérico)
            else if (e.KeyCode == Keys.Oemcomma || e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod)
            {
                btnComa.PerformClick();
                e.Handled = true;
            }
            // 3. Botón Borrar (Backspace)
            else if (e.KeyCode == Keys.Back)
            {
                btnBorrar.PerformClick();
                e.Handled = true;
            }
            // 4. Botón Aceptar (Enter)
            else if (e.KeyCode == Keys.Enter)
            {
                btnAceptar.PerformClick();
                e.Handled = true;
            }
            // 5. Botón Cancelar (Escape)
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancelar.PerformClick();
                e.Handled = true;
            }
        }

        private void TeclaPulsada(object? sender, EventArgs e)
        {
            Button btn = (Button)sender!;
            string tecla = btn.Text;

            if (txtIngreso.Text == "0" && tecla != ",")
            {
                txtIngreso.Text = tecla;
            }
            else
            {
                // Evitar múltiples comas
                if (tecla == "," && txtIngreso.Text.Contains(",")) return;

                txtIngreso.Text += tecla;
            }
        }

        private void btnBorrar_Click(object? sender, EventArgs e)
        {
            txtIngreso.Text = "0";
        }

        private void btnAceptar_Click(object? sender, EventArgs e)
        {
            if (!decimal.TryParse(txtIngreso.Text, out decimal montoIngresado) || montoIngresado <= 0)
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_MontoInvalido", "Ingrese un monto válido mayor a 0."),
                                GestorIdioma.GetInstance.TraducirMensaje("msg_Atencion", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal montoACobrar = montoIngresado;

            // Si entrega más plata de la que debe, calculamos vuelto y cobramos solo lo necesario
            if (montoIngresado > montoRestante)
            {
                decimal vuelto = montoIngresado - montoRestante;
                MessageBox.Show($"{GestorIdioma.GetInstance.TraducirMensaje("msg_Vuelto", "Entregar vuelto al cliente:")} {vuelto:C2}",
                                "Vuelto", MessageBoxButtons.OK, MessageBoxIcon.Information);

                montoACobrar = montoRestante; // Evita el error en la BLL por cobrar de más
            }

            CobroGenerado = new Cobro
            {
                MontoCobrado = montoACobrar,
                MetodoPago = "Efectivo",
                Cuotas = 1,
                FechaCobro = DateTime.Now
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            this.Text = GestorIdioma.GetInstance.TraducirMensaje("FormCalculadoraEfectivoUI", "Calculadora Efectivo");
            btnAceptar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnAceptar", "Aceptar");
            btnCancelar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnCancelar", "Cancelar");
            btnBorrar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnBorrar", "Borrar");
        }
    }
}