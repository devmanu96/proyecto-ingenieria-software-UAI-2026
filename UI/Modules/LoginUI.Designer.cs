namespace UI.Login
{
    partial class LoginUI
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
            loginUILabelUsername = new Label();
            loginUILabelContrasena = new Label();
            textBoxUsername = new TextBox();
            textBoxContrasena = new TextBox();
            loginUIButtonIniciarSesion = new Button();
            SuspendLayout();
            // 
            // loginUILabelUsername
            // 
            loginUILabelUsername.AutoSize = true;
            loginUILabelUsername.Location = new Point(69, 73);
            loginUILabelUsername.Name = "loginUILabelUsername";
            loginUILabelUsername.Size = new Size(60, 15);
            loginUILabelUsername.TabIndex = 0;
            loginUILabelUsername.Text = "Username";
            // 
            // loginUILabelContrasena
            // 
            loginUILabelContrasena.AutoSize = true;
            loginUILabelContrasena.Location = new Point(69, 111);
            loginUILabelContrasena.Name = "loginUILabelContrasena";
            loginUILabelContrasena.Size = new Size(67, 15);
            loginUILabelContrasena.TabIndex = 1;
            loginUILabelContrasena.Text = "Contraseña";
            // 
            // textBoxUsername
            // 
            textBoxUsername.Location = new Point(165, 65);
            textBoxUsername.Name = "textBoxUsername";
            textBoxUsername.Size = new Size(209, 23);
            textBoxUsername.TabIndex = 2;
            // 
            // textBoxContrasena
            // 
            textBoxContrasena.Location = new Point(165, 108);
            textBoxContrasena.Name = "textBoxContrasena";
            textBoxContrasena.Size = new Size(209, 23);
            textBoxContrasena.TabIndex = 3;
            textBoxContrasena.UseSystemPasswordChar = true;
            // 
            // loginUIButtonIniciarSesion
            // 
            loginUIButtonIniciarSesion.Location = new Point(213, 158);
            loginUIButtonIniciarSesion.Name = "loginUIButtonIniciarSesion";
            loginUIButtonIniciarSesion.Size = new Size(92, 37);
            loginUIButtonIniciarSesion.TabIndex = 4;
            loginUIButtonIniciarSesion.Text = "Iniciar Sesión";
            loginUIButtonIniciarSesion.UseVisualStyleBackColor = true;
            loginUIButtonIniciarSesion.Click += loginUIButtonIniciarSesion_Click;
            // 
            // LoginUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 244);
            ControlBox = false;
            Controls.Add(loginUIButtonIniciarSesion);
            Controls.Add(textBoxContrasena);
            Controls.Add(textBoxUsername);
            Controls.Add(loginUILabelContrasena);
            Controls.Add(loginUILabelUsername);
            FormBorderStyle = FormBorderStyle.None;
            Name = "LoginUI";
            Text = "LoginUI";
            WindowState = FormWindowState.Maximized;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label loginUILabelUsername;
        private Label loginUILabelContrasena;
        private TextBox textBoxUsername;
        private TextBox textBoxContrasena;
        private Button loginUIButtonIniciarSesion;
    }
}