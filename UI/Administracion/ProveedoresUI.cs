using BE;
using BLL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI.Almacen
{
    // 1. Heredamos de FormBaseObserver en lugar de Form
    public partial class ProveedoresUI : FormBaseObserver
    {
        private GestorProveedor gestorProveedor;
        private Guid idSeleccionado = Guid.Empty;

        public ProveedoresUI()
        {
            InitializeComponent();
            gestorProveedor = new GestorProveedor();

            this.Load += ProveedoresUI_Load;
            btnGuardar.Click += BtnGuardar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            btnMigrarSQL.Click += BtnMigrarSQL_Click;
            dgvProveedores.SelectionChanged += DgvProveedores_SelectionChanged;
        }

        private void ProveedoresUI_Load(object? sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            dgvProveedores.DataSource = null;
            dgvProveedores.DataSource = gestorProveedor.ObtenerProveedoresActivos();

            if (dgvProveedores.Columns.Contains("Id")) dgvProveedores.Columns["Id"].Visible = false;
            if (dgvProveedores.Columns.Contains("Activo")) dgvProveedores.Columns["Activo"].Visible = false;

            // Forzamos la traducción de las columnas cada vez que se recarga la grilla
            TraducirElementosParticulares(GestorIdioma.GetInstance.IdiomaActual);

            dgvProveedores.ClearSelection();
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                Proveedor prov = new Proveedor
                {
                    Id = idSeleccionado,
                    CUIT = txtCUIT.Text.Trim(),
                    RazonSocial = txtRazonSocial.Text.Trim(),
                    CondicionComercial = txtCondicionComercial.Text.Trim(),
                    Activo = true
                };

                string tituloExito = GestorIdioma.GetInstance.TraducirMensaje("msg_Exito", "Éxito");

                if (idSeleccionado == Guid.Empty)
                {
                    gestorProveedor.RegistrarProveedor(prov);
                    string msjGuardado = GestorIdioma.GetInstance.TraducirMensaje("msg_ProvGuardadoExito", "Proveedor guardado en el archivo JSON exitosamente.");
                    MessageBox.Show(msjGuardado, tituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    gestorProveedor.ModificarProveedor(prov);
                    string msjActualizado = GestorIdioma.GetInstance.TraducirMensaje("msg_ProvActualizadoExito", "Proveedor actualizado en el archivo JSON exitosamente.");
                    MessageBox.Show(msjActualizado, tituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LimpiarCampos();
                CargarGrilla();
            }
            catch (Exception ex)
            {
                string tituloError = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error");
                MessageBox.Show(ex.Message, tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvProveedores_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvProveedores.CurrentRow != null && dgvProveedores.CurrentRow.DataBoundItem != null)
            {
                Proveedor prov = (Proveedor)dgvProveedores.CurrentRow.DataBoundItem;
                idSeleccionado = prov.Id;

                txtCUIT.Text = prov.CUIT;
                txtRazonSocial.Text = prov.RazonSocial;
                txtCondicionComercial.Text = prov.CondicionComercial;
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (idSeleccionado != Guid.Empty)
            {
                string msjConfirmar = GestorIdioma.GetInstance.TraducirMensaje("msg_ConfirmarBajaProv", "¿Está seguro que desea dar de baja este proveedor?");
                string tituloConfirmar = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloConfirmar", "Confirmar");

                DialogResult res = MessageBox.Show(msjConfirmar, tituloConfirmar, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    gestorProveedor.DarDeBaja(idSeleccionado);
                    LimpiarCampos();
                    CargarGrilla();
                }
            }
            else
            {
                string msjSelec = GestorIdioma.GetInstance.TraducirMensaje("msg_SelecProvEliminar", "Seleccione un proveedor de la grilla para eliminar.");
                string tituloAten = GestorIdioma.GetInstance.TraducirMensaje("msg_Atencion", "Atención");
                MessageBox.Show(msjSelec, tituloAten, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnLimpiar_Click(object? sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            idSeleccionado = Guid.Empty;
            txtCUIT.Clear();
            txtRazonSocial.Clear();
            txtCondicionComercial.Clear();
            dgvProveedores.ClearSelection();
        }

        private void BtnMigrarSQL_Click(object? sender, EventArgs e)
        {
            string msjMigracion = GestorIdioma.GetInstance.TraducirMensaje("msg_AvisoMigracion", "¿Desea importar los proveedores de la Base de Datos SQL al archivo JSON?\n\n¡ATENCIÓN! Esto sobrescribirá los datos del archivo local actual.");
            string tituloMigracion = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloMigracion", "Migración SQL -> JSON");

            DialogResult res = MessageBox.Show(msjMigracion, tituloMigracion, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                try
                {
                    gestorProveedor.MigrarProveedoresDesdeSQL();
                    LimpiarCampos();
                    CargarGrilla();

                    string msjExitoMigracion = GestorIdioma.GetInstance.TraducirMensaje("msg_ExitoMigracion", "¡Migración completada! Los proveedores de prueba ahora viven en el archivo JSON.");
                    string tituloExito = GestorIdioma.GetInstance.TraducirMensaje("msg_Exito", "Éxito");
                    MessageBox.Show(msjExitoMigracion, tituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    string tituloError = GestorIdioma.GetInstance.TraducirMensaje("msg_ErrorMigracion", "Error de Migración");
                    MessageBox.Show(ex.Message, tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 2. Método obligatorio del FormBaseObserver
        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            this.Text = GestorIdioma.GetInstance.TraducirMensaje("ProveedoresUI_Text", "ABM Proveedores (JSON)");
            lblTitulo.Text = GestorIdioma.GetInstance.TraducirMensaje("ProveedoresUI_Titulo", "Gestión de Proveedores (Persistencia JSON)");

            lblCUIT.Text = GestorIdioma.GetInstance.TraducirMensaje("lblCUIT", "CUIT");
            lblRazonSocial.Text = GestorIdioma.GetInstance.TraducirMensaje("lblRazonSocial", "Razón Social");
            lblCondicion.Text = GestorIdioma.GetInstance.TraducirMensaje("lblCondicion", "Condición Comercial");

            btnGuardar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnGuardarProv", "Guardar Proveedor");
            btnLimpiar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnLimpiarSeleccion", "Limpiar Selección");
            btnEliminar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnEliminarProv", "Eliminar Proveedor");
            btnMigrarSQL.Text = GestorIdioma.GetInstance.TraducirMensaje("btnMigrarSQL", "Importar Datos de Prueba (SQL -> JSON)");

            if (dgvProveedores != null && dgvProveedores.Columns.Count > 0)
            {
                if (dgvProveedores.Columns.Contains("RazonSocial"))
                    dgvProveedores.Columns["RazonSocial"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_RazonSocial", "Razón Social");

                if (dgvProveedores.Columns.Contains("CondicionComercial"))
                    dgvProveedores.Columns["CondicionComercial"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_CondicionComercial", "Condición Comercial");
            }
        }
    }
}