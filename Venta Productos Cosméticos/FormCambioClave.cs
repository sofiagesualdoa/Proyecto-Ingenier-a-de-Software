using BLL;
using Servicios;
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
            try
            {

                if (string.IsNullOrEmpty(txtClaveActual.Text) ||
                    string.IsNullOrEmpty(txtClaveNueva.Text) ||
                    string.IsNullOrEmpty(txtConfirmacion.Text))
                {
                    throw new Exception("Todos los campos son obligatorios.");
                }
                if (txtClaveNueva.Text != txtConfirmacion.Text)
                {
                    throw new Exception("La nueva contraseña y su confirmación no coinciden.");
                }
                BLLUsuario bll = new BLLUsuario();
                bll.ModificarClave(txtClaveActual.Text, txtClaveNueva.Text);
                MessageBox.Show("Contraseña modificada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                bll.CerrarSesion();

                FormInicioSesion frmInicioSesion = new FormInicioSesion();
                frmInicioSesion.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormSistema frmMenu = new FormSistema();
            frmMenu.Show();
            this.Close();
        }
    }
}
