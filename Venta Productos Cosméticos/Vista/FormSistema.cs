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
    }
}
