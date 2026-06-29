using BLL;
using DAL;
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
    public partial class FormCambioClave : Form, IObserver
    {
        private BLLIdioma bllIdioma = new BLLIdioma();
        private Dictionary<Control, string> textosOriginales = new Dictionary<Control, string>();
        public FormCambioClave()
        {
            InitializeComponent();
        }

        private void FormCambioClave_Load(object sender, EventArgs e)
        {
            RegistrarTextos(this.Controls);

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

                if (leyendas != null && leyendas.ContainsKey(textoBase))
                    ctrl.Text = leyendas[textoBase];
                else
                    ctrl.Text = textoBase;
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(txtClaveActual.Text) ||
                    string.IsNullOrEmpty(txtClaveNueva.Text) ||
                    string.IsNullOrEmpty(txtConfirmacion.Text))
                {
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Todos los campos son obligatorios."));
                }
                if (txtClaveNueva.Text != txtConfirmacion.Text)
                {
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("La nueva contraseña y su confirmación no coinciden."));
                }
                BLLUsuario bll = new BLLUsuario();
                bll.ModificarClave(txtClaveActual.Text, txtClaveNueva.Text);
                MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Contraseña modificada exitosamente."), 
                                ServicioSessionManager.GetInstance().Traducir("Éxito"), 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                var usuarioActivo = ServicioSessionManager.GetInstance().ObtenerUsuario();
                if (usuarioActivo != null)
                {
                    bll.ActualizarIdiomaUsuario(usuarioActivo.DNI, usuarioActivo.IdIdioma);
                }

                bll.CerrarSesion();

                FormInicioSesion frmInicioSesion = new FormInicioSesion();
                frmInicioSesion.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                MessageBox.Show(errorTraducido, ServicioSessionManager.GetInstance().Traducir("Error de Seguridad"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormSistema frmMenu = new FormSistema();
            frmMenu.Show();
            this.Close();
        }

        private void FormCambioClave_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void FormCambioClave_FormClosing(object sender, FormClosingEventArgs e)
        {
            bllIdioma.BorrarSuscriptor(this);
        }
    }
}
