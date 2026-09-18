using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UI.Almacen
{
    public partial class GenerarSolicitudUI : FormBaseObserver
    {
        private GestorProducto gestorProducto;
        private List<Producto> listaInventario;
        private List<DetalleSolicitud> listaSolicitudActual;
        private GestorSolicitud gestorSolicitud;

        public GenerarSolicitudUI()
        {
            InitializeComponent();

            gestorProducto = new GestorProducto();
            listaInventario = new List<Producto>();
            listaSolicitudActual = new List<DetalleSolicitud>();
            gestorSolicitud = new GestorSolicitud();

            CargarInventario();
            ConfigurarGrillaInventario();
            ConfigurarGrillaSolicitud();
        }

        private void GenerarSolicitudUI_Load(object sender, EventArgs e)
        {

        }

        private void CargarInventario()
        {
            try
            {
                listaInventario = gestorProducto.ObtenerInventario();
                dgvInventario.DataSource = null;
                dgvInventario.DataSource = listaInventario;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cargar inventario", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrillaInventario()
        {
            dgvInventario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventario.AllowUserToAddRows = false;
            dgvInventario.ReadOnly = true;

            // Renombramos las cabeceras (idealmente, luego las pasás por tu GestorIdioma)
            if (dgvInventario.Columns.Count > 0)
            {
                dgvInventario.Columns["CodigoBarra"].HeaderText = "Código de Barras";
                dgvInventario.Columns["Nombre"].HeaderText = "Producto";
                dgvInventario.Columns["StockActual"].HeaderText = "Stock Actual";
                dgvInventario.Columns["PuntoPedido"].HeaderText = "Punto de Pedido";

                // Ocultamos la propiedad booleana calculada para que no se vea como una columna de Checkbox
                dgvInventario.Columns["RequiereAbastecimiento"].Visible = false;
            }
        }

        private void dgvInventario_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvInventario.Rows)
            {
                Producto prod = (Producto)row.DataBoundItem;

                if (prod.RequiereAbastecimiento)
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void ConfigurarGrillaSolicitud()
        {
            dgvSolicitud.AutoGenerateColumns = false;
            dgvSolicitud.AllowUserToAddRows = false;
            dgvSolicitud.ReadOnly = true;
            dgvSolicitud.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSolicitud.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvSolicitud.Columns.Add("NombreProducto", "Producto Faltante");
            dgvSolicitud.Columns.Add("Fecha", "Fecha de Solicitud");
            dgvSolicitud.Columns.Add("Empleado", "Solicitado por");
        }

        private void ActualizarGrillaSolicitud()
        {
            dgvSolicitud.Rows.Clear();

            string fechaActual = DateTime.Now.ToString("dd/MM/yyyy");

            foreach (var detalle in listaSolicitudActual)
            {
                dgvSolicitud.Rows.Add(detalle.Producto.Nombre, fechaActual, SessionManager.getInstance.ObtenerUsuarioActivo().Email);
            }
        }
        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            // Aquí integrarás la traducción de las columnas de dgvInventario cuando sumes las etiquetas a tu BD
        }

        private void Agregar_Click(object sender, EventArgs e)
        {
            if (dgvInventario.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione un producto del inventario que necesite reabastecimiento.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Producto prodSeleccionado = (Producto)dgvInventario.CurrentRow.DataBoundItem;

            bool yaExisteEnCarrito = listaSolicitudActual.Exists(d => d.Producto.CodigoBarra == prodSeleccionado.CodigoBarra);
            if (yaExisteEnCarrito)
            {
                MessageBox.Show("Este producto ya está en la lista actual de faltantes.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //NUEVA PREVENCIÓN AUTOMÁTICA
            bool yaPedidoACompras = gestorSolicitud.ValidarProductoPendienteCompras(prodSeleccionado.CodigoBarra);
            if (yaPedidoACompras)
            {
                MessageBox.Show("Este producto ya se encuentra en una solicitud anterior pendiente de revisión por Compras.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DetalleSolicitud nuevoDetalle = new DetalleSolicitud
            {
                IdDetalle = Guid.NewGuid(),
                Producto = prodSeleccionado,
                CantidadSolicitada = 0
            };

            listaSolicitudActual.Add(nuevoDetalle);
            ActualizarGrillaSolicitud();
        }

        private void btn_Solicitud_Abastecimiento_Click(object sender, EventArgs e)
        {
            if (listaSolicitudActual.Count == 0)
            {
                MessageBox.Show("No hay productos en la lista para solicitar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SolicitudAbastecimiento nuevaSolicitud = new SolicitudAbastecimiento();
                nuevaSolicitud.Detalles = listaSolicitudActual;

                gestorSolicitud.GenerarNuevaSolicitud(nuevaSolicitud);

                MessageBox.Show("Solicitud enviada a Compras exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                listaSolicitudActual.Clear();
                ActualizarGrillaSolicitud();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al confirmar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
