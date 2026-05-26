using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Venta_Productos_Cosméticos.BE;
using static Venta_Productos_Cosméticos.BLL;

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
            groupBox1.Text = "Modo Consulta";

            comboBox1.Items.Add("Administrador");
            comboBox1.Items.Add("Vendedor");
            comboBox1.Items.Add("Supervisor");

            comboBox1.SelectedIndex = 0;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;  
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            modo = "Añadir";

            groupBox1.Text = "Modo Añadir";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            modo = "Desbloquear";

            groupBox1.Text = "Modo Desbloquear";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            modo = "Modificar";

            groupBox1.Text = "Modo Modificar";

            if (dataGridView1.CurrentRow != null)
            {
                textBox1.Text =
                    dataGridView1.CurrentRow.Cells["DNI"]
                    .Value.ToString();

                textBox2.Text =
                    dataGridView1.CurrentRow.Cells["Nombre"]
                    .Value.ToString();

                textBox3.Text =
                    dataGridView1.CurrentRow.Cells["nombreUsuario"]
                    .Value.ToString();

                textBox4.Text =
                    dataGridView1.CurrentRow.Cells["Apellido"]
                    .Value.ToString();

                textBox5.Text =
                    dataGridView1.CurrentRow.Cells["Email"]
                    .Value.ToString();

                comboBox1.Text =
                    dataGridView1.CurrentRow.Cells["Rol"]
                    .Value.ToString();

                bool activo = Convert.ToBoolean(
                    dataGridView1.CurrentRow.Cells["Activo"].Value);

                if (activo)
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (modo == "Añadir")
            {
                try
                {
                    BLLUsuario bll = new BLLUsuario();

                    Usuario usuario = new Usuario();

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
                    BLLUsuario bll = new BLLUsuario();

                    Usuario usuario = new Usuario();

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

                    BLLUsuario bll = new BLLUsuario();

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
        }

        private void button6_Click(object sender, EventArgs e)
        {

            modo = "Cancelar";

            groupBox1.Text = "Modo Cancelar";
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

            if (dataGridView1.CurrentRow != null)
            {
                int DNISeleccionado = Convert.ToInt32(dataGridView1.CurrentRow.Cells["DNI"].Value);
                try
                {
                    BLLUsuario bll = new BLLUsuario();
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

        private void MostrarGrilla(Object lista)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = lista;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            BLLUsuario bll = new BLLUsuario();
            List<Usuario> usuarios = bll.ObtenerUsuarios();
            if (radioButton3.Checked)
            {
                MostrarGrilla(usuarios);
            }
            else if (radioButton4.Checked)
            {
                MostrarGrilla(usuarios.Where(u => u.Activo).ToList());
            }
        }
    }
}
