namespace UI.Venta
{
    partial class PuntoVentaUI
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
            dgvPagos = new DataGridView();
            btnAgregarPago = new Button();
            lblRestante = new Label();
            btnQuitarPago = new Button();
            ((System.ComponentModel.ISupportInitialize)numCantidad).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCarrito).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPagos).BeginInit();
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
            cmbClientes.DropDownStyle = ComboBoxStyle.DropDownList;
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
            cmbProductos.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProductos.FormattingEnabled = true;
            cmbProductos.Location = new Point(414, 83);
            cmbProductos.Name = "cmbProductos";
            cmbProductos.Size = new Size(140, 23);
            cmbProductos.TabIndex = 3;
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
            lblMetodoPago.Location = new Point(44, 480);
            lblMetodoPago.Name = "lblMetodoPago";
            lblMetodoPago.Size = new Size(95, 15);
            lblMetodoPago.TabIndex = 8;
            lblMetodoPago.Text = "Método de Pago";
            // 
            // cmbMetodoPago
            // 
            cmbMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMetodoPago.FormattingEnabled = true;
            cmbMetodoPago.Location = new Point(44, 498);
            cmbMetodoPago.Name = "cmbMetodoPago";
            cmbMetodoPago.Size = new Size(134, 23);
            cmbMetodoPago.TabIndex = 9;
            // 
            // lblTotalMonto
            // 
            lblTotalMonto.AutoSize = true;
            lblTotalMonto.BackColor = SystemColors.Control;
            lblTotalMonto.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold);
            lblTotalMonto.Location = new Point(411, 456);
            lblTotalMonto.Name = "lblTotalMonto";
            lblTotalMonto.Size = new Size(143, 30);
            lblTotalMonto.TabIndex = 10;
            lblTotalMonto.Text = "TOTAL: $ 0.00";
            // 
            // btnConfirmarVenta
            // 
            btnConfirmarVenta.BackColor = Color.SeaShell;
            btnConfirmarVenta.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            btnConfirmarVenta.Location = new Point(748, 552);
            btnConfirmarVenta.Name = "btnConfirmarVenta";
            btnConfirmarVenta.Size = new Size(143, 56);
            btnConfirmarVenta.TabIndex = 11;
            btnConfirmarVenta.Text = "Confirmar Venta";
            btnConfirmarVenta.UseVisualStyleBackColor = false;
            // 
            // btnQuitar
            // 
            btnQuitar.Location = new Point(44, 431);
            btnQuitar.Name = "btnQuitar";
            btnQuitar.Size = new Size(188, 32);
            btnQuitar.TabIndex = 12;
            btnQuitar.Text = "Quitar del carrito";
            btnQuitar.UseVisualStyleBackColor = true;
            // 
            // dgvPagos
            // 
            dgvPagos.AllowUserToAddRows = false;
            dgvPagos.AllowUserToDeleteRows = false;
            dgvPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPagos.Location = new Point(44, 532);
            dgvPagos.MultiSelect = false;
            dgvPagos.Name = "dgvPagos";
            dgvPagos.ReadOnly = true;
            dgvPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPagos.Size = new Size(393, 107);
            dgvPagos.TabIndex = 13;
            // 
            // btnAgregarPago
            // 
            btnAgregarPago.BackColor = Color.LightSteelBlue;
            btnAgregarPago.Location = new Point(195, 491);
            btnAgregarPago.Name = "btnAgregarPago";
            btnAgregarPago.Size = new Size(113, 35);
            btnAgregarPago.TabIndex = 14;
            btnAgregarPago.Text = "Agregar Pago";
            btnAgregarPago.UseVisualStyleBackColor = false;
            btnAgregarPago.Click += btnAgregarPago_Click;
            // 
            // lblRestante
            // 
            lblRestante.AutoSize = true;
            lblRestante.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblRestante.ForeColor = Color.Crimson;
            lblRestante.Location = new Point(414, 496);
            lblRestante.Name = "lblRestante";
            lblRestante.Size = new Size(196, 21);
            lblRestante.TabIndex = 15;
            lblRestante.Text = "SALDO RESTANTE: $ 0.00";
            // 
            // btnQuitarPago
            // 
            btnQuitarPago.Location = new Point(443, 552);
            btnQuitarPago.Name = "btnQuitarPago";
            btnQuitarPago.Size = new Size(111, 35);
            btnQuitarPago.TabIndex = 16;
            btnQuitarPago.Text = "Quitar Pago";
            btnQuitarPago.UseVisualStyleBackColor = true;
            // 
            // PuntoVentaUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = SystemColors.Control;
            ClientSize = new Size(920, 680);
            Controls.Add(btnQuitarPago);
            Controls.Add(lblRestante);
            Controls.Add(btnAgregarPago);
            Controls.Add(dgvPagos);
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
            ((System.ComponentModel.ISupportInitialize)dgvPagos).EndInit();
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
        private DataGridView dgvPagos;
        private Button btnAgregarPago;
        private Label lblRestante;
        private Button btnQuitarPago;
    }
}