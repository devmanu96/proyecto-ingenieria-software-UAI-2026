namespace UI.Compras
{
    partial class BandejaSolicitudesUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvSolicitudesPendientes = new DataGridView();
            dgvDetalleSolicitud = new DataGridView();
            btnAtenderSolicitud = new Button();
            btnVolver = new Button();
            lblDetalleProductos = new Label();
            lblSolicitudesPendientes = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvSolicitudesPendientes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalleSolicitud).BeginInit();
            SuspendLayout();
            // 
            // dgvSolicitudesPendientes
            // 
            dgvSolicitudesPendientes.AllowUserToAddRows = false;
            dgvSolicitudesPendientes.AllowUserToDeleteRows = false;
            dgvSolicitudesPendientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSolicitudesPendientes.Location = new Point(27, 61);
            dgvSolicitudesPendientes.MultiSelect = false;
            dgvSolicitudesPendientes.Name = "dgvSolicitudesPendientes";
            dgvSolicitudesPendientes.ReadOnly = true;
            dgvSolicitudesPendientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSolicitudesPendientes.Size = new Size(396, 211);
            dgvSolicitudesPendientes.TabIndex = 0;
            // 
            // dgvDetalleSolicitud
            // 
            dgvDetalleSolicitud.AllowUserToAddRows = false;
            dgvDetalleSolicitud.AllowUserToDeleteRows = false;
            dgvDetalleSolicitud.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalleSolicitud.Location = new Point(27, 310);
            dgvDetalleSolicitud.MultiSelect = false;
            dgvDetalleSolicitud.Name = "dgvDetalleSolicitud";
            dgvDetalleSolicitud.ReadOnly = true;
            dgvDetalleSolicitud.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetalleSolicitud.Size = new Size(396, 211);
            dgvDetalleSolicitud.TabIndex = 1;
            // 
            // btnAtenderSolicitud
            // 
            btnAtenderSolicitud.Enabled = false;
            btnAtenderSolicitud.Location = new Point(443, 86);
            btnAtenderSolicitud.Name = "btnAtenderSolicitud";
            btnAtenderSolicitud.Size = new Size(125, 39);
            btnAtenderSolicitud.TabIndex = 2;
            btnAtenderSolicitud.Text = "Atender Solicitud y Emitir Orden";
            btnAtenderSolicitud.UseVisualStyleBackColor = true;
            // 
            // btnVolver
            // 
            btnVolver.Location = new Point(27, 527);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(93, 39);
            btnVolver.TabIndex = 3;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = true;
            // 
            // lblDetalleProductos
            // 
            lblDetalleProductos.AutoSize = true;
            lblDetalleProductos.Location = new Point(27, 274);
            lblDetalleProductos.Name = "lblDetalleProductos";
            lblDetalleProductos.Size = new Size(200, 15);
            lblDetalleProductos.TabIndex = 4;
            lblDetalleProductos.Text = "Productos Requeridos en la Solicitud";
            // 
            // lblSolicitudesPendientes
            // 
            lblSolicitudesPendientes.AutoSize = true;
            lblSolicitudesPendientes.Location = new Point(27, 43);
            lblSolicitudesPendientes.Name = "lblSolicitudesPendientes";
            lblSolicitudesPendientes.Size = new Size(191, 15);
            lblSolicitudesPendientes.TabIndex = 5;
            lblSolicitudesPendientes.Text = "Solicitudes Pendientes de Revisión:";
            // 
            // BandejaSolicitudesUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(582, 649);
            Controls.Add(lblSolicitudesPendientes);
            Controls.Add(lblDetalleProductos);
            Controls.Add(btnVolver);
            Controls.Add(btnAtenderSolicitud);
            Controls.Add(dgvDetalleSolicitud);
            Controls.Add(dgvSolicitudesPendientes);
            FormBorderStyle = FormBorderStyle.None;
            Name = "BandejaSolicitudesUI";
            Text = "BandejaSolicitudesUI";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)dgvSolicitudesPendientes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetalleSolicitud).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvSolicitudesPendientes;
        private DataGridView dgvDetalleSolicitud;
        private Button btnAtenderSolicitud;
        private Button btnVolver;
        private Label lblDetalleProductos;
        private Label lblSolicitudesPendientes;
    }
}