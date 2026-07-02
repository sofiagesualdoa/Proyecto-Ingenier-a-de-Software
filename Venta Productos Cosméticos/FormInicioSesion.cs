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
    public partial class FormInicioSesion : Form, IObserver
    {
        private BLLIdioma bllIdioma = new BLLIdioma();
        private Dictionary<Control, string> textosOriginales = new Dictionary<Control, string>();
        private ServicioIdioma idiomaSeleccionadoLogin = null;
        private bool idiomaLoginCambiado = false;
        public FormInicioSesion()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsuario.Text) || string.IsNullOrEmpty(txtContraseña.Text))
            {
                MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Por favor, complete todos los campos para continuar."),
                                ServicioSessionManager.GetInstance().Traducir("Éxito"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            try
            {
                ServicioSessionManager servicioSessionManager = ServicioSessionManager.GetInstance();

                if (servicioSessionManager.ObtenerUsuario() != null)
                {
                    FormSistema frmMenu = new FormSistema();
                    frmMenu.Show();
                    this.Close();
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Ya existe una sesión activa: ") + servicioSessionManager.ObtenerUsuario().nombreUsuario);
                }

                BLLUsuario bll = new BLLUsuario();
                bool loginExitoso = bll.IniciarSesion(txtUsuario.Text, txtContraseña.Text);

                if (loginExitoso)
                {
                    if (idiomaLoginCambiado)
                    {
                        try
                        {
                            bllIdioma.CambiarIdioma(idiomaSeleccionadoLogin);
                        }
                        catch (InvalidOperationException ex) when (ex.Message == "El idioma seleccionado ya se encuentra activo.")
                        {
                            string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                            MessageBox.Show(errorTraducido, ServicioSessionManager.GetInstance().Traducir("Cambio de Idioma"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    FormSistema frmMenu = new FormSistema();
                    frmMenu.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "ERROR_INTEGRIDAD_ADMIN")
                {
                    FormErrorIntegridad frm = new FormErrorIntegridad();
                    frm.Show();
                    this.Hide();
                    return;
                }

                if (ex.Message == "ERROR_INTEGRIDAD_NO_ADMIN")
                {
                    MessageBox.Show(
                        ServicioSessionManager.GetInstance().Traducir("Se detectó un error de integridad. Sólo un administrador puede gestionar la reparación."),
                        ServicioSessionManager.GetInstance().Traducir("Error de Integridad"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                MessageBox.Show(errorTraducido, ServicioSessionManager.GetInstance().Traducir("Error de Autenticación"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContraseña.Clear();
                txtContraseña.Focus();
            }
        }
        private void FormInicioSesion_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void FormInicioSesion_Load(object sender, EventArgs e)
        {
            RegistrarTextos(this.Controls);

            idiomaSeleccionadoLogin = new ServicioIdioma
            {
                IdIdioma = 1,
                CodigoIdioma = "es",
                Nombre = "Español",
                DiccionarioLeyendas = new Dictionary<string, string>()
            };

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

                if (c.Controls.Count > 0)
                    RegistrarTextos(c.Controls);
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

        private void FormInicioSesion_FormClosing(object sender, FormClosingEventArgs e)
        {
            bllIdioma.BorrarSuscriptor(this);
        }

        private void btnCambioIdioma_Click(object sender, EventArgs e)
        {
            if (idiomaSeleccionadoLogin == null || idiomaSeleccionadoLogin.CodigoIdioma == "es")
            {
                idiomaSeleccionadoLogin = new ServicioIdioma
                {
                    IdIdioma = 2,
                    CodigoIdioma = "en",
                    Nombre = "English",
                    DiccionarioLeyendas = bllIdioma.ObtenerTraducciones()
                };
            }
            else
            {
                idiomaSeleccionadoLogin = new ServicioIdioma
                {
                    IdIdioma = 1,
                    CodigoIdioma = "es",
                    Nombre = "Español",
                    DiccionarioLeyendas = new Dictionary<string, string>()
                };
            }

            Actualizar(idiomaSeleccionadoLogin);
            idiomaLoginCambiado = true;
        }
    }
}
