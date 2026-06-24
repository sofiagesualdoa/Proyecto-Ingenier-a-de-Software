using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Venta_Productos_Cosméticos
{
    public partial class FormPantallaInicio : Form
    {
        public FormPantallaInicio()
        {
            InitializeComponent();
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            FormInicioSesion frmLogin = new FormInicioSesion();
            frmLogin.MdiParent = this.MdiParent;
            frmLogin.Show();
            this.Hide();

            //this.panel1.Visible = false;
            //FormInicioSesion frmLogin = new FormInicioSesion();
            //frmLogin.MdiParent = this;
            //frmLogin.Show();
        }
    }
}
