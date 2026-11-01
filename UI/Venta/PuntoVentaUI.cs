//using BE;
//using BLL;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//namespace UI.Venta
//{
//    public partial class PuntoVentaUI : FormBaseObserver
//    {
//        private GestorVentas gestorVentas;
//        private GestorProducto gestorProducto;
//        private GestorOfertas gestorOfertas; // <-- NUEVO: Motor de Ofertas

//        private List<DetalleVenta> carrito = new List<DetalleVenta>();
//        private List<Cobro> pagosCombinados = new List<Cobro>();
//        private List<Producto> inventarioDisponible = new List<Producto>();

//        public PuntoVentaUI()
//        {
//            InitializeComponent();
//            gestorVentas = new GestorVentas();
//            gestorProducto = new GestorProducto();
//            gestorOfertas = new GestorOfertas(); // <-- NUEVO: Instancia del motor

//            this.Load += PuntoVentaUI_Load;
//            btnAgregar.Click += btnAgregar_Click;
//            btnConfirmarVenta.Click += btnConfirmarVenta_Click;
//            btnQuitar.Click += btnQuitar_Click;
//            btnQuitarPago.Click += btnQuitarPago_Click;
//            cmbProductos.SelectedIndexChanged += cmbProductos_SelectedIndexChanged;
//        }

//        private void PuntoVentaUI_Load(object? sender, EventArgs e)
//        {
//            numCantidad.Minimum = 1;
//            panelUpselling.Visible = false; // <-- NUEVO: Ocultar panel promo al inicio
//            CargarClientes();
//            CargarInventario();
//        }

//        private void CargarClientes()
//        {
//            try
//            {
//                DataView dvClientes = gestorVentas.ObtenerClientesActivos();
//                cmbClientes.DataSource = dvClientes;
//                cmbClientes.DisplayMember = "RazonSocial";
//                cmbClientes.ValueMember = "IdCliente";

//                for (int i = 0; i < cmbClientes.Items.Count; i++)
//                {
//                    DataRowView row = (DataRowView)cmbClientes.Items[i];
//                    if (row["CUIT"].ToString() == "00-00000000-0")
//                    {
//                        cmbClientes.SelectedIndex = i;
//                        break;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }

//        private void CargarInventario()
//        {
//            try
//            {
//                inventarioDisponible = gestorProducto.ObtenerInventario().Where(p => p.Activo && p.StockActual > 0).ToList();

//                var comboItems = inventarioDisponible.Select(p => new
//                {
//                    Id = p.CodigoBarra,
//                    Display = $"{p.Nombre} - Stock: {p.StockActual} - Precio: {p.Precio:C2}"
//                }).ToList();

//                cmbProductos.DataSource = comboItems;
//                cmbProductos.DisplayMember = "Display";
//                cmbProductos.ValueMember = "Id";
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }

//        private void btnAgregar_Click(object? sender, EventArgs e)
//        {
//            if (cmbProductos.SelectedValue == null) return;

//            string idProducto = cmbProductos.SelectedValue.ToString();
//            int cantidadSolicitada = (int)numCantidad.Value;

//            if (cantidadSolicitada <= 0)
//            {
//                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_CantidadMayorCero", "La cantidad debe ser mayor a 0."), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            Producto prod = inventarioDisponible.FirstOrDefault(p => p.CodigoBarra == idProducto);
//            if (prod == null) return;

//            int cantidadEnCarrito = carrito.Where(x => x.IdProducto == idProducto).Sum(x => x.Cantidad);
//            if ((cantidadEnCarrito + cantidadSolicitada) > prod.StockActual)
//            {
//                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_StockInsuficiente", $"No hay suficiente stock. Disponible: {prod.StockActual}"), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            var itemExistente = carrito.FirstOrDefault(x => x.IdProducto == idProducto);
//            if (itemExistente != null)
//            {
//                itemExistente.Cantidad += cantidadSolicitada;
//                itemExistente.Subtotal = itemExistente.Cantidad * itemExistente.PrecioUnitario;
//            }
//            else
//            {
//                carrito.Add(new DetalleVenta
//                {
//                    IdProducto = idProducto,
//                    Cantidad = cantidadSolicitada,
//                    PrecioUnitario = prod.Precio,
//                    Subtotal = cantidadSolicitada * prod.Precio
//                });
//            }

//            ActualizarGrillaCarrito();

//            // =========================================================
//            // NUEVO: DISPARADOR DE INTELIGENCIA COMERCIAL (UPSELLING)
//            // =========================================================
//            VerificarPromoCruzada(idProducto);
//        }

//        // <-- NUEVO MÉTODO PARA EVALUAR PROMOCIONES CRUZADAS -->
//        private void VerificarPromoCruzada(string idProductoAgregado)
//        {
//            try
//            {
//                OfertaSugerida? promoSugerida = gestorOfertas.VerificarOportunidadUpselling(idProductoAgregado);

//                if (promoSugerida != null)
//                {
//                    panelUpselling.Visible = true;
//                    lblSugerenciaUpselling.Text = promoSugerida.TituloOferta;
//                }
//                else
//                {
//                    panelUpselling.Visible = false;
//                }
//            }
//            catch
//            {
//                panelUpselling.Visible = false; // Falla silenciosa para no bloquear la venta
//            }
//        }

//        private void ActualizarGrillaCarrito()
//        {
//            var vistaGrilla = carrito.Select(c => new VistaCarritoDTO
//            {
//                IdProducto = c.IdProducto,
//                NombreProducto = inventarioDisponible.First(p => p.CodigoBarra == c.IdProducto).Nombre,
//                Cantidad = c.Cantidad,
//                PrecioUnitario = c.PrecioUnitario.ToString("C2"),
//                Subtotal = c.Subtotal.ToString("C2")
//            }).ToList();

//            dgvCarrito.DataSource = null;
//            dgvCarrito.DataSource = vistaGrilla;

//            if (dgvCarrito.Columns.Contains("IdProducto")) dgvCarrito.Columns["IdProducto"].Visible = false;

//            decimal totalFinal = carrito.Sum(c => c.Subtotal);
//            string lblBase = GestorIdioma.GetInstance.TraducirMensaje("lblTotalVenta", "TOTAL:");
//            lblTotalMonto.Text = $"{lblBase} {totalFinal:C2}";

//            ActualizarGrillaPagos();
//            TraducirElementosParticulares(GestorIdioma.GetInstance.IdiomaActual);
//            dgvCarrito.ClearSelection();
//        }

//        private void ActualizarGrillaPagos()
//        {
//            var vistaPagos = pagosCombinados.Select(p => new VistaPagoDTO
//            {
//                MetodoPago = p.MetodoPago,
//                Monto = p.MontoCobrado.ToString("C2"),
//                Detalle = p.Cuotas > 1 ? $"{p.Cuotas} Cuotas ({p.NumeroTarjeta})" : (p.NumeroTarjeta ?? "N/A")
//            }).ToList();

//            dgvPagos.DataSource = null;
//            dgvPagos.DataSource = vistaPagos;

//            decimal totalVenta = carrito.Sum(c => c.Subtotal);
//            decimal totalPagado = pagosCombinados.Sum(p => p.MontoCobrado);
//            decimal restante = totalVenta - totalPagado;

//            if (restante < 0) restante = 0;

//            string lblBase = GestorIdioma.GetInstance.TraducirMensaje("lblRestante", "SALDO RESTANTE:");
//            lblRestante.Text = $"{lblBase} {restante:C2}";

//            lblRestante.ForeColor = restante <= 0 ? Color.ForestGreen : Color.Crimson;
//            dgvPagos.ClearSelection();
//        }

//        private void btnQuitarPago_Click(object? sender, EventArgs e)
//        {
//            if (dgvPagos.CurrentRow == null) return;

//            int index = dgvPagos.CurrentRow.Index;
//            if (index >= 0 && index < pagosCombinados.Count)
//            {
//                pagosCombinados.RemoveAt(index);
//                ActualizarGrillaPagos();
//            }
//        }

//        private void btnConfirmarVenta_Click(object? sender, EventArgs e)
//        {
//            if (!carrito.Any())
//            {
//                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_CarritoVacio", "El carrito está vacío."), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            decimal totalVenta = carrito.Sum(c => c.Subtotal);
//            decimal totalPagado = pagosCombinados.Sum(p => p.MontoCobrado);

//            if (totalPagado < totalVenta)
//            {
//                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_PagoIncompleto", $"Debe cubrir el total de la venta para confirmar.\nFalta abonar: {(totalVenta - totalPagado):C2}"), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            try
//            {
//                int idCliente = Convert.ToInt32(cmbClientes.SelectedValue);

//                var usuarioActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
//                if (usuarioActivo == null)
//                {
//                    MessageBox.Show("Error de sesión: No se detecta un usuario logueado.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return;
//                }

//                Guid idUsuarioReal = usuarioActivo.Id;
//                string username = usuarioActivo.Username ?? "Sistema";

//                string nroComprobante = gestorVentas.ProcesarNuevaVenta(idCliente, idUsuarioReal, carrito, pagosCombinados);

//                GestorBitacora.GetInstance.Update(username, "LOG_REGISTRO_VENTA");

//                var detallesImpresion = carrito.Select(c => new VistaCarritoDTO
//                {
//                    IdProducto = c.IdProducto,
//                    NombreProducto = inventarioDisponible.First(p => p.CodigoBarra == c.IdProducto).Nombre,
//                    Cantidad = c.Cantidad,
//                    PrecioUnitario = c.PrecioUnitario.ToString("C2"),
//                    Subtotal = c.Subtotal.ToString("C2")
//                }).ToList();

//                string rutaPdf = UI.Utilidades.GeneradorTicketPDF.GenerarTicket(nroComprobante, username, detallesImpresion, pagosCombinados, totalVenta);

//                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
//                {
//                    FileName = rutaPdf,
//                    UseShellExecute = true
//                });

//                MessageBox.Show("Venta registrada y comprobante emitido exitosamente.", GestorIdioma.GetInstance.TraducirMensaje("msg_Exito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);

//                carrito.Clear();
//                pagosCombinados.Clear();
//                ActualizarGrillaCarrito();
//                ActualizarGrillaPagos();

//                panelUpselling.Visible = false; // <-- NUEVO: Reiniciar panel promo

//                CargarInventario();
//                if (cmbProductos.Items.Count > 0) cmbProductos.SelectedIndex = 0;

//                numCantidad.Value = 1;

//                for (int i = 0; i < cmbClientes.Items.Count; i++)
//                {
//                    DataRowView row = (DataRowView)cmbClientes.Items[i];
//                    if (row["CUIT"].ToString() == "00-00000000-0")
//                    {
//                        cmbClientes.SelectedIndex = i;
//                        break;
//                    }
//                }

//                if (cmbMetodoPago.Items.Count > 0) cmbMetodoPago.SelectedIndex = 0;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message, GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void btnQuitar_Click(object sender, EventArgs e)
//        {
//            if (dgvCarrito.CurrentRow == null)
//            {
//                string msj = GestorIdioma.GetInstance.TraducirMensaje("msg_SeleccioneItemCarrito", "Seleccione un producto de la grilla para quitar.");
//                string titulo = GestorIdioma.GetInstance.TraducirMensaje("msg_Atencion", "Atención");
//                MessageBox.Show(msj, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            string? idProducto = dgvCarrito.CurrentRow.Cells["IdProducto"].Value?.ToString();

//            if (!string.IsNullOrEmpty(idProducto))
//            {
//                var itemAQuitar = carrito.FirstOrDefault(x => x.IdProducto == idProducto);
//                if (itemAQuitar != null)
//                {
//                    carrito.Remove(itemAQuitar);
//                    ActualizarGrillaCarrito();
//                    panelUpselling.Visible = false; // <-- NUEVO: Ocultamos promo al quitar items
//                }
//            }
//        }

//        private void cmbProductos_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (numCantidad != null) numCantidad.Value = 1;
//        }

//        protected override void TraducirElementosParticulares(string codigoIdioma)
//        {
//            this.Text = GestorIdioma.GetInstance.TraducirMensaje("PuntoVentaUI", "Punto de Venta");
//            lblCliente.Text = GestorIdioma.GetInstance.TraducirMensaje("lblCliente", "Cliente:");
//            lblProducto.Text = GestorIdioma.GetInstance.TraducirMensaje("lblProducto", "Producto:");
//            lblCantidad.Text = GestorIdioma.GetInstance.TraducirMensaje("lblCantidad", "Cantidad:");
//            lblMetodoPago.Text = GestorIdioma.GetInstance.TraducirMensaje("lblMetodoPago", "Método de Pago:");
//            btnAgregar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnAgregar", "Agregar al Carrito");
//            btnConfirmarVenta.Text = GestorIdioma.GetInstance.TraducirMensaje("btnConfirmarVenta", "Cobrar y Emitir Ticket");

//            // <-- NUEVO: Traducción del panel
//            if (lblTituloUpselling != null) lblTituloUpselling.Text = GestorIdioma.GetInstance.TraducirMensaje("lblUpselling_Titulo", "¡OPORTUNIDAD DE VENTA!");

//            if (btnAgregarPago != null) btnAgregarPago.Text = GestorIdioma.GetInstance.TraducirMensaje("btnAgregarPago", "Agregar Pago");
//            if (btnQuitarPago != null) btnQuitarPago.Text = GestorIdioma.GetInstance.TraducirMensaje("btnQuitarPago", "Quitar Pago");

//            int indexPrevio = cmbMetodoPago.SelectedIndex;

//            cmbMetodoPago.Items.Clear();
//            cmbMetodoPago.Items.Add(GestorIdioma.GetInstance.TraducirMensaje("pago_Efectivo", "Efectivo"));
//            cmbMetodoPago.Items.Add(GestorIdioma.GetInstance.TraducirMensaje("pago_Transferencia", "Transferencia"));
//            cmbMetodoPago.Items.Add(GestorIdioma.GetInstance.TraducirMensaje("pago_Debito", "Tarjeta de Débito"));
//            cmbMetodoPago.Items.Add(GestorIdioma.GetInstance.TraducirMensaje("pago_Credito", "Tarjeta de Crédito"));

//            if (indexPrevio >= 0 && indexPrevio < cmbMetodoPago.Items.Count)
//            {
//                cmbMetodoPago.SelectedIndex = indexPrevio;
//            }
//            else
//            {
//                cmbMetodoPago.SelectedIndex = 0;
//            }

//            if (dgvCarrito != null && dgvCarrito.Columns.Count > 0)
//            {
//                if (dgvCarrito.Columns.Contains("NombreProducto"))
//                    dgvCarrito.Columns["NombreProducto"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Producto", "Producto");
//                if (dgvCarrito.Columns.Contains("Cantidad"))
//                    dgvCarrito.Columns["Cantidad"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Cantidad", "Cantidad");
//                if (dgvCarrito.Columns.Contains("PrecioUnitario"))
//                    dgvCarrito.Columns["PrecioUnitario"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_MontoUnitario", "Monto Unitario");
//                if (dgvCarrito.Columns.Contains("Subtotal"))
//                    dgvCarrito.Columns["Subtotal"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Subtotal", "Subtotal");
//            }
//            if (btnQuitar != null)
//                btnQuitar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnQuitar", "Quitar del Carrito");
//        }

//        private void btnAgregarPago_Click(object sender, EventArgs e)
//        {
//            if (!carrito.Any())
//            {
//                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_CarritoVacioPagos", "Agregue productos al carrito antes de cobrar."), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            decimal totalVenta = carrito.Sum(c => c.Subtotal);
//            decimal totalPagado = pagosCombinados.Sum(p => p.MontoCobrado);
//            decimal restante = totalVenta - totalPagado;

//            if (restante <= 0)
//            {
//                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_TotalCubierto", "El total de la venta ya ha sido cubierto."), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                return;
//            }

//            int metodoIndex = cmbMetodoPago.SelectedIndex;

//            if (metodoIndex == 2 || metodoIndex == 3)
//            {
//                using (FormMockTarjetaUI formTarjeta = new FormMockTarjetaUI(restante))
//                {
//                    if (formTarjeta.ShowDialog() == DialogResult.OK)
//                    {
//                        pagosCombinados.Add(formTarjeta.CobroGenerado);
//                        ActualizarGrillaPagos();
//                    }
//                }
//            }
//            else if (metodoIndex == 1)
//            {
//                using (FormTransferenciaUI formTransf = new FormTransferenciaUI(restante))
//                {
//                    if (formTransf.ShowDialog() == DialogResult.OK)
//                    {
//                        pagosCombinados.Add(formTransf.CobroGenerado);
//                        ActualizarGrillaPagos();
//                    }
//                }
//            }
//            else if (metodoIndex == 0)
//            {
//                using (FormCalculadoraEfectivoUI formEfectivo = new FormCalculadoraEfectivoUI(restante))
//                {
//                    if (formEfectivo.ShowDialog() == DialogResult.OK)
//                    {
//                        pagosCombinados.Add(formEfectivo.CobroGenerado);
//                        ActualizarGrillaPagos();
//                    }
//                }
//            }
//        }
//    }
//}

using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI.Venta
{
    public partial class PuntoVentaUI : FormBaseObserver
    {
        private GestorVentas gestorVentas;
        private GestorProducto gestorProducto;
        private GestorOfertas gestorOfertas;

        private List<DetalleVenta> carrito = new List<DetalleVenta>();
        private List<Cobro> pagosCombinados = new List<Cobro>();
        private List<Producto> inventarioDisponible = new List<Producto>();

        private Dictionary<string, string> nombresPersonalizadosUI = new Dictionary<string, string>();

        public PuntoVentaUI()
        {
            InitializeComponent();
            gestorVentas = new GestorVentas();
            gestorProducto = new GestorProducto();
            gestorOfertas = new GestorOfertas();

            this.Load += PuntoVentaUI_Load;
            btnAgregar.Click += btnAgregar_Click;
            btnConfirmarVenta.Click += btnConfirmarVenta_Click;
            btnQuitar.Click += btnQuitar_Click;
            btnQuitarPago.Click += btnQuitarPago_Click;
            cmbProductos.SelectedIndexChanged += cmbProductos_SelectedIndexChanged;
        }

        private void PuntoVentaUI_Load(object? sender, EventArgs e)
        {
            numCantidad.Minimum = 1;
            panelUpselling.Visible = false;
            CargarClientes();
            CargarInventario();
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

            string idProducto = cmbProductos.SelectedValue.ToString() ?? "";
            int cantidadSolicitada = (int)numCantidad.Value;

            if (cantidadSolicitada <= 0) return;

            Producto? prod = inventarioDisponible.FirstOrDefault(p => p.CodigoBarra == idProducto);
            if (prod == null) return;

            int cantidadEnCarrito = carrito.Where(x => x.IdProducto == idProducto).Sum(x => x.Cantidad);
            if ((cantidadEnCarrito + cantidadSolicitada) > prod.StockActual) return; // (Mantén tus MessageBox aquí)

            var itemExistente = carrito.FirstOrDefault(x => x.IdProducto == idProducto);
            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidadSolicitada;
            }
            else
            {
                // Lo agregamos con precio base inicialmente
                carrito.Add(new DetalleVenta
                {
                    IdProducto = idProducto,
                    Cantidad = cantidadSolicitada,
                    PrecioUnitario = prod.Precio,
                    Subtotal = prod.Precio * cantidadSolicitada
                });
            }

            // MAGIA: Recalculamos todo para activar o desactivar descuentos cruzados
            RecalcularDescuentosCarrito();
            ActualizarGrillaCarrito();
        }
        private void VerificarPromoCruzada(string idProductoAgregado)
        {
            try
            {
                OfertaSugerida? promoSugerida = gestorOfertas.VerificarOportunidadUpselling(idProductoAgregado);

                if (promoSugerida != null)
                {
                    panelUpselling.Visible = true;
                    lblSugerenciaUpselling.Text = promoSugerida.TituloOferta;
                }
                else
                {
                    panelUpselling.Visible = false;
                }
            }
            catch
            {
                panelUpselling.Visible = false;
            }
        }

        private void ActualizarGrillaCarrito()
        {
            var vistaGrilla = carrito.Select(c => new VistaCarritoDTO
            {
                IdProducto = c.IdProducto,
                // Leemos del diccionario visual si existe, si no, sacamos el original
                NombreProducto = nombresPersonalizadosUI.ContainsKey(c.IdProducto) ? nombresPersonalizadosUI[c.IdProducto] : inventarioDisponible.First(p => p.CodigoBarra == c.IdProducto).Nombre,
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

            ActualizarGrillaPagos();
            TraducirElementosParticulares(GestorIdioma.GetInstance.IdiomaActual);
            dgvCarrito.ClearSelection();
        }

        private void ActualizarGrillaPagos()
        {
            var vistaPagos = pagosCombinados.Select(p => new VistaPagoDTO
            {
                MetodoPago = p.MetodoPago,
                Monto = p.MontoCobrado.ToString("C2"),
                Detalle = p.Cuotas > 1 ? $"{p.Cuotas} Cuotas ({p.NumeroTarjeta})" : (p.NumeroTarjeta ?? "N/A")
            }).ToList();

            dgvPagos.DataSource = null;
            dgvPagos.DataSource = vistaPagos;

            decimal totalVenta = carrito.Sum(c => c.Subtotal);
            decimal totalPagado = pagosCombinados.Sum(p => p.MontoCobrado);
            decimal restante = totalVenta - totalPagado;

            if (restante < 0) restante = 0;

            string lblBase = GestorIdioma.GetInstance.TraducirMensaje("lblRestante", "SALDO RESTANTE:");
            lblRestante.Text = $"{lblBase} {restante:C2}";

            lblRestante.ForeColor = restante <= 0 ? Color.ForestGreen : Color.Crimson;
            dgvPagos.ClearSelection();
        }

        private void btnQuitarPago_Click(object? sender, EventArgs e)
        {
            if (dgvPagos.CurrentRow == null) return;

            int index = dgvPagos.CurrentRow.Index;
            if (index >= 0 && index < pagosCombinados.Count)
            {
                pagosCombinados.RemoveAt(index);
                ActualizarGrillaPagos();
            }
        }

        private void btnConfirmarVenta_Click(object? sender, EventArgs e)
        {
            if (!carrito.Any())
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_CarritoVacio", "El carrito está vacío."), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalVenta = carrito.Sum(c => c.Subtotal);
            decimal totalPagado = pagosCombinados.Sum(p => p.MontoCobrado);

            if (totalPagado < totalVenta)
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_PagoIncompleto", $"Debe cubrir el total de la venta para confirmar.\nFalta abonar: {(totalVenta - totalPagado):C2}"), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idCliente = Convert.ToInt32(cmbClientes.SelectedValue);

                var usuarioActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
                if (usuarioActivo == null)
                {
                    MessageBox.Show("Error de sesión: No se detecta un usuario logueado.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Guid idUsuarioReal = usuarioActivo.Id;
                string username = usuarioActivo.Username ?? "Sistema";

                string nroComprobante = gestorVentas.ProcesarNuevaVenta(idCliente, idUsuarioReal, carrito, pagosCombinados);

                GestorBitacora.GetInstance.Update(username, "LOG_REGISTRO_VENTA");

                // Generamos los detalles de impresión inyectando el diccionario visual
                var detallesImpresion = carrito.Select(c => new VistaCarritoDTO
                {
                    IdProducto = c.IdProducto,
                    NombreProducto = nombresPersonalizadosUI.ContainsKey(c.IdProducto) ? nombresPersonalizadosUI[c.IdProducto] : inventarioDisponible.First(p => p.CodigoBarra == c.IdProducto).Nombre,
                    Cantidad = c.Cantidad,
                    PrecioUnitario = c.PrecioUnitario.ToString("C2"),
                    Subtotal = c.Subtotal.ToString("C2")
                }).ToList();

                string rutaPdf = UI.Utilidades.GeneradorTicketPDF.GenerarTicket(nroComprobante, username, detallesImpresion, pagosCombinados, totalVenta);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = rutaPdf,
                    UseShellExecute = true
                });

                MessageBox.Show("Venta registrada y comprobante emitido exitosamente.", GestorIdioma.GetInstance.TraducirMensaje("msg_Exito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpieza total post-venta
                carrito.Clear();
                pagosCombinados.Clear();
                nombresPersonalizadosUI.Clear(); // Limpiamos el diccionario de nombres
                ActualizarGrillaCarrito();
                ActualizarGrillaPagos();

                panelUpselling.Visible = false;

                CargarInventario();
                if (cmbProductos.Items.Count > 0) cmbProductos.SelectedIndex = 0;

                numCantidad.Value = 1;

                for (int i = 0; i < cmbClientes.Items.Count; i++)
                {
                    DataRowView row = (DataRowView)cmbClientes.Items[i];
                    if (row["CUIT"].ToString() == "00-00000000-0")
                    {
                        cmbClientes.SelectedIndex = i;
                        break;
                    }
                }

                if (cmbMetodoPago.Items.Count > 0) cmbMetodoPago.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQuitar_Click(object? sender, EventArgs e)
        {
            if (dgvCarrito.CurrentRow == null) return;

            string? idProducto = dgvCarrito.CurrentRow.Cells["IdProducto"].Value?.ToString();

            if (!string.IsNullOrEmpty(idProducto))
            {
                var itemAQuitar = carrito.FirstOrDefault(x => x.IdProducto == idProducto);
                if (itemAQuitar != null)
                {
                    carrito.Remove(itemAQuitar);
                    nombresPersonalizadosUI.Remove(idProducto);

                    // Si quita la Pepsi, el Baggio debe volver a ser caro automáticamente
                    RecalcularDescuentosCarrito();
                    ActualizarGrillaCarrito();
                }
            }
        }

        private void cmbProductos_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (numCantidad != null) numCantidad.Value = 1;

            // Apenas selecciona algo, el motor busca si hay que mostrar el aviso
            if (cmbProductos.SelectedValue != null)
            {
                string idSeleccionado = cmbProductos.SelectedValue.ToString() ?? "";
                string? textoPromo = gestorOfertas.ObtenerTextoPromoRelacionada(idSeleccionado);

                if (!string.IsNullOrEmpty(textoPromo))
                {
                    panelUpselling.Visible = true;
                    lblSugerenciaUpselling.Text = textoPromo;
                }
                else
                {
                    panelUpselling.Visible = false;
                }
            }
        }
        private void RecalcularDescuentosCarrito()
        {
            // Extraemos todos los IDs actuales en el carrito (Ej: ["Pepsi_1.5L", "Baggio_1L"])
            List<string> idsEnCarrito = carrito.Select(c => c.IdProducto).ToList();

            foreach (var item in carrito)
            {
                Producto? prodOrig = inventarioDisponible.FirstOrDefault(p => p.CodigoBarra == item.IdProducto);
                if (prodOrig == null) continue;

                decimal precioBase = prodOrig.Precio;
                // Le pasamos el carrito entero al gestor para que sepa si se cumplen los combos
                decimal porcentaje = gestorOfertas.CalcularDescuentoParaProducto(item.IdProducto, idsEnCarrito);

                if (porcentaje > 0)
                {
                    decimal montoDesc = precioBase * (porcentaje / 100);
                    item.PrecioUnitario = precioBase - montoDesc;
                    nombresPersonalizadosUI[item.IdProducto] = $"{prodOrig.Nombre}\n   (Promo: {porcentaje}% OFF | Orig: {precioBase:C2} | Desc: -{montoDesc:C2})";
                }
                else
                {
                    // Si el combo se rompe (ej: quitaste la Pepsi), el Baggio vuelve a su precio normal
                    item.PrecioUnitario = precioBase;
                    nombresPersonalizadosUI[item.IdProducto] = prodOrig.Nombre;
                }

                item.Subtotal = item.Cantidad * item.PrecioUnitario;
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

            if (lblTituloUpselling != null) lblTituloUpselling.Text = GestorIdioma.GetInstance.TraducirMensaje("lblUpselling_Titulo", "¡OPORTUNIDAD DE VENTA!");

            if (btnAgregarPago != null) btnAgregarPago.Text = GestorIdioma.GetInstance.TraducirMensaje("btnAgregarPago", "Agregar Pago");
            if (btnQuitarPago != null) btnQuitarPago.Text = GestorIdioma.GetInstance.TraducirMensaje("btnQuitarPago", "Quitar Pago");

            int indexPrevio = cmbMetodoPago.SelectedIndex;

            cmbMetodoPago.Items.Clear();
            cmbMetodoPago.Items.Add(GestorIdioma.GetInstance.TraducirMensaje("pago_Efectivo", "Efectivo"));
            cmbMetodoPago.Items.Add(GestorIdioma.GetInstance.TraducirMensaje("pago_Transferencia", "Transferencia"));
            cmbMetodoPago.Items.Add(GestorIdioma.GetInstance.TraducirMensaje("pago_Debito", "Tarjeta de Débito"));
            cmbMetodoPago.Items.Add(GestorIdioma.GetInstance.TraducirMensaje("pago_Credito", "Tarjeta de Crédito"));

            if (indexPrevio >= 0 && indexPrevio < cmbMetodoPago.Items.Count)
            {
                cmbMetodoPago.SelectedIndex = indexPrevio;
            }
            else
            {
                cmbMetodoPago.SelectedIndex = 0;
            }

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

        private void btnAgregarPago_Click(object? sender, EventArgs e)
        {
            if (!carrito.Any())
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_CarritoVacioPagos", "Agregue productos al carrito antes de cobrar."), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalVenta = carrito.Sum(c => c.Subtotal);
            decimal totalPagado = pagosCombinados.Sum(p => p.MontoCobrado);
            decimal restante = totalVenta - totalPagado;

            if (restante <= 0)
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_TotalCubierto", "El total de la venta ya ha sido cubierto."), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int metodoIndex = cmbMetodoPago.SelectedIndex;

            if (metodoIndex == 2 || metodoIndex == 3)
            {
                using (FormMockTarjetaUI formTarjeta = new FormMockTarjetaUI(restante))
                {
                    if (formTarjeta.ShowDialog() == DialogResult.OK)
                    {
                        pagosCombinados.Add(formTarjeta.CobroGenerado);
                        ActualizarGrillaPagos();
                    }
                }
            }
            else if (metodoIndex == 1)
            {
                using (FormTransferenciaUI formTransf = new FormTransferenciaUI(restante))
                {
                    if (formTransf.ShowDialog() == DialogResult.OK)
                    {
                        pagosCombinados.Add(formTransf.CobroGenerado);
                        ActualizarGrillaPagos();
                    }
                }
            }
            else if (metodoIndex == 0)
            {
                using (FormCalculadoraEfectivoUI formEfectivo = new FormCalculadoraEfectivoUI(restante))
                {
                    if (formEfectivo.ShowDialog() == DialogResult.OK)
                    {
                        pagosCombinados.Add(formEfectivo.CobroGenerado);
                        ActualizarGrillaPagos();
                    }
                }
            }
        }
    }
}