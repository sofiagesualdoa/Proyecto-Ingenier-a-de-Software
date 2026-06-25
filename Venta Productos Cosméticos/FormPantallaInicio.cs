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
    public partial class FormPantallaInicio : Form, IObserver
    {
        private BLLIdioma bllIdioma = new BLLIdioma();
        private Dictionary<Control, string> textosOriginales = new Dictionary<Control, string>();
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
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormPantallaInicio_Load(object sender, EventArgs e)
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

        private void FormPantallaInicio_FormClosing(object sender, FormClosingEventArgs e)
        {
            bllIdioma.BorrarSuscriptor(this);
        }
    }
}
