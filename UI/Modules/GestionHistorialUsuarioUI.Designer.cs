namespace UI.Modules
{
    partial class GestionHistorialUsuarioUI
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
            dataGridViewUsuarios = new DataGridView();
            dataGridViewHistorial = new DataGridView();
            gestionHistorialUIButtonRecuperarEstado = new Button();
            gestionHistorialUILabelGridUsuarios = new Label();
            gestionHistorialUILabelGridEstadoUsuarios = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewHistorial).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewUsuarios
            // 
            dataGridViewUsuarios.AllowUserToAddRows = false;
            dataGridViewUsuarios.AllowUserToDeleteRows = false;
            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsuarios.Location = new Point(73, 90);
            dataGridViewUsuarios.MultiSelect = false;
            dataGridViewUsuarios.Name = "dataGridViewUsuarios";
            dataGridViewUsuarios.ReadOnly = true;
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.Size = new Size(548, 392);
            dataGridViewUsuarios.TabIndex = 0;
            dataGridViewUsuarios.SelectionChanged += dataGridViewUsuarios_SelectionChanged;
            // 
            // dataGridViewHistorial
            // 
            dataGridViewHistorial.AllowUserToAddRows = false;
            dataGridViewHistorial.AllowUserToDeleteRows = false;
            dataGridViewHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewHistorial.Location = new Point(627, 90);
            dataGridViewHistorial.MultiSelect = false;
            dataGridViewHistorial.Name = "dataGridViewHistorial";
            dataGridViewHistorial.ReadOnly = true;
            dataGridViewHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewHistorial.Size = new Size(192, 392);
            dataGridViewHistorial.TabIndex = 1;
            // 
            // gestionHistorialUIButtonRecuperarEstado
            // 
            gestionHistorialUIButtonRecuperarEstado.Location = new Point(825, 91);
            gestionHistorialUIButtonRecuperarEstado.Name = "gestionHistorialUIButtonRecuperarEstado";
            gestionHistorialUIButtonRecuperarEstado.Size = new Size(149, 388);
            gestionHistorialUIButtonRecuperarEstado.TabIndex = 2;
            gestionHistorialUIButtonRecuperarEstado.Text = "Recuperar estado";
            gestionHistorialUIButtonRecuperarEstado.UseVisualStyleBackColor = true;
            gestionHistorialUIButtonRecuperarEstado.Click += gestionHistorialUIButtonRecuperarEstado_Click;
            // 
            // gestionHistorialUILabelGridUsuarios
            // 
            gestionHistorialUILabelGridUsuarios.AutoSize = true;
            gestionHistorialUILabelGridUsuarios.Location = new Point(73, 72);
            gestionHistorialUILabelGridUsuarios.Name = "gestionHistorialUILabelGridUsuarios";
            gestionHistorialUILabelGridUsuarios.Size = new Size(115, 15);
            gestionHistorialUILabelGridUsuarios.TabIndex = 3;
            gestionHistorialUILabelGridUsuarios.Text = "Usuarios disponibles";
            // 
            // gestionHistorialUILabelGridEstadoUsuarios
            // 
            gestionHistorialUILabelGridEstadoUsuarios.AutoSize = true;
            gestionHistorialUILabelGridEstadoUsuarios.Location = new Point(627, 72);
            gestionHistorialUILabelGridEstadoUsuarios.Name = "gestionHistorialUILabelGridEstadoUsuarios";
            gestionHistorialUILabelGridEstadoUsuarios.Size = new Size(184, 15);
            gestionHistorialUILabelGridEstadoUsuarios.TabIndex = 4;
            gestionHistorialUILabelGridEstadoUsuarios.Text = "Historial del usuario seleccionado";
            // 
            // GestionHistorialUsuarioUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1003, 506);
            ControlBox = false;
            Controls.Add(gestionHistorialUILabelGridEstadoUsuarios);
            Controls.Add(gestionHistorialUILabelGridUsuarios);
            Controls.Add(gestionHistorialUIButtonRecuperarEstado);
            Controls.Add(dataGridViewHistorial);
            Controls.Add(dataGridViewUsuarios);
            FormBorderStyle = FormBorderStyle.None;
            Name = "GestionHistorialUsuarioUI";
            StartPosition = FormStartPosition.CenterParent;
            Text = "GestionHistorialUsuarioUI";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewHistorial).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridViewUsuarios;
        private DataGridView dataGridViewHistorial;
        private Button gestionHistorialUIButtonRecuperarEstado;
        private Label gestionHistorialUILabelGridUsuarios;
        private Label gestionHistorialUILabelGridEstadoUsuarios;
    }
}