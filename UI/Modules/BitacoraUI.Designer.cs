namespace UI.Modules
{
    partial class BitacoraUI
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
            dataGridViewRegistrosBitacora = new DataGridView();
            bitacoraUILabelGrid = new Label();
            comboBoxAccion = new ComboBox();
            bitacoraUILabelComboBoxAccion = new Label();
            bitacoraUILabelComboBoxUsername = new Label();
            comboBoxUsername = new ComboBox();
            bitacoraUIButtonLimpiarFiltros = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewRegistrosBitacora).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewRegistrosBitacora
            // 
            dataGridViewRegistrosBitacora.AllowUserToAddRows = false;
            dataGridViewRegistrosBitacora.AllowUserToDeleteRows = false;
            dataGridViewRegistrosBitacora.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewRegistrosBitacora.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewRegistrosBitacora.Location = new Point(73, 83);
            dataGridViewRegistrosBitacora.MultiSelect = false;
            dataGridViewRegistrosBitacora.Name = "dataGridViewRegistrosBitacora";
            dataGridViewRegistrosBitacora.ReadOnly = true;
            dataGridViewRegistrosBitacora.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewRegistrosBitacora.Size = new Size(560, 342);
            dataGridViewRegistrosBitacora.TabIndex = 0;
            // 
            // bitacoraUILabelGrid
            // 
            bitacoraUILabelGrid.AutoSize = true;
            bitacoraUILabelGrid.Location = new Point(73, 65);
            bitacoraUILabelGrid.Name = "bitacoraUILabelGrid";
            bitacoraUILabelGrid.Size = new Size(129, 15);
            bitacoraUILabelGrid.TabIndex = 1;
            bitacoraUILabelGrid.Text = "Registros de la bitacora";
            // 
            // comboBoxAccion
            // 
            comboBoxAccion.FormattingEnabled = true;
            comboBoxAccion.Location = new Point(649, 100);
            comboBoxAccion.Name = "comboBoxAccion";
            comboBoxAccion.Size = new Size(226, 23);
            comboBoxAccion.TabIndex = 2;
            comboBoxAccion.SelectedIndexChanged += comboBoxAccion_SelectedIndexChanged;
            // 
            // bitacoraUILabelComboBoxAccion
            // 
            bitacoraUILabelComboBoxAccion.AutoSize = true;
            bitacoraUILabelComboBoxAccion.Location = new Point(649, 82);
            bitacoraUILabelComboBoxAccion.Name = "bitacoraUILabelComboBoxAccion";
            bitacoraUILabelComboBoxAccion.Size = new Size(106, 15);
            bitacoraUILabelComboBoxAccion.TabIndex = 3;
            bitacoraUILabelComboBoxAccion.Text = "Filtrado por acción";
            bitacoraUILabelComboBoxAccion.Click += bitacoraUILabelComboBoxAccion_Click;
            // 
            // bitacoraUILabelComboBoxUsername
            // 
            bitacoraUILabelComboBoxUsername.AutoSize = true;
            bitacoraUILabelComboBoxUsername.Location = new Point(649, 137);
            bitacoraUILabelComboBoxUsername.Name = "bitacoraUILabelComboBoxUsername";
            bitacoraUILabelComboBoxUsername.Size = new Size(123, 15);
            bitacoraUILabelComboBoxUsername.TabIndex = 5;
            bitacoraUILabelComboBoxUsername.Text = "Filtrado por username";
            // 
            // comboBoxUsername
            // 
            comboBoxUsername.FormattingEnabled = true;
            comboBoxUsername.Location = new Point(649, 155);
            comboBoxUsername.Name = "comboBoxUsername";
            comboBoxUsername.Size = new Size(226, 23);
            comboBoxUsername.TabIndex = 4;
            comboBoxUsername.SelectedIndexChanged += comboBoxUsername_SelectedIndexChanged;
            // 
            // bitacoraUIButtonLimpiarFiltros
            // 
            bitacoraUIButtonLimpiarFiltros.Location = new Point(649, 184);
            bitacoraUIButtonLimpiarFiltros.Name = "bitacoraUIButtonLimpiarFiltros";
            bitacoraUIButtonLimpiarFiltros.Size = new Size(226, 33);
            bitacoraUIButtonLimpiarFiltros.TabIndex = 6;
            bitacoraUIButtonLimpiarFiltros.Text = "Limpiar Filtros";
            bitacoraUIButtonLimpiarFiltros.UseVisualStyleBackColor = true;
            bitacoraUIButtonLimpiarFiltros.Click += bitacoraUIButtonLimpiarFiltros_Click;
            // 
            // BitacoraUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(903, 456);
            ControlBox = false;
            Controls.Add(bitacoraUIButtonLimpiarFiltros);
            Controls.Add(bitacoraUILabelComboBoxUsername);
            Controls.Add(comboBoxUsername);
            Controls.Add(bitacoraUILabelComboBoxAccion);
            Controls.Add(comboBoxAccion);
            Controls.Add(bitacoraUILabelGrid);
            Controls.Add(dataGridViewRegistrosBitacora);
            FormBorderStyle = FormBorderStyle.None;
            Name = "BitacoraUI";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BitacoraUI";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)dataGridViewRegistrosBitacora).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridViewRegistrosBitacora;
        private Label bitacoraUILabelGrid;
        private ComboBox comboBoxAccion;
        private Label bitacoraUILabelComboBoxAccion;
        private Label bitacoraUILabelComboBoxUsername;
        private ComboBox comboBoxUsername;
        private Button bitacoraUIButtonLimpiarFiltros;
    }
}