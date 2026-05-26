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

        private void usuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUsuario frmUsuario = new FormUsuario();
            frmUsuario.MdiParent = this.MdiParent;
            frmUsuario.Show();
            this.Close();
        }
    }
}
