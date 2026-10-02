namespace UI.Venta
{
    partial class FormMockTarjetaUI
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
            lblMonto = new Label();
            txtMonto = new TextBox();
            lblTitular = new Label();
            txtTitular = new TextBox();
            lblNumero = new Label();
            txtNumero = new TextBox();
            lblVencimiento = new Label();
            txtVencimiento = new MaskedTextBox();
            lblCVV = new Label();
            txtCVV = new TextBox();
            grpTipo = new GroupBox();
            rbCredito = new RadioButton();
            rbDebito = new RadioButton();
            lblCuotas = new Label();
            cmbCuotas = new ComboBox();
            btnAceptar = new Button();
            btnCancelar = new Button();
            grpTipo.SuspendLayout();
            SuspendLayout();
            // 
            // lblMonto
            // 
            lblMonto.AutoSize = true;
            lblMonto.Location = new Point(23, 24);
            lblMonto.Name = "lblMonto";
            lblMonto.Size = new Size(95, 15);
            lblMonto.TabIndex = 0;
            lblMonto.Text = "Monto a cobrar:";
            // 
            // txtMonto
            // 
            txtMonto.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            txtMonto.Location = new Point(138, 16);
            txtMonto.Name = "txtMonto";
            txtMonto.ReadOnly = true;
            txtMonto.Size = new Size(160, 29);
            txtMonto.TabIndex = 1;
            // 
            // lblTitular
            // 
            lblTitular.AutoSize = true;
            lblTitular.Location = new Point(23, 73);
            lblTitular.Name = "lblTitular";
            lblTitular.Size = new Size(110, 15);
            lblTitular.TabIndex = 2;
            lblTitular.Text = "Nombre del Titular:";
            // 
            // txtTitular
            // 
            txtTitular.Location = new Point(138, 70);
            txtTitular.Name = "txtTitular";
            txtTitular.Size = new Size(160, 23);
            txtTitular.TabIndex = 3;
            // 
            // lblNumero
            // 
            lblNumero.AutoSize = true;
            lblNumero.Location = new Point(23, 113);
            lblNumero.Name = "lblNumero";
            lblNumero.Size = new Size(109, 15);
            lblNumero.TabIndex = 4;
            lblNumero.Text = "Número de Tarjeta:";
            // 
            // txtNumero
            // 
            txtNumero.Location = new Point(138, 110);
            txtNumero.MaxLength = 16;
            txtNumero.Name = "txtNumero";
            txtNumero.Size = new Size(160, 23);
            txtNumero.TabIndex = 5;
            // 
            // lblVencimiento
            // 
            lblVencimiento.AutoSize = true;
            lblVencimiento.Location = new Point(23, 153);
            lblVencimiento.Name = "lblVencimiento";
            lblVencimiento.Size = new Size(76, 15);
            lblVencimiento.TabIndex = 6;
            lblVencimiento.Text = "Vencimiento:";
            // 
            // txtVencimiento
            // 
            txtVencimiento.Location = new Point(138, 150);
            txtVencimiento.Mask = "00/00";
            txtVencimiento.Name = "txtVencimiento";
            txtVencimiento.Size = new Size(54, 23);
            txtVencimiento.TabIndex = 7;
            txtVencimiento.ValidatingType = typeof(DateTime);
            // 
            // lblCVV
            // 
            lblCVV.AutoSize = true;
            lblCVV.Location = new Point(207, 153);
            lblCVV.Name = "lblCVV";
            lblCVV.Size = new Size(32, 15);
            lblCVV.TabIndex = 8;
            lblCVV.Text = "CVV:";
            // 
            // txtCVV
            // 
            txtCVV.Location = new Point(245, 150);
            txtCVV.MaxLength = 4;
            txtCVV.Name = "txtCVV";
            txtCVV.PasswordChar = '*';
            txtCVV.Size = new Size(53, 23);
            txtCVV.TabIndex = 9;
            // 
            // grpTipo
            // 
            grpTipo.Controls.Add(rbCredito);
            grpTipo.Controls.Add(rbDebito);
            grpTipo.Location = new Point(23, 190);
            grpTipo.Name = "grpTipo";
            grpTipo.Size = new Size(169, 58);
            grpTipo.TabIndex = 10;
            grpTipo.TabStop = false;
            grpTipo.Text = "Tipo de Tarjeta";
            // 
            // rbCredito
            // 
            rbCredito.AutoSize = true;
            rbCredito.Location = new Point(87, 22);
            rbCredito.Name = "rbCredito";
            rbCredito.Size = new Size(64, 19);
            rbCredito.TabIndex = 1;
            rbCredito.TabStop = true;
            rbCredito.Text = "Crédito";
            rbCredito.UseVisualStyleBackColor = true;
            // 
            // rbDebito
            // 
            rbDebito.AutoSize = true;
            rbDebito.Location = new Point(11, 22);
            rbDebito.Name = "rbDebito";
            rbDebito.Size = new Size(61, 19);
            rbDebito.TabIndex = 0;
            rbDebito.TabStop = true;
            rbDebito.Text = "Débito";
            rbDebito.UseVisualStyleBackColor = true;
            // 
            // lblCuotas
            // 
            lblCuotas.AutoSize = true;
            lblCuotas.Location = new Point(207, 203);
            lblCuotas.Name = "lblCuotas";
            lblCuotas.Size = new Size(47, 15);
            lblCuotas.TabIndex = 11;
            lblCuotas.Text = "Cuotas:";
            // 
            // cmbCuotas
            // 
            cmbCuotas.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCuotas.FormattingEnabled = true;
            cmbCuotas.Location = new Point(255, 200);
            cmbCuotas.Name = "cmbCuotas";
            cmbCuotas.Size = new Size(43, 23);
            cmbCuotas.TabIndex = 12;
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(138, 269);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(160, 36);
            btnAceptar.TabIndex = 13;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(23, 269);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(95, 36);
            btnCancelar.TabIndex = 14;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // FormMockTarjetaUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(328, 327);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(cmbCuotas);
            Controls.Add(lblCuotas);
            Controls.Add(grpTipo);
            Controls.Add(txtCVV);
            Controls.Add(lblCVV);
            Controls.Add(txtVencimiento);
            Controls.Add(lblVencimiento);
            Controls.Add(txtNumero);
            Controls.Add(lblNumero);
            Controls.Add(txtTitular);
            Controls.Add(lblTitular);
            Controls.Add(txtMonto);
            Controls.Add(lblMonto);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormMockTarjetaUI";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Pago con Tarjeta";
            grpTipo.ResumeLayout(false);
            grpTipo.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMonto;
        private TextBox txtMonto;
        private Label lblTitular;
        private TextBox txtTitular;
        private Label lblNumero;
        private TextBox txtNumero;
        private Label lblVencimiento;
        private MaskedTextBox txtVencimiento;
        private Label lblCVV;
        private TextBox txtCVV;
        private GroupBox grpTipo;
        private RadioButton rbCredito;
        private RadioButton rbDebito;
        private Label lblCuotas;
        private ComboBox cmbCuotas;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}