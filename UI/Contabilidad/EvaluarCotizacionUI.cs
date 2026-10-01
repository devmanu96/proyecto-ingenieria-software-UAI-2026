using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI.Contabilidad
{
    public partial class EvaluarCotizacionUI : FormBaseObserver
    {
        private GestorContabilidad gestorContabilidad;
        private GestorProducto gestorProducto;
        private decimal montoActualCalculado = 0;

        public EvaluarCotizacionUI()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            gestorContabilidad = new GestorContabilidad();
            gestorProducto = new GestorProducto();

            this.Load += EvaluarCotizacionUI_Load;
            btnEfectuarPago.Click += btnEfectuarPago_Click;
            dgvOrdenesPendientes.SelectionChanged += dgvOrdenesPendientes_SelectionChanged;

            dgvDetalleOrden.DataBindingComplete += dgvDetalleOrden_DataBindingComplete;
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

            if (dgvOrdenesPendientes.Columns.Contains("IdPresupuesto"))
                dgvOrdenesPendientes.Columns["IdPresupuesto"].Visible = false;
        }

        private void dgvOrdenesPendientes_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvOrdenesPendientes.CurrentRow != null)
            {
                int idOrden = Convert.ToInt32(dgvOrdenesPendientes.CurrentRow.Cells["IdOrden"].Value);

                var dvDetalles = gestorContabilidad.ObtenerDetallesPorOrden(idOrden);
                var inventario = gestorProducto.ObtenerInventario();

                List<FilaDetalleContable> listaGrilla = new List<FilaDetalleContable>();
                decimal sumaTotal = 0;

                foreach (DataRowView row in dvDetalles)
                {
                    string idProd = row["IdProducto"].ToString() ?? "";
                    int cant = Convert.ToInt32(row["CantidadSolicitada"]);
                    decimal precio = Convert.ToDecimal(row["PrecioAcordado"]);
                    decimal subtotal = cant * precio;

                    sumaTotal += subtotal;

                    var prodObj = inventario.FirstOrDefault(p => p.CodigoBarra == idProd);
                    string nombreProducto = prodObj != null ? prodObj.Nombre : idProd;

                    listaGrilla.Add(new FilaDetalleContable
                    {
                        Producto = nombreProducto,
                        Cantidad = cant.ToString(),
                        MontoUnitario = precio.ToString("C2"),
                        Subtotal = subtotal.ToString("C2")
                    });
                }

                listaGrilla.Add(new FilaDetalleContable
                {
                    Producto = GestorIdioma.GetInstance.TraducirMensaje("grid_TotalCorte", ">>> TOTAL DE LA ORDEN <<<"),
                    Cantidad = "",
                    MontoUnitario = "",
                    Subtotal = sumaTotal.ToString("C2")
                });

                dgvDetalleOrden.DataSource = null;
                dgvDetalleOrden.DataSource = listaGrilla;

                dgvDetalleOrden.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvDetalleOrden.ReadOnly = true;
                dgvDetalleOrden.AllowUserToAddRows = false;
                dgvDetalleOrden.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                montoActualCalculado = sumaTotal;
                btnEfectuarPago.Enabled = true;
            }
            else
            {
                dgvDetalleOrden.DataSource = null;
                btnEfectuarPago.Enabled = false;
            }

            TraducirElementosParticulares(GestorIdioma.GetInstance.IdiomaActual);
        }

        private void dgvDetalleOrden_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvDetalleOrden.Rows.Count > 0)
            {
                int ultimaFilaIndex = dgvDetalleOrden.Rows.Count - 1;
                dgvDetalleOrden.Rows[ultimaFilaIndex].DefaultCellStyle.BackColor = Color.LightSteelBlue;
                dgvDetalleOrden.Rows[ultimaFilaIndex].DefaultCellStyle.Font = new Font(dgvDetalleOrden.Font, FontStyle.Bold);
                dgvDetalleOrden.Rows[ultimaFilaIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void btnEfectuarPago_Click(object? sender, EventArgs e)
        {
            if (dgvOrdenesPendientes.CurrentRow != null)
            {
                if (montoActualCalculado <= 0)
                {
                    MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_MontoCero", "El monto de la orden es 0. Verifique los detalles."), GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idOrden = Convert.ToInt32(dgvOrdenesPendientes.CurrentRow.Cells["IdOrden"].Value);

                try
                {
                    gestorContabilidad.ProcesarPago(idOrden, montoActualCalculado);

                    // LOG BITACORA: Registro de pago emitido
                    string username = SessionManager.getInstance.ObtenerUsuarioActivo()?.Username ?? "Sistema";
                    GestorBitacora.GetInstance.Update(username, $"LOG_PAGO_EFECTUADO_ORDEN_{idOrden}");

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
        }

        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            if (dgvOrdenesPendientes.Columns.Contains("IdOrden"))
                dgvOrdenesPendientes.Columns["IdOrden"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_NumOrden", "N° Orden");
            if (dgvOrdenesPendientes.Columns.Contains("FechaEmision"))
                dgvOrdenesPendientes.Columns["FechaEmision"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_FechaEmision", "Fecha Emisión");
            if (dgvOrdenesPendientes.Columns.Contains("Estado"))
                dgvOrdenesPendientes.Columns["Estado"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_EstadoOC", "Estado");

            if (dgvDetalleOrden != null && dgvDetalleOrden.Columns.Count > 0)
            {
                if (dgvDetalleOrden.Columns.Contains("Producto"))
                    dgvDetalleOrden.Columns["Producto"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Producto", "Producto");
                if (dgvDetalleOrden.Columns.Contains("Cantidad"))
                    dgvDetalleOrden.Columns["Cantidad"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_CantSugerida", "Cantidad");
                if (dgvDetalleOrden.Columns.Contains("MontoUnitario"))
                    dgvDetalleOrden.Columns["MontoUnitario"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_MontoUnitario", "Monto Unitario");
                if (dgvDetalleOrden.Columns.Contains("Subtotal"))
                    dgvDetalleOrden.Columns["Subtotal"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Subtotal", "Subtotal");
            }
        }
    }
}