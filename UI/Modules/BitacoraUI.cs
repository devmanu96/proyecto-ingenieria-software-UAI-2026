using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace UI.Modules
{
    public partial class BitacoraUI : FormBaseObserver
    {
        // Bandera para evitar que los combos filtren al momento de cargar
        private bool inicializando = true;

        public BitacoraUI()
        {
            InitializeComponent();
            this.Load += BitacoraUI_Load;

            List<Registro> registros = GestorBitacora.GetInstance.ConsultarBitacoraCompleta();
            List<string> usernames = registros.Select(r => r.Username).Distinct().ToList();
            List<string> acciones = registros.Select(r => r.Accion).Distinct().ToList();

            dataGridViewRegistrosBitacora.DataSource = registros;

            comboBoxAccion.DataSource = acciones;
            comboBoxAccion.SelectedIndex = -1;

            comboBoxUsername.DataSource = usernames;
            comboBoxUsername.SelectedIndex = -1;

            // Terminamos de cargar, liberamos la bandera
            inicializando = false;
        }

        private void BitacoraUI_Load(object? sender, EventArgs e)
        {
            string username = SessionManager.getInstance.ObtenerUsuarioActivo()?.Username ?? "Sistema";
            GestorBitacora.GetInstance.Update(username, "LOG_CONSULTA_BITACORA");
        }

        private void bitacoraUIButtonLimpiarFiltros_Click(object sender, EventArgs e)
        {
            // Bloqueamos temporalmente para que los SelectedIndex = -1 no disparen la búsqueda
            inicializando = true;

            dataGridViewRegistrosBitacora.DataSource = GestorBitacora.GetInstance.ConsultarBitacoraCompleta();
            comboBoxAccion.SelectedIndex = -1;
            comboBoxUsername.SelectedIndex = -1;

            inicializando = false;

            string username = SessionManager.getInstance.ObtenerUsuarioActivo()?.Username ?? "Sistema";
            GestorBitacora.GetInstance.Update(username, "LOG_LIMPIEZA_FILTROS_BITACORA");
        }

        private void comboBoxAccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (inicializando) return; // Si se está construyendo el form, ignoramos el evento

            int selectedIndex = comboBoxAccion.SelectedIndex;
            if (selectedIndex >= 0)
            {
                // Limpiamos el otro combo para no confundir al usuario visualmente
                inicializando = true;
                comboBoxUsername.SelectedIndex = -1;
                inicializando = false;

                string selectedAccion = comboBoxAccion.SelectedItem.ToString() ?? "";
                List<Registro> registrosFiltrados = GestorBitacora.GetInstance.ConsultarBitacoraFiltradaPorAccion(selectedAccion);
                dataGridViewRegistrosBitacora.DataSource = registrosFiltrados;
            }
        }

        private void comboBoxUsername_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (inicializando) return; // Si se está construyendo el form, ignoramos el evento

            int selectedIndex = comboBoxUsername.SelectedIndex;
            if (selectedIndex >= 0)
            {
                // Limpiamos el otro combo para no confundir al usuario visualmente
                inicializando = true;
                comboBoxAccion.SelectedIndex = -1;
                inicializando = false;

                string selectedUsername = comboBoxUsername.SelectedItem.ToString() ?? "";
                List<Registro> registrosFiltrados = GestorBitacora.GetInstance.ConsultarBitacoraFiltradaPorUsername(selectedUsername);
                dataGridViewRegistrosBitacora.DataSource = registrosFiltrados;
            }
        }

        private void bitacoraUILabelComboBoxAccion_Click(object sender, EventArgs e)
        {
        }

        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            this.Text = GestorIdioma.GetInstance.TraducirMensaje("BitacoraUI", "Bitácora del Sistema");

            if (dataGridViewRegistrosBitacora.Columns.Contains("Username"))
                dataGridViewRegistrosBitacora.Columns["Username"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("GridBitacora_Usuario", "Usuario");
            if (dataGridViewRegistrosBitacora.Columns.Contains("Fecha"))
                dataGridViewRegistrosBitacora.Columns["Fecha"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("GridBitacora_Fecha", "Fecha");
            if (dataGridViewRegistrosBitacora.Columns.Contains("Accion"))
                dataGridViewRegistrosBitacora.Columns["Accion"].HeaderText = GestorIdioma.GetInstance.TraducirMensaje("GridBitacora_Accion", "Acción");
        }
    }
}