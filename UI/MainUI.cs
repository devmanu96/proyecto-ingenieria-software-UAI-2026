using BE;
using BLL;
using servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UI.Login;
using UI.Modules;

namespace UI
{
    public partial class MainUI : Form, IObserver
    {
        private Form? formCargadoActualmente;

        public MainUI()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            GestorIdioma.GetInstance.Attach(this);

            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboIdiomasGlobal.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.IsMdiContainer = true;
            GestorIdioma.GetInstance.Attach(this);

            // Ocultamos todos los menús superiores al arrancar, excepto Inicio
            foreach (ToolStripItem item in menuStrip1.Items)
            {
                if (item.Name != "mainUIStripMenuItemInicio")
                {
                    item.Visible = false;
                }
            }
        }

        private void cargarFormulario(Form formulario)
        {
            if (formCargadoActualmente != null)
            {
                formCargadoActualmente.Dispose();
                formCargadoActualmente = null;
            }
            formCargadoActualmente = formulario;

            if (formulario is LoginUI loginUI)
            {
                loginUI.SesionIniciada += LoginUI_SesionIniciada;
            }

            formCargadoActualmente.MdiParent = this;
            formCargadoActualmente.Show();
        }

        private void LoginUI_SesionIniciada(object? sender, EventArgs e)
        {
            try
            {
                // Limpieza de menús por si hubo un cambio de sesión
                foreach (ToolStripItem item in menuStrip1.Items)
                {
                    if (item.Name != "mainUIStripMenuItemInicio")
                        item.Visible = false;
                }

                mainUIStripMenuItemInicio.Visible = true;
                mainUIStripMenuItemCerrarSesion.Visible = true;
                mainUIStripMenuItemCerrarSesion.Enabled = true;
                mainUIStripMenuItemIniciarSesion.Visible = false;
                mainUIStripMenuItemIniciarSesion.Enabled = false;

                Usuario? usuarioActual = SessionManager.getInstance.ObtenerUsuarioActivo();

                if (usuarioActual != null)
                {
                    // Módulo de Administración (Solo visibles si tienen el permiso)
                    mainUIStripMenuItemGestionDeUsuarios.Visible = usuarioActual.Permisos.Any(p => p.ValidarPermiso("PERM-GESTIONAR-USR"));
                    recuperarIntegridadToolStripMenuItem.Visible = usuarioActual.Permisos.Any(p => p.ValidarPermiso("PERM-GESTIONAR-USR"));
                    mainUIStripMenuItemGestionDePerfiles.Visible = usuarioActual.Permisos.Any(p => p.ValidarPermiso("PERM-GESTIONAR-PERFIL"));
                    mainUIStripMenuItemBitacora.Visible = usuarioActual.Permisos.Any(p => p.ValidarPermiso("PERM-CONSULTA-BIT"));
                    mainUIStripMenuItemHistorialUsuario.Visible = usuarioActual.Permisos.Any(p => p.ValidarPermiso("PERM-GESTIONAR-HISTORIAL"));
                    agregarIdiomaToolStripMenuItem.Visible = usuarioActual.Permisos.Any(p => p.ValidarPermiso("PERM-AGREGAR-IDM"));
                    // Módulo de Almacén, Compras y Contabilidad (Solo visibles si tienen el permiso)
                    menuStripItemAlmacen.Visible = usuarioActual.Permisos.Any(p => p.ValidarPermiso("PERM-GEN-SOLICITUD") || p.ValidarPermiso("PERM-ABM-PROD"));
                    menuStripItemCompras.Visible = usuarioActual.Permisos.Any(p => p.ValidarPermiso("PERM-EMITIR-OC") || p.ValidarPermiso("PERM-SELECCIONAR-PROD"));
                    menuStripItemContabilidad.Visible = usuarioActual.Permisos.Any(p => p.ValidarPermiso("PERM-EVALUAR-COTIZ"));
                }
                else
                {
                    throw new Exception("Login error");
                }

                formCargadoActualmente?.Close();
            }
            catch (Exception ex)
            {
                string tituloError = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error");
                MessageBox.Show(ex.Message, tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<Usuario> corruptos = GestorIntegridad.VerificarIntegridadDVH();
                bool dvvValido = GestorIntegridad.VerificarIntegridadDVV();

                if (corruptos.Count > 0 || !dvvValido)
                {
                    string mensaje = GestorIdioma.GetInstance.TraducirMensaje("err_IntegridadCorrupta",
                        "Alerta Crítica: Se ha detectado una violación en la integridad de la base de datos.\n\nEl sistema ha entrado en Modo de Recuperación. Solo los administradores pueden iniciar sesión.");
                    string titulo = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloError", "Error Crítico de Integridad");

                    MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar la integridad del sistema: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoginUI loginUI = new LoginUI();
            cargarFormulario(loginUI);

            mainUIStripMenuItemCerrarSesion.Visible = false;

            var listaIdiomas = GestorIdioma.GetInstance.ObtenerIdiomasDisponibles();

            comboIdiomasGlobal.SelectedIndexChanged -= ComboIdiomasGlobal_SelectedIndexChanged;

            comboIdiomasGlobal.DataSource = listaIdiomas;
            comboIdiomasGlobal.DisplayMember = "Nombre";
            comboIdiomasGlobal.ValueMember = "Codigo";

            Usuario? usuarioActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
            if (usuarioActivo != null && !string.IsNullOrEmpty(usuarioActivo.Idioma))
            {
                comboIdiomasGlobal.SelectedValue = usuarioActivo.Idioma;
            }
            else
            {
                comboIdiomasGlobal.SelectedValue = "ES";
            }
            comboIdiomasGlobal.SelectedIndexChanged += ComboIdiomasGlobal_SelectedIndexChanged;
        }

        private void mainUIStripMenuItemIniciarSesion_Click(object sender, EventArgs e)
        {
            LoginUI loginUI = new LoginUI();
            cargarFormulario(loginUI);
        }

        private void mainUIStripMenuItemCerrarSesion_Click(object sender, EventArgs e)
        {
            GestorLogin gestorLogin = new GestorLogin();
            gestorLogin.LogOut();

            string mensaje = GestorIdioma.GetInstance.TraducirMensaje("msg_CierreSesionExito", "Sesión cerrada correctamente.");
            string titulo = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloCierreSesion", "Cerrar sesión");

            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);

            foreach (ToolStripItem item in menuStrip1.Items)
            {
                if (item.Name != "mainUIStripMenuItemInicio")
                    item.Visible = false;
            }

            mainUIStripMenuItemCerrarSesion.Visible = false;
            mainUIStripMenuItemCerrarSesion.Enabled = false;
            mainUIStripMenuItemIniciarSesion.Visible = true;
            mainUIStripMenuItemIniciarSesion.Enabled = true;

            LoginUI loginUI = new LoginUI();
            cargarFormulario(loginUI);
        }

        private void mainUIStripMenuItemABMUsuarios_Click(object sender, EventArgs e)
        {
            GestionUsuariosUI gestorUsuariosUI = new GestionUsuariosUI();
            cargarFormulario(gestorUsuariosUI);
        }

        private void mainUIStripMenuItemDesbloqueoUsuarios_Click(object sender, EventArgs e)
        {
            BloqueoUsuariosUI bloqueoUsuariosUI = new BloqueoUsuariosUI();
            cargarFormulario(bloqueoUsuariosUI);
        }

        private void mainUIStripMenuItemABMPerfiles_Click(object sender, EventArgs e)
        {
            GestionPerfilesUI gestionPerfilesUI = new GestionPerfilesUI();
            cargarFormulario(gestionPerfilesUI);
        }

        private void mainUIStripMenuItemConsultarBitacora_Click(object sender, EventArgs e)
        {
            BitacoraUI bitacoraUI = new BitacoraUI();
            cargarFormulario(bitacoraUI);
        }

        private void ComboIdiomasGlobal_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboIdiomasGlobal.SelectedItem == null) return;

            BE.Idioma idiomaSeleccionado = (BE.Idioma)comboIdiomasGlobal.SelectedItem;
            GestorIdioma.GetInstance.CambiarIdioma(idiomaSeleccionado.Codigo);
        }

        public void Update(string username, string action)
        {
            if (action.StartsWith("Idioma:"))
            {
                string codigoIdioma = action.Split(':')[1];
                Dictionary<string, string> traducciones = GestorIdioma.GetInstance.ObtenerTraduccionesActuales(codigoIdioma);
                TranslateServices.TraducirObjeto(this, traducciones);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            GestorIdioma.GetInstance.Detach(this);
            base.OnFormClosed(e);
        }

        private void mainUIStripMenuItemHistorialUsuario_Click(object sender, EventArgs e)
        {
            GestionHistorialUsuarioUI gestionHistorialUsuarioUI = new GestionHistorialUsuarioUI();
            cargarFormulario(gestionHistorialUsuarioUI);
        }

        private void agregarIdiomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionIdiomasUI gestorIdiomasUI = new GestionIdiomasUI();
            cargarFormulario(gestorIdiomasUI);
        }

        private void recuperarIntegridadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestorIntegridad.RecuperarIntegridadDVH();
            MessageBox.Show("Integridad de la base de datos recuperada correctamente.", "Recuperación de Integridad", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void almacenToolStripMenuItemGenerarSolicitud_Click(object sender, EventArgs e)
        {
            UI.Almacen.GenerarSolicitudUI formSolicitud = new UI.Almacen.GenerarSolicitudUI();

            // Usamos el método que ya tenés en MainUI para cargarlo como hijo MDI
            cargarFormulario(formSolicitud);
        }

        private void comprasToolStripMenuItemEmitirOC_Click(object sender, EventArgs e)
        {
            UI.Compras.BandejaSolicitudesUI formBandeja = new UI.Compras.BandejaSolicitudesUI();
            cargarFormulario(formBandeja);
        }
    }
}