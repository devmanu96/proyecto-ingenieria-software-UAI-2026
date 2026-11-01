namespace UI
{
    partial class MainUI
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
            gestionarProductosToolStripMenuItem = new ToolStripMenuItem();
            menuStripItemCompras = new ToolStripMenuItem();
            comprasToolStripMenuItemEmitirOC = new ToolStripMenuItem();
            recepcionDeMercaderiaToolStripMenuItem = new ToolStripMenuItem();
            menuStripItemContabilidad = new ToolStripMenuItem();
            contabilidadToolStripMenuItemEvaluarCotiz = new ToolStripMenuItem();
            ventasToolStripMenuItem = new ToolStripMenuItem();
            realizarVentaToolStripMenuItem = new ToolStripMenuItem();
            historialDeVentasToolStripMenuItem = new ToolStripMenuItem();
            historialDeTicketsPDFToolStripMenuItem = new ToolStripMenuItem();
            administracionToolStripMenuItem = new ToolStripMenuItem();
            gestionOfertasToolStripMenuItem = new ToolStripMenuItem();
            gestionProveedoresToolStripMenuItem = new ToolStripMenuItem();
            comboIdiomasGlobal = new ComboBox();
            label1 = new Label();
            pnlHeader = new Panel();
            lblTituloLogo = new Label();
            menuStrip1.SuspendLayout();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.FromArgb(38, 55, 70); // Azul Grafito
            menuStrip1.ForeColor = Color.White;
            menuStrip1.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            menuStrip1.Items.AddRange(new ToolStripItem[] { mainUIStripMenuItemInicio, mainUIStripMenuItemGestionDeUsuarios, mainUIStripMenuItemGestionDePerfiles, mainUIStripMenuItemBitacora, mainUIStripMenuItemHistorialUsuario, agregarIdiomaToolStripMenuItem, recuperarIntegridadToolStripMenuItem, menuStripItemAlmacen, menuStripItemCompras, menuStripItemContabilidad, ventasToolStripMenuItem, administracionToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(6, 4, 0, 4);
            menuStrip1.Size = new Size(1200, 31);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // mainUIStripMenuItemInicio
            // 
            mainUIStripMenuItemInicio.DropDownItems.AddRange(new ToolStripItem[] { mainUIStripMenuItemIniciarSesion, mainUIStripMenuItemCerrarSesion });
            mainUIStripMenuItemInicio.Name = "mainUIStripMenuItemInicio";
            mainUIStripMenuItemInicio.Size = new Size(51, 23);
            mainUIStripMenuItemInicio.Text = "Inicio";
            // 
            // mainUIStripMenuItemIniciarSesion
            // 
            mainUIStripMenuItemIniciarSesion.Name = "mainUIStripMenuItemIniciarSesion";
            mainUIStripMenuItemIniciarSesion.Size = new Size(157, 22);
            mainUIStripMenuItemIniciarSesion.Text = "Iniciar sesión";
            mainUIStripMenuItemIniciarSesion.Click += mainUIStripMenuItemIniciarSesion_Click;
            // 
            // mainUIStripMenuItemCerrarSesion
            // 
            mainUIStripMenuItemCerrarSesion.Name = "mainUIStripMenuItemCerrarSesion";
            mainUIStripMenuItemCerrarSesion.Size = new Size(157, 22);
            mainUIStripMenuItemCerrarSesion.Text = "Cerrar sesión";
            mainUIStripMenuItemCerrarSesion.Click += mainUIStripMenuItemCerrarSesion_Click;
            // 
            // mainUIStripMenuItemGestionDeUsuarios
            // 
            mainUIStripMenuItemGestionDeUsuarios.DropDownItems.AddRange(new ToolStripItem[] { mainUIStripMenuItemABMUsuarios, mainUIStripMenuItemDesbloqueoUsuarios });
            mainUIStripMenuItemGestionDeUsuarios.Name = "mainUIStripMenuItemGestionDeUsuarios";
            mainUIStripMenuItemGestionDeUsuarios.Size = new Size(135, 23);
            mainUIStripMenuItemGestionDeUsuarios.Text = "Gestión de usuarios";
            // 
            // mainUIStripMenuItemABMUsuarios
            // 
            mainUIStripMenuItemABMUsuarios.Name = "mainUIStripMenuItemABMUsuarios";
            mainUIStripMenuItemABMUsuarios.Size = new Size(218, 22);
            mainUIStripMenuItemABMUsuarios.Text = "ABM Usuarios";
            mainUIStripMenuItemABMUsuarios.Click += mainUIStripMenuItemABMUsuarios_Click;
            // 
            // mainUIStripMenuItemDesbloqueoUsuarios
            // 
            mainUIStripMenuItemDesbloqueoUsuarios.Name = "mainUIStripMenuItemDesbloqueoUsuarios";
            mainUIStripMenuItemDesbloqueoUsuarios.Size = new Size(218, 22);
            mainUIStripMenuItemDesbloqueoUsuarios.Text = "Desbloqueo de usuarios";
            mainUIStripMenuItemDesbloqueoUsuarios.Click += mainUIStripMenuItemDesbloqueoUsuarios_Click;
            // 
            // mainUIStripMenuItemGestionDePerfiles
            // 
            mainUIStripMenuItemGestionDePerfiles.DropDownItems.AddRange(new ToolStripItem[] { mainUIStripMenuItemABMPerfiles });
            mainUIStripMenuItemGestionDePerfiles.Name = "mainUIStripMenuItemGestionDePerfiles";
            mainUIStripMenuItemGestionDePerfiles.Size = new Size(127, 23);
            mainUIStripMenuItemGestionDePerfiles.Text = "Gestión de perfiles";
            // 
            // mainUIStripMenuItemABMPerfiles
            // 
            mainUIStripMenuItemABMPerfiles.Name = "mainUIStripMenuItemABMPerfiles";
            mainUIStripMenuItemABMPerfiles.Size = new Size(236, 22);
            mainUIStripMenuItemABMPerfiles.Text = "Alta y asignación de perfiles";
            mainUIStripMenuItemABMPerfiles.Click += mainUIStripMenuItemABMPerfiles_Click;
            // 
            // mainUIStripMenuItemBitacora
            // 
            mainUIStripMenuItemBitacora.DropDownItems.AddRange(new ToolStripItem[] { mainUIStripMenuItemConsultarBitacora });
            mainUIStripMenuItemBitacora.Name = "mainUIStripMenuItemBitacora";
            mainUIStripMenuItemBitacora.Size = new Size(68, 23);
            mainUIStripMenuItemBitacora.Text = "Bitácora";
            // 
            // mainUIStripMenuItemConsultarBitacora
            // 
            mainUIStripMenuItemConsultarBitacora.Name = "mainUIStripMenuItemConsultarBitacora";
            mainUIStripMenuItemConsultarBitacora.Size = new Size(183, 22);
            mainUIStripMenuItemConsultarBitacora.Text = "Consultar bitácora";
            mainUIStripMenuItemConsultarBitacora.Click += mainUIStripMenuItemConsultarBitacora_Click;
            // 
            // mainUIStripMenuItemHistorialUsuario
            // 
            mainUIStripMenuItemHistorialUsuario.Name = "mainUIStripMenuItemHistorialUsuario";
            mainUIStripMenuItemHistorialUsuario.Size = new Size(117, 23);
            mainUIStripMenuItemHistorialUsuario.Text = "Historial usuario";
            mainUIStripMenuItemHistorialUsuario.Click += mainUIStripMenuItemHistorialUsuario_Click;
            // 
            // agregarIdiomaToolStripMenuItem
            // 
            agregarIdiomaToolStripMenuItem.Name = "agregarIdiomaToolStripMenuItem";
            agregarIdiomaToolStripMenuItem.Size = new Size(113, 23);
            agregarIdiomaToolStripMenuItem.Text = "Agregar Idioma";
            agregarIdiomaToolStripMenuItem.Click += agregarIdiomaToolStripMenuItem_Click;
            // 
            // recuperarIntegridadToolStripMenuItem
            // 
            recuperarIntegridadToolStripMenuItem.Name = "recuperarIntegridadToolStripMenuItem";
            recuperarIntegridadToolStripMenuItem.Size = new Size(142, 23);
            recuperarIntegridadToolStripMenuItem.Text = "Recuperar integridad";
            recuperarIntegridadToolStripMenuItem.Click += recuperarIntegridadToolStripMenuItem_Click;
            // 
            // menuStripItemAlmacen
            // 
            menuStripItemAlmacen.DropDownItems.AddRange(new ToolStripItem[] { almacenToolStripMenuItemGenerarSolicitud, gestionarProductosToolStripMenuItem });
            menuStripItemAlmacen.Name = "menuStripItemAlmacen";
            menuStripItemAlmacen.Size = new Size(71, 23);
            menuStripItemAlmacen.Text = "Almacén";
            // 
            // almacenToolStripMenuItemGenerarSolicitud
            // 
            almacenToolStripMenuItemGenerarSolicitud.Name = "almacenToolStripMenuItemGenerarSolicitud";
            almacenToolStripMenuItemGenerarSolicitud.Size = new Size(290, 22);
            almacenToolStripMenuItemGenerarSolicitud.Text = "Generar Solicitud de Abastecimiento";
            almacenToolStripMenuItemGenerarSolicitud.Click += almacenToolStripMenuItemGenerarSolicitud_Click;
            // 
            // gestionarProductosToolStripMenuItem
            // 
            gestionarProductosToolStripMenuItem.Name = "gestionarProductosToolStripMenuItem";
            gestionarProductosToolStripMenuItem.Size = new Size(290, 22);
            gestionarProductosToolStripMenuItem.Text = "Gestionar Productos";
            gestionarProductosToolStripMenuItem.Click += gestionarProductosToolStripMenuItem_Click;
            // 
            // menuStripItemCompras
            // 
            menuStripItemCompras.DropDownItems.AddRange(new ToolStripItem[] { comprasToolStripMenuItemEmitirOC, recepcionDeMercaderiaToolStripMenuItem });
            menuStripItemCompras.Name = "menuStripItemCompras";
            menuStripItemCompras.Size = new Size(72, 23);
            menuStripItemCompras.Text = "Compras";
            // 
            // comprasToolStripMenuItemEmitirOC
            // 
            comprasToolStripMenuItemEmitirOC.Name = "comprasToolStripMenuItemEmitirOC";
            comprasToolStripMenuItemEmitirOC.Size = new Size(227, 22);
            comprasToolStripMenuItemEmitirOC.Text = "Orden de Compra";
            comprasToolStripMenuItemEmitirOC.Click += comprasToolStripMenuItemEmitirOC_Click;
            // 
            // recepcionDeMercaderiaToolStripMenuItem
            // 
            recepcionDeMercaderiaToolStripMenuItem.Name = "recepcionDeMercaderiaToolStripMenuItem";
            recepcionDeMercaderiaToolStripMenuItem.Size = new Size(227, 22);
            recepcionDeMercaderiaToolStripMenuItem.Text = "Recepción de Mercadería";
            recepcionDeMercaderiaToolStripMenuItem.Click += recepcionDeMercaderiaToolStripMenuItem_Click;
            // 
            // menuStripItemContabilidad
            // 
            menuStripItemContabilidad.DropDownItems.AddRange(new ToolStripItem[] { contabilidadToolStripMenuItemEvaluarCotiz });
            menuStripItemContabilidad.Name = "menuStripItemContabilidad";
            menuStripItemContabilidad.Size = new Size(95, 23);
            menuStripItemContabilidad.Text = "Contabilidad";
            // 
            // contabilidadToolStripMenuItemEvaluarCotiz
            // 
            contabilidadToolStripMenuItemEvaluarCotiz.Name = "contabilidadToolStripMenuItemEvaluarCotiz";
            contabilidadToolStripMenuItemEvaluarCotiz.Size = new Size(185, 22);
            contabilidadToolStripMenuItemEvaluarCotiz.Text = "Evaluar Cotización";
            contabilidadToolStripMenuItemEvaluarCotiz.Click += contabilidadToolStripMenuItemEvaluarCotiz_Click;
            // 
            // ventasToolStripMenuItem
            // 
            ventasToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { realizarVentaToolStripMenuItem, historialDeVentasToolStripMenuItem, historialDeTicketsPDFToolStripMenuItem });
            ventasToolStripMenuItem.Name = "ventasToolStripMenuItem";
            ventasToolStripMenuItem.Size = new Size(59, 23);
            ventasToolStripMenuItem.Text = "Ventas";
            // 
            // realizarVentaToolStripMenuItem
            // 
            realizarVentaToolStripMenuItem.Name = "realizarVentaToolStripMenuItem";
            realizarVentaToolStripMenuItem.Size = new Size(187, 22);
            realizarVentaToolStripMenuItem.Text = "Realizar Venta";
            realizarVentaToolStripMenuItem.Click += realizarVentaToolStripMenuItem_Click;
            // 
            // historialDeVentasToolStripMenuItem
            // 
            historialDeVentasToolStripMenuItem.Name = "historialDeVentasToolStripMenuItem";
            historialDeVentasToolStripMenuItem.Size = new Size(187, 22);
            historialDeVentasToolStripMenuItem.Text = "Historial de ventas";
            historialDeVentasToolStripMenuItem.Click += historialDeVentasToolStripMenuItem_Click;
            // 
            // historialDeTicketsPDFToolStripMenuItem
            // 
            historialDeTicketsPDFToolStripMenuItem.Name = "historialDeTicketsPDFToolStripMenuItem";
            historialDeTicketsPDFToolStripMenuItem.Size = new Size(187, 22);
            historialDeTicketsPDFToolStripMenuItem.Text = "Historial de Tickets";
            historialDeTicketsPDFToolStripMenuItem.Click += historialDeTicketsPDFToolStripMenuItem_Click;
            // 
            // administracionToolStripMenuItem
            // 
            administracionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { gestionOfertasToolStripMenuItem, gestionProveedoresToolStripMenuItem });
            administracionToolStripMenuItem.Name = "administracionToolStripMenuItem";
            administracionToolStripMenuItem.Size = new Size(108, 23);
            administracionToolStripMenuItem.Text = "Administración";
            // 
            // gestionOfertasToolStripMenuItem
            // 
            gestionOfertasToolStripMenuItem.Name = "gestionOfertasToolStripMenuItem";
            gestionOfertasToolStripMenuItem.Size = new Size(198, 22);
            gestionOfertasToolStripMenuItem.Text = "Gestión Ofertas";
            gestionOfertasToolStripMenuItem.Click += gestionOfertasToolStripMenuItem_Click;
            // 
            // gestionProveedoresToolStripMenuItem
            // 
            gestionProveedoresToolStripMenuItem.Name = "gestionProveedoresToolStripMenuItem";
            gestionProveedoresToolStripMenuItem.Size = new Size(198, 22);
            gestionProveedoresToolStripMenuItem.Text = "Gestión Proveedores";
            gestionProveedoresToolStripMenuItem.Click += gestionProveedoresToolStripMenuItem_Click;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(40, 127, 165); // Azul Celeste
            pnlHeader.Controls.Add(lblTituloLogo);
            pnlHeader.Controls.Add(label1);
            pnlHeader.Controls.Add(comboIdiomasGlobal);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 31);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1200, 60);
            pnlHeader.TabIndex = 3;
            // 
            // lblTituloLogo
            // 
            lblTituloLogo.AutoSize = true;
            lblTituloLogo.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            lblTituloLogo.ForeColor = Color.White;
            lblTituloLogo.Location = new Point(20, 15);
            lblTituloLogo.Name = "lblTituloLogo";
            lblTituloLogo.Size = new Size(318, 30);
            lblTituloLogo.TabIndex = 3;
            lblTituloLogo.Text = "DISTRIBUIDORA MEGADRINK";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(1020, 22);
            label1.Name = "label1";
            label1.Size = new Size(51, 17);
            label1.TabIndex = 2;
            label1.Text = "Idioma:";
            // 
            // comboIdiomasGlobal
            // 
            comboIdiomasGlobal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboIdiomasGlobal.DropDownStyle = ComboBoxStyle.DropDownList;
            comboIdiomasGlobal.FormattingEnabled = true;
            comboIdiomasGlobal.Location = new Point(1080, 19);
            comboIdiomasGlobal.Name = "comboIdiomasGlobal";
            comboIdiomasGlobal.Size = new Size(100, 23);
            comboIdiomasGlobal.TabIndex = 1;
            comboIdiomasGlobal.SelectedIndexChanged += ComboIdiomasGlobal_SelectedIndexChanged;
            // 
            // MainUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(242, 244, 247); // Gris Azulado Claro
            ClientSize = new Size(1200, 768);
            Controls.Add(pnlHeader);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainUI";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sistema de Gestión - MegaDrink";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
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
        private ToolStripMenuItem ventasToolStripMenuItem;
        private ToolStripMenuItem realizarVentaToolStripMenuItem;
        private ToolStripMenuItem historialDeVentasToolStripMenuItem;
        private ToolStripMenuItem historialDeTicketsPDFToolStripMenuItem;
        private ToolStripMenuItem recepcionDeMercaderiaToolStripMenuItem;
        private ToolStripMenuItem administracionToolStripMenuItem;
        private ToolStripMenuItem gestionOfertasToolStripMenuItem;
        private ToolStripMenuItem gestionarProductosToolStripMenuItem;
        private ToolStripMenuItem gestionProveedoresToolStripMenuItem;
        private Panel pnlHeader;
        private Label lblTituloLogo;
    }
}