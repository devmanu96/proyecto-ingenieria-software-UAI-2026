using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UI.Modules
{
    public partial class GestionIdiomasUI : FormBaseObserver
    {
        private Dictionary<string, string> _traduccionesBaseES;

        public GestionIdiomasUI()
        {
            InitializeComponent();
            _traduccionesBaseES = new Dictionary<string, string>();
        }

        private void GestionIdiomasUI_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarTraduccionesDeReferencia();
        }

        private void ConfigurarGrid()
        {
            dataGridViewTraducciones.Columns.Clear();
            dataGridViewTraducciones.AllowUserToAddRows = false;
            dataGridViewTraducciones.AllowUserToDeleteRows = false;

            DataGridViewTextBoxColumn colKey = new DataGridViewTextBoxColumn();
            colKey.Name = "KeyEtiqueta";
            colKey.HeaderText = "Componente / Etiqueta";
            colKey.ReadOnly = true;
            colKey.Width = 250;
            dataGridViewTraducciones.Columns.Add(colKey);

            DataGridViewTextBoxColumn colRef = new DataGridViewTextBoxColumn();
            colRef.Name = "TextoES";
            colRef.HeaderText = "Referencia (Español)";
            colRef.ReadOnly = true;
            colRef.Width = 300;
            dataGridViewTraducciones.Columns.Add(colRef);

            DataGridViewTextBoxColumn colNuevoTexto = new DataGridViewTextBoxColumn();
            colNuevoTexto.Name = "NuevoTexto";
            colNuevoTexto.HeaderText = "Nueva Traducción";
            colNuevoTexto.ReadOnly = false;
            colNuevoTexto.Width = 300;
            dataGridViewTraducciones.Columns.Add(colNuevoTexto);
        }

        private void CargarTraduccionesDeReferencia()
        {
            try
            {
                _traduccionesBaseES = GestorIdioma.GetInstance.ObtenerTraduccionesActuales("ES");

                dataGridViewTraducciones.Rows.Clear();
                foreach (KeyValuePair<string, string> kvp in _traduccionesBaseES)
                {
                    int rowIndex = dataGridViewTraducciones.Rows.Add();
                    dataGridViewTraducciones.Rows[rowIndex].Cells["KeyEtiqueta"].Value = kvp.Key;
                    dataGridViewTraducciones.Rows[rowIndex].Cells["TextoES"].Value = kvp.Value;
                    dataGridViewTraducciones.Rows[rowIndex].Cells["NuevoTexto"].Value = ""; // Inicialmente vacío para traducir
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar etiquetas de referencia: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardarIdioma_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = textBoxCodigo.Text.Trim();
                string nombre = textBoxNombre.Text.Trim();

                Dictionary<string, string> nuevasTraducciones = new Dictionary<string, string>();

                foreach (DataGridViewRow row in dataGridViewTraducciones.Rows)
                {
                    string key = row.Cells["KeyEtiqueta"].Value?.ToString() ?? string.Empty;
                    string nuevoTexto = row.Cells["NuevoTexto"].Value?.ToString() ?? string.Empty;

                    if (!string.IsNullOrEmpty(key))
                    {
                        // NUEVA VALIDACIÓN: Verifica si la celda de la nueva traducción está en blanco
                        if (string.IsNullOrWhiteSpace(nuevoTexto))
                        {
                            string msgError = GestorIdioma.GetInstance.TraducirMensaje(
                                "err_TraduccionesIncompletas",
                                "Debe completar absolutamente todas las traducciones antes de guardar el nuevo idioma."
                            );
                            throw new Exception(msgError); // Esto interrumpe el guardado y envía el mensaje al catch
                        }

                        nuevasTraducciones.Add(key, nuevoTexto);
                    }
                }

                GestorIdioma.GetInstance.RegistrarNuevoIdioma(codigo, nombre, nuevasTraducciones);

                string msgExito = GestorIdioma.GetInstance.TraducirMensaje("msg_IdiomaGuardadoExito", "El idioma y sus respectivas traducciones se han guardado exitosamente.");
                string tituloExito = GestorIdioma.GetInstance.TraducirMensaje("msg_TituloExito", "Éxito");

                MessageBox.Show(msgExito, tituloExito, MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBoxCodigo.Clear();
                textBoxNombre.Clear();
                CargarTraduccionesDeReferencia();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        protected override void TraducirElementosParticulares(string codigoIdioma)
        {
            var traducciones = GestorIdioma.GetInstance.ObtenerTraduccionesActuales(codigoIdioma);

            if (dataGridViewTraducciones.Columns.Count > 0)
            {
                if (traducciones.ContainsKey("GridIdioma_ColKey") && dataGridViewTraducciones.Columns["KeyEtiqueta"] != null)
                    dataGridViewTraducciones.Columns["KeyEtiqueta"].HeaderText = traducciones["GridIdioma_ColKey"];

                if (traducciones.ContainsKey("GridIdioma_ColRef") && dataGridViewTraducciones.Columns["TextoES"] != null)
                    dataGridViewTraducciones.Columns["TextoES"].HeaderText = traducciones["GridIdioma_ColRef"];

                if (traducciones.ContainsKey("GridIdioma_ColNuevo") && dataGridViewTraducciones.Columns["NuevoTexto"] != null)
                    dataGridViewTraducciones.Columns["NuevoTexto"].HeaderText = traducciones["GridIdioma_ColNuevo"];
            }
        }
    }
}
