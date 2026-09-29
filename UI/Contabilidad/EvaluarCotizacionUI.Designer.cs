namespace UI.Contabilidad
{
    partial class EvaluarCotizacionUI
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
            lblTituloOrdenes = new Label();
            dgvOrdenesPendientes = new DataGridView();
            btnEfectuarPago = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenesPendientes).BeginInit();
            SuspendLayout();
            // 
            // lblTituloOrdenes
            // 
            lblTituloOrdenes.AutoSize = true;
            lblTituloOrdenes.Location = new Point(85, 76);
            lblTituloOrdenes.Name = "lblTituloOrdenes";
            lblTituloOrdenes.Size = new Size(174, 15);
            lblTituloOrdenes.TabIndex = 0;
            lblTituloOrdenes.Text = "Órdenes de Compra Pendientes";
            // 
            // dgvOrdenesPendientes
            // 
            dgvOrdenesPendientes.AllowUserToAddRows = false;
            dgvOrdenesPendientes.AllowUserToDeleteRows = false;
            dgvOrdenesPendientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenesPendientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrdenesPendientes.Location = new Point(85, 119);
            dgvOrdenesPendientes.Name = "dgvOrdenesPendientes";
            dgvOrdenesPendientes.ReadOnly = true;
            dgvOrdenesPendientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrdenesPendientes.Size = new Size(676, 223);
            dgvOrdenesPendientes.TabIndex = 1;
            // 
            // btnEfectuarPago
            // 
            btnEfectuarPago.BackColor = Color.SeaShell;
            btnEfectuarPago.Location = new Point(633, 367);
            btnEfectuarPago.Name = "btnEfectuarPago";
            btnEfectuarPago.Size = new Size(128, 45);
            btnEfectuarPago.TabIndex = 2;
            btnEfectuarPago.Text = "Efectuar Pago a Proveedor";
            btnEfectuarPago.UseVisualStyleBackColor = false;
            // 
            // EvaluarCotizacionUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnEfectuarPago);
            Controls.Add(dgvOrdenesPendientes);
            Controls.Add(lblTituloOrdenes);
            FormBorderStyle = FormBorderStyle.None;
            Name = "EvaluarCotizacionUI";
            Text = "Evaluación de Cotizaciones y Pagos";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)dgvOrdenesPendientes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTituloOrdenes;
        private DataGridView dgvOrdenesPendientes;
        private Button btnEfectuarPago;
    }
}