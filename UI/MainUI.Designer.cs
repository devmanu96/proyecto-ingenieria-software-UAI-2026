namespace UI
{
    partial class MainUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            mainUIStripMenuItemInicio = new ToolStripMenuItem();
            mainUIStripMenuItemIniciarSesion = new ToolStripMenuItem();
            mainUIStripMenuItemCerrarSesion = new ToolStripMenuItem();
            mainUIStripMenuItemGestionDeUsuarios = new ToolStripMenuItem();
            mainUIStripMenuItemABMUsuarios = new ToolStripMenuItem();
            mainUIStripMenuItemDesbloqueoUsuarios = new ToolStripMenuItem();
            mainUIStripMenuItemGestionDePerfiles = new ToolStripMenuItem();
            mainUIStripMenuItemABMPerfiles = new ToolStripMenuItem();
            mainUIStripMenuItemBitacora = new ToolStripMenuItem();
            mainUIStripMenuItemConsultarBitacora = new ToolStripMenuItem();
            mainUIStripMenuItemHistorialUsuario = new ToolStripMenuItem();
            agregarIdiomaToolStripMenuItem = new ToolStripMenuItem();
            recuperarIntegridadToolStripMenuItem = new ToolStripMenuItem();
            menuStripItemAlmacen = new ToolStripMenuItem();
            almacenToolStripMenuItemGenerarSolicitud = new ToolStripMenuItem();
            menuStripItemCompras = new ToolStripMenuItem();
            comprasToolStripMenuItemEmitirOC = new ToolStripMenuItem();
            menuStripItemContabilidad = new ToolStripMenuItem();
            contabilidadToolStripMenuItemEvaluarCotiz = new ToolStripMenuItem();
            comboIdiomasGlobal = new ComboBox();
            label1 = new Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { mainUIStripMenuItemInicio, mainUIStripMenuItemGestionDeUsuarios, mainUIStripMenuItemGestionDePerfiles, mainUIStripMenuItemBitacora, mainUIStripMenuItemHistorialUsuario, agregarIdiomaToolStripMenuItem, recuperarIntegridadToolStripMenuItem, menuStripItemAlmacen, menuStripItemCompras, menuStripItemContabilidad });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1140, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // mainUIStripMenuItemInicio
            // 
            mainUIStripMenuItemInicio.DropDownItems.AddRange(new ToolStripItem[] { mainUIStripMenuItemIniciarSesion, mainUIStripMenuItemCerrarSesion });
            mainUIStripMenuItemInicio.Name = "mainUIStripMenuItemInicio";
            mainUIStripMenuItemInicio.Size = new Size(48, 20);
            mainUIStripMenuItemInicio.Text = "Inicio";
            // 
            // mainUIStripMenuItemIniciarSesion
            // 
            mainUIStripMenuItemIniciarSesion.Name = "mainUIStripMenuItemIniciarSesion";
            mainUIStripMenuItemIniciarSesion.Size = new Size(142, 22);
            mainUIStripMenuItemIniciarSesion.Text = "Iniciar sesion";
            mainUIStripMenuItemIniciarSesion.Click += mainUIStripMenuItemIniciarSesion_Click;
            // 
            // mainUIStripMenuItemCerrarSesion
            // 
            mainUIStripMenuItemCerrarSesion.Name = "mainUIStripMenuItemCerrarSesion";
            mainUIStripMenuItemCerrarSesion.Size = new Size(142, 22);
            mainUIStripMenuItemCerrarSesion.Text = "Cerrar sesion";
            mainUIStripMenuItemCerrarSesion.Click += mainUIStripMenuItemCerrarSesion_Click;
            // 
            // mainUIStripMenuItemGestionDeUsuarios
            // 
            mainUIStripMenuItemGestionDeUsuarios.DropDownItems.AddRange(new ToolStripItem[] { mainUIStripMenuItemABMUsuarios, mainUIStripMenuItemDesbloqueoUsuarios });
            mainUIStripMenuItemGestionDeUsuarios.Name = "mainUIStripMenuItemGestionDeUsuarios";
            mainUIStripMenuItemGestionDeUsuarios.Size = new Size(122, 20);
            mainUIStripMenuItemGestionDeUsuarios.Text = "Gestión de usuarios";
            // 
            // mainUIStripMenuItemABMUsuarios
            // 
            mainUIStripMenuItemABMUsuarios.Name = "mainUIStripMenuItemABMUsuarios";
            mainUIStripMenuItemABMUsuarios.Size = new Size(200, 22);
            mainUIStripMenuItemABMUsuarios.Text = "ABM Usuarios";
            mainUIStripMenuItemABMUsuarios.Click += mainUIStripMenuItemABMUsuarios_Click;
            // 
            // mainUIStripMenuItemDesbloqueoUsuarios
            // 
            mainUIStripMenuItemDesbloqueoUsuarios.Name = "mainUIStripMenuItemDesbloqueoUsuarios";
            mainUIStripMenuItemDesbloqueoUsuarios.Size = new Size(200, 22);
            mainUIStripMenuItemDesbloqueoUsuarios.Text = "Desbloqueo de usuarios";
            mainUIStripMenuItemDesbloqueoUsuarios.Click += mainUIStripMenuItemDesbloqueoUsuarios_Click;
            // 
            // mainUIStripMenuItemGestionDePerfiles
            // 
            mainUIStripMenuItemGestionDePerfiles.DropDownItems.AddRange(new ToolStripItem[] { mainUIStripMenuItemABMPerfiles });
            mainUIStripMenuItemGestionDePerfiles.Name = "mainUIStripMenuItemGestionDePerfiles";
            mainUIStripMenuItemGestionDePerfiles.Size = new Size(116, 20);
            mainUIStripMenuItemGestionDePerfiles.Text = "Gestión de perfiles";
            // 
            // mainUIStripMenuItemABMPerfiles
            // 
            mainUIStripMenuItemABMPerfiles.Name = "mainUIStripMenuItemABMPerfiles";
            mainUIStripMenuItemABMPerfiles.Size = new Size(221, 22);
            mainUIStripMenuItemABMPerfiles.Text = "Alta y asignación de perfiles";
            mainUIStripMenuItemABMPerfiles.Click += mainUIStripMenuItemABMPerfiles_Click;
            // 
            // mainUIStripMenuItemBitacora
            // 
            mainUIStripMenuItemBitacora.DropDownItems.AddRange(new ToolStripItem[] { mainUIStripMenuItemConsultarBitacora });
            mainUIStripMenuItemBitacora.Name = "mainUIStripMenuItemBitacora";
            mainUIStripMenuItemBitacora.Size = new Size(62, 20);
            mainUIStripMenuItemBitacora.Text = "Bitacora";
            // 
            // mainUIStripMenuItemConsultarBitacora
            // 
            mainUIStripMenuItemConsultarBitacora.Name = "mainUIStripMenuItemConsultarBitacora";
            mainUIStripMenuItemConsultarBitacora.Size = new Size(171, 22);
            mainUIStripMenuItemConsultarBitacora.Text = "Consultar bitacora";
            mainUIStripMenuItemConsultarBitacora.Click += mainUIStripMenuItemConsultarBitacora_Click;
            // 
            // mainUIStripMenuItemHistorialUsuario
            // 
            mainUIStripMenuItemHistorialUsuario.Name = "mainUIStripMenuItemHistorialUsuario";
            mainUIStripMenuItemHistorialUsuario.Size = new Size(105, 20);
            mainUIStripMenuItemHistorialUsuario.Text = "Historial usuario";
            mainUIStripMenuItemHistorialUsuario.Click += mainUIStripMenuItemHistorialUsuario_Click;
            // 
            // agregarIdiomaToolStripMenuItem
            // 
            agregarIdiomaToolStripMenuItem.Name = "agregarIdiomaToolStripMenuItem";
            agregarIdiomaToolStripMenuItem.Size = new Size(101, 20);
            agregarIdiomaToolStripMenuItem.Text = "Agregar Idioma";
            agregarIdiomaToolStripMenuItem.Click += agregarIdiomaToolStripMenuItem_Click;
            // 
            // recuperarIntegridadToolStripMenuItem
            // 
            recuperarIntegridadToolStripMenuItem.Name = "recuperarIntegridadToolStripMenuItem";
            recuperarIntegridadToolStripMenuItem.Size = new Size(129, 20);
            recuperarIntegridadToolStripMenuItem.Text = "Recuperar integridad";
            recuperarIntegridadToolStripMenuItem.Click += recuperarIntegridadToolStripMenuItem_Click;
            // 
            // menuStripItemAlmacen
            // 
            menuStripItemAlmacen.DropDownItems.AddRange(new ToolStripItem[] { almacenToolStripMenuItemGenerarSolicitud });
            menuStripItemAlmacen.Name = "menuStripItemAlmacen";
            menuStripItemAlmacen.Size = new Size(66, 20);
            menuStripItemAlmacen.Text = "Almacen";
            // 
            // almacenToolStripMenuItemGenerarSolicitud
            // 
            almacenToolStripMenuItemGenerarSolicitud.Name = "almacenToolStripMenuItemGenerarSolicitud";
            almacenToolStripMenuItemGenerarSolicitud.Size = new Size(266, 22);
            almacenToolStripMenuItemGenerarSolicitud.Text = "Generar Solicitud de Abastecimiento";
            almacenToolStripMenuItemGenerarSolicitud.Click += almacenToolStripMenuItemGenerarSolicitud_Click;
            // 
            // menuStripItemCompras
            // 
            menuStripItemCompras.DropDownItems.AddRange(new ToolStripItem[] { comprasToolStripMenuItemEmitirOC });
            menuStripItemCompras.Name = "menuStripItemCompras";
            menuStripItemCompras.Size = new Size(67, 20);
            menuStripItemCompras.Text = "Compras";
            // 
            // comprasToolStripMenuItemEmitirOC
            // 
            comprasToolStripMenuItemEmitirOC.Name = "comprasToolStripMenuItemEmitirOC";
            comprasToolStripMenuItemEmitirOC.Size = new Size(180, 22);
            comprasToolStripMenuItemEmitirOC.Text = "Orden de Compra";
            comprasToolStripMenuItemEmitirOC.Click += comprasToolStripMenuItemEmitirOC_Click;
            // 
            // menuStripItemContabilidad
            // 
            menuStripItemContabilidad.DropDownItems.AddRange(new ToolStripItem[] { contabilidadToolStripMenuItemEvaluarCotiz });
            menuStripItemContabilidad.Name = "menuStripItemContabilidad";
            menuStripItemContabilidad.Size = new Size(87, 20);
            menuStripItemContabilidad.Text = "Contabilidad";
            // 
            // contabilidadToolStripMenuItemEvaluarCotiz
            // 
            contabilidadToolStripMenuItemEvaluarCotiz.Name = "contabilidadToolStripMenuItemEvaluarCotiz";
            contabilidadToolStripMenuItemEvaluarCotiz.Size = new Size(171, 22);
            contabilidadToolStripMenuItemEvaluarCotiz.Text = "Evaluar Cotizacion";
            // 
            // comboIdiomasGlobal
            // 
            comboIdiomasGlobal.FormattingEnabled = true;
            comboIdiomasGlobal.Location = new Point(1064, 60);
            comboIdiomasGlobal.Name = "comboIdiomasGlobal";
            comboIdiomasGlobal.Size = new Size(63, 23);
            comboIdiomasGlobal.TabIndex = 1;
            comboIdiomasGlobal.SelectedIndexChanged += ComboIdiomasGlobal_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1064, 42);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 2;
            label1.Text = "Idioma";
            // 
            // MainUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1140, 564);
            Controls.Add(label1);
            Controls.Add(comboIdiomasGlobal);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainUI";
            Text = "Sistema de gestion";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem mainUIStripMenuItemInicio;
        private ToolStripMenuItem mainUIStripMenuItemIniciarSesion;
        private ToolStripMenuItem mainUIStripMenuItemCerrarSesion;
        private ToolStripMenuItem mainUIStripMenuItemGestionDeUsuarios;
        private ToolStripMenuItem mainUIStripMenuItemABMUsuarios;
        private ToolStripMenuItem mainUIStripMenuItemDesbloqueoUsuarios;
        private ToolStripMenuItem mainUIStripMenuItemGestionDePerfiles;
        private ToolStripMenuItem mainUIStripMenuItemABMPerfiles;
        private ToolStripMenuItem mainUIStripMenuItemBitacora;
        private ToolStripMenuItem mainUIStripMenuItemConsultarBitacora;
        private ComboBox comboIdiomasGlobal;
        private Label label1;
        private ToolStripMenuItem mainUIStripMenuItemHistorialUsuario;
        private ToolStripMenuItem agregarIdiomaToolStripMenuItem;
        private ToolStripMenuItem recuperarIntegridadToolStripMenuItem;
        private ToolStripMenuItem menuStripItemAlmacen;
        private ToolStripMenuItem menuStripItemCompras;
        private ToolStripMenuItem menuStripItemContabilidad;
        private ToolStripMenuItem almacenToolStripMenuItemGenerarSolicitud;
        private ToolStripMenuItem comprasToolStripMenuItemEmitirOC;
        private ToolStripMenuItem contabilidadToolStripMenuItemEvaluarCotiz;
    }
}
