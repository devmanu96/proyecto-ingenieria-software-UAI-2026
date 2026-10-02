using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UI.Venta
{
    public partial class VisorTicketsUI : FormBaseObserver // Cambia a FormBaseObserver si necesitas traducirlo
    {
        public VisorTicketsUI()
        {
            InitializeComponent();
            this.Load += VisorTicketsUI_Load;
            dgvTickets.CellDoubleClick += dgvTickets_CellDoubleClick;
        }

        private void VisorTicketsUI_Load(object? sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            CargarTickets();
        }

        private void CargarTickets()
        {
            string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tickets");

            // Si la carpeta no existe, la creamos para evitar errores
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            DataTable dt = new DataTable();
            dt.Columns.Add("Nombre del Archivo");
            dt.Columns.Add("Fecha de Emisión");
            dt.Columns.Add("RutaCompleta"); // Columna oculta

            // Leemos todos los PDF ordenados del más reciente al más antiguo
            var archivos = new DirectoryInfo(carpeta).GetFiles("*.pdf")
                                                     .OrderByDescending(f => f.CreationTime);

            foreach (var archivo in archivos)
            {
                dt.Rows.Add(archivo.Name, archivo.CreationTime.ToString("dd/MM/yyyy HH:mm:ss"), archivo.FullName);
            }

            dgvTickets.DataSource = null;
            dgvTickets.DataSource = dt;

            // Ajustes visuales
            dgvTickets.Columns["RutaCompleta"].Visible = false;
            dgvTickets.Columns["Nombre del Archivo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTickets.ClearSelection();
        }

        private void dgvTickets_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string rutaPdf = dgvTickets.Rows[e.RowIndex].Cells["RutaCompleta"].Value?.ToString();

                if (!string.IsNullOrEmpty(rutaPdf) && File.Exists(rutaPdf))
                {
                    // Abre el PDF al hacer doble clic
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = rutaPdf,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("El archivo físico ya no existe o fue movido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}