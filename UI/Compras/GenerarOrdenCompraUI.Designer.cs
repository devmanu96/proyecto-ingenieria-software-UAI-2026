namespace UI.Compras
{
    partial class GenerarOrdenCompraUI
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
            dgvFaltantes = new DataGridView();
            lblFaltantesSolicitados = new Label();
            dgvCarritoOC = new DataGridView();
            btnCancelarOC = new Button();
            lblCatalogo = new Label();
            lblOrdenCompra = new Label();
            lblCantidadPallets = new Label();
            lblTotalOC = new Label();
            btnEmitirOrden = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            dgvCatalogo = new DataGridView();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            numericUpDown1 = new NumericUpDown();
            btnAgregarCarrito = new Button();
            lblMonto = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvFaltantes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCarritoOC).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCatalogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // dgvFaltantes
            // 
            dgvFaltantes.AllowUserToAddRows = false;
            dgvFaltantes.AllowUserToDeleteRows = false;
            dgvFaltantes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFaltantes.Location = new Point(12, 81);
            dgvFaltantes.Name = "dgvFaltantes";
            dgvFaltantes.ReadOnly = true;
            dgvFaltantes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFaltantes.Size = new Size(303, 271);
            dgvFaltantes.TabIndex = 0;
            // 
            // lblFaltantesSolicitados
            // 
            lblFaltantesSolicitados.AutoSize = true;
            lblFaltantesSolicitados.Location = new Point(12, 49);
            lblFaltantesSolicitados.Name = "lblFaltantesSolicitados";
            lblFaltantesSolicitados.Size = new Size(142, 15);
            lblFaltantesSolicitados.TabIndex = 1;
            lblFaltantesSolicitados.Text = "FALTANTES SOLICITADOS";
            // 
            // dgvCarritoOC
            // 
            dgvCarritoOC.AllowUserToAddRows = false;
            dgvCarritoOC.AllowUserToDeleteRows = false;
            dgvCarritoOC.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCarritoOC.Location = new Point(12, 430);
            dgvCarritoOC.Name = "dgvCarritoOC";
            dgvCarritoOC.ReadOnly = true;
            dgvCarritoOC.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCarritoOC.Size = new Size(783, 190);
            dgvCarritoOC.TabIndex = 3;
            // 
            // btnCancelarOC
            // 
            btnCancelarOC.Location = new Point(12, 656);
            btnCancelarOC.Name = "btnCancelarOC";
            btnCancelarOC.Size = new Size(101, 40);
            btnCancelarOC.TabIndex = 4;
            btnCancelarOC.Text = "Volver";
            btnCancelarOC.UseVisualStyleBackColor = true;
            // 
            // lblCatalogo
            // 
            lblCatalogo.AutoSize = true;
            lblCatalogo.Location = new Point(361, 49);
            lblCatalogo.Name = "lblCatalogo";
            lblCatalogo.Size = new Size(205, 15);
            lblCatalogo.TabIndex = 5;
            lblCatalogo.Text = "CATÁLOGO PROVEEDORES DIRECTOS";
            // 
            // lblOrdenCompra
            // 
            lblOrdenCompra.AutoSize = true;
            lblOrdenCompra.Location = new Point(12, 412);
            lblOrdenCompra.Name = "lblOrdenCompra";
            lblOrdenCompra.Size = new Size(116, 15);
            lblOrdenCompra.TabIndex = 6;
            lblOrdenCompra.Text = "ORDEN DE COMPRA";
            // 
            // lblCantidadPallets
            // 
            lblCantidadPallets.AutoSize = true;
            lblCantidadPallets.Location = new Point(417, 364);
            lblCantidadPallets.Name = "lblCantidadPallets";
            lblCantidadPallets.Size = new Size(95, 15);
            lblCantidadPallets.TabIndex = 7;
            lblCantidadPallets.Text = "Cantidad Pallets:";
            // 
            // lblTotalOC
            // 
            lblTotalOC.AutoSize = true;
            lblTotalOC.Location = new Point(639, 638);
            lblTotalOC.Name = "lblTotalOC";
            lblTotalOC.Size = new Size(65, 15);
            lblTotalOC.TabIndex = 8;
            lblTotalOC.Text = "Total Neto:";
            // 
            // btnEmitirOrden
            // 
            btnEmitirOrden.BackColor = Color.MistyRose;
            btnEmitirOrden.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEmitirOrden.Location = new Point(634, 656);
            btnEmitirOrden.Name = "btnEmitirOrden";
            btnEmitirOrden.Size = new Size(161, 40);
            btnEmitirOrden.TabIndex = 9;
            btnEmitirOrden.Text = "Emitir Orden Compra";
            btnEmitirOrden.UseVisualStyleBackColor = false;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Location = new Point(361, 81);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(434, 271);
            tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dgvCatalogo);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(426, 243);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Coca-Cola";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvCatalogo
            // 
            dgvCatalogo.AllowUserToAddRows = false;
            dgvCatalogo.AllowUserToDeleteRows = false;
            dgvCatalogo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCatalogo.Dock = DockStyle.Fill;
            dgvCatalogo.Location = new Point(3, 3);
            dgvCatalogo.Name = "dgvCatalogo";
            dgvCatalogo.ReadOnly = true;
            dgvCatalogo.Size = new Size(420, 237);
            dgvCatalogo.TabIndex = 13;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(426, 243);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "PepsiCo";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(426, 243);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Manaos";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Location = new Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(426, 243);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Baggio";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(518, 364);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(48, 23);
            numericUpDown1.TabIndex = 14;
            numericUpDown1.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnAgregarCarrito
            // 
            btnAgregarCarrito.Location = new Point(593, 364);
            btnAgregarCarrito.Name = "btnAgregarCarrito";
            btnAgregarCarrito.Size = new Size(111, 41);
            btnAgregarCarrito.TabIndex = 15;
            btnAgregarCarrito.Text = "Agregar al Carrito";
            btnAgregarCarrito.UseVisualStyleBackColor = true;
            // 
            // lblMonto
            // 
            lblMonto.AutoSize = true;
            lblMonto.Location = new Point(710, 638);
            lblMonto.Name = "lblMonto";
            lblMonto.Size = new Size(17, 15);
            lblMonto.TabIndex = 16;
            lblMonto.Text = "--";
            // 
            // GenerarOrdenCompraUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(840, 729);
            Controls.Add(lblMonto);
            Controls.Add(btnAgregarCarrito);
            Controls.Add(numericUpDown1);
            Controls.Add(tabControl1);
            Controls.Add(btnEmitirOrden);
            Controls.Add(lblTotalOC);
            Controls.Add(lblCantidadPallets);
            Controls.Add(lblOrdenCompra);
            Controls.Add(lblCatalogo);
            Controls.Add(btnCancelarOC);
            Controls.Add(dgvCarritoOC);
            Controls.Add(lblFaltantesSolicitados);
            Controls.Add(dgvFaltantes);
            Name = "GenerarOrdenCompraUI";
            Text = "GenerarOrdenCompraUI";
            ((System.ComponentModel.ISupportInitialize)dgvFaltantes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCarritoOC).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCatalogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvFaltantes;
        private Label lblFaltantesSolicitados;
        private DataGridView dgvCarritoOC;
        private Button btnCancelarOC;
        private Label lblCatalogo;
        private Label lblOrdenCompra;
        private Label lblCantidadPallets;
        private Label lblTotalOC;
        private Button btnEmitirOrden;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private DataGridView dgvCatalogo;
        private NumericUpDown numericUpDown1;
        private Button btnAgregarCarrito;
        private Label lblMonto;
    }
}