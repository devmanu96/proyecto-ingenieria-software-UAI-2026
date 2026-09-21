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
                MessageBox.Show(ex.Message, GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error al cargar inventario"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrillaInventario()
        {
            dgvInventario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventario.AllowUserToAddRows = false;
            dgvInventario.ReadOnly = true;

            if (dgvInventario.Columns.Count > 0)
            {
                dgvInventario.Columns["CodigoBarra"].HeaderText = "Código de Barras";
                dgvInventario.Columns["Nombre"].HeaderText = "Producto";
                dgvInventario.Columns["StockActual"].HeaderText = "Stock Actual";
                dgvInventario.Columns["PuntoPedido"].HeaderText = "Punto de Pedido";
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

            // CORRECCIÓN: Se añade la columna Cantidad para evitar el NullReferenceException
            dgvSolicitud.Columns.Add("Cantidad", "Cant. Sugerida");

            dgvSolicitud.Columns.Add("Fecha", "Fecha de Solicitud");
            dgvSolicitud.Columns.Add("Empleado", "Solicitado por");
        }

        private void ActualizarGrillaSolicitud()
        {
            dgvSolicitud.Rows.Clear();
            string fechaActual = DateTime.Now.ToString("dd/MM/yyyy");

            foreach (var detalle in listaSolicitudActual)
            {
                // CORRECCIÓN: Se incluye detalle.CantidadSolicitada en la grilla visual
                dgvSolicitud.Rows.Add(detalle.Producto.Nombre, detalle.CantidadSolicitada, fechaActual, SessionManager.getInstance.ObtenerUsuarioActivo().Email);
            }
        }

        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            // CORRECCIÓN SEGURIDAD: Validamos con Contains antes de traducir para evitar crashes
            if (dgvInventario.Columns.Count > 0)
            {
                if (dgvInventario.Columns.Contains("CodigoBarra")) dgvInventario.Columns["CodigoBarra"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_CodigoBarras", "Código de Barras");
                if (dgvInventario.Columns.Contains("Nombre")) dgvInventario.Columns["Nombre"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Producto", "Producto");
                if (dgvInventario.Columns.Contains("StockActual")) dgvInventario.Columns["StockActual"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_StockActual", "Stock Actual");
                if (dgvInventario.Columns.Contains("PuntoPedido")) dgvInventario.Columns["PuntoPedido"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_PuntoPedido", "Punto de Pedido");
            }

            if (dgvSolicitud.Columns.Count > 0)
            {
                if (dgvSolicitud.Columns.Contains("NombreProducto")) dgvSolicitud.Columns["NombreProducto"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_ProdFaltante", "Producto Faltante");
                if (dgvSolicitud.Columns.Contains("Cantidad")) dgvSolicitud.Columns["Cantidad"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_CantSugerida", "Cant. Sugerida");
                if (dgvSolicitud.Columns.Contains("Fecha")) dgvSolicitud.Columns["Fecha"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_FechaSol", "Fecha de Solicitud");
                if (dgvSolicitud.Columns.Contains("Empleado")) dgvSolicitud.Columns["Empleado"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_SolicitadoPor", "Solicitado por");
            }
        }

        private void Agregar_Click(object sender, EventArgs e)
        {
            string tituloAtencion = GestorIdioma.GetInstance.TraducirMensaje("msg_Atencion", "Atención");

            if (dgvInventario.CurrentRow == null)
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_SeleccioneProdAbastecimiento", "Por favor, seleccione un producto del inventario que necesite reabastecimiento."), tituloAtencion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Producto prodSeleccionado = (Producto)dgvInventario.CurrentRow.DataBoundItem;

            bool yaExisteEnCarrito = listaSolicitudActual.Exists(d => d.Producto.CodigoBarra == prodSeleccionado.CodigoBarra);
            if (yaExisteEnCarrito)
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_ProdYaEnFaltantes", "Este producto ya está en la lista actual de faltantes."), tituloAtencion, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool yaPedidoACompras = gestorSolicitud.ValidarProductoPendienteCompras(prodSeleccionado.CodigoBarra);
            if (yaPedidoACompras)
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_ProdYaEnCompras", "Este producto ya se encuentra en una solicitud anterior pendiente de revisión por Compras."), tituloAtencion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cálculo automático de reabastecimiento ideal
            int cantidadCalculada = (prodSeleccionado.PuntoPedido * 2) - prodSeleccionado.StockActual;
            if (cantidadCalculada <= 0) cantidadCalculada = 50;

            DetalleSolicitud nuevoDetalle = new DetalleSolicitud
            {
                IdDetalle = Guid.NewGuid(),
                Producto = prodSeleccionado,
                CantidadSolicitada = cantidadCalculada
            };

            listaSolicitudActual.Add(nuevoDetalle);
            ActualizarGrillaSolicitud();
        }

        private void btn_Solicitud_Abastecimiento_Click(object sender, EventArgs e)
        {
            if (listaSolicitudActual.Count == 0)
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_NoHayProductosSolicitar", "No hay productos en la lista para solicitar."), GestorIdioma.GetInstance.TraducirMensaje("msg_Atencion", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SolicitudAbastecimiento nuevaSolicitud = new SolicitudAbastecimiento();
                nuevaSolicitud.Detalles = listaSolicitudActual;

                gestorSolicitud.GenerarNuevaSolicitud(nuevaSolicitud);

                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_SolicitudEnviadaExito", "Solicitud enviada a Compras exitosamente."), GestorIdioma.GetInstance.TraducirMensaje("msg_TituloExito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                listaSolicitudActual.Clear();
                ActualizarGrillaSolicitud();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error al confirmar"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}