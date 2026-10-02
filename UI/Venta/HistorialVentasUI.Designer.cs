namespace UI.Venta
{
    partial class HistorialVentasUI
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dgvHistorial = new DataGridView();
            btnActualizar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).BeginInit();
            SuspendLayout();
            // 
            // btnActualizar
            // 
            btnActualizar.BackColor = Color.LightSteelBlue;
            btnActualizar.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            btnActualizar.Location = new Point(24, 15);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(170, 35);
            btnActualizar.TabIndex = 1;
            btnActualizar.Text = "Actualizar Historial";
            btnActualizar.UseVisualStyleBackColor = false;
            // 
            // dgvHistorial
            // 
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.AllowUserToDeleteRows = false;
            // El Anchor mantiene la grilla pegada a los bordes al estirar la ventana, pero respetando la posición inicial
            dgvHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistorial.Location = new Point(24, 65); // Forzamos a que empiece más abajo, dejando espacio para el botón
            dgvHistorial.MultiSelect = false;
            dgvHistorial.Name = "dgvHistorial";
            dgvHistorial.ReadOnly = true;
            dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorial.Size = new Size(1150, 580);
            dgvHistorial.TabIndex = 0;
            // 
            // HistorialVentasUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 680);
            Controls.Add(btnActualizar);
            Controls.Add(dgvHistorial);
            FormBorderStyle = FormBorderStyle.None;
            Name = "HistorialVentasUI";
            Text = "Historial de Ventas";
            ((System.ComponentModel.ISupportInitialize)dgvHistorial).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvHistorial;
        private Button btnActualizar;
    }
}