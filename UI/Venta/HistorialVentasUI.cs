using BE;
using BLL;
using System;
using System.Data;
using System.Windows.Forms;

namespace UI.Venta
{
    public partial class HistorialVentasUI : FormBaseObserver
    {
        private GestorHistorialVentas gestorHistorial;

        public HistorialVentasUI()
        {
            InitializeComponent();
            gestorHistorial = new GestorHistorialVentas();

            this.Load += HistorialVentasUI_Load;
            btnActualizar.Click += btnActualizar_Click;
        }

        private void HistorialVentasUI_Load(object? sender, EventArgs e)
        {
            // Forzamos el acople perfecto en el contenedor padre
            this.Dock = DockStyle.Fill;
            CargarHistorial();
        }

        private void btnActualizar_Click(object? sender, EventArgs e)
        {
            CargarHistorial();
        }

        private void CargarHistorial()
        {
            try
            {
                DataView dvHistorial = gestorHistorial.ObtenerHistorial();

                // 1. Desvincular y resetear completamente la grilla
                dgvHistorial.DataSource = null;
                dgvHistorial.AutoGenerateColumns = true;
                dgvHistorial.Columns.Clear();

                // 2. Asignar los datos frescos (esto crea las columnas dinámicamente)
                dgvHistorial.DataSource = dvHistorial;

                // 3. Ocultar la columna interna si existe
                if (dgvHistorial.Columns.Contains("IdVenta"))
                    dgvHistorial.Columns["IdVenta"].Visible = false;

                // 4. Traducir encabezados
                TraducirElementosParticulares(GestorIdioma.GetInstance.IdiomaActual);
                dgvHistorial.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            this.Text = GestorIdioma.GetInstance.TraducirMensaje("HistorialVentasUI", "Historial de Ventas");
            btnActualizar.Text = GestorIdioma.GetInstance.TraducirMensaje("btnActualizar", "Actualizar Historial");

            if (dgvHistorial != null && dgvHistorial.Columns.Count > 0)
            {
                if (dgvHistorial.Columns.Contains("FechaVenta"))
                    dgvHistorial.Columns["FechaVenta"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_FechaVenta", "Fecha");
                if (dgvHistorial.Columns.Contains("Cliente"))
                    dgvHistorial.Columns["Cliente"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Cliente", "Cliente");
                if (dgvHistorial.Columns.Contains("Vendedor"))
                    dgvHistorial.Columns["Vendedor"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Vendedor", "Vendedor");
                if (dgvHistorial.Columns.Contains("MontoTotal"))
                    dgvHistorial.Columns["MontoTotal"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_MontoTotal", "Monto Total");
                if (dgvHistorial.Columns.Contains("Estado"))
                    dgvHistorial.Columns["Estado"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Estado", "Estado");
                if (dgvHistorial.Columns.Contains("PagosCombinados"))
                    dgvHistorial.Columns["PagosCombinados"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_PagosCombinados", "Detalle de Pagos");
            }
        }
    }
}