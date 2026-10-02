namespace UI.Venta
{
    partial class FormTransferenciaUI
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
            lblMonto = new Label();
            lblBanco = new Label();
            lblComprobante = new Label();
            txtMonto = new TextBox();
            cmbBanco = new ComboBox();
            btnCancelar = new Button();
            btnAceptar = new Button();
            SuspendLayout();
            // 
            // lblMonto
            // 
            lblMonto.AutoSize = true;
            lblMonto.Location = new Point(80, 76);
            lblMonto.Name = "lblMonto";
            lblMonto.Size = new Size(92, 15);
            lblMonto.TabIndex = 0;
            lblMonto.Text = "Monto a cobrar:";
            // 
            // lblBanco
            // 
            lblBanco.AutoSize = true;
            lblBanco.Location = new Point(80, 165);
            lblBanco.Name = "lblBanco";
            lblBanco.Size = new Size(96, 15);
            lblBanco.TabIndex = 1;
            lblBanco.Text = "Banco / Billetera:";
            // 
            // lblComprobante
            // 
            lblComprobante.AutoSize = true;
            lblComprobante.Location = new Point(80, 250);
            lblComprobante.Name = "lblComprobante";
            lblComprobante.Size = new Size(110, 15);
            lblComprobante.TabIndex = 2;
            lblComprobante.Text = "Nro. Comprobante:";
            // 
            // txtMonto
            // 
            txtMonto.Location = new Point(80, 106);
            txtMonto.Multiline = true;
            txtMonto.Name = "txtMonto";
            txtMonto.Size = new Size(292, 31);
            txtMonto.TabIndex = 3;
            // 
            // cmbBanco
            // 
            cmbBanco.FormattingEnabled = true;
            cmbBanco.Location = new Point(80, 207);
            cmbBanco.Name = "cmbBanco";
            cmbBanco.Size = new Size(113, 23);
            cmbBanco.TabIndex = 4;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(80, 305);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(109, 33);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(230, 305);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(109, 33);
            btnAceptar.TabIndex = 6;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            // 
            // FormTransferenciaUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(469, 450);
            Controls.Add(btnAceptar);
            Controls.Add(btnCancelar);
            Controls.Add(cmbBanco);
            Controls.Add(txtMonto);
            Controls.Add(lblComprobante);
            Controls.Add(lblBanco);
            Controls.Add(lblMonto);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormTransferenciaUI";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FormTransferenciaUI";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMonto;
        private Label lblBanco;
        private Label lblComprobante;
        private TextBox txtMonto;
        private ComboBox cmbBanco;
        private Button btnCancelar;
        private Button btnAceptar;
    }
}