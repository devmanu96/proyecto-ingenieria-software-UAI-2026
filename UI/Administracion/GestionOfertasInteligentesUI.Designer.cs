namespace UI.Administracion
{
    partial class GestionOfertasInteligentesUI
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
            btnEjecutarMotor = new Button();
            dgvOfertas = new DataGridView();
            panelEdicion = new Panel();
            btnDescartar = new Button();
            btnAprobar = new Button();
            lblSimbolo = new Label();
            numDescuento = new NumericUpDown();
            lblPropuestaDetalle = new Label();
            lblTituloAjuste = new Label();
            lblTituloUI = new Label();
            btnGestionarOfertas = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvOfertas).BeginInit();
            panelEdicion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDescuento).BeginInit();
            SuspendLayout();
            // 
            // btnEjecutarMotor
            // 
            btnEjecutarMotor.BackColor = Color.MidnightBlue;
            btnEjecutarMotor.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnEjecutarMotor.ForeColor = Color.White;
            btnEjecutarMotor.Location = new Point(20, 60);
            btnEjecutarMotor.Name = "btnEjecutarMotor";
            btnEjecutarMotor.Size = new Size(350, 45);
            btnEjecutarMotor.TabIndex = 1;
            btnEjecutarMotor.Text = "\U0001f9e0 Ejecutar Motor Analítico";
            btnEjecutarMotor.UseVisualStyleBackColor = false;
            // 
            // dgvOfertas
            // 
            dgvOfertas.AllowUserToAddRows = false;
            dgvOfertas.AllowUserToDeleteRows = false;
            dgvOfertas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOfertas.Location = new Point(20, 120);
            dgvOfertas.Name = "dgvOfertas";
            dgvOfertas.ReadOnly = true;
            dgvOfertas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOfertas.Size = new Size(850, 250);
            dgvOfertas.TabIndex = 2;
            // 
            // panelEdicion
            // 
            panelEdicion.BorderStyle = BorderStyle.FixedSingle;
            panelEdicion.Controls.Add(btnGestionarOfertas);
            panelEdicion.Controls.Add(btnDescartar);
            panelEdicion.Controls.Add(btnAprobar);
            panelEdicion.Controls.Add(lblSimbolo);
            panelEdicion.Controls.Add(numDescuento);
            panelEdicion.Controls.Add(lblPropuestaDetalle);
            panelEdicion.Controls.Add(lblTituloAjuste);
            panelEdicion.Location = new Point(20, 390);
            panelEdicion.Name = "panelEdicion";
            panelEdicion.Size = new Size(850, 120);
            panelEdicion.TabIndex = 3;
            // 
            // btnDescartar
            // 
            btnDescartar.BackColor = Color.LightCoral;
            btnDescartar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDescartar.Location = new Point(460, 65);
            btnDescartar.Name = "btnDescartar";
            btnDescartar.Size = new Size(150, 40);
            btnDescartar.TabIndex = 5;
            btnDescartar.Text = "❌ Descartar";
            btnDescartar.UseVisualStyleBackColor = false;
            // 
            // btnAprobar
            // 
            btnAprobar.BackColor = Color.LightGreen;
            btnAprobar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAprobar.Location = new Point(620, 65);
            btnAprobar.Name = "btnAprobar";
            btnAprobar.Size = new Size(210, 40);
            btnAprobar.TabIndex = 4;
            btnAprobar.Text = "✅ Aprobar Oferta";
            btnAprobar.UseVisualStyleBackColor = false;
            btnAprobar.Click += BtnAprobar_Click;
            // 
            // lblSimbolo
            // 
            lblSimbolo.AutoSize = true;
            lblSimbolo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSimbolo.Location = new Point(100, 78);
            lblSimbolo.Name = "lblSimbolo";
            lblSimbolo.Size = new Size(56, 21);
            lblSimbolo.TabIndex = 3;
            lblSimbolo.Text = "% OFF";
            // 
            // numDescuento
            // 
            numDescuento.DecimalPlaces = 2;
            numDescuento.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            numDescuento.Location = new Point(15, 75);
            numDescuento.Name = "numDescuento";
            numDescuento.Size = new Size(80, 29);
            numDescuento.TabIndex = 2;
            // 
            // lblPropuestaDetalle
            // 
            lblPropuestaDetalle.AutoSize = true;
            lblPropuestaDetalle.Location = new Point(15, 45);
            lblPropuestaDetalle.Name = "lblPropuestaDetalle";
            lblPropuestaDetalle.Size = new Size(208, 15);
            lblPropuestaDetalle.TabIndex = 1;
            lblPropuestaDetalle.Text = "Seleccione una propuesta de la grilla...";
            // 
            // lblTituloAjuste
            // 
            lblTituloAjuste.AutoSize = true;
            lblTituloAjuste.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTituloAjuste.Location = new Point(15, 15);
            lblTituloAjuste.Name = "lblTituloAjuste";
            lblTituloAjuste.Size = new Size(187, 15);
            lblTituloAjuste.TabIndex = 0;
            lblTituloAjuste.Text = "Ajustar Decisión de la Propuesta:";
            // 
            // lblTituloUI
            // 
            lblTituloUI.AutoSize = true;
            lblTituloUI.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTituloUI.Location = new Point(20, 20);
            lblTituloUI.Name = "lblTituloUI";
            lblTituloUI.Size = new Size(338, 25);
            lblTituloUI.TabIndex = 0;
            lblTituloUI.Text = "Bandeja de Recomendaciones (FEFO)";
            // 
            // btnGestionarOfertas
            // 
            btnGestionarOfertas.Location = new Point(620, 8);
            btnGestionarOfertas.Name = "btnGestionarOfertas";
            btnGestionarOfertas.Size = new Size(210, 23);
            btnGestionarOfertas.TabIndex = 6;
            btnGestionarOfertas.Text = "Gestionar Ofertas";
            btnGestionarOfertas.UseVisualStyleBackColor = true;
            btnGestionarOfertas.Click += btnGestionarOfertas_Click;
            // 
            // GestionOfertasInteligentesUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(890, 530);
            Controls.Add(panelEdicion);
            Controls.Add(dgvOfertas);
            Controls.Add(btnEjecutarMotor);
            Controls.Add(lblTituloUI);
            Name = "GestionOfertasInteligentesUI";
            Text = "Gestor de Promociones Inteligentes";
            ((System.ComponentModel.ISupportInitialize)dgvOfertas).EndInit();
            panelEdicion.ResumeLayout(false);
            panelEdicion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDescuento).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTituloUI;
        private System.Windows.Forms.Button btnEjecutarMotor;
        private System.Windows.Forms.DataGridView dgvOfertas;
        private System.Windows.Forms.Panel panelEdicion;
        private System.Windows.Forms.Label lblPropuestaDetalle;
        private System.Windows.Forms.Label lblTituloAjuste;
        private System.Windows.Forms.Button btnDescartar;
        private System.Windows.Forms.Button btnAprobar;
        private System.Windows.Forms.Label lblSimbolo;
        private System.Windows.Forms.NumericUpDown numDescuento;
        private Button btnGestionarOfertas;
    }
}