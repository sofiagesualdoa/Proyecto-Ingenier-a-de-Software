using BLL;
using DAL;
using Servicios;
using System.Diagnostics;
using Venta_Productos_Cosméticos.Vista;

namespace Venta_Productos_Cosméticos
{
    public partial class FormSistema : Form, IObserver
    {
        private Dictionary<Control, string> textosOriginales = new Dictionary<Control, string>();
        private Dictionary<ToolStripItem, string> textosOriginalesMenu = new Dictionary<ToolStripItem, string>();
        public FormSistema()
        {
            InitializeComponent();
        }

        BLLIdioma bllIdioma = new BLLIdioma();

        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCambioClave frmClave = new FormCambioClave();
            frmClave.MdiParent = this.MdiParent;
            frmClave.Show();
            this.Close();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUsuario frmUsuario = new FormUsuario();
            frmUsuario.MdiParent = this.MdiParent;
            frmUsuario.Show();
            this.Close();
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("¿Está seguro que desea cerrar su sesión activa?"),
    ServicioSessionManager.GetInstance().Traducir("Confirmación de Cierre de Sesión"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                BLLUsuario bll = new BLLUsuario();

                var usuarioActivo = ServicioSessionManager.GetInstance().ObtenerUsuario();
                if (usuarioActivo != null)
                {

                    bll.ActualizarIdiomaUsuario(usuarioActivo.DNI, usuarioActivo.IdIdioma);
                }

                bll.CerrarSesion();
                FormInicioSesion frmLogin = new FormInicioSesion();
                frmLogin.Show();
                frmLogin.WindowState = FormWindowState.Normal;
                this.Close();
            }
        }

        private void bitácoraEventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBitacora frmBitacora = new FormBitacora();
            frmBitacora.MdiParent = this.MdiParent;
            frmBitacora.Show();
            this.Close();
        }

        private void reLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormInicioSesion frmLogin = new FormInicioSesion();
            frmLogin.MdiParent = this.MdiParent;
            frmLogin.Show();
            this.Close();
        }

        private void FormSistema_Load(object sender, EventArgs e)
        {
            BLLPerfil bllPerfil = new BLLPerfil();

            ServicioUsuario usuarioLogueado = ServicioSessionManager.GetInstance().ObtenerUsuario();

            if (usuarioLogueado != null)
            {
                ConfigurarPermisosControl(this.Controls, bllPerfil, usuarioLogueado);
            }
            else
            {
                MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("No se detectó una sesión activa. El sistema se cerrará."), ServicioSessionManager.GetInstance().Traducir("Error de Seguridad"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            RegistrarTextos(this.Controls);
            RegistrarTextosMenu(menuStrip1.Items);

            bllIdioma.AgregarSuscriptor(this);

            var usuario = ServicioSessionManager.GetInstance().ObtenerUsuario();
            if (usuario != null && usuario.Idioma != null)
            {
                Actualizar(usuario.Idioma);
            }
        }

        private void RegistrarTextos(Control.ControlCollection controles)
        {
            foreach (Control c in controles)
            {
                if (!string.IsNullOrEmpty(c.Text))
                    textosOriginales[c] = c.Text;

                if (c.Controls.Count > 0) RegistrarTextos(c.Controls);
            }
        }

        private void RegistrarTextosMenu(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                if (!string.IsNullOrEmpty(item.Text))
                {
                    textosOriginalesMenu[item] = item.Text;
                }

                if (item is ToolStripMenuItem menuItem && menuItem.DropDownItems.Count > 0)
                {
                    RegistrarTextosMenu(menuItem.DropDownItems);
                }
            }
        }

        public void Actualizar(ServicioIdioma idioma)
        {
            ActualizarIdioma(idioma);
        }

        private void ActualizarIdioma(ServicioIdioma idioma)
        {
            var leyendas = idioma.DiccionarioLeyendas;

            foreach (var entry in textosOriginales)
            {
                Control ctrl = entry.Key;
                string textoBase = entry.Value;
                ctrl.Text = (leyendas != null && leyendas.ContainsKey(textoBase)) ? leyendas[textoBase] : textoBase;
            }

            foreach (Control control in this.Controls)
            {
                if (control is MenuStrip menuStrip)
                {
                    TraducirMenu(leyendas);
                }
            }
        }

        private void TraducirMenu(Dictionary<string, string> leyendas)
        {
            foreach (var entry in textosOriginalesMenu)
            {
                ToolStripItem item = entry.Key;
                string textoOriginal = entry.Value;

                if (leyendas != null && leyendas.ContainsKey(textoOriginal))
                {
                    item.Text = leyendas[textoOriginal];
                }
                else
                {
                    item.Text = textoOriginal;
                }
            }
        }


        private void ConfigurarPermisosControl(Control.ControlCollection controles, BLLPerfil bllPerfil, ServicioUsuario usuario)
        {
            foreach (Control c in controles)
            {
                if (c is MenuStrip menuStrip)
                {
                    ConfigurarPermisosMenu(menuStrip.Items, bllPerfil, usuario);
                    continue;
                }

                string? permisoControl = c.Tag?.ToString();
                if (!string.IsNullOrWhiteSpace(permisoControl))
                {
                    c.Enabled = bllPerfil.TienePermiso(usuario, permisoControl);
                }

                if (c.HasChildren)
                {
                    ConfigurarPermisosControl(c.Controls, bllPerfil, usuario);
                }
            }
        }

        private void ConfigurarPermisosMenu(ToolStripItemCollection items, BLLPerfil bllPerfil, ServicioUsuario usuario)
        {
            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripSeparator) continue;

                item.Visible = true;

                if (item is ToolStripMenuItem menuItem && menuItem.HasDropDownItems)
                {
                    ConfigurarPermisosMenu(menuItem.DropDownItems, bllPerfil, usuario);
                }

                string? permisoMenu = item.Tag?.ToString();
                if (!string.IsNullOrWhiteSpace(permisoMenu))
                {
                    item.Enabled = bllPerfil.TienePermiso(usuario, permisoMenu);
                }
                else
                {
                    item.Enabled = true;
                }
            }
        }

        private void perfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPerfil frmPerfil = new FormPerfil();
            frmPerfil.MdiParent = this.MdiParent;
            frmPerfil.Show();
            this.Close();
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cambiarIdiomaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void españolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ServicioIdioma idiomaEspanol = new ServicioIdioma
                {
                    IdIdioma = 1,
                    CodigoIdioma = "es",
                    Nombre = "Español"
                };

                bllIdioma.CambiarIdioma(idiomaEspanol);
            }
            catch (Exception ex)
            {
                string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Error al cambiar el idioma: ") + errorTraducido, ServicioSessionManager.GetInstance().Traducir("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void inglésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ServicioIdioma idiomaIngles = new ServicioIdioma
                {
                    IdIdioma = 2,
                    CodigoIdioma = "en",
                    Nombre = "English"
                };
                bllIdioma.CambiarIdioma(idiomaIngles);
            }
            catch (Exception ex)
            {
                string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Error al cambiar el idioma: ") + errorTraducido, ServicioSessionManager.GetInstance().Traducir("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormSistema_FormClosing(object sender, FormClosingEventArgs e)
        {
            bllIdioma.BorrarSuscriptor(this);
        }

        private void backUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog dialogoCarpeta = new FolderBrowserDialog())
                {
                    dialogoCarpeta.Description = ServicioSessionManager.GetInstance().Traducir("Seleccione la ruta de destino donde se guardará el archivo.");

                    if (dialogoCarpeta.ShowDialog() == DialogResult.OK)
                    {
                        BLLBackUp bllBackUp = new BLLBackUp();
                        bllBackUp.RealizarBackup(dialogoCarpeta.SelectedPath);

                        MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Backup realizado exitosamente en la carpeta seleccionada."),
                                        ServicioSessionManager.GetInstance().Traducir("Información"),
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialogoArchivo = new OpenFileDialog())
            {
                dialogoArchivo.Filter = "Archivos de Respaldo SQL Server (*.bak)|*.bak";
                dialogoArchivo.Title = ServicioSessionManager.GetInstance().Traducir("Seleccione el archivo de backup previo que desea restaurar.");

                if (dialogoArchivo.ShowDialog() == DialogResult.OK)
                {
                    DialogResult confirmacionCritica = MessageBox.Show(
                        ServicioSessionManager.GetInstance().Traducir("¡ADVERTENCIA! Esta acción sobrescribirá todos los datos actuales almacenados en la base de datos de manera irreversible. ¿Está completamente seguro de continuar?"),
                        ServicioSessionManager.GetInstance().Traducir("Atención"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (confirmacionCritica == DialogResult.Yes)
                    {
                        try
                        {
                            BLLBackUp bllBackUp = new BLLBackUp();
                            bllBackUp.RealizarRestore(dialogoArchivo.FileName);

                            MessageBox.Show(
                                ServicioSessionManager.GetInstance().Traducir("La restauración se completó con éxito. El sistema se reiniciará por seguridad."),
                                ServicioSessionManager.GetInstance().Traducir("Éxito"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            BLLUsuario bllUser = new BLLUsuario();
                            bllUser.CerrarSesion();
                            FormInicioSesion frmLogin = new FormInicioSesion();
                            frmLogin.Show();
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            ServicioSessionManager.GetInstance().Traducir("La restauración fue cancelada."),
                            ServicioSessionManager.GetInstance().Traducir("Cancelación"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
            }
        }

        private void guíaDeInstalaciónArchivoLeémeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string rutaReadme = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "README.md");

                if (!File.Exists(rutaReadme))
                {
                    MessageBox.Show(
                        ServicioSessionManager.GetInstance().Traducir("No se encontró el archivo README.md."),
                        ServicioSessionManager.GetInstance().Traducir("Error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = rutaReadme,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ServicioSessionManager.GetInstance().Traducir("No se pudo abrir el archivo README.md: ") + ex.Message,
                    ServicioSessionManager.GetInstance().Traducir("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
