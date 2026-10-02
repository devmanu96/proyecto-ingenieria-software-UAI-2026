namespace UI.Venta
{
    partial class FormCalculadoraEfectivoUI
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
            lblSaldo = new Label();
            txtIngreso = new TextBox();
            btn7 = new Button();
            btn8 = new Button();
            btn9 = new Button();
            btn4 = new Button();
            btn5 = new Button();
            btn6 = new Button();
            btn1 = new Button();
            btn2 = new Button();
            btn3 = new Button();
            btn0 = new Button();
            btnComa = new Button();
            btnBorrar = new Button();
            btnAceptar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // lblSaldo
            // 
            lblSaldo.AutoSize = true;
            lblSaldo.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            lblSaldo.Location = new Point(22, 18);
            lblSaldo.Name = "lblSaldo";
            lblSaldo.Size = new Size(135, 20);
            lblSaldo.TabIndex = 0;
            lblSaldo.Text = "Saldo a cubrir: $0";
            // 
            // txtIngreso
            // 
            txtIngreso.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            txtIngreso.Location = new Point(22, 52);
            txtIngreso.Name = "txtIngreso";
            txtIngreso.ReadOnly = true;
            txtIngreso.Size = new Size(246, 50);
            txtIngreso.TabIndex = 1;
            txtIngreso.Text = "0";
            txtIngreso.TextAlign = HorizontalAlignment.Right;
            // 
            // Teclado Numérico
            // 
            btn7.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btn7.Location = new Point(22, 118);
            btn7.Name = "btn7";
            btn7.Size = new Size(60, 60);
            btn7.TabIndex = 2;
            btn7.Text = "7";
            // 
            btn8.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btn8.Location = new Point(88, 118);
            btn8.Name = "btn8";
            btn8.Size = new Size(60, 60);
            btn8.TabIndex = 3;
            btn8.Text = "8";
            // 
            btn9.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btn9.Location = new Point(154, 118);
            btn9.Name = "btn9";
            btn9.Size = new Size(60, 60);
            btn9.TabIndex = 4;
            btn9.Text = "9";
            // 
            btn4.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btn4.Location = new Point(22, 184);
            btn4.Name = "btn4";
            btn4.Size = new Size(60, 60);
            btn4.TabIndex = 5;
            btn4.Text = "4";
            // 
            btn5.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btn5.Location = new Point(88, 184);
            btn5.Name = "btn5";
            btn5.Size = new Size(60, 60);
            btn5.TabIndex = 6;
            btn5.Text = "5";
            // 
            btn6.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btn6.Location = new Point(154, 184);
            btn6.Name = "btn6";
            btn6.Size = new Size(60, 60);
            btn6.TabIndex = 7;
            btn6.Text = "6";
            // 
            btn1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btn1.Location = new Point(22, 250);
            btn1.Name = "btn1";
            btn1.Size = new Size(60, 60);
            btn1.TabIndex = 8;
            btn1.Text = "1";
            // 
            btn2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btn2.Location = new Point(88, 250);
            btn2.Name = "btn2";
            btn2.Size = new Size(60, 60);
            btn2.TabIndex = 9;
            btn2.Text = "2";
            // 
            btn3.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btn3.Location = new Point(154, 250);
            btn3.Name = "btn3";
            btn3.Size = new Size(60, 60);
            btn3.TabIndex = 10;
            btn3.Text = "3";
            // 
            btn0.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btn0.Location = new Point(22, 316);
            btn0.Name = "btn0";
            btn0.Size = new Size(126, 60);
            btn0.TabIndex = 11;
            btn0.Text = "0";
            // 
            btnComa.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            btnComa.Location = new Point(154, 316);
            btnComa.Name = "btnComa";
            btnComa.Size = new Size(60, 60);
            btnComa.TabIndex = 12;
            btnComa.Text = ",";
            // 
            // Acciones
            // 
            btnBorrar.BackColor = Color.LightCoral;
            btnBorrar.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btnBorrar.Location = new Point(220, 118);
            btnBorrar.Name = "btnBorrar";
            btnBorrar.Size = new Size(48, 60);
            btnBorrar.TabIndex = 13;
            btnBorrar.Text = "C";
            btnBorrar.UseVisualStyleBackColor = false;
            // 
            btnAceptar.BackColor = Color.LightGreen;
            btnAceptar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnAceptar.Location = new Point(220, 184);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(48, 192);
            btnAceptar.TabIndex = 14;
            btnAceptar.Text = "OK";
            btnAceptar.UseVisualStyleBackColor = false;
            // 
            btnCancelar.Location = new Point(22, 386);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(246, 36);
            btnCancelar.TabIndex = 15;
            btnCancelar.Text = "Cancelar";
            // 
            // FormCalculadoraEfectivoUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(292, 439);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(btnBorrar);
            Controls.Add(btnComa);
            Controls.Add(btn0);
            Controls.Add(btn3);
            Controls.Add(btn2);
            Controls.Add(btn1);
            Controls.Add(btn6);
            Controls.Add(btn5);
            Controls.Add(btn4);
            Controls.Add(btn9);
            Controls.Add(btn8);
            Controls.Add(btn7);
            Controls.Add(txtIngreso);
            Controls.Add(lblSaldo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCalculadoraEfectivoUI";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Efectivo";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSaldo;
        private TextBox txtIngreso;
        private Button btn7;
        private Button btn8;
        private Button btn9;
        private Button btn4;
        private Button btn5;
        private Button btn6;
        private Button btn1;
        private Button btn2;
        private Button btn3;
        private Button btn0;
        private Button btnComa;
        private Button btnBorrar;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}