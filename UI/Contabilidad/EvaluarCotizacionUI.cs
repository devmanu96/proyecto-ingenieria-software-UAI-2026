using BE;
using BLL;
using System;
using System.Data;
using System.Windows.Forms;

namespace UI.Contabilidad
{
    public partial class EvaluarCotizacionUI : FormBaseObserver
    {
        private GestorContabilidad gestorContabilidad;

        public EvaluarCotizacionUI()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            gestorContabilidad = new GestorContabilidad();

            this.Load += EvaluarCotizacionUI_Load;
            btnEfectuarPago.Click += btnEfectuarPago_Click;
        }

        private void EvaluarCotizacionUI_Load(object? sender, EventArgs e)
        {
            CargarOrdenes();
        }

        private void CargarOrdenes()
        {
            dgvOrdenesPendientes.DataSource = gestorContabilidad.ObtenerOrdenesPendientesPago();

            dgvOrdenesPendientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrdenesPendientes.ReadOnly = true;
            dgvOrdenesPendientes.AllowUserToAddRows = false;
            dgvOrdenesPendientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Ocultamos claves foráneas que no le sirven al contador
            if (dgvOrdenesPendientes.Columns.Contains("IdPresupuesto"))
                dgvOrdenesPendientes.Columns["IdPresupuesto"].Visible = false;
        }

        private void btnEfectuarPago_Click(object? sender, EventArgs e)
        {
            if (dgvOrdenesPendientes.CurrentRow != null)
            {
                int idOrden = Convert.ToInt32(dgvOrdenesPendientes.CurrentRow.Cells["IdOrden"].Value);

                // NOTA: Queda pendiente cruzar con DETALLE_OC para calcular el monto exacto
                decimal montoSimulado = 500000m;

                try
                {
                    gestorContabilidad.ProcesarPago(idOrden, montoSimulado);

                    string tituloExito = GestorIdioma.GetInstance.TraducirMensaje("msg_ExitoContable", "Éxito Contable");
                    string mensajeExito = GestorIdioma.GetInstance.TraducirMensaje("msg_PagoExitoso", "Transferencia realizada y Orden de Compra saldada.");

                    MessageBox.Show(mensajeExito, tituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarOrdenes();
                }
                catch (Exception ex)
                {
                    string tituloError = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error");
                    MessageBox.Show(ex.Message, tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                string tituloAviso = GestorIdioma.GetInstance.TraducirMensaje("msg_Atencion", "Atención");
                string mensajeAviso = GestorIdioma.GetInstance.TraducirMensaje("msg_SeleccioneOrden", "Seleccione una orden para pagar.");

                MessageBox.Show(mensajeAviso, tituloAviso, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            // Traducción dinámica de la grilla extraída de ADO.NET
            if (dgvOrdenesPendientes.Columns.Contains("IdOrden"))
                dgvOrdenesPendientes.Columns["IdOrden"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_NumOrden", "N° Orden");

            if (dgvOrdenesPendientes.Columns.Contains("FechaEmision"))
                dgvOrdenesPendientes.Columns["FechaEmision"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_FechaEmision", "Fecha Emisión");

            if (dgvOrdenesPendientes.Columns.Contains("Estado"))
                dgvOrdenesPendientes.Columns["Estado"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_EstadoOC", "Estado");
        }
    }
}
