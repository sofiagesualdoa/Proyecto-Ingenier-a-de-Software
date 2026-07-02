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
    public partial class FormErrorIntegridad : Form, IObserver
    {

        public FormErrorIntegridad()
        {
            InitializeComponent();
            this.FormClosing += FormErrorIntegridad_FormClosing;
        }
        private BLLIdioma bllIdioma = new BLLIdioma();
        private Dictionary<Control, string> textosOriginales = new Dictionary<Control, string>();
        private void FormErrorIntegridad_Load(object sender, EventArgs e)
        {
            RegistrarTextos(this.Controls);

            bllIdioma.AgregarSuscriptor(this);

            CargarErrores();

            var usuario = ServicioSessionManager.GetInstance().ObtenerUsuario();
            if (usuario != null && usuario.Idioma != null)
            {
                Actualizar(usuario.Idioma);
            }
        }

        private void CargarErrores()
        {
            BLLDVV bllDVV = new BLLDVV();
            List<ServicioErrorIntegridad> errores = bllDVV.ObtenerErroresIntegridad();

            dgvErrores.AutoGenerateColumns = true;
            dgvErrores.DataSource = null;
            dgvErrores.DataSource = errores;

            TraducirHeadersGrilla();
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            try
            {
                BLLDVV bllDVV = new BLLDVV();
                bllDVV.RecalcularDigitosVerificadores();
                List<ServicioErrorIntegridad> errores = bllDVV.ObtenerErroresIntegridad();

                dgvErrores.DataSource = null;
                dgvErrores.DataSource = errores;
                if (errores.Count == 0)
                {
                    MessageBox.Show(
                        ServicioSessionManager.GetInstance().Traducir("La integridad fue restablecida correctamente."),
                        ServicioSessionManager.GetInstance().Traducir("Integridad"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    ServicioSessionManager.GetInstance().CerrarSesion();
                    FormInicioSesion login = new FormInicioSesion();
                    login.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        ServicioSessionManager.GetInstance().Traducir("La integridad no pudo ser restablecida. Revise los datos alterados."),
                        ServicioSessionManager.GetInstance().Traducir("Error de Integridad"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Error al recalcular dígitos verificadores: ") + ex.Message);
            }
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            new BLLUsuario().CerrarSesion();

            FormInicioSesion login = new FormInicioSesion();
            login.Show();

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialogoArchivo = new OpenFileDialog())
            {
                dialogoArchivo.Filter = "Archivos de Respaldo SQL Server (*.bak)|*.bak";
                dialogoArchivo.Title = ServicioSessionManager.GetInstance().Traducir("Seleccione el archivo de backup previo que desea restaurar.");

                if (dialogoArchivo.ShowDialog() == DialogResult.OK)
                {
                    DialogResult confirmacionCritica = MessageBox.Show(
                        ServicioSessionManager.GetInstance().Traducir("¡ADVERTENCIA! Esta acción sobrescribirá todos los datos actuales almacenados en la base de datos de manera irreversible. ¿Está completamente seguro de continuar?"),
                        ServicioSessionManager.GetInstance().Traducir("Atención"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (confirmacionCritica == DialogResult.Yes)
                    {
                        try
                        {
                            BLLBackUp bllBackUp = new BLLBackUp();
                            bllBackUp.RealizarRestore(dialogoArchivo.FileName);

                            MessageBox.Show(
                                ServicioSessionManager.GetInstance().Traducir("La restauración se completó con éxito. El sistema se reiniciará por seguridad."),
                                ServicioSessionManager.GetInstance().Traducir("Éxito"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            BLLUsuario bllUser = new BLLUsuario();
                            bllUser.CerrarSesion();
                            FormInicioSesion frmLogin = new FormInicioSesion();
                            frmLogin.Show();
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            ServicioSessionManager.GetInstance().Traducir("La restauración fue cancelada."),
                            ServicioSessionManager.GetInstance().Traducir("Cancelación"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
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

            TraducirHeadersGrilla();
        }

        private void TraducirHeadersGrilla()
        {
            var usuarioActivo = ServicioSessionManager.GetInstance().ObtenerUsuario();

            if (usuarioActivo?.Idioma?.DiccionarioLeyendas != null)
            {
                var leyendas = usuarioActivo.Idioma.DiccionarioLeyendas;

                foreach (DataGridViewColumn columna in dgvErrores.Columns)
                {
                    if (leyendas.ContainsKey(columna.Name))
                    {
                        columna.HeaderText = leyendas[columna.Name];
                    }
                }
            }
        }

        private void FormErrorIntegridad_FormClosing(object sender, FormClosingEventArgs e)
        {
            bllIdioma.BorrarSuscriptor(this);
        }
    }
}
