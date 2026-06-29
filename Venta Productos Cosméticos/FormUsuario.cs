using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Servicios;
using BLL;

namespace Venta_Productos_Cosméticos.Vista
{
    public partial class FormUsuario : Form, IObserver
    {
        private string modo = "Consulta";
        private BLLIdioma bllIdioma = new BLLIdioma();
        private Dictionary<Control, string> textosOriginales = new Dictionary<Control, string>();
        public FormUsuario()
        {
            InitializeComponent();
        }

        private void FormUsuario_Load(object sender, EventArgs e)
        {
            CargarComboPerfiles();

            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            RegresarAModoConsulta();

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

        private void CargarComboPerfiles()
        {
            BLLPerfil bllPerfil = new BLLPerfil();
            List<ServicioPerfil> perfiles = bllPerfil.ObtenerPerfiles();

            comboBox1.DataSource = perfiles;
            comboBox1.DisplayMember = "Nombre";
            comboBox1.ValueMember = "IdPerfil";
            comboBox1.SelectedIndex = perfiles.Count > 0 ? 0 : -1;
        }

        private void RegresarAModoConsulta()
        {
            modo = "Consulta";
            groupBox1.Text = bllIdioma.TraducirTexto("Modo Consulta");
            button5.Enabled = false;
            button6.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button8.Enabled = true;

            LimpiarCampos();
            HabilitarTextBox();

            BLLUsuario bll = new BLLUsuario();
            List<ServicioUsuario> usuarios = bll.ObtenerUsuarios();
            MostrarGrilla(usuarios.Where(u => u.Activo).ToList());
            radioButton4.Checked = true;
        }

        private void HabilitarTextBox()
        {
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
            textBox5.ReadOnly = false;
            comboBox1.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
        }

        private void LimpiarCampos()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
        }

        private void ActivarModoEdicion()
        {
            button5.Enabled = true;
            button6.Enabled = true;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button8.Enabled = false;
        }

        private void DeshabilitarTextBox()
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            comboBox1.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            modo = "Añadir";
            groupBox1.Text = bllIdioma.TraducirTexto("Modo Añadir"); 
            LimpiarCampos();
            ActivarModoEdicion();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Por favor, seleccione un usuario de la grilla superior para desbloquear."),
                                ServicioSessionManager.GetInstance().Traducir("Atención"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
            modo = "Desbloquear";
            groupBox1.Text = bllIdioma.TraducirTexto("Modo Desbloquear");
            ServicioUsuario seleccionado = (ServicioUsuario)dataGridView1.CurrentRow.DataBoundItem;
            LlenarTextBox(seleccionado);
            ActivarModoEdicion();
            DeshabilitarTextBox();
        }

        private void LlenarTextBox(ServicioUsuario seleccionado)
        {
            textBox1.Text = seleccionado.DNI.ToString();
            textBox2.Text = seleccionado.Nombre;
            textBox3.Text = seleccionado.nombreUsuario;
            textBox4.Text = seleccionado.Apellido;
            textBox5.Text = seleccionado.Email;
            comboBox1.SelectedValue = seleccionado.IdPerfil;
        }

        private int ObtenerIdPerfilSeleccionado()
        {
            if (comboBox1.SelectedValue == null)
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("Debe seleccionar un perfil."));
            }

            return Convert.ToInt32(comboBox1.SelectedValue);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Por favor, seleccione un usuario de la grilla antes de presionar Modificar."), 
                                ServicioSessionManager.GetInstance().Traducir("Atención"), 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Warning);
                return;
            }
            modo = "Modificar";
            groupBox1.Text = bllIdioma.TraducirTexto("Modo Modificar");
            try
            {
                ServicioUsuario usuarioSeleccionado = (ServicioUsuario)dataGridView1.CurrentRow.DataBoundItem;

                if (usuarioSeleccionado != null)
                {
                    LlenarTextBox(usuarioSeleccionado);
                    if (usuarioSeleccionado.Activo) radioButton1.Checked = true;
                    else radioButton2.Checked = true;
                    ActivarModoEdicion();
                    HabilitarTextBox();
                    textBox1.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                MessageBox.Show((ServicioSessionManager.GetInstance().Traducir("Error al mapear datos: ")) + errorTraducido,
                                ServicioSessionManager.GetInstance().Traducir("Error"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BLLUsuario bll = new BLLUsuario();
            if (modo == "Añadir")
            {
                try
                {
                    ServicioUsuario usuario = new ServicioUsuario();
                    usuario.Nombre = textBox2.Text;
                    usuario.Apellido = textBox4.Text;
                    usuario.Email = textBox5.Text;
                    usuario.DNI = int.Parse(textBox1.Text);
                    usuario.nombreUsuario = textBox3.Text;
                    usuario.IdPerfil = ObtenerIdPerfilSeleccionado();
                    if (radioButton1.Checked)
                    {
                        usuario.Activo = true;
                    }
                    else if (radioButton2.Checked)
                    {
                        usuario.Activo = false;
                    }
                    bll.CrearUsuario(usuario);
                    MostrarGrilla(bll.ObtenerUsuarios());
                    MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Usuario creado correctamente."),
                                    ServicioSessionManager.GetInstance().Traducir("Éxito"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                    MessageBox.Show(
                        errorTraducido,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else if (modo == "Modificar")
            {
                try
                {
                    ServicioUsuario usuario = new ServicioUsuario();
                    usuario.Nombre = textBox2.Text;
                    usuario.Apellido = textBox4.Text;
                    usuario.Email = textBox5.Text;
                    usuario.DNI = int.Parse(textBox1.Text);
                    usuario.nombreUsuario = textBox3.Text;
                    usuario.IdPerfil = ObtenerIdPerfilSeleccionado();
                    if (radioButton1.Checked)
                    {
                        usuario.Activo = true;
                    }
                    else if (radioButton2.Checked)
                    {
                        usuario.Activo = false;
                    }
                    bll.ModificarUsuario(usuario);
                    MostrarGrilla(bll.ObtenerUsuarios());

                    MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Usuario modificado correctamente."),
                                    ServicioSessionManager.GetInstance().Traducir("Éxito"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                    MessageBox.Show(
                        errorTraducido,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else if (modo == "Desbloquear")
            {
                try
                {
                    if (dataGridView1.CurrentRow == null)
                    {
                        MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Seleccione un usuario."));
                        return;
                    }
                    int dni = Convert.ToInt32(dataGridView1.CurrentRow.Cells["DNI"].Value);
                    bll.DesbloquearUsuario(dni);
                    MostrarGrilla(bll.ObtenerUsuarios());

                    MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Usuario desbloqueado correctamente."),
                                    ServicioSessionManager.GetInstance().Traducir("Éxito"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                    MessageBox.Show(
                        errorTraducido,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else if (modo == "Activar / Desactivar")
            {
                if (dataGridView1.CurrentRow != null)
                {
                    int DNISeleccionado = Convert.ToInt32(dataGridView1.CurrentRow.Cells["DNI"].Value);
                    try
                    {
                        if (bll.ModificarEstado(DNISeleccionado))
                        {
                            MostrarGrilla(bll.ObtenerUsuarios());
                            MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("El estado del usuario se actualizó correctamente."),
                                            ServicioSessionManager.GetInstance().Traducir("Éxito"),
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                        MessageBox.Show(errorTraducido, ServicioSessionManager.GetInstance().Traducir("Error de Validación"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Por favor, seleccione un usuario de la lista."), 
                                    ServicioSessionManager.GetInstance().Traducir("Atención"), 
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            RegresarAModoConsulta();
            HabilitarTextBox();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            RegresarAModoConsulta();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormSistema frmMenu = new FormSistema();
            frmMenu.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            modo = "Activar / Desactivar";
            groupBox1.Text = bllIdioma.TraducirTexto("Modo Activar / Desactivar");
            ServicioUsuario seleccionado = (ServicioUsuario)dataGridView1.CurrentRow.DataBoundItem;
            LlenarTextBox(seleccionado);
            ActivarModoEdicion();
            DeshabilitarTextBox();
        }

        private void MostrarGrilla(Object lista)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = lista;
            var usuarioActivo = ServicioSessionManager.GetInstance().ObtenerUsuario();
            if (usuarioActivo?.Idioma?.DiccionarioLeyendas != null)
            {
                var leyendas = usuarioActivo.Idioma.DiccionarioLeyendas;
                foreach (DataGridViewColumn columna in dataGridView1.Columns)
                {
                    if (leyendas.ContainsKey(columna.Name))
                    {
                        columna.HeaderText = leyendas[columna.Name];
                    }
                }
            }
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            BLLUsuario bll = new BLLUsuario();
            MostrarGrilla(bll.ObtenerUsuarios());
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            BLLUsuario bll = new BLLUsuario();
            List<ServicioUsuario> usuarios = bll.ObtenerUsuarios();
            MostrarGrilla(usuarios.Where(u => u.Activo).ToList());
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridView1.Rows[e.RowIndex].DataBoundItem == null) return;
            if (e.CellStyle == null) return;

            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is ServicioUsuario usuario)
            {
                if (!usuario.Activo)
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 192, 192);
                    e.CellStyle.SelectionBackColor = Color.Red;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ActivarModoEdicion();
            BLLUsuario bll = new BLLUsuario();
            List<ServicioUsuario> listaFiltrada = bll.ObtenerUsuarios();
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
                listaFiltrada = listaFiltrada.Where(u => u.DNI.ToString().Contains(textBox1.Text)).ToList();

            if (!string.IsNullOrWhiteSpace(textBox2.Text))
                listaFiltrada = listaFiltrada.Where(u => u.Nombre.ToLower().Contains(textBox2.Text.ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(textBox4.Text))
                listaFiltrada = listaFiltrada.Where(u => u.Apellido.ToLower().Contains(textBox4.Text.ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(textBox5.Text))
                listaFiltrada = listaFiltrada.Where(u => u.Email.ToLower().Contains(textBox5.Text.ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(textBox3.Text))
                listaFiltrada = listaFiltrada.Where(u => u.nombreUsuario.ToLower().Contains(textBox3.Text.ToLower())).ToList();

            if (comboBox1.SelectedIndex != -1)
            {
                int idPerfilSeleccionado = ObtenerIdPerfilSeleccionado();
                listaFiltrada = listaFiltrada.Where(u => u.IdPerfil == idPerfilSeleccionado).ToList();
            }

            MostrarGrilla(listaFiltrada);
            radioButton3.Checked = false;
            radioButton4.Checked = false;
        }

        private void FormUsuario_FormClosing(object sender, FormClosingEventArgs e)
        {
            bllIdioma.BorrarSuscriptor(this);
        }
    }
}
