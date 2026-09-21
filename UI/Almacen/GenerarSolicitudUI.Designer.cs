namespace UI.Almacen
{
    partial class GenerarSolicitudUI
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
            dgvInventario = new DataGridView();
            dgvSolicitud = new DataGridView();
            Agregar = new Button();
            btn_Solicitud_Abastecimiento = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvInventario).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvSolicitud).BeginInit();
            SuspendLayout();
            // 
            // dgvInventario
            // 
            dgvInventario.AllowUserToAddRows = false;
            dgvInventario.AllowUserToDeleteRows = false;
            dgvInventario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInventario.Location = new Point(12, 59);
            dgvInventario.Name = "dgvInventario";
            dgvInventario.ReadOnly = true;
            dgvInventario.Size = new Size(409, 248);
            dgvInventario.TabIndex = 0;
            dgvInventario.DataBindingComplete += dgvInventario_DataBindingComplete;
            // 
            // dgvSolicitud
            // 
            dgvSolicitud.AllowUserToAddRows = false;
            dgvSolicitud.AllowUserToDeleteRows = false;
            dgvSolicitud.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSolicitud.Location = new Point(508, 59);
            dgvSolicitud.Name = "dgvSolicitud";
            dgvSolicitud.ReadOnly = true;
            dgvSolicitud.Size = new Size(417, 245);
            dgvSolicitud.TabIndex = 1;
            // 
            // Agregar
            // 
            Agregar.Location = new Point(931, 59);
            Agregar.Name = "Agregar";
            Agregar.Size = new Size(91, 29);
            Agregar.TabIndex = 2;
            Agregar.Text = "Agregar";
            Agregar.UseVisualStyleBackColor = true;
            Agregar.Click += Agregar_Click;
            // 
            // btn_Solicitud_Abastecimiento
            // 
            btn_Solicitud_Abastecimiento.Location = new Point(636, 310);
            btn_Solicitud_Abastecimiento.Name = "btn_Solicitud_Abastecimiento";
            btn_Solicitud_Abastecimiento.Size = new Size(129, 34);
            btn_Solicitud_Abastecimiento.TabIndex = 3;
            btn_Solicitud_Abastecimiento.Text = "Confirmar Solicitud";
            btn_Solicitud_Abastecimiento.UseVisualStyleBackColor = true;
            btn_Solicitud_Abastecimiento.Click += btn_Solicitud_Abastecimiento_Click;
            // 
            // GenerarSolicitudUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1065, 450);
            Controls.Add(btn_Solicitud_Abastecimiento);
            Controls.Add(Agregar);
            Controls.Add(dgvSolicitud);
            Controls.Add(dgvInventario);
            FormBorderStyle = FormBorderStyle.None;
            Name = "GenerarSolicitudUI";
            Text = "GenerarSolicitudUI";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)dgvInventario).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvSolicitud).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvInventario;
        private DataGridView dgvSolicitud;
        private Button Agregar;
        private Button btn_Solicitud_Abastecimiento;
    }
}