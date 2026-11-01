namespace UI.Almacen
{
    partial class GestionProductosUI
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
            gbxListado = new GroupBox();
            dgvProductos = new DataGridView();
            gbxFormulario = new GroupBox();
            btnLimpiar = new Button();
            btnEliminar = new Button();
            btnGuardar = new Button();
            numPuntoPedido = new NumericUpDown();
            lblPuntoPedido = new Label();
            numPrecio = new NumericUpDown();
            lblPrecio = new Label();
            txtNombre = new TextBox();
            lblNombre = new Label();
            txtCodigoBarra = new TextBox();
            lblCodigoBarra = new Label();
            btnConsultarLotes = new Button();
            gbxListado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            gbxFormulario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPuntoPedido).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrecio).BeginInit();
            SuspendLayout();
            // 
            // gbxListado
            // 
            gbxListado.Controls.Add(dgvProductos);
            gbxListado.Location = new Point(12, 12);
            gbxListado.Name = "gbxListado";
            gbxListado.Size = new Size(650, 480);
            gbxListado.TabIndex = 0;
            gbxListado.TabStop = false;
            gbxListado.Text = "Catálogo Activo";
            // 
            // dgvProductos
            // 
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Location = new Point(15, 30);
            dgvProductos.Name = "dgvProductos";
            dgvProductos.Size = new Size(620, 430);
            dgvProductos.TabIndex = 0;
            // 
            // gbxFormulario
            // 
            gbxFormulario.Controls.Add(btnConsultarLotes);
            gbxFormulario.Controls.Add(btnLimpiar);
            gbxFormulario.Controls.Add(btnEliminar);
            gbxFormulario.Controls.Add(btnGuardar);
            gbxFormulario.Controls.Add(numPuntoPedido);
            gbxFormulario.Controls.Add(lblPuntoPedido);
            gbxFormulario.Controls.Add(numPrecio);
            gbxFormulario.Controls.Add(lblPrecio);
            gbxFormulario.Controls.Add(txtNombre);
            gbxFormulario.Controls.Add(lblNombre);
            gbxFormulario.Controls.Add(txtCodigoBarra);
            gbxFormulario.Controls.Add(lblCodigoBarra);
            gbxFormulario.Location = new Point(680, 12);
            gbxFormulario.Name = "gbxFormulario";
            gbxFormulario.Size = new Size(300, 480);
            gbxFormulario.TabIndex = 1;
            gbxFormulario.TabStop = false;
            gbxFormulario.Text = "Datos del Producto";
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(20, 432);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(260, 35);
            btnLimpiar.TabIndex = 10;
            btnLimpiar.Text = "Limpiar Campos";
            btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.LightCoral;
            btnEliminar.Location = new Point(20, 382);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(260, 35);
            btnEliminar.TabIndex = 9;
            btnEliminar.Text = "Dar de Baja";
            btnEliminar.UseVisualStyleBackColor = false;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.LightGreen;
            btnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGuardar.Location = new Point(20, 332);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(260, 40);
            btnGuardar.TabIndex = 8;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            // 
            // numPuntoPedido
            // 
            numPuntoPedido.Location = new Point(20, 230);
            numPuntoPedido.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numPuntoPedido.Name = "numPuntoPedido";
            numPuntoPedido.Size = new Size(260, 23);
            numPuntoPedido.TabIndex = 7;
            // 
            // lblPuntoPedido
            // 
            lblPuntoPedido.AutoSize = true;
            lblPuntoPedido.Location = new Point(20, 210);
            lblPuntoPedido.Name = "lblPuntoPedido";
            lblPuntoPedido.Size = new Size(140, 15);
            lblPuntoPedido.TabIndex = 6;
            lblPuntoPedido.Text = "Punto de Pedido (Alerta):";
            // 
            // numPrecio
            // 
            numPrecio.DecimalPlaces = 2;
            numPrecio.Location = new Point(20, 170);
            numPrecio.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numPrecio.Name = "numPrecio";
            numPrecio.Size = new Size(260, 23);
            numPrecio.TabIndex = 5;
            // 
            // lblPrecio
            // 
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(20, 150);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(116, 15);
            lblPrecio.TabIndex = 4;
            lblPrecio.Text = "Precio Unitario local:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(20, 110);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(260, 23);
            txtNombre.TabIndex = 3;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(20, 90);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(125, 15);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre del Producto:";
            // 
            // txtCodigoBarra
            // 
            txtCodigoBarra.Location = new Point(20, 50);
            txtCodigoBarra.Name = "txtCodigoBarra";
            txtCodigoBarra.Size = new Size(260, 23);
            txtCodigoBarra.TabIndex = 1;
            // 
            // lblCodigoBarra
            // 
            lblCodigoBarra.AutoSize = true;
            lblCodigoBarra.Location = new Point(20, 30);
            lblCodigoBarra.Name = "lblCodigoBarra";
            lblCodigoBarra.Size = new Size(132, 15);
            lblCodigoBarra.TabIndex = 0;
            lblCodigoBarra.Text = "Código de Barras (SKU):";
            // 
            // btnConsultarLotes
            // 
            btnConsultarLotes.Location = new Point(20, 267);
            btnConsultarLotes.Name = "btnConsultarLotes";
            btnConsultarLotes.Size = new Size(260, 35);
            btnConsultarLotes.TabIndex = 11;
            btnConsultarLotes.Text = "Consultar Lotes";
            btnConsultarLotes.UseVisualStyleBackColor = true;
            // 
            // GestionProductosUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 515);
            Controls.Add(gbxFormulario);
            Controls.Add(gbxListado);
            Name = "GestionProductosUI";
            Text = "Gestión de Productos";
            gbxListado.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            gbxFormulario.ResumeLayout(false);
            gbxFormulario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPuntoPedido).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrecio).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox gbxListado;
        private System.Windows.Forms.DataGridView dgvProductos;
        private System.Windows.Forms.GroupBox gbxFormulario;
        private System.Windows.Forms.TextBox txtCodigoBarra;
        private System.Windows.Forms.Label lblCodigoBarra;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.NumericUpDown numPuntoPedido;
        private System.Windows.Forms.Label lblPuntoPedido;
        private System.Windows.Forms.NumericUpDown numPrecio;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnGuardar;
        private Button btnConsultarLotes;
    }
}