namespace UI.Modules
{
    partial class BloqueoUsuariosUI
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
            dataGridViewBloqueados = new DataGridView();
            btnDesbloquear = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBloqueados).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewBloqueados
            // 
            dataGridViewBloqueados.AllowUserToAddRows = false;
            dataGridViewBloqueados.AllowUserToDeleteRows = false;
            dataGridViewBloqueados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewBloqueados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewBloqueados.Location = new Point(129, 83);
            dataGridViewBloqueados.Name = "dataGridViewBloqueados";
            dataGridViewBloqueados.ReadOnly = true;
            dataGridViewBloqueados.Size = new Size(462, 265);
            dataGridViewBloqueados.TabIndex = 0;
            // 
            // btnDesbloquear
            // 
            btnDesbloquear.Location = new Point(613, 110);
            btnDesbloquear.Name = "btnDesbloquear";
            btnDesbloquear.Size = new Size(75, 23);
            btnDesbloquear.TabIndex = 1;
            btnDesbloquear.Text = "Desbloquear";
            btnDesbloquear.UseVisualStyleBackColor = true;
            // 
            // BloqueoUsuariosUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            ControlBox = false;
            Controls.Add(btnDesbloquear);
            Controls.Add(dataGridViewBloqueados);
            FormBorderStyle = FormBorderStyle.None;
            Name = "BloqueoUsuariosUI";
            Text = "BloqueoUsuariosUI";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)dataGridViewBloqueados).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewBloqueados;
        private Button btnDesbloquear;
    }
}