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
    public partial class FormInicioSesion : Form
    {
        public FormInicioSesion()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            //Controla usuario y contraseña
            Sistema frmMenu = new Sistema();
            frmMenu.MdiParent = this.MdiParent;
            frmMenu.Show();
            this.Close();
        }
    }
}
