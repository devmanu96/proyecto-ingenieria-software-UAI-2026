using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using BE;
using BLL;
using System.Windows.Forms;

namespace UI.Compras
{
    public partial class BandejaSolicitudesUI : FormBaseObserver
    {
        private GestorCompras gestorCompras;
        private GestorProducto gestorProducto;
        private DataView dvDetalles;
        private List<Producto> inventario;

        public BandejaSolicitudesUI()
        {
            InitializeComponent();
            gestorCompras = new GestorCompras();
            gestorProducto = new GestorProducto();

            this.Load += BandejaSolicitudesUI_Load;
            dgvSolicitudesPendientes.SelectionChanged += dgvSolicitudesPendientes_SelectionChanged;
            btnAtenderSolicitud.Click += btnAtenderSolicitud_Click;
            btnVolver.Click += btnVolver_Click;
        }

        private void BandejaSolicitudesUI_Load(object? sender, EventArgs e)
        {
            // LOG BITACORA: Auditoría de acceso a información comercial
            string username = SessionManager.getInstance.ObtenerUsuarioActivo()?.Username ?? "Sistema";
            GestorBitacora.GetInstance.Update(username, "LOG_INGRESO_BANDEJA_COMPRAS");

            inventario = gestorProducto.ObtenerInventario();
            CargarBandeja();

            dgvDetalleSolicitud.DataSource = null;
            btnAtenderSolicitud.Enabled = false;
        }

        private void CargarBandeja()
        {
            DataView dvSolicitudes = gestorCompras.ObtenerSolicitudesPendientes();
            DataTable dtSolicitudes = dvSolicitudes.ToTable();

            DataTable dtTraducido = dtSolicitudes.Clone();
            dtTraducido.Columns["Estado"].DataType = typeof(string);

            foreach (DataRow row in dtSolicitudes.Rows)
            {
                DataRow nuevaFila = dtTraducido.NewRow();
                nuevaFila.ItemArray = row.ItemArray;

                if (nuevaFila["Estado"].ToString() == "Pendiente de Compras")
                {
                    nuevaFila["Estado"] = GestorIdioma.GetInstance.TraducirMensaje("estado_PendienteCompras", "Pendiente de Compras");
                }

                dtTraducido.Rows.Add(nuevaFila);
            }

            dgvSolicitudesPendientes.DataSource = dtTraducido;

            if (dgvSolicitudesPendientes.Columns.Contains("IdSolicitud"))
                dgvSolicitudesPendientes.Columns["IdSolicitud"].HeaderText = "N° Solicitud";
        }

        private void dgvSolicitudesPendientes_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvSolicitudesPendientes.CurrentRow != null)
            {
                int idSolicitud = Convert.ToInt32(dgvSolicitudesPendientes.CurrentRow.Cells["IdSolicitud"].Value);
                dvDetalles = gestorCompras.ObtenerDetallesPorSolicitud(idSolicitud);

                var detallesFormateados = (from d in dvDetalles.Table.AsEnumerable()
                                           where d.Field<int>("IdSolicitud") == idSolicitud
                                           join p in inventario on d.Field<string>("IdProducto") equals p.CodigoBarra
                                           select new
                                           {
                                               IdProducto = d.Field<string>("IdProducto"),
                                               NombreProducto = p.Nombre,
                                               CantidadSolicitada = d.Field<int>("CantidadSolicitada")
                                           }).ToList();

                dgvDetalleSolicitud.DataSource = detallesFormateados;
                dgvDetalleSolicitud.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                TraducirElementosParticulares(GestorIdioma.GetInstance.IdiomaActual);

                btnAtenderSolicitud.Enabled = true;
            }
            else
            {
                dgvDetalleSolicitud.DataSource = null;
                btnAtenderSolicitud.Enabled = false;
            }
        }

        private void btnAtenderSolicitud_Click(object? sender, EventArgs e)
        {
            if (dgvSolicitudesPendientes.CurrentRow != null)
            {
                int idSolicitud = Convert.ToInt32(dgvSolicitudesPendientes.CurrentRow.Cells["IdSolicitud"].Value);

                GenerarOrdenCompraUI formOrdenCompra = new GenerarOrdenCompraUI(idSolicitud);
                formOrdenCompra.ShowDialog();

                CargarBandeja();
            }
        }

        private void btnVolver_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            this.Text = GestorIdioma.GetInstance.TraducirMensaje("BandejaSolicitudesUI", "Bandeja de Solicitudes");
            if (dgvSolicitudesPendientes.Columns.Contains("IdSolicitud"))
                dgvSolicitudesPendientes.Columns["IdSolicitud"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_NumSolicitud", "N° Solicitud");

            if (dgvSolicitudesPendientes.Columns.Contains("FechaGeneracion"))
                dgvSolicitudesPendientes.Columns["FechaGeneracion"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_FechaGeneracion", "Fecha de Generación");

            if (dgvSolicitudesPendientes.Columns.Contains("Estado"))
                dgvSolicitudesPendientes.Columns["Estado"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Estado", "Estado");

            if (dgvDetalleSolicitud.Columns.Contains("IdProducto"))
                dgvDetalleSolicitud.Columns["IdProducto"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_CodigoBarras", "SKU / Código");

            if (dgvDetalleSolicitud.Columns.Contains("NombreProducto"))
                dgvDetalleSolicitud.Columns["NombreProducto"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Producto", "Producto");

            if (dgvDetalleSolicitud.Columns.Contains("CantidadSolicitada"))
                dgvDetalleSolicitud.Columns["CantidadSolicitada"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Cantidad", "Cantidad Solicitada");
        }
    }
}