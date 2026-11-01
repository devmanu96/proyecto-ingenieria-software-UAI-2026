namespace UI.Almacen
{
    partial class ProveedoresUI
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
            lblCUIT = new System.Windows.Forms.Label();
            txtCUIT = new System.Windows.Forms.TextBox();
            lblRazonSocial = new System.Windows.Forms.Label();
            txtRazonSocial = new System.Windows.Forms.TextBox();
            lblCondicion = new System.Windows.Forms.Label();
            txtCondicionComercial = new System.Windows.Forms.TextBox();
            btnGuardar = new System.Windows.Forms.Button();
            btnLimpiar = new System.Windows.Forms.Button();
            btnEliminar = new System.Windows.Forms.Button();
            dgvProveedores = new System.Windows.Forms.DataGridView();
            btnMigrarSQL = new System.Windows.Forms.Button();
            panelFormulario = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)dgvProveedores).BeginInit();
            panelFormulario.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblTitulo.Location = new System.Drawing.Point(12, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(434, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de Proveedores (Persistencia JSON)";
            // 
            // panelFormulario
            // 
            panelFormulario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelFormulario.Controls.Add(lblCUIT);
            panelFormulario.Controls.Add(txtCUIT);
            panelFormulario.Controls.Add(lblRazonSocial);
            panelFormulario.Controls.Add(txtRazonSocial);
            panelFormulario.Controls.Add(lblCondicion);
            panelFormulario.Controls.Add(txtCondicionComercial);
            panelFormulario.Controls.Add(btnGuardar);
            panelFormulario.Controls.Add(btnLimpiar);
            panelFormulario.Controls.Add(btnEliminar);
            panelFormulario.Location = new System.Drawing.Point(17, 70);
            panelFormulario.Name = "panelFormulario";
            panelFormulario.Size = new System.Drawing.Size(280, 360);
            panelFormulario.TabIndex = 1;
            // 
            // lblCUIT
            // 
            lblCUIT.AutoSize = true;
            lblCUIT.Location = new System.Drawing.Point(15, 15);
            lblCUIT.Name = "lblCUIT";
            lblCUIT.Size = new System.Drawing.Size(32, 15);
            lblCUIT.TabIndex = 0;
            lblCUIT.Text = "CUIT";
            // 
            // txtCUIT
            // 
            txtCUIT.Location = new System.Drawing.Point(18, 33);
            txtCUIT.Name = "txtCUIT";
            txtCUIT.Size = new System.Drawing.Size(240, 23);
            txtCUIT.TabIndex = 1;
            // 
            // lblRazonSocial
            // 
            lblRazonSocial.AutoSize = true;
            lblRazonSocial.Location = new System.Drawing.Point(15, 75);
            lblRazonSocial.Name = "lblRazonSocial";
            lblRazonSocial.Size = new System.Drawing.Size(73, 15);
            lblRazonSocial.TabIndex = 2;
            lblRazonSocial.Text = "Razón Social";
            // 
            // txtRazonSocial
            // 
            txtRazonSocial.Location = new System.Drawing.Point(18, 93);
            txtRazonSocial.Name = "txtRazonSocial";
            txtRazonSocial.Size = new System.Drawing.Size(240, 23);
            txtRazonSocial.TabIndex = 3;
            // 
            // lblCondicion
            // 
            lblCondicion.AutoSize = true;
            lblCondicion.Location = new System.Drawing.Point(15, 135);
            lblCondicion.Name = "lblCondicion";
            lblCondicion.Size = new System.Drawing.Size(121, 15);
            lblCondicion.TabIndex = 4;
            lblCondicion.Text = "Condición Comercial";
            // 
            // txtCondicionComercial
            // 
            txtCondicionComercial.Location = new System.Drawing.Point(18, 153);
            txtCondicionComercial.Name = "txtCondicionComercial";
            txtCondicionComercial.Size = new System.Drawing.Size(240, 23);
            txtCondicionComercial.TabIndex = 5;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = System.Drawing.Color.LightGreen;
            btnGuardar.Location = new System.Drawing.Point(18, 205);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new System.Drawing.Size(240, 40);
            btnGuardar.TabIndex = 6;
            btnGuardar.Text = "Guardar Proveedor";
            btnGuardar.UseVisualStyleBackColor = false;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new System.Drawing.Point(18, 255);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new System.Drawing.Size(240, 35);
            btnLimpiar.TabIndex = 7;
            btnLimpiar.Text = "Limpiar Selección";
            btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = System.Drawing.Color.LightCoral;
            btnEliminar.Location = new System.Drawing.Point(18, 300);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new System.Drawing.Size(240, 35);
            btnEliminar.TabIndex = 8;
            btnEliminar.Text = "Eliminar Proveedor";
            btnEliminar.UseVisualStyleBackColor = false;
            // 
            // dgvProveedores
            // 
            dgvProveedores.AllowUserToAddRows = false;
            dgvProveedores.AllowUserToDeleteRows = false;
            dgvProveedores.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvProveedores.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dgvProveedores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProveedores.Location = new System.Drawing.Point(318, 70);
            dgvProveedores.MultiSelect = false;
            dgvProveedores.Name = "dgvProveedores";
            dgvProveedores.ReadOnly = true;
            dgvProveedores.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvProveedores.Size = new System.Drawing.Size(650, 360);
            dgvProveedores.TabIndex = 2;
            // 
            // btnMigrarSQL
            // 
            btnMigrarSQL.BackColor = System.Drawing.Color.LightSkyBlue;
            btnMigrarSQL.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnMigrarSQL.Location = new System.Drawing.Point(318, 445);
            btnMigrarSQL.Name = "btnMigrarSQL";
            btnMigrarSQL.Size = new System.Drawing.Size(300, 40);
            btnMigrarSQL.TabIndex = 3;
            btnMigrarSQL.Text = "Importar Datos de Prueba (SQL -> JSON)";
            btnMigrarSQL.UseVisualStyleBackColor = false;
            // 
            // ProveedoresUI
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(984, 511);
            Controls.Add(btnMigrarSQL);
            Controls.Add(dgvProveedores);
            Controls.Add(panelFormulario);
            Controls.Add(lblTitulo);
            Name = "ProveedoresUI";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "ABM Proveedores (JSON)";
            ((System.ComponentModel.ISupportInitialize)dgvProveedores).EndInit();
            panelFormulario.ResumeLayout(false);
            panelFormulario.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelFormulario;
        private System.Windows.Forms.Label lblCUIT;
        private System.Windows.Forms.TextBox txtCUIT;
        private System.Windows.Forms.Label lblRazonSocial;
        private System.Windows.Forms.TextBox txtRazonSocial;
        private System.Windows.Forms.Label lblCondicion;
        private System.Windows.Forms.TextBox txtCondicionComercial;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.DataGridView dgvProveedores;
        private System.Windows.Forms.Button btnMigrarSQL;
    }
}