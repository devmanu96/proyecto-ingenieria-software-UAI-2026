namespace UI.Modules
{
    partial class GestionIdiomasUI
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBoxCodigo;
        private System.Windows.Forms.TextBox textBoxNombre;
        private System.Windows.Forms.Label labelCodigo;
        private System.Windows.Forms.Label labelNombre;
        private System.Windows.Forms.DataGridView dataGridViewTraducciones;
        private System.Windows.Forms.Button btnGuardarIdioma;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            textBoxCodigo = new TextBox();
            textBoxNombre = new TextBox();
            labelCodigo = new Label();
            labelNombre = new Label();
            dataGridViewTraducciones = new DataGridView();
            btnGuardarIdioma = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTraducciones).BeginInit();
            SuspendLayout();
            // 
            // textBoxCodigo
            // 
            textBoxCodigo.Location = new Point(165, 71);
            textBoxCodigo.MaxLength = 5;
            textBoxCodigo.Name = "textBoxCodigo";
            textBoxCodigo.Size = new Size(80, 23);
            textBoxCodigo.TabIndex = 5;
            // 
            // textBoxNombre
            // 
            textBoxNombre.Location = new Point(375, 71);
            textBoxNombre.Name = "textBoxNombre";
            textBoxNombre.Size = new Size(180, 23);
            textBoxNombre.TabIndex = 4;
            // 
            // labelCodigo
            // 
            labelCodigo.Location = new Point(55, 74);
            labelCodigo.Name = "labelCodigo";
            labelCodigo.Size = new Size(100, 23);
            labelCodigo.TabIndex = 3;
            labelCodigo.Text = "Código (Ej: FR):";
            // 
            // labelNombre
            // 
            labelNombre.Location = new Point(275, 74);
            labelNombre.Name = "labelNombre";
            labelNombre.Size = new Size(100, 23);
            labelNombre.TabIndex = 2;
            labelNombre.Text = "Nombre Idioma:";
            // 
            // dataGridViewTraducciones
            // 
            dataGridViewTraducciones.AllowUserToDeleteRows = false;
            dataGridViewTraducciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewTraducciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTraducciones.Location = new Point(55, 116);
            dataGridViewTraducciones.Name = "dataGridViewTraducciones";
            dataGridViewTraducciones.Size = new Size(880, 400);
            dataGridViewTraducciones.TabIndex = 1;
            // 
            // btnGuardarIdioma
            // 
            btnGuardarIdioma.Location = new Point(603, 64);
            btnGuardarIdioma.Name = "btnGuardarIdioma";
            btnGuardarIdioma.Size = new Size(118, 35);
            btnGuardarIdioma.TabIndex = 0;
            btnGuardarIdioma.Text = "Guardar Idioma";
            btnGuardarIdioma.UseVisualStyleBackColor = true;
            btnGuardarIdioma.Click += btnGuardarIdioma_Click;
            // 
            // GestionIdiomasUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(966, 540);
            ControlBox = false;
            Controls.Add(btnGuardarIdioma);
            Controls.Add(dataGridViewTraducciones);
            Controls.Add(labelNombre);
            Controls.Add(labelCodigo);
            Controls.Add(textBoxNombre);
            Controls.Add(textBoxCodigo);
            FormBorderStyle = FormBorderStyle.None;
            Name = "GestionIdiomasUI";
            Text = "Configuración de Nuevos Idiomas";
            WindowState = FormWindowState.Maximized;
            Load += GestionIdiomasUI_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewTraducciones).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}