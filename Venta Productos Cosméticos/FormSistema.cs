using BLL;
using Servicios;
using Venta_Productos_Cosméticos.Vista;

namespace Venta_Productos_Cosméticos
{
    public partial class FormSistema : Form
    {
        public FormSistema()
        {
            InitializeComponent();
        }

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
            DialogResult resultado = MessageBox.Show("¿Está seguro que desea cerrar su sesión activa?",
            "Confirmación de Cierre de Sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                BLLUsuario bll = new BLLUsuario();
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
                MessageBox.Show("No se detectó una sesión activa. El sistema se cerrará.", "Error de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
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
    }
}
