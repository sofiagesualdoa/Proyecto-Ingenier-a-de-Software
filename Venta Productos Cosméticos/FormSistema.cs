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
    }
}
