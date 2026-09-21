using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using BE;
using BLL;
using System;
using System.Data;
using System.Windows.Forms;

namespace UI.Compras
{
    public partial class BandejaSolicitudesUI : FormBaseObserver
    {
        private GestorCompras gestorCompras;
        private DataView dvDetalles;

        public BandejaSolicitudesUI()
        {
            InitializeComponent();
            gestorCompras = new GestorCompras();

            // Suscripción de eventos de UI
            this.Load += BandejaSolicitudesUI_Load;
            dgvSolicitudesPendientes.SelectionChanged += dgvSolicitudesPendientes_SelectionChanged;
            btnAtenderSolicitud.Click += btnAtenderSolicitud_Click;
            btnVolver.Click += btnVolver_Click;
        }

        private void BandejaSolicitudesUI_Load(object? sender, EventArgs e)
        {
            CargarBandeja();

            // Inicializar el detalle vacío
            dvDetalles = gestorCompras.ObtenerDetallesPorSolicitud(-1);
            dgvDetalleSolicitud.DataSource = dvDetalles;
            OcultarColumnasTecnicasDetalle();

            btnAtenderSolicitud.Enabled = false;
        }

        private void CargarBandeja()
        {
            dgvSolicitudesPendientes.DataSource = gestorCompras.ObtenerSolicitudesPendientes();

            if (dgvSolicitudesPendientes.Columns.Contains("IdSolicitud"))
                dgvSolicitudesPendientes.Columns["IdSolicitud"].HeaderText = "N° Solicitud";
        }

        private void dgvSolicitudesPendientes_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvSolicitudesPendientes.CurrentRow != null)
            {
                int idSolicitud = Convert.ToInt32(dgvSolicitudesPendientes.CurrentRow.Cells["IdSolicitud"].Value);

                // Pedimos al gestor que actualice el detalle
                dvDetalles = gestorCompras.ObtenerDetallesPorSolicitud(idSolicitud);
                dgvDetalleSolicitud.DataSource = dvDetalles;
                OcultarColumnasTecnicasDetalle();

                btnAtenderSolicitud.Enabled = true;
            }
            else
            {
                dvDetalles.RowFilter = "1 = 0";
                btnAtenderSolicitud.Enabled = false;
            }
        }

        private void OcultarColumnasTecnicasDetalle()
        {
            if (dgvDetalleSolicitud.Columns.Contains("IdDetalle")) dgvDetalleSolicitud.Columns["IdDetalle"].Visible = false;
            if (dgvDetalleSolicitud.Columns.Contains("IdSolicitud")) dgvDetalleSolicitud.Columns["IdSolicitud"].Visible = false;
        }

        private void btnAtenderSolicitud_Click(object? sender, EventArgs e)
        {
            if (dgvSolicitudesPendientes.CurrentRow != null)
            {
                int idSolicitud = Convert.ToInt32(dgvSolicitudesPendientes.CurrentRow.Cells["IdSolicitud"].Value);

                // Abrimos el generador pasándole el ID
                GenerarOrdenCompraUI formOrdenCompra = new GenerarOrdenCompraUI(idSolicitud);
                formOrdenCompra.ShowDialog();

                // Recargamos la grilla superior por si la solicitud se procesó y debe desaparecer
                CargarBandeja();
            }
        }

        private void btnVolver_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}