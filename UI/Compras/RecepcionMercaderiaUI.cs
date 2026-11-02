using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;

namespace UI.Compras
{
    public partial class RecepcionMercaderiaUI : FormBaseObserver // Cambia a FormBaseObserver si usas traducciones dinámicas
    {
        private GestorCompras gestorCompras;
        private GestorRecepcion gestorRecepcion; // Lo crearemos en el próximo paso
        private GestorProducto gestorProducto;
        private List<Producto> inventarioCache;

        public RecepcionMercaderiaUI()
        {
            InitializeComponent();
            gestorCompras = new GestorCompras();
            gestorProducto = new GestorProducto();
            gestorRecepcion = new GestorRecepcion();

            this.Load += RecepcionMercaderiaUI_Load;
            dgvOrdenesPendientes.SelectionChanged += DgvOrdenesPendientes_SelectionChanged;
            btnConfirmarIngreso.Click += BtnConfirmarIngreso_Click;
        }

        private void RecepcionMercaderiaUI_Load(object? sender, EventArgs e)
        {
            inventarioCache = gestorProducto.ObtenerInventario();
            CargarOrdenesEnTransito();
        }

        private void CargarOrdenesEnTransito()
        {
            // Simulamos obtener órdenes que ya fueron emitidas pero no recibidas
            // En tu DAL de Compras deberás hacer un select filtrando por estado 'Emitida' o 'Pagada'
            dgvOrdenesPendientes.DataSource = gestorCompras.ObtenerOrdenesPendientesDeRecepcion();

            if (dgvOrdenesPendientes.Columns.Contains("IdOrden"))
                dgvOrdenesPendientes.Columns["IdOrden"].HeaderText = "N° Orden";
        }

        private void DgvOrdenesPendientes_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvOrdenesPendientes.CurrentRow != null)
            {
                int idOrden = Convert.ToInt32(dgvOrdenesPendientes.CurrentRow.Cells["IdOrden"].Value);
                string estadoOrden = dgvOrdenesPendientes.CurrentRow.Cells["Estado"].Value.ToString() ?? "";

                // REGLA DE NEGOCIO VISUAL: 
                // Si no está pagada, el almacenero puede verla pero NO puede interactuar con ella.
                bool habilitarIngreso = (estadoOrden == "Pagada");

                btnConfirmarIngreso.Enabled = habilitarIngreso;
                txtNroRemito.Enabled = habilitarIngreso;
                dgvDetallesRecepcion.Enabled = habilitarIngreso;

                // Opcional: Cambiar el color del botón para dar feedback visual
                btnConfirmarIngreso.BackColor = habilitarIngreso ? Color.LightGreen : Color.LightGray;

                // Cargamos los detalles de lo que esperamos recibir
                DataView dvDetalles = gestorCompras.ObtenerDetallesPorOrden(idOrden);

                DataTable dtRecepcion = new DataTable();
                dtRecepcion.Columns.Add("IdProducto", typeof(string));
                dtRecepcion.Columns.Add("Producto", typeof(string));
                dtRecepcion.Columns.Add("CantEsperada", typeof(int));
                dtRecepcion.Columns.Add("LoteIngresado", typeof(string));
                dtRecepcion.Columns.Add("FechaVto", typeof(string));

                foreach (DataRowView dr in dvDetalles)
                {
                    string idProd = dr["IdProducto"].ToString();
                    var prodInfo = inventarioCache.FirstOrDefault(p => p.CodigoBarra == idProd);

                    dtRecepcion.Rows.Add(
                        idProd,
                        prodInfo != null ? prodInfo.Nombre : "Producto Desconocido",
                        dr["CantidadSolicitada"],
                        "", // Lote vacío
                        ""  // Vto vacío
                    );
                }

                dgvDetallesRecepcion.DataSource = dtRecepcion;
                ConfigurarGrillaDetalles();
            }
            else
            {
                btnConfirmarIngreso.Enabled = false;
                txtNroRemito.Enabled = false;
                dgvDetallesRecepcion.DataSource = null;
            }
        }

        private void ConfigurarGrillaDetalles()
        {
            dgvDetallesRecepcion.AllowUserToAddRows = false;
            foreach (DataGridViewColumn col in dgvDetallesRecepcion.Columns)
            {
                // Bloqueamos las columnas informativas
                if (col.Name == "LoteIngresado" || col.Name == "FechaVto")
                {
                    col.ReadOnly = false;
                    col.DefaultCellStyle.BackColor = Color.LightYellow; // Resaltamos visualmente lo editable
                }
                else
                {
                    col.ReadOnly = true;
                    col.DefaultCellStyle.BackColor = Color.WhiteSmoke;
                }
            }
        }

        private void BtnConfirmarIngreso_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNroRemito.Text))
            {
                MessageBox.Show("Debe ingresar el número de remito físico entregado por el transportista.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Armamos el objeto Recepción
            Recepcion nuevaRecepcion = new Recepcion
            {
                IdOrden = Convert.ToInt32(dgvOrdenesPendientes.CurrentRow.Cells["IdOrden"].Value),
                NumeroRemito = txtNroRemito.Text.Trim(),
                FechaIngreso = DateTime.Now
            };

            // 2. Extraemos y validamos los lotes de la grilla
            foreach (DataGridViewRow row in dgvDetallesRecepcion.Rows)
            {
                string loteStr = row.Cells["LoteIngresado"].Value?.ToString();
                string vtoStr = row.Cells["FechaVto"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(loteStr) || string.IsNullOrWhiteSpace(vtoStr))
                {
                    MessageBox.Show($"Falta ingresar el Lote o Vencimiento para el producto {row.Cells["Producto"].Value}.", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!DateTime.TryParse(vtoStr, out DateTime fechaVtoParseada))
                {
                    MessageBox.Show($"El formato de fecha de vencimiento es inválido para {row.Cells["Producto"].Value}. Use DD/MM/YYYY.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Generamos el lote físico
                LoteBebida loteFisico = new LoteBebida
                {
                    IdProducto = row.Cells["IdProducto"].Value.ToString(),
                    NumeroLote = loteStr,
                    FechaVencimiento = fechaVtoParseada,
                    CantidadRecibida = Convert.ToInt32(row.Cells["CantEsperada"].Value),
                    StockActual = Convert.ToInt32(row.Cells["CantEsperada"].Value) // Al nacer, el stock es el 100% de lo recibido
                };

                nuevaRecepcion.LotesRecibidos.Add(loteFisico);
            }

            try
            {
                // Reemplazamos el // TODO por la llamada real al gestor:
                gestorRecepcion.ProcesarIngresoMercaderia(nuevaRecepcion);

                MessageBox.Show("Mercadería ingresada al inventario y Lotes creados exitosamente.", "Éxito Operativo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtNroRemito.Clear();
                CargarOrdenesEnTransito(); // Al refrescar, la orden desaparecerá porque su estado ahora es 'Recibida'
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            this.Text = GestorIdioma.GetInstance.TraducirMensaje("RecepcionMercaderiaUI", "Recepción de Mercadería");
            lblTitulo.Text = GestorIdioma.GetInstance.TraducirMensaje("lblTituloRecepcion", "Paso 1: Seleccione la OC que acaba de llegar");
            lblRemito.Text = GestorIdioma.GetInstance.TraducirMensaje("lblRemitoRecepcion", "Paso 2: Ingrese el Nro. de Remito Papel");
            lblInstrucciones.Text = GestorIdioma.GetInstance.TraducirMensaje("lblInstruccionesRecepcion", "Paso 3: Complete Lote y Vencimiento en las celdas amarillas");
            btnConfirmarIngreso.Text = GestorIdioma.GetInstance.TraducirMensaje("btnConfirmarIngreso", "Confirmar Ingreso");

            if (dgvOrdenesPendientes != null && dgvOrdenesPendientes.Columns.Count > 0)
            {
                if (dgvOrdenesPendientes.Columns.Contains("IdOrden"))
                    dgvOrdenesPendientes.Columns["IdOrden"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_IdOrden", "N° Orden");
                if (dgvOrdenesPendientes.Columns.Contains("Estado"))
                    dgvOrdenesPendientes.Columns["Estado"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_EstadoOC", "Estado Actual");
            }

            if (dgvDetallesRecepcion != null && dgvDetallesRecepcion.Columns.Count > 0)
            {
                if (dgvDetallesRecepcion.Columns.Contains("IdProducto"))
                    dgvDetallesRecepcion.Columns["IdProducto"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_CodigoBarras", "Código de Barras");
                if (dgvDetallesRecepcion.Columns.Contains("Producto"))
                    dgvDetallesRecepcion.Columns["Producto"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Producto", "Producto");
                if (dgvDetallesRecepcion.Columns.Contains("CantEsperada"))
                    dgvDetallesRecepcion.Columns["CantEsperada"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_CantEsperada", "Cant. Esperada");
                if (dgvDetallesRecepcion.Columns.Contains("LoteIngresado"))
                    dgvDetallesRecepcion.Columns["LoteIngresado"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_LoteIngresado", "Lote Ingresado");
                if (dgvDetallesRecepcion.Columns.Contains("FechaVto"))
                    dgvDetallesRecepcion.Columns["FechaVto"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_FechaVto", "Fecha de Vencimiento");
            }
        }
    }
}