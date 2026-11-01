namespace UI.Compras
{
    partial class RecepcionMercaderiaUI
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
            lblTitulo = new System.Windows.Forms.Label();
            dgvOrdenesPendientes = new System.Windows.Forms.DataGridView();
            lblRemito = new System.Windows.Forms.Label();
            txtNroRemito = new System.Windows.Forms.TextBox();
            dgvDetallesRecepcion = new System.Windows.Forms.DataGridView();
            btnConfirmarIngreso = new System.Windows.Forms.Button();
            lblInstrucciones = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dgvOrdenesPendientes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetallesRecepcion).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTitulo.Location = new System.Drawing.Point(20, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(325, 21);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Paso 1: Seleccione la OC que acaba de llegar";
            // 
            // dgvOrdenesPendientes
            // 
            dgvOrdenesPendientes.AllowUserToAddRows = false;
            dgvOrdenesPendientes.AllowUserToDeleteRows = false;
            dgvOrdenesPendientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenesPendientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrdenesPendientes.Location = new System.Drawing.Point(20, 50);
            dgvOrdenesPendientes.Name = "dgvOrdenesPendientes";
            dgvOrdenesPendientes.ReadOnly = true;
            dgvOrdenesPendientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvOrdenesPendientes.Size = new System.Drawing.Size(750, 150);
            dgvOrdenesPendientes.TabIndex = 1;
            // 
            // lblRemito
            // 
            lblRemito.AutoSize = true;
            lblRemito.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblRemito.Location = new System.Drawing.Point(20, 220);
            lblRemito.Name = "lblRemito";
            lblRemito.Size = new System.Drawing.Size(264, 21);
            lblRemito.TabIndex = 2;
            lblRemito.Text = "Paso 2: Ingrese el Nro. de Remito Papel";
            // 
            // txtNroRemito
            // 
            txtNroRemito.Font = new System.Drawing.Font("Segoe UI", 11F);
            txtNroRemito.Location = new System.Drawing.Point(20, 250);
            txtNroRemito.Name = "txtNroRemito";
            txtNroRemito.Size = new System.Drawing.Size(250, 27);
            txtNroRemito.TabIndex = 3;
            // 
            // lblInstrucciones
            // 
            lblInstrucciones.AutoSize = true;
            lblInstrucciones.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblInstrucciones.Location = new System.Drawing.Point(20, 300);
            lblInstrucciones.Name = "lblInstrucciones";
            lblInstrucciones.Size = new System.Drawing.Size(434, 21);
            lblInstrucciones.TabIndex = 4;
            lblInstrucciones.Text = "Paso 3: Complete Lote y Vencimiento en las celdas amarillas";
            // 
            // dgvDetallesRecepcion
            // 
            dgvDetallesRecepcion.AllowUserToAddRows = false;
            dgvDetallesRecepcion.AllowUserToDeleteRows = false;
            dgvDetallesRecepcion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetallesRecepcion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetallesRecepcion.Location = new System.Drawing.Point(20, 330);
            dgvDetallesRecepcion.Name = "dgvDetallesRecepcion";
            dgvDetallesRecepcion.Size = new System.Drawing.Size(750, 200);
            dgvDetallesRecepcion.TabIndex = 5;
            // 
            // btnConfirmarIngreso
            // 
            btnConfirmarIngreso.BackColor = System.Drawing.Color.LightGreen;
            btnConfirmarIngreso.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            btnConfirmarIngreso.Location = new System.Drawing.Point(570, 550);
            btnConfirmarIngreso.Name = "btnConfirmarIngreso";
            btnConfirmarIngreso.Size = new System.Drawing.Size(200, 45);
            btnConfirmarIngreso.TabIndex = 6;
            btnConfirmarIngreso.Text = "Confirmar Ingreso";
            btnConfirmarIngreso.UseVisualStyleBackColor = false;
            // 
            // RecepcionMercaderiaUI
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 620);
            Controls.Add(btnConfirmarIngreso);
            Controls.Add(dgvDetallesRecepcion);
            Controls.Add(lblInstrucciones);
            Controls.Add(txtNroRemito);
            Controls.Add(lblRemito);
            Controls.Add(dgvOrdenesPendientes);
            Controls.Add(lblTitulo);
            Name = "RecepcionMercaderiaUI";
            Text = "Recepción de Mercadería";
            ((System.ComponentModel.ISupportInitialize)dgvOrdenesPendientes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetallesRecepcion).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvOrdenesPendientes;
        private System.Windows.Forms.Label lblRemito;
        private System.Windows.Forms.TextBox txtNroRemito;
        private System.Windows.Forms.DataGridView dgvDetallesRecepcion;
        private System.Windows.Forms.Button btnConfirmarIngreso;
        private System.Windows.Forms.Label lblInstrucciones;
    }
}