namespace UI.Venta
{
    partial class PuntoVentaUI
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
            lblCliente = new Label();
            cmbClientes = new ComboBox();
            lblProducto = new Label();
            cmbProductos = new ComboBox();
            lblCantidad = new Label();
            numCantidad = new NumericUpDown();
            btnAgregar = new Button();
            dgvCarrito = new DataGridView();
            lblMetodoPago = new Label();
            cmbMetodoPago = new ComboBox();
            lblTotalMonto = new Label();
            btnConfirmarVenta = new Button();
            btnQuitar = new Button();
            ((System.ComponentModel.ISupportInitialize)numCantidad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCarrito).BeginInit();
            SuspendLayout();
            // 
            // lblCliente
            // 
            lblCliente.AutoSize = true;
            lblCliente.Location = new Point(44, 51);
            lblCliente.Name = "lblCliente";
            lblCliente.Size = new Size(50, 15);
            lblCliente.TabIndex = 0;
            lblCliente.Text = "Cliente: ";
            // 
            // cmbClientes
            // 
            cmbClientes.FormattingEnabled = true;
            cmbClientes.Location = new Point(44, 83);
            cmbClientes.Name = "cmbClientes";
            cmbClientes.Size = new Size(140, 23);
            cmbClientes.TabIndex = 1;
            // 
            // lblProducto
            // 
            lblProducto.AutoSize = true;
            lblProducto.Location = new Point(414, 51);
            lblProducto.Name = "lblProducto";
            lblProducto.Size = new Size(62, 15);
            lblProducto.TabIndex = 2;
            lblProducto.Text = "Producto: ";
            // 
            // cmbProductos
            // 
            cmbProductos.FormattingEnabled = true;
            cmbProductos.Location = new Point(414, 83);
            cmbProductos.Name = "cmbProductos";
            cmbProductos.Size = new Size(140, 23);
            cmbProductos.TabIndex = 3;
            cmbProductos.SelectedIndexChanged += cmbProductos_SelectedIndexChanged;
            // 
            // lblCantidad
            // 
            lblCantidad.AutoSize = true;
            lblCantidad.Location = new Point(771, 48);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(61, 15);
            lblCantidad.TabIndex = 4;
            lblCantidad.Text = "Cantidad: ";
            // 
            // numCantidad
            // 
            numCantidad.Location = new Point(771, 83);
            numCantidad.Name = "numCantidad";
            numCantidad.Size = new Size(120, 23);
            numCantidad.TabIndex = 5;
            numCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnAgregar
            // 
            btnAgregar.Location = new Point(771, 126);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(120, 49);
            btnAgregar.TabIndex = 6;
            btnAgregar.Text = "Agregar al carrito";
            btnAgregar.UseVisualStyleBackColor = true;
            // 
            // dgvCarrito
            // 
            dgvCarrito.AllowUserToAddRows = false;
            dgvCarrito.AllowUserToDeleteRows = false;
            dgvCarrito.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCarrito.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCarrito.Location = new Point(44, 181);
            dgvCarrito.MultiSelect = false;
            dgvCarrito.Name = "dgvCarrito";
            dgvCarrito.ReadOnly = true;
            dgvCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCarrito.Size = new Size(847, 244);
            dgvCarrito.TabIndex = 7;
            // 
            // lblMetodoPago
            // 
            lblMetodoPago.AutoSize = true;
            lblMetodoPago.Location = new Point(44, 468);
            lblMetodoPago.Name = "lblMetodoPago";
            lblMetodoPago.Size = new Size(95, 15);
            lblMetodoPago.TabIndex = 8;
            lblMetodoPago.Text = "Metodo de Pago";
            // 
            // cmbMetodoPago
            // 
            cmbMetodoPago.FormattingEnabled = true;
            cmbMetodoPago.Location = new Point(44, 486);
            cmbMetodoPago.Name = "cmbMetodoPago";
            cmbMetodoPago.Size = new Size(134, 23);
            cmbMetodoPago.TabIndex = 9;
            // 
            // lblTotalMonto
            // 
            lblTotalMonto.AutoSize = true;
            lblTotalMonto.BackColor = SystemColors.Control;
            lblTotalMonto.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalMonto.Location = new Point(411, 477);
            lblTotalMonto.Name = "lblTotalMonto";
            lblTotalMonto.Size = new Size(143, 30);
            lblTotalMonto.TabIndex = 10;
            lblTotalMonto.Text = "TOTAL: $ 0.00";
            // 
            // btnConfirmarVenta
            // 
            btnConfirmarVenta.BackColor = Color.SeaShell;
            btnConfirmarVenta.Location = new Point(748, 468);
            btnConfirmarVenta.Name = "btnConfirmarVenta";
            btnConfirmarVenta.Size = new Size(143, 45);
            btnConfirmarVenta.TabIndex = 11;
            btnConfirmarVenta.Text = "Confirmar Venta";
            btnConfirmarVenta.UseVisualStyleBackColor = false;
            // 
            // btnQuitar
            // 
            btnQuitar.Location = new Point(391, 431);
            btnQuitar.Name = "btnQuitar";
            btnQuitar.Size = new Size(188, 32);
            btnQuitar.TabIndex = 12;
            btnQuitar.Text = "Quitar del carrito";
            btnQuitar.UseVisualStyleBackColor = true;
            btnQuitar.Click += btnQuitar_Click;
            // 
            // PuntoVentaUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(920, 545);
            Controls.Add(btnQuitar);
            Controls.Add(btnConfirmarVenta);
            Controls.Add(lblTotalMonto);
            Controls.Add(cmbMetodoPago);
            Controls.Add(lblMetodoPago);
            Controls.Add(dgvCarrito);
            Controls.Add(btnAgregar);
            Controls.Add(numCantidad);
            Controls.Add(lblCantidad);
            Controls.Add(cmbProductos);
            Controls.Add(lblProducto);
            Controls.Add(cmbClientes);
            Controls.Add(lblCliente);
            FormBorderStyle = FormBorderStyle.None;
            Name = "PuntoVentaUI";
            Text = "Venta";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)numCantidad).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCarrito).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCliente;
        private ComboBox cmbClientes;
        private Label lblProducto;
        private ComboBox cmbProductos;
        private Label lblCantidad;
        private NumericUpDown numCantidad;
        private Button btnAgregar;
        private DataGridView dgvCarrito;
        private Label lblMetodoPago;
        private ComboBox cmbMetodoPago;
        private Label lblTotalMonto;
        private Button btnConfirmarVenta;
        private Button btnQuitar;
    }
}