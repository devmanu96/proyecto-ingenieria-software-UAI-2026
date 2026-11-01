using BE;
using BLL; // Lo crearemos en el próximo paso
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI.Administracion
{
    public partial class GestionOfertasInteligentesUI : FormBaseObserver
    {
        private GestorOfertas gestorOfertas;
        private List<OfertaSugerida> ofertasPendientes;
        private OfertaSugerida ofertaSeleccionada;

        public GestionOfertasInteligentesUI()
        {
            InitializeComponent();
            gestorOfertas = new GestorOfertas();
            ofertasPendientes = new List<OfertaSugerida>();

            this.Load += GestionOfertasInteligentesUI_Load;
            btnEjecutarMotor.Click += BtnEjecutarMotor_Click;
            dgvOfertas.SelectionChanged += DgvOfertas_SelectionChanged;
            btnAprobar.Click += BtnAprobar_Click;
            btnDescartar.Click += BtnDescartar_Click;
        }

        private void GestionOfertasInteligentesUI_Load(object? sender, EventArgs e)
        {
            panelEdicion.Enabled = false;
            CargarGrilla();
        }

        private void BtnEjecutarMotor_Click(object? sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // 1. Ejecutamos el motor (ahora guarda directo en DB)
                gestorOfertas.EjecutarMotorAnaliticoCompleto();

                // 2. Volvemos a leer de la DB las ofertas pendientes
                // (Asegúrate de tener este método en tu GestorOfertas que filtre por Estado = "Pendiente")
                ofertasPendientes = gestorOfertas.ObtenerOfertasPendientes();

                CargarGrilla();

                MessageBox.Show(
                    GestorIdioma.GetInstance.TraducirMensaje("msg_MotorCompletado", "El motor analítico ha terminado de procesar los lotes y el historial de ventas."),
                    GestorIdioma.GetInstance.TraducirMensaje("msg_TituloCompletado", "Análisis Completado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void CargarGrilla()
        {
            dgvOfertas.DataSource = null;
            var vistaGrilla = ofertasPendientes.Select(o => new
            {
                ID = o.IdOferta,
                Propuesta = o.TituloOferta,
                Descuento = o.DescuentoSugerido + "%",
                Estado = o.Estado
            }).ToList();

            dgvOfertas.DataSource = vistaGrilla;

            if (dgvOfertas.Columns.Count > 0)
            {
                dgvOfertas.Columns["ID"].Width = 40;
                dgvOfertas.Columns["Propuesta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                TraducirColumnasGrilla();
            }

            // --- NUEVO: Limpieza visual si la grilla queda vacía ---
            if (ofertasPendientes.Count == 0)
            {
                lblPropuestaDetalle.Text = GestorIdioma.GetInstance.TraducirMensaje("lbl_SeleccionePropuesta", "Seleccione una propuesta de la grilla...");
                panelEdicion.Enabled = false;
                numDescuento.Value = 0;
            }
        }

        private void TraducirColumnasGrilla()
        {
            if (dgvOfertas.Columns.Contains("ID"))
                dgvOfertas.Columns["ID"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_IdOferta", "ID");

            if (dgvOfertas.Columns.Contains("Propuesta"))
                dgvOfertas.Columns["Propuesta"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_PropuestaComercial", "Propuesta Comercial");

            if (dgvOfertas.Columns.Contains("Descuento"))
                dgvOfertas.Columns["Descuento"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_DescuentoSugerido", "Descuento");

            if (dgvOfertas.Columns.Contains("Estado"))
                dgvOfertas.Columns["Estado"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("grid_Estado", "Estado");
        }

        private void DgvOfertas_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvOfertas.CurrentRow != null)
            {
                int idOferta = Convert.ToInt32(dgvOfertas.CurrentRow.Cells["ID"].Value);
                ofertaSeleccionada = ofertasPendientes.FirstOrDefault(o => o.IdOferta == idOferta);

                if (ofertaSeleccionada != null)
                {
                    lblPropuestaDetalle.Text = ofertaSeleccionada.TituloOferta;
                    numDescuento.Value = ofertaSeleccionada.DescuentoSugerido;
                    panelEdicion.Enabled = true;
                }
            }
            else
            {
                panelEdicion.Enabled = false;
                lblPropuestaDetalle.Text = GestorIdioma.GetInstance.TraducirMensaje("lbl_SeleccionePropuesta", "Seleccione una propuesta de la grilla...");
            }
        }

        private void BtnAprobar_Click(object? sender, EventArgs e)
        {
            if (ofertaSeleccionada != null)
            {
                ofertaSeleccionada.DescuentoSugerido = numDescuento.Value;

                gestorOfertas.AprobarOferta(ofertaSeleccionada);

                string msgBase = GestorIdioma.GetInstance.TraducirMensaje("msg_OfertaAprobada", "Oferta Aprobada con {0}% de descuento. Ya está disponible para los vendedores.");
                MessageBox.Show(string.Format(msgBase, numDescuento.Value), GestorIdioma.GetInstance.TraducirMensaje("msg_TituloOfertaActiva", "Oferta Activa"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                ofertasPendientes.Remove(ofertaSeleccionada);
                CargarGrilla();
            }
        }

        private void BtnDescartar_Click(object? sender, EventArgs e)
        {
            if (ofertaSeleccionada != null)
            {
                gestorOfertas.RechazarOferta(ofertaSeleccionada);
                MessageBox.Show(
                    GestorIdioma.GetInstance.TraducirMensaje("msg_OfertaDescartada", "La sugerencia del motor ha sido descartada."),
                    GestorIdioma.GetInstance.TraducirMensaje("msg_TituloOfertaDescartada", "Oferta Rechazada"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                ofertasPendientes.Remove(ofertaSeleccionada);
                CargarGrilla();
            }
        }

        
        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            this.Text = GestorIdioma.GetInstance.TraducirMensaje("GestionOfertasInteligentesUI", "Gestor de Promociones Inteligentes");
            lblTituloUI.Text = GestorIdioma.GetInstance.TraducirMensaje("lblTituloUI_Ofertas", "Bandeja de Recomendaciones (FEFO)");

            btnEjecutarMotor.Text = "🧠 " + GestorIdioma.GetInstance.TraducirMensaje("btnEjecutarMotor", "Ejecutar Motor Analítico");
            lblTituloAjuste.Text = GestorIdioma.GetInstance.TraducirMensaje("lblTituloAjuste", "Ajustar Decisión de la Propuesta:");

            btnAprobar.Text = "✅ " + GestorIdioma.GetInstance.TraducirMensaje("btnAprobarOferta", "Aprobar Oferta");
            btnDescartar.Text = "❌ " + GestorIdioma.GetInstance.TraducirMensaje("btnDescartarOferta", "Descartar");


            if (dgvOfertas.CurrentRow == null)
            {
                lblPropuestaDetalle.Text = GestorIdioma.GetInstance.TraducirMensaje("lbl_SeleccionePropuesta", "Seleccione una propuesta de la grilla...");
            }

            if (dgvOfertas.DataSource != null)
            {
                TraducirColumnasGrilla();
            }
        }

        private void btnGestionarOfertas_Click(object sender, EventArgs e)
        {
            GestorOfertas gestorOfertas = new GestorOfertas();
            DataTable historial = gestorOfertas.ConsultarHistorialCompleto();


            Form frmHistorial = new Form
            {
                Text = GestorIdioma.GetInstance.TraducirMensaje("frmAuditoria_Titulo", "Gestión y Auditoría de Ofertas"),
                Size = new Size(700, 350),
                StartPosition = FormStartPosition.CenterParent,
                ShowIcon = false,
                MinimizeBox = false,
                MaximizeBox = false
            };

            DataGridView dgvHistorial = new DataGridView
            {
                DataSource = historial,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.WhiteSmoke
            };

            Panel pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 50 };
            Button btnEliminar = new Button
            {
                Text = GestorIdioma.GetInstance.TraducirMensaje("btnEliminarOfertaAudit", "Eliminar Oferta Seleccionada"),
                Location = new Point(10, 10),
                Width = 200,
                BackColor = Color.LightCoral,
                Cursor = Cursors.Hand
            };


            btnEliminar.Click += (s, ev) =>
            {
                if (dgvHistorial.CurrentRow != null)
                {
                    int idOferta = Convert.ToInt32(dgvHistorial.CurrentRow.Cells["IdOferta"].Value);

                    // ¡ESTA ES LA LÍNEA QUE FALTABA! Extraemos el nombre del producto de la grilla:
                    string producto = dgvHistorial.CurrentRow.Cells["Producto"].Value.ToString() ?? "";

                    string msgConfirmar = GestorIdioma.GetInstance.TraducirMensaje("msg_ConfirmarEliminarOferta", $"¿Está seguro que desea eliminar definitivamente la oferta #{idOferta} para el producto {producto}?");
                    string tituloConfirmar = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloConfirmar", "Confirmar Eliminación");

                    DialogResult res = MessageBox.Show(msgConfirmar, tituloConfirmar, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (res == DialogResult.Yes)
                    {
                        gestorOfertas.EliminarOfertaDefinitivamente(idOferta);

                        string msgExito = GestorIdioma.GetInstance.TraducirMensaje("msg_OfertaEliminadaExito", "Oferta eliminada de la base de datos.");
                        MessageBox.Show(msgExito, GestorIdioma.GetInstance.TraducirMensaje("msg_Exito", "Éxito"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dgvHistorial.DataSource = gestorOfertas.ConsultarHistorialCompleto();
                    }
                }
            };

            pnlBottom.Controls.Add(btnEliminar);
            frmHistorial.Controls.Add(dgvHistorial);
            frmHistorial.Controls.Add(pnlBottom);

            frmHistorial.ShowDialog();
        }
    }
}