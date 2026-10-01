using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.Venta
{
    public partial class PuntoVentaUI : FormBaseObserver
    {
        private GestorVentas gestorVentas;
        private GestorProducto gestorProducto;

        private List<DetalleVenta> carrito = new List<DetalleVenta>();
        private List<Producto> inventarioDisponible = new List<Producto>();

        private class FilaVistaCarrito
        {
            public string IdProducto { get; set; }
            public string NombreProducto { get; set; }
            public int Cantidad { get; set; }
            public string PrecioUnitario { get; set; }
            public string Subtotal { get; set; }
        }

        public PuntoVentaUI()
        {
            InitializeComponent();
            gestorVentas = new GestorVentas();
            gestorProducto = new GestorProducto();

            this.Load += PuntoVentaUI_Load;
            btnAgregar.Click += btnAgregar_Click;
            btnConfirmarVenta.Click += btnConfirmarVenta_Click;

            // btnQuitar.Click += btnQuitar_Click;  <--- ELIMINA ESTA LÍNEA

            cmbProductos.SelectedIndexChanged += cmbProductos_SelectedIndexChanged;
        }

        private void PuntoVentaUI_Load(object? sender, EventArgs e)
        {
            numCantidad.Minimum = 1;
            CargarClientes();
            CargarInventario();
            //cmbMetodoPago.SelectedIndex = 0;
        }

        private void CargarClientes()
        {
            try
            {
                DataView dvClientes = gestorVentas.ObtenerClientesActivos();
                cmbClientes.DataSource = dvClientes;
                cmbClientes.DisplayMember = "RazonSocial";
                cmbClientes.ValueMember = "IdCliente";

                for (int i = 0; i < cmbClientes.Items.Count; i++)
                {
                    DataRowView row = (DataRowView)cmbClientes.Items[i];
                    if (row["CUIT"].ToString() == "00-00000000-0")
                    {
                        cmbClientes.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CargarInventario()
        {
            try
            {
                inventarioDisponible = gestorProducto.ObtenerInventario().Where(p => p.Activo && p.StockActual > 0).ToList();

                var comboItems = inventarioDisponible.Select(p => new
                {
                    Id = p.CodigoBarra,
                    Display = $"{p.Nombre} - Stock: {p.StockActual} - Precio: {p.Precio:C2}"
                }).ToList();

                cmbProductos.DataSource = comboItems;
                cmbProductos.DisplayMember = "Display";
                cmbProductos.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAgregar_Click(object? sender, EventArgs e)
        {
            if (cmbProductos.SelectedValue == null) return;

            string idProducto = cmbProductos.SelectedValue.ToString();
            int cantidadSolicitada = (int)numCantidad.Value;

            if (cantidadSolicitada <= 0)
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_CantidadMayorCero", "La cantidad debe ser mayor a 0."), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Producto prod = inventarioDisponible.FirstOrDefault(p => p.CodigoBarra == idProducto);
            if (prod == null) return;

            int cantidadEnCarrito = carrito.Where(x => x.IdProducto == idProducto).Sum(x => x.Cantidad);
            if ((cantidadEnCarrito + cantidadSolicitada) > prod.StockActual)
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_StockInsuficiente", $"No hay suficiente stock. Disponible: {prod.StockActual}"), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var itemExistente = carrito.FirstOrDefault(x => x.IdProducto == idProducto);
            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidadSolicitada;
                itemExistente.Subtotal = itemExistente.Cantidad * itemExistente.PrecioUnitario;
            }
            else
            {
                carrito.Add(new DetalleVenta
                {
                    IdProducto = idProducto,
                    Cantidad = cantidadSolicitada,
                    PrecioUnitario = prod.Precio,
                    Subtotal = cantidadSolicitada * prod.Precio
                });
            }

            ActualizarGrillaCarrito();
        }

        private void ActualizarGrillaCarrito()
        {
            var vistaGrilla = carrito.Select(c => new FilaVistaCarrito
            {
                IdProducto = c.IdProducto,
                NombreProducto = inventarioDisponible.First(p => p.CodigoBarra == c.IdProducto).Nombre,
                Cantidad = c.Cantidad,
                PrecioUnitario = c.PrecioUnitario.ToString("C2"),
                Subtotal = c.Subtotal.ToString("C2")
            }).ToList();

            dgvCarrito.DataSource = null;
            dgvCarrito.DataSource = vistaGrilla;

            if (dgvCarrito.Columns.Contains("IdProducto")) dgvCarrito.Columns["IdProducto"].Visible = false;

            decimal totalFinal = carrito.Sum(c => c.Subtotal);
            string lblBase = GestorIdioma.GetInstance.TraducirMensaje("lblTotalVenta", "TOTAL:");
            lblTotalMonto.Text = $"{lblBase} {totalFinal:C2}";

            TraducirElementosParticulares(GestorIdioma.GetInstance.IdiomaActual);
            dgvCarrito.ClearSelection();
        }

        private void btnConfirmarVenta_Click(object? sender, EventArgs e)
        {
            if (!carrito.Any())
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_CarritoVacio", "El carrito está vacío."), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idCliente = Convert.ToInt32(cmbClientes.SelectedValue);
                string metodoPago = cmbMetodoPago.SelectedItem.ToString();

                Guid idUsuarioMock = Guid.Parse("d1eda407-3582-4e0c-85cc-ae51eb67b826");

                gestorVentas.ProcesarNuevaVenta(idCliente, idUsuarioMock, carrito, metodoPago);

                // LOG BITACORA: Registro de Venta Confirmada
                string username = SessionManager.getInstance.ObtenerUsuarioActivo()?.Username ?? "Sistema";
                GestorBitacora.GetInstance.Update(username, "LOG_REGISTRO_VENTA");

                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_VentaExitosa", "Venta registrada exitosamente."), GestorIdioma.GetInstance.TraducirMensaje("msg_Exito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                carrito.Clear();
                ActualizarGrillaCarrito();
                CargarInventario();
                numCantidad.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            this.Text = GestorIdioma.GetInstance.TraducirMensaje("PuntoVentaUI", "Punto de Venta");
            lblCliente.Text = GestorIdioma.GetInstance.TraducirMensaje("lblCliente", "Cliente:");
            lblProducto.Text = GestorIdioma.GetInstance.TraducirMensaje("lblProducto", "Producto:");
            lblCantidad.Text = GestorIdioma.GetInstance.TraducirMensaje("lblCantidad", "Cantidad:");
            lblMetodoPago.Text = GestorIdioma.GetInstance.TraducirMensaje("lblMetodoPago", "Método de Pago:");
            btnAgregar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnAgregar", "Agregar al Carrito");
            btnConfirmarVenta.Text = GestorIdioma.GetInstance.TraducirMensaje("btnConfirmarVenta", "Cobrar y Emitir Ticket");

            string? seleccionPago = cmbMetodoPago.SelectedItem?.ToString();
            cmbMetodoPago.Items.Clear();
            cmbMetodoPago.Items.Add(GestorIdioma.GetInstance.TraducirMensaje("pago_Efectivo", "Efectivo"));
            cmbMetodoPago.Items.Add(GestorIdioma.GetInstance.TraducirMensaje("pago_Transferencia", "Transferencia"));
            cmbMetodoPago.Items.Add(GestorIdioma.GetInstance.TraducirMensaje("pago_Debito", "Tarjeta de Débito"));
            cmbMetodoPago.Items.Add(GestorIdioma.GetInstance.TraducirMensaje("pago_Credito", "Tarjeta de Crédito"));
            if (cmbMetodoPago.SelectedIndex == -1) cmbMetodoPago.SelectedIndex = 0;

            if (dgvCarrito != null && dgvCarrito.Columns.Count > 0)
            {
                if (dgvCarrito.Columns.Contains("NombreProducto"))
                    dgvCarrito.Columns["NombreProducto"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Producto", "Producto");
                if (dgvCarrito.Columns.Contains("Cantidad"))
                    dgvCarrito.Columns["Cantidad"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Cantidad", "Cantidad");
                if (dgvCarrito.Columns.Contains("PrecioUnitario"))
                    dgvCarrito.Columns["PrecioUnitario"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_MontoUnitario", "Monto Unitario");
                if (dgvCarrito.Columns.Contains("Subtotal"))
                    dgvCarrito.Columns["Subtotal"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Subtotal", "Subtotal");
            }
            if (btnQuitar != null)
                btnQuitar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnQuitar", "Quitar del Carrito");

        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.CurrentRow == null)
            {
                string msj = GestorIdioma.GetInstance.TraducirMensaje("msg_SeleccioneItemCarrito", "Seleccione un producto de la grilla para quitar.");
                string titulo = GestorIdioma.GetInstance.TraducirMensaje("msg_Atencion", "Atención");

                MessageBox.Show(msj, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtenemos el ID del producto de la fila seleccionada (la columna oculta)
            string? idProducto = dgvCarrito.CurrentRow.Cells["IdProducto"].Value?.ToString();

            if (!string.IsNullOrEmpty(idProducto))
            {
                // Buscamos el producto en la lista en memoria y lo eliminamos
                var itemAQuitar = carrito.FirstOrDefault(x => x.IdProducto == idProducto);
                if (itemAQuitar != null)
                {
                    carrito.Remove(itemAQuitar);

                    // Refrescamos la grilla y el total
                    ActualizarGrillaCarrito();
                }
            }
        }

        private void cmbProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (numCantidad != null)
            {
                numCantidad.Value = 1;
            }
        }
    }
}
