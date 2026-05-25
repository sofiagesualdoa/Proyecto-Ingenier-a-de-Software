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
    public partial class FormCambioClave : Form
    {
        public FormCambioClave()
        {
            InitializeComponent();
        }

        private void FormCambioClave_Load(object sender, EventArgs e)
        {

        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClaveActual.Text) ||
                string.IsNullOrEmpty(txtClaveNueva.Text) ||
                string.IsNullOrEmpty(txtConfirmacion.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtClaveNueva.Text != txtConfirmacion.Text)
            {
                MessageBox.Show("La nueva contraseña y su confirmación no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                BLL.BLLUsuario bll = new BLL.BLLUsuario();

                bll.ModificarClave(txtClaveActual.Text, txtClaveNueva.Text);
                MessageBox.Show("Contraseña modificada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
