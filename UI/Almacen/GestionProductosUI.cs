using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;

namespace UI.Almacen
{
    public partial class GestionProductosUI : FormBaseObserver
    {
        private GestorProducto gestorProducto;
        private List<Producto> inventarioCache;
        private bool modoEdicion = false;

        public GestionProductosUI()
        {
            InitializeComponent();
            gestorProducto = new GestorProducto();

            this.Load += GestionProductosUI_Load;
            btnGuardar.Click += BtnGuardar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;

            // NUEVO: Evento del botón para ver los lotes
            if (btnConsultarLotes != null)
                btnConsultarLotes.Click += BtnConsultarLotes_Click;

            dgvProductos.SelectionChanged += DgvProductos_SelectionChanged;
        }

        private void GestionProductosUI_Load(object? sender, EventArgs e)
        {
            CargarGrilla();
            LimpiarCampos();
        }

        private void CargarGrilla()
        {
            try
            {
                inventarioCache = gestorProducto.ObtenerInventario().Where(p => p.Activo).ToList();
                dgvProductos.DataSource = null;
                dgvProductos.DataSource = inventarioCache;

                ConfigurarColumnasGrilla();
                TraducirElementosParticulares(GestorIdioma.GetInstance.IdiomaActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnasGrilla()
        {
            if (dgvProductos.Columns.Contains("Activo")) dgvProductos.Columns["Activo"].Visible = false;

            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.ReadOnly = true;
            dgvProductos.MultiSelect = false;
        }

        private void DgvProductos_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow != null)
            {
                Producto prod = (Producto)dgvProductos.CurrentRow.DataBoundItem;

                txtCodigoBarra.Text = prod.CodigoBarra;
                txtCodigoBarra.Enabled = false; // No se permite editar el ID/Código una vez creado

                txtNombre.Text = prod.Nombre;
                numPrecio.Value = prod.Precio;
                numPuntoPedido.Value = prod.PuntoPedido;

                modoEdicion = true;
                btnEliminar.Enabled = true;
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigoBarra.Text) || string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("err_CamposVacios", "Todos los campos obligatorios deben estar completos."),
                                GestorIdioma.GetInstance.TraducirMensaje("msg_Atencion", "Atención"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numPrecio.Value <= 0)
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("err_PrecioInvalido", "El precio debe ser mayor a cero."),
                                GestorIdioma.GetInstance.TraducirMensaje("msg_Atencion", "Atención"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Producto prod = new Producto
                {
                    CodigoBarra = txtCodigoBarra.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Precio = numPrecio.Value,
                    PuntoPedido = (int)numPuntoPedido.Value,
                    Activo = true,
                    // CORRECCIÓN: Si es nuevo, el stock físico empieza en 0 hasta que reciba un remito.
                    StockActual = modoEdicion ? (int)dgvProductos.CurrentRow.Cells["StockActual"].Value : 0
                };

                if (modoEdicion)
                {
                    gestorProducto.ModificarProducto(prod);

                    string username = SessionManager.getInstance.ObtenerUsuarioActivo()?.Username ?? "Sistema";
                    GestorBitacora.GetInstance.Update(username, $"LOG_PROD_MODIFICADO_{prod.CodigoBarra}");

                    MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_ProductoModificado", "Producto modificado exitosamente."),
                                    GestorIdioma.GetInstance.TraducirMensaje("msg_TituloExito", "Éxito"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    gestorProducto.RegistrarNuevoProducto(prod);

                    string username = SessionManager.getInstance.ObtenerUsuarioActivo()?.Username ?? "Sistema";
                    GestorBitacora.GetInstance.Update(username, $"LOG_PROD_NUEVO_{prod.CodigoBarra}");

                    MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_ProductoRegistrado", "Producto registrado exitosamente."),
                                    GestorIdioma.GetInstance.TraducirMensaje("msg_TituloExito", "Éxito"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarGrilla();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null) return;

            string codigoProd = txtCodigoBarra.Text;

            DialogResult respuesta = MessageBox.Show(
                GestorIdioma.GetInstance.TraducirMensaje("msg_ConfirmarBajaProd", "¿Está seguro que desea dar de baja este producto del catálogo?"),
                GestorIdioma.GetInstance.TraducirMensaje("msg_Atencion", "Atención"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    gestorProducto.DarDeBajaProducto(codigoProd);

                    string username = SessionManager.getInstance.ObtenerUsuarioActivo()?.Username ?? "Sistema";
                    GestorBitacora.GetInstance.Update(username, $"LOG_PROD_BAJA_{codigoProd}");

                    MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_ProductoBajaExitosa", "Producto dado de baja exitosamente."),
                                    GestorIdioma.GetInstance.TraducirMensaje("msg_TituloExito", "Éxito"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarGrilla();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnLimpiar_Click(object? sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtCodigoBarra.Clear();
            txtCodigoBarra.Enabled = true;
            txtNombre.Clear();
            numPrecio.Value = 0;
            numPuntoPedido.Value = 0;

            modoEdicion = false;
            btnEliminar.Enabled = false;
            dgvProductos.ClearSelection();
        }

        // =========================================================
        // NUEVO: LÓGICA DE VENTANA EMERGENTE PARA AUDITORÍA FEFO
        // =========================================================
        //private void BtnConsultarLotes_Click(object? sender, EventArgs e)
        //{
        //    if (dgvProductos.CurrentRow != null)
        //    {
        //        string idProd = dgvProductos.CurrentRow.Cells["CodigoBarra"].Value.ToString() ?? "";
        //        string nombreProd = dgvProductos.CurrentRow.Cells["Nombre"].Value.ToString() ?? "";

        //        DataTable lotes = gestorProducto.ConsultarLotesActivos(idProd);

        //        if (lotes.Rows.Count == 0)
        //        {
        //            MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_SinLotes", "No hay lotes con stock activo para este producto."),
        //                            GestorIdioma.GetInstance.TraducirMensaje("msg_Informacion", "Información"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return;
        //        }

        //        Form frmLotes = new Form
        //        {
        //            Text = GestorIdioma.GetInstance.TraducirMensaje("lbl_LotesActivos", "Lotes Activos y Vencimientos") + $" - {nombreProd}",
        //            Size = new System.Drawing.Size(600, 300),
        //            StartPosition = FormStartPosition.CenterParent,
        //            ShowIcon = false,
        //            MinimizeBox = false,
        //            MaximizeBox = false
        //        };

        //        DataGridView dgvLotes = new DataGridView
        //        {
        //            DataSource = lotes,
        //            Dock = DockStyle.Fill,
        //            ReadOnly = true,
        //            AllowUserToAddRows = false,
        //            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        //            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        //            BackgroundColor = System.Drawing.Color.WhiteSmoke
        //        };

        //        dgvLotes.DataBindingComplete += (s, ev) =>
        //        {
        //            if (dgvLotes.Columns.Contains("IdLote")) dgvLotes.Columns["IdLote"].Visible = false;
        //            if (dgvLotes.Columns.Contains("IdRecepcion")) dgvLotes.Columns["IdRecepcion"].Visible = false;
        //            if (dgvLotes.Columns.Contains("IdProducto")) dgvLotes.Columns["IdProducto"].Visible = false;

        //            if (dgvLotes.Columns.Contains("NumeroLote")) dgvLotes.Columns["NumeroLote"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_NumLote", "N° de Lote Físico");
        //            if (dgvLotes.Columns.Contains("CantidadRecibida")) dgvLotes.Columns["CantidadRecibida"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_CantRecibida", "Cant. Original");
        //            if (dgvLotes.Columns.Contains("StockActual")) dgvLotes.Columns["StockActual"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_StockRestante", "Stock Restante");

        //            if (dgvLotes.Columns.Contains("FechaVencimiento"))
        //            {
        //                dgvLotes.Columns["FechaVencimiento"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_FechaVto", "Fecha de Vto.");
        //                dgvLotes.Columns["FechaVencimiento"].DefaultCellStyle.Format = "dd/MM/yyyy";
        //                dgvLotes.Columns["FechaVencimiento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //            }
        //        };

        //        frmLotes.Controls.Add(dgvLotes);
        //        frmLotes.ShowDialog();
        //    }
        //    else
        //    {
        //        MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_SeleccioneProd", "Seleccione un producto para ver sus lotes."),
        //                        GestorIdioma.GetInstance.TraducirMensaje("msg_Atencion", "Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}
        private void BtnConsultarLotes_Click(object? sender, EventArgs e)
        {
            DataTable todosLosLotes = gestorProducto.ConsultarTodosLotesActivos();

            if (todosLosLotes.Rows.Count == 0)
            {
                MessageBox.Show(GestorIdioma.GetInstance.TraducirMensaje("msg_SinLotesGlobal", "No hay ningún lote con stock activo en todo el almacén."),
                                GestorIdioma.GetInstance.TraducirMensaje("msg_Informacion", "Información"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ==========================================
            // VENTANA GLOBAL DE LOTES CON FILTRO INTEGRADO
            // ==========================================
            Form frmLotes = new Form
            {
                Text = GestorIdioma.GetInstance.TraducirMensaje("lbl_DashboardLotes", "Dashboard Global de Lotes y Vencimientos"),
                Size = new System.Drawing.Size(750, 400),
                StartPosition = FormStartPosition.CenterParent,
                ShowIcon = false,
                MinimizeBox = false,
                MaximizeBox = false
            };

            // Panel superior para el buscador
            Panel pnlTop = new Panel { Dock = DockStyle.Top, Height = 50 };
            Label lblFiltro = new Label { Text = GestorIdioma.GetInstance.TraducirMensaje("lbl_BuscarLote", "Buscar por Producto:"), Location = new Point(10, 15), AutoSize = true };
            TextBox txtFiltro = new TextBox { Location = new Point(150, 12), Width = 300 };

            pnlTop.Controls.Add(lblFiltro);
            pnlTop.Controls.Add(txtFiltro);

            // DataView para permitir el filtrado en vivo
            DataView dvLotes = new DataView(todosLosLotes);

            DataGridView dgvLotes = new DataGridView
            {
                DataSource = dvLotes,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.WhiteSmoke
            };

            // Evento para filtrar la grilla al escribir
            txtFiltro.TextChanged += (s, ev) =>
            {
                string filtro = txtFiltro.Text.Trim().Replace("'", "''");
                if (string.IsNullOrEmpty(filtro))
                    dvLotes.RowFilter = "";
                else
                    dvLotes.RowFilter = $"Producto LIKE '%{filtro}%' OR CodigoBarra LIKE '%{filtro}%'";
            };

            // Formateo visual
            dgvLotes.DataBindingComplete += (s, ev) =>
            {
                if (dgvLotes.Columns.Contains("CodigoBarra")) dgvLotes.Columns["CodigoBarra"].HeaderText = "SKU";
                if (dgvLotes.Columns.Contains("Producto")) dgvLotes.Columns["Producto"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Producto", "Producto");
                if (dgvLotes.Columns.Contains("NumeroLote")) dgvLotes.Columns["NumeroLote"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_NumLote", "Lote Físico");
                if (dgvLotes.Columns.Contains("StockActual")) dgvLotes.Columns["StockActual"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_StockRestante", "Stock");

                if (dgvLotes.Columns.Contains("FechaVencimiento"))
                {
                    dgvLotes.Columns["FechaVencimiento"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_FechaVto", "Vencimiento");
                    dgvLotes.Columns["FechaVencimiento"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvLotes.Columns["FechaVencimiento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // Colorear en rojo los lotes que vencen en menos de 15 días (Alerta Visual)
                foreach (DataGridViewRow row in dgvLotes.Rows)
                {
                    if (row.Cells["FechaVencimiento"].Value != DBNull.Value)
                    {
                        DateTime fechaVto = Convert.ToDateTime(row.Cells["FechaVencimiento"].Value);
                        if ((fechaVto - DateTime.Now).TotalDays <= 15)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
            };

            frmLotes.Controls.Add(dgvLotes);
            frmLotes.Controls.Add(pnlTop);
            frmLotes.ShowDialog();
        }

        // =========================================================
        // IMPLEMENTACIÓN DE PATRÓN OBSERVER (MULTI-IDIOMA)
        // =========================================================
        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            this.Text = GestorIdioma.GetInstance.TraducirMensaje("GestionProductosUI", "Gestión de Productos (ABM)");
            gbxFormulario.Text = GestorIdioma.GetInstance.TraducirMensaje("gbxFormularioABM", "Datos del Producto");
            gbxListado.Text = GestorIdioma.GetInstance.TraducirMensaje("gbxListadoProductos", "Catálogo Activo");

            lblCodigoBarra.Text = GestorIdioma.GetInstance.TraducirMensaje("lblCodigoBarra", "Código de Barras (SKU):");
            lblNombre.Text = GestorIdioma.GetInstance.TraducirMensaje("lblNombreProd", "Nombre del Producto:");
            lblPrecio.Text = GestorIdioma.GetInstance.TraducirMensaje("lblPrecioUnitario", "Precio Unitario local:");
            lblPuntoPedido.Text = GestorIdioma.GetInstance.TraducirMensaje("lblPuntoPedidoAbm", "Punto de Pedido (Alerta):");

            btnGuardar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnGuardar", "Guardar");
            btnEliminar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnEliminar", "Dar de Baja");
            btnLimpiar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnLimpiar", "Limpiar Campos");

            if (btnConsultarLotes != null)
                btnConsultarLotes.Text = GestorIdioma.GetInstance.TraducirMensaje("btnConsultarLotes", "Consultar Lotes");

            if (dgvProductos.Columns.Count > 0)
            {
                if (dgvProductos.Columns.Contains("CodigoBarra"))
                    dgvProductos.Columns["CodigoBarra"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_CodigoBarras", "Código de Barras");
                if (dgvProductos.Columns.Contains("Nombre"))
                    dgvProductos.Columns["Nombre"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Producto", "Producto");
                if (dgvProductos.Columns.Contains("Precio"))
                    dgvProductos.Columns["Precio"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_PrecioUnitario", "Precio Unitario");
                if (dgvProductos.Columns.Contains("StockActual"))
                    dgvProductos.Columns["StockActual"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_StockActual", "Stock Actual");
                if (dgvProductos.Columns.Contains("PuntoPedido"))
                    dgvProductos.Columns["PuntoPedido"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_PuntoPedido", "Punto de Pedido");
            }
        }
    }
}