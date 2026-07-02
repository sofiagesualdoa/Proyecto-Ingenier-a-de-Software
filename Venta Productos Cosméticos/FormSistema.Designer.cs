namespace Venta_Productos_Cosméticos
{
    partial class FormSistema
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSistema));
            menuStrip1 = new MenuStrip();
            usuarioToolStripMenuItem = new ToolStripMenuItem();
            cambiarClaveToolStripMenuItem = new ToolStripMenuItem();
            cambiarIdiomaToolStripMenuItem = new ToolStripMenuItem();
            españolToolStripMenuItem = new ToolStripMenuItem();
            inglésToolStripMenuItem = new ToolStripMenuItem();
            reLoginToolStripMenuItem = new ToolStripMenuItem();
            administraciónToolStripMenuItem = new ToolStripMenuItem();
            usuariosToolStripMenuItem = new ToolStripMenuItem();
            perfilesToolStripMenuItem = new ToolStripMenuItem();
            bitácoraEventosToolStripMenuItem = new ToolStripMenuItem();
            backUpToolStripMenuItem = new ToolStripMenuItem();
            restoreToolStripMenuItem = new ToolStripMenuItem();
            ventasToolStripMenuItem = new ToolStripMenuItem();
            nuevaVentaToolStripMenuItem = new ToolStripMenuItem();
            historialVentasToolStripMenuItem = new ToolStripMenuItem();
            inventarioToolStripMenuItem = new ToolStripMenuItem();
            productosToolStripMenuItem = new ToolStripMenuItem();
            clientesToolStripMenuItem = new ToolStripMenuItem();
            proveedoresToolStripMenuItem = new ToolStripMenuItem();
            reportesToolStripMenuItem = new ToolStripMenuItem();
            productosMásVendidosToolStripMenuItem = new ToolStripMenuItem();
            productosMenosVendidosToolStripMenuItem = new ToolStripMenuItem();
            productosConBajoStockToolStripMenuItem = new ToolStripMenuItem();
            centroDeAyudaToolStripMenuItem = new ToolStripMenuItem();
            manualDeUsuarioToolStripMenuItem = new ToolStripMenuItem();
            guíaDeInstalaciónArchivoLeémeToolStripMenuItem = new ToolStripMenuItem();
            cerrarSesiónToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.RosyBrown;
            menuStrip1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            menuStrip1.Items.AddRange(new ToolStripItem[] { usuarioToolStripMenuItem, administraciónToolStripMenuItem, ventasToolStripMenuItem, inventarioToolStripMenuItem, reportesToolStripMenuItem, centroDeAyudaToolStripMenuItem, cerrarSesiónToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 33);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // usuarioToolStripMenuItem
            // 
            usuarioToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cambiarClaveToolStripMenuItem, cambiarIdiomaToolStripMenuItem, reLoginToolStripMenuItem });
            usuarioToolStripMenuItem.Name = "usuarioToolStripMenuItem";
            usuarioToolStripMenuItem.Size = new Size(93, 29);
            usuarioToolStripMenuItem.Tag = "";
            usuarioToolStripMenuItem.Text = "Usuario";
            // 
            // cambiarClaveToolStripMenuItem
            // 
            cambiarClaveToolStripMenuItem.Name = "cambiarClaveToolStripMenuItem";
            cambiarClaveToolStripMenuItem.Size = new Size(225, 30);
            cambiarClaveToolStripMenuItem.Tag = "Cambiar Clave";
            cambiarClaveToolStripMenuItem.Text = "Cambiar Clave";
            cambiarClaveToolStripMenuItem.Click += cambiarClaveToolStripMenuItem_Click;
            // 
            // cambiarIdiomaToolStripMenuItem
            // 
            cambiarIdiomaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { españolToolStripMenuItem, inglésToolStripMenuItem });
            cambiarIdiomaToolStripMenuItem.Name = "cambiarIdiomaToolStripMenuItem";
            cambiarIdiomaToolStripMenuItem.Size = new Size(225, 30);
            cambiarIdiomaToolStripMenuItem.Tag = "Cambiar Idioma";
            cambiarIdiomaToolStripMenuItem.Text = "Cambiar Idioma";
            cambiarIdiomaToolStripMenuItem.Click += cambiarIdiomaToolStripMenuItem_Click;
            // 
            // españolToolStripMenuItem
            // 
            españolToolStripMenuItem.Name = "españolToolStripMenuItem";
            españolToolStripMenuItem.Size = new Size(153, 30);
            españolToolStripMenuItem.Text = "Español";
            españolToolStripMenuItem.Click += españolToolStripMenuItem_Click;
            // 
            // inglésToolStripMenuItem
            // 
            inglésToolStripMenuItem.Name = "inglésToolStripMenuItem";
            inglésToolStripMenuItem.Size = new Size(153, 30);
            inglésToolStripMenuItem.Text = "Inglés";
            inglésToolStripMenuItem.Click += inglésToolStripMenuItem_Click;
            // 
            // reLoginToolStripMenuItem
            // 
            reLoginToolStripMenuItem.Name = "reLoginToolStripMenuItem";
            reLoginToolStripMenuItem.Size = new Size(225, 30);
            reLoginToolStripMenuItem.Tag = "ReLogin";
            reLoginToolStripMenuItem.Text = "ReLogin";
            reLoginToolStripMenuItem.Click += reLoginToolStripMenuItem_Click;
            // 
            // administraciónToolStripMenuItem
            // 
            administraciónToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { usuariosToolStripMenuItem, perfilesToolStripMenuItem, bitácoraEventosToolStripMenuItem, backUpToolStripMenuItem, restoreToolStripMenuItem });
            administraciónToolStripMenuItem.Name = "administraciónToolStripMenuItem";
            administraciónToolStripMenuItem.Size = new Size(159, 29);
            administraciónToolStripMenuItem.Text = "Administración";
            // 
            // usuariosToolStripMenuItem
            // 
            usuariosToolStripMenuItem.Name = "usuariosToolStripMenuItem";
            usuariosToolStripMenuItem.Size = new Size(231, 30);
            usuariosToolStripMenuItem.Tag = "Gestionar Usuarios";
            usuariosToolStripMenuItem.Text = "Usuarios";
            usuariosToolStripMenuItem.Click += usuariosToolStripMenuItem_Click;
            // 
            // perfilesToolStripMenuItem
            // 
            perfilesToolStripMenuItem.Name = "perfilesToolStripMenuItem";
            perfilesToolStripMenuItem.Size = new Size(231, 30);
            perfilesToolStripMenuItem.Tag = "Gestionar Perfiles";
            perfilesToolStripMenuItem.Text = "Perfiles";
            perfilesToolStripMenuItem.Click += perfilesToolStripMenuItem_Click;
            // 
            // bitácoraEventosToolStripMenuItem
            // 
            bitácoraEventosToolStripMenuItem.Name = "bitácoraEventosToolStripMenuItem";
            bitácoraEventosToolStripMenuItem.Size = new Size(231, 30);
            bitácoraEventosToolStripMenuItem.Tag = "Auditar Bitacora";
            bitácoraEventosToolStripMenuItem.Text = "Bitácora Eventos";
            bitácoraEventosToolStripMenuItem.Click += bitácoraEventosToolStripMenuItem_Click;
            // 
            // backUpToolStripMenuItem
            // 
            backUpToolStripMenuItem.Name = "backUpToolStripMenuItem";
            backUpToolStripMenuItem.Size = new Size(231, 30);
            backUpToolStripMenuItem.Tag = "Gestionar Backup";
            backUpToolStripMenuItem.Text = "BackUp";
            backUpToolStripMenuItem.Click += backUpToolStripMenuItem_Click;
            // 
            // restoreToolStripMenuItem
            // 
            restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            restoreToolStripMenuItem.Size = new Size(231, 30);
            restoreToolStripMenuItem.Tag = "Gestionar Restore";
            restoreToolStripMenuItem.Text = "Restore";
            restoreToolStripMenuItem.Click += restoreToolStripMenuItem_Click;
            // 
            // ventasToolStripMenuItem
            // 
            ventasToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nuevaVentaToolStripMenuItem, historialVentasToolStripMenuItem });
            ventasToolStripMenuItem.Name = "ventasToolStripMenuItem";
            ventasToolStripMenuItem.Size = new Size(83, 29);
            ventasToolStripMenuItem.Text = "Ventas";
            // 
            // nuevaVentaToolStripMenuItem
            // 
            nuevaVentaToolStripMenuItem.Name = "nuevaVentaToolStripMenuItem";
            nuevaVentaToolStripMenuItem.Size = new Size(223, 30);
            nuevaVentaToolStripMenuItem.Tag = "Registrar Venta";
            nuevaVentaToolStripMenuItem.Text = "Nueva Venta";
            // 
            // historialVentasToolStripMenuItem
            // 
            historialVentasToolStripMenuItem.Name = "historialVentasToolStripMenuItem";
            historialVentasToolStripMenuItem.Size = new Size(223, 30);
            historialVentasToolStripMenuItem.Tag = "Ver Historial Ventas";
            historialVentasToolStripMenuItem.Text = "Historial Ventas";
            // 
            // inventarioToolStripMenuItem
            // 
            inventarioToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { productosToolStripMenuItem, clientesToolStripMenuItem, proveedoresToolStripMenuItem });
            inventarioToolStripMenuItem.Name = "inventarioToolStripMenuItem";
            inventarioToolStripMenuItem.Size = new Size(97, 29);
            inventarioToolStripMenuItem.Text = "Maestro";
            // 
            // productosToolStripMenuItem
            // 
            productosToolStripMenuItem.Name = "productosToolStripMenuItem";
            productosToolStripMenuItem.Size = new Size(196, 30);
            productosToolStripMenuItem.Tag = "Gestionar Productos";
            productosToolStripMenuItem.Text = "Productos";
            // 
            // clientesToolStripMenuItem
            // 
            clientesToolStripMenuItem.Name = "clientesToolStripMenuItem";
            clientesToolStripMenuItem.Size = new Size(196, 30);
            clientesToolStripMenuItem.Tag = "Gestionar Clientes";
            clientesToolStripMenuItem.Text = "Clientes";
            // 
            // proveedoresToolStripMenuItem
            // 
            proveedoresToolStripMenuItem.Name = "proveedoresToolStripMenuItem";
            proveedoresToolStripMenuItem.Size = new Size(196, 30);
            proveedoresToolStripMenuItem.Tag = "Gestionar Proveedores";
            proveedoresToolStripMenuItem.Text = "Proveedores";
            // 
            // reportesToolStripMenuItem
            // 
            reportesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { productosMásVendidosToolStripMenuItem, productosMenosVendidosToolStripMenuItem, productosConBajoStockToolStripMenuItem });
            reportesToolStripMenuItem.Name = "reportesToolStripMenuItem";
            reportesToolStripMenuItem.Size = new Size(104, 29);
            reportesToolStripMenuItem.Text = "Reportes";
            reportesToolStripMenuItem.Click += reportesToolStripMenuItem_Click;
            // 
            // productosMásVendidosToolStripMenuItem
            // 
            productosMásVendidosToolStripMenuItem.Name = "productosMásVendidosToolStripMenuItem";
            productosMásVendidosToolStripMenuItem.Size = new Size(329, 30);
            productosMásVendidosToolStripMenuItem.Tag = "Reporte Mas Vendidos";
            productosMásVendidosToolStripMenuItem.Text = "Productos Más Vendidos";
            // 
            // productosMenosVendidosToolStripMenuItem
            // 
            productosMenosVendidosToolStripMenuItem.Name = "productosMenosVendidosToolStripMenuItem";
            productosMenosVendidosToolStripMenuItem.Size = new Size(329, 30);
            productosMenosVendidosToolStripMenuItem.Tag = "Reporte Menos Vendidos";
            productosMenosVendidosToolStripMenuItem.Text = "Productos Menos Vendidos";
            // 
            // productosConBajoStockToolStripMenuItem
            // 
            productosConBajoStockToolStripMenuItem.Name = "productosConBajoStockToolStripMenuItem";
            productosConBajoStockToolStripMenuItem.Size = new Size(329, 30);
            productosConBajoStockToolStripMenuItem.Tag = "Reporte Bajo Stock";
            productosConBajoStockToolStripMenuItem.Text = "Productos con Bajo Stock";
            // 
            // centroDeAyudaToolStripMenuItem
            // 
            centroDeAyudaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { manualDeUsuarioToolStripMenuItem, guíaDeInstalaciónArchivoLeémeToolStripMenuItem });
            centroDeAyudaToolStripMenuItem.Name = "centroDeAyudaToolStripMenuItem";
            centroDeAyudaToolStripMenuItem.Size = new Size(81, 29);
            centroDeAyudaToolStripMenuItem.Text = "Ayuda";
            // 
            // manualDeUsuarioToolStripMenuItem
            // 
            manualDeUsuarioToolStripMenuItem.Name = "manualDeUsuarioToolStripMenuItem";
            manualDeUsuarioToolStripMenuItem.Size = new Size(402, 30);
            manualDeUsuarioToolStripMenuItem.Tag = "Ver Manual Usuario";
            manualDeUsuarioToolStripMenuItem.Text = "Manual de Usuario";
            // 
            // guíaDeInstalaciónArchivoLeémeToolStripMenuItem
            // 
            guíaDeInstalaciónArchivoLeémeToolStripMenuItem.Name = "guíaDeInstalaciónArchivoLeémeToolStripMenuItem";
            guíaDeInstalaciónArchivoLeémeToolStripMenuItem.Size = new Size(402, 30);
            guíaDeInstalaciónArchivoLeémeToolStripMenuItem.Tag = "Ver Guia Instalacion";
            guíaDeInstalaciónArchivoLeémeToolStripMenuItem.Text = "Guía de Instalación (Archivo Leéme)";
            guíaDeInstalaciónArchivoLeémeToolStripMenuItem.Click += guíaDeInstalaciónArchivoLeémeToolStripMenuItem_Click;
            // 
            // cerrarSesiónToolStripMenuItem
            // 
            cerrarSesiónToolStripMenuItem.Name = "cerrarSesiónToolStripMenuItem";
            cerrarSesiónToolStripMenuItem.Size = new Size(143, 29);
            cerrarSesiónToolStripMenuItem.Tag = "Cerrar Sesion";
            cerrarSesiónToolStripMenuItem.Text = "Cerrar Sesión";
            cerrarSesiónToolStripMenuItem.Click += cerrarSesiónToolStripMenuItem_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            label1.Location = new Point(328, 88);
            label1.Name = "label1";
            label1.Size = new Size(158, 32);
            label1.TabIndex = 3;
            label1.Text = "¡Bienvenido!";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.LightGray;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(161, 140);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(479, 222);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // FormSistema
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MistyRose;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "FormSistema";
            Text = "Sistema de Venta de Productos Cosméticos";
            FormClosing += FormSistema_FormClosing;
            Load += FormSistema_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem ventasToolStripMenuItem;
        private ToolStripMenuItem inventarioToolStripMenuItem;
        private ToolStripMenuItem reportesToolStripMenuItem;
        private ToolStripMenuItem usuarioToolStripMenuItem;
        private ToolStripMenuItem administraciónToolStripMenuItem;
        private ToolStripMenuItem centroDeAyudaToolStripMenuItem;
        private ToolStripMenuItem cerrarSesiónToolStripMenuItem;
        private ToolStripMenuItem nuevaVentaToolStripMenuItem;
        private ToolStripMenuItem historialVentasToolStripMenuItem;
        private ToolStripMenuItem productosMásVendidosToolStripMenuItem;
        private ToolStripMenuItem productosConBajoStockToolStripMenuItem;
        private ToolStripMenuItem usuariosToolStripMenuItem;
        private ToolStripMenuItem perfilesToolStripMenuItem;
        private ToolStripMenuItem bitácoraEventosToolStripMenuItem;
        private ToolStripMenuItem backUpToolStripMenuItem;
        private ToolStripMenuItem restoreToolStripMenuItem;
        private ToolStripMenuItem productosToolStripMenuItem;
        private ToolStripMenuItem clientesToolStripMenuItem;
        private ToolStripMenuItem productosMenosVendidosToolStripMenuItem;
        private ToolStripMenuItem cambiarClaveToolStripMenuItem;
        private ToolStripMenuItem cambiarIdiomaToolStripMenuItem;
        private ToolStripMenuItem manualDeUsuarioToolStripMenuItem;
        private ToolStripMenuItem guíaDeInstalaciónArchivoLeémeToolStripMenuItem;
        private ToolStripMenuItem proveedoresToolStripMenuItem;
        private Label label1;
        private PictureBox pictureBox1;
        private ToolStripMenuItem reLoginToolStripMenuItem;
        private ToolStripMenuItem españolToolStripMenuItem;
        private ToolStripMenuItem inglésToolStripMenuItem;
    }
}
