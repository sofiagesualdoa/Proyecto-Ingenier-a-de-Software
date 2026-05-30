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
    public partial class FormUsuario : Form
    {
        private string modo = "Consulta";
        public FormUsuario()
        {
            InitializeComponent();
        }

        private void FormUsuario_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Administrador");
            comboBox1.Items.Add("Vendedor");
            comboBox1.Items.Add("Supervisor");
            comboBox1.SelectedIndex = 0;

            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            RegresarAModoConsulta();
        }

        private void RegresarAModoConsulta()
        {
            modo = "Consulta";
            groupBox1.Text = "Modo Consulta";
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
            groupBox1.Text = "Modo Añadir";
            LimpiarCampos();
            ActivarModoEdicion();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione un usuario de la grilla superior para desbloquear.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            modo = "Desbloquear";
            groupBox1.Text = "Modo Desbloquear";
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
            comboBox1.Text = seleccionado.Rol;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione un usuario de la grilla antes de presionar Modificar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            modo = "Modificar";
            groupBox1.Text = "Modo Modificar";
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
                MessageBox.Show("Error al mapear datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    usuario.Rol = comboBox1.Text;
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
                    MessageBox.Show(
                        "Usuario creado correctamente.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
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
                    usuario.Rol = comboBox1.Text;
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

                    MessageBox.Show(
                        "Usuario modificado correctamente.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
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
                        MessageBox.Show("Seleccione un usuario.");
                        return;
                    }
                    int dni = Convert.ToInt32(dataGridView1.CurrentRow.Cells["DNI"].Value);
                    bll.DesbloquearUsuario(dni);
                    MostrarGrilla(bll.ObtenerUsuarios());

                    MessageBox.Show(
                        "Usuario desbloqueado correctamente.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
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
                            MessageBox.Show("El estado del usuario se actualizó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un usuario de la lista.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            groupBox1.Text = "Modo Activar / Desactivar";
            ServicioUsuario seleccionado = (ServicioUsuario)dataGridView1.CurrentRow.DataBoundItem;
            LlenarTextBox(seleccionado);
            ActivarModoEdicion();
            DeshabilitarTextBox();
        }

        private void MostrarGrilla(Object lista)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = lista;
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
                string rolSeleccionado = comboBox1.Text;
                listaFiltrada = listaFiltrada.Where(u => u.Rol == rolSeleccionado).ToList();
            }

            MostrarGrilla(listaFiltrada);
            radioButton3.Checked = false; 
            radioButton4.Checked = false;
        }
    }
}
