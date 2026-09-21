using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace UI.Compras
{
    public partial class GenerarOrdenCompraUI : FormBaseObserver
    {
        private int idSolicitudReferencia;
        private GestorCompras gestorCompras;

        private DataView dvCatalogo;
        private BindingList<DetalleOC> bindingCarrito;
        private OrdenCompra ordenActual;
        private Dictionary<int, int> mapaProveedoresTab;

        // Controles dinámicos para el selector de idioma responsive
        private Label lblIdiomaForm;
        private ComboBox comboIdiomaForm;

        public GenerarOrdenCompraUI(int idSolicitud)
        {
            InitializeComponent();
            idSolicitudReferencia = idSolicitud;
            gestorCompras = new GestorCompras();

            ordenActual = new OrdenCompra();
            bindingCarrito = new BindingList<DetalleOC>(ordenActual.Detalles);

            // --- CREACIÓN DINÁMICA DEL SELECTOR DE IDIOMAS RESPONSIVE ---
            lblIdiomaForm = new Label();
            lblIdiomaForm.Text = "Idioma";
            lblIdiomaForm.AutoSize = true;
            lblIdiomaForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            comboIdiomaForm = new ComboBox();
            comboIdiomaForm.DropDownStyle = ComboBoxStyle.DropDownList;
            comboIdiomaForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboIdiomaForm.Width = 100;

            ActualizarPosicionIdioma();

            var listaIdiomas = GestorIdioma.GetInstance.ObtenerIdiomasDisponibles();
            comboIdiomaForm.DataSource = listaIdiomas;
            comboIdiomaForm.DisplayMember = "Nombre";
            comboIdiomaForm.ValueMember = "Codigo";

            Usuario? usuarioActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
            if (usuarioActivo != null && !string.IsNullOrEmpty(usuarioActivo.Idioma))
            {
                comboIdiomaForm.SelectedValue = usuarioActivo.Idioma;
            }
            else
            {
                comboIdiomaForm.SelectedValue = "ES";
            }

            comboIdiomaForm.SelectedIndexChanged += (s, e) =>
            {
                if (comboIdiomaForm.SelectedItem == null) return;
                BE.Idioma idiomaSeleccionado = (BE.Idioma)comboIdiomaForm.SelectedItem;
                GestorIdioma.GetInstance.CambiarIdioma(idiomaSeleccionado.Codigo);
            };

            this.Controls.Add(lblIdiomaForm);
            this.Controls.Add(comboIdiomaForm);
            // -------------------------------------------------------------

            // Suscripción de eventos de UI
            this.Load += GenerarOrdenCompraUI_Load;
            this.Resize += (s, e) => ActualizarPosicionIdioma();

            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            btnAgregarCarrito.Click += btnAgregarCarrito_Click;
            btnEmitirOrden.Click += btnEmitirOrden_Click;
            btnCancelarOC.Click += btnCancelarOC_Click;
        }

        private void ActualizarPosicionIdioma()
        {
            int margenDerecho = 30;
            int topEtiqueta = 15;
            int topCombo = 33;

            comboIdiomaForm.Location = new System.Drawing.Point(this.ClientSize.Width - comboIdiomaForm.Width - margenDerecho, topCombo);
            lblIdiomaForm.Location = new System.Drawing.Point(comboIdiomaForm.Left, topEtiqueta);

            lblIdiomaForm.BringToFront();
            comboIdiomaForm.BringToFront();
        }

        private void GenerarOrdenCompraUI_Load(object? sender, EventArgs e)
        {
            // 1. Cargar Faltantes
            dgvFaltantes.DataSource = gestorCompras.ObtenerDetallesPorSolicitud(idSolicitudReferencia);
            if (dgvFaltantes.Columns.Contains("IdDetalle")) dgvFaltantes.Columns["IdDetalle"].Visible = false;
            if (dgvFaltantes.Columns.Contains("IdSolicitud")) dgvFaltantes.Columns["IdSolicitud"].Visible = false;

            // 2. Cargar Carrito
            dgvCarritoOC.DataSource = bindingCarrito;
            if (dgvCarritoOC.Columns.Contains("IdOrden")) dgvCarritoOC.Columns["IdOrden"].Visible = false;
            if (dgvCarritoOC.Columns.Contains("IdDetalleOC")) dgvCarritoOC.Columns["IdDetalleOC"].Visible = false;

            // 3. Cargar Catálogo B2B
            dvCatalogo = gestorCompras.ObtenerCatalogoB2B();
            dgvCatalogo.DataSource = dvCatalogo;
            dgvCatalogo.ReadOnly = true;
            dgvCatalogo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // 4. Mapear Tabs y acomodar grilla
            mapaProveedoresTab = gestorCompras.ObtenerMapaProveedoresTab();
            AcomodarGrillaCatalogo();
            ActualizarTotal();
        }

        private void tabControl1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            AcomodarGrillaCatalogo();
        }

        private void AcomodarGrillaCatalogo()
        {
            // CORRECCIÓN GRILLA: Añadimos la grilla a la pestaña seleccionada
            tabControl1.SelectedTab.Controls.Add(dgvCatalogo);
            dgvCatalogo.Dock = DockStyle.Fill;
            dgvCatalogo.BringToFront();

            int tabIndex = tabControl1.SelectedIndex;
            if (mapaProveedoresTab.ContainsKey(tabIndex))
            {
                dvCatalogo.RowFilter = $"IdProveedor = {mapaProveedoresTab[tabIndex]}";
            }
        }

        private void btnAgregarCarrito_Click(object? sender, EventArgs e)
        {
            if (dgvCatalogo.CurrentRow != null)
            {
                string idProd = dgvCatalogo.CurrentRow.Cells["IdProducto"].Value.ToString() ?? string.Empty;
                decimal precio = Convert.ToDecimal(dgvCatalogo.CurrentRow.Cells["PrecioPallet"].Value);
                int cantidad = (int)numericUpDown1.Value;

                var itemExistente = ordenActual.Detalles.FirstOrDefault(d => d.IdProducto == idProd);
                if (itemExistente != null)
                {
                    itemExistente.CantidadSolicitada += cantidad;
                }
                else
                {
                    ordenActual.Detalles.Add(new DetalleOC
                    {
                        IdProducto = idProd,
                        CantidadSolicitada = cantidad,
                        PrecioAcordado = precio
                    });
                }

                bindingCarrito.ResetBindings();
                ActualizarTotal();
                numericUpDown1.Value = 1;
            }
            else
            {
                MessageBox.Show("Seleccione un producto del catálogo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActualizarTotal()
        {
            lblTotalOC.Text = $"Total Neto: {ordenActual.MontoTotal:C2}";
        }

        private void btnEmitirOrden_Click(object? sender, EventArgs e)
        {
            if (ordenActual.Detalles.Count == 0)
            {
                MessageBox.Show("El carrito está vacío. Agregue pallets antes de emitir la orden.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int tabIndex = tabControl1.SelectedIndex;
                ordenActual.IdProveedor = mapaProveedoresTab[tabIndex];

                // CORRECCIÓN CAMPOS NULOS: Establecemos los datos de auditoría
                ordenActual.FechaEmision = DateTime.Now;
                ordenActual.Estado = "Emitida";

                // Delegamos la emisión al BLL
                gestorCompras.EmitirOrdenCompra(ordenActual, idSolicitudReferencia);

                MessageBox.Show("Orden de Compra generada y enviada a Contabilidad exitosamente.", "Éxito B2B", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al emitir la orden: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelarOC_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}