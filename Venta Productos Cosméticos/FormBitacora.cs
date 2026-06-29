using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Servicios;
using BLL;
using DALs;

namespace Venta_Productos_Cosméticos
{
    public partial class FormBitacora : Form, IObserver
    {

        private BLLIdioma bllIdioma = new BLLIdioma();
        private Dictionary<Control, string> textosOriginales = new Dictionary<Control, string>();
        public FormBitacora()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormSistema frmMenu = new FormSistema();
            frmMenu.Show();
            this.Close();
        }

        private void FormBitacora_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            CargarComboBox();
            RestablecerFiltrosPorDefecto();

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

            foreach (Control control in this.Controls)
            {
                if (control is MenuStrip menuStrip)
                {
                    TraducirMenu(menuStrip.Items, leyendas);
                }
            }

            TraducirCombo(cmbModulo, leyendas);
            TraducirCombo(cmbEvento, leyendas);
            TraducirCombo(cmbCriticidad, leyendas);
            TraducirCombo(cmbLogin, leyendas);
        }

        private void TraducirMenu(ToolStripItemCollection menuItems, Dictionary<string, string> leyendas)
        {
            foreach (ToolStripItem item in menuItems)
            {
                if (leyendas != null && leyendas.ContainsKey(item.Text))
                    item.Text = leyendas[item.Text];

                if (item is ToolStripMenuItem menuItem && menuItem.DropDownItems.Count > 0)
                    TraducirMenu(menuItem.DropDownItems, leyendas);
            }
        }

        private void TraducirCombo(ComboBox combo, Dictionary<string, string> leyendas)
        {

            for (int i = 0; i < combo.Items.Count; i++)
            {
                string itemOriginal = combo.Items[i].ToString();
                if (leyendas != null && leyendas.ContainsKey(itemOriginal))
                    combo.Items[i] = leyendas[itemOriginal];
                else
                    combo.Items[i] = itemOriginal;
            }
        }

        private void CargarComboBox()
        {
            cmbModulo.Items.AddRange(new string[] { "Todos", "Usuario", "Ventas", "Compras", "Maestro", "Perfil" });

            cmbEvento.Items.AddRange(new string[] {
                                                        "Todos",
                                                        "Login",
                                                        "Logout",
                                                        "Crear Usuario",
                                                        "Modificar Usuario",
                                                        "Activar / Desactivar Usuario",
                                                        "Desbloquear Usuario",
                                                        "Bloquear Usuario",
                                                        "Cambiar Clave",
                                                        "Cambio de Idioma",
                                                        "Creación de nueva Familia",
                                                        "Eliminación de Familia",
                                                        "Modificación Familia",
                                                        "Creación de nuevo Perfil",
                                                        "Eliminación de Perfil",
                                                        "Modificación Perfil"
                                                    });

            cmbCriticidad.Items.AddRange(new string[] { "Todos", "1 (Alta)", "2 (Media)", "3 (Baja)" });

            cmbModulo.SelectedIndex = 0;
            cmbEvento.SelectedIndex = 0;
            cmbCriticidad.SelectedIndex = 0;
            CargarComboLogins();
        }

        private void CargarComboLogins()
        {
            try
            {
                BLLUsuario bll = new BLLUsuario();
                cmbLogin.Items.Clear();
                cmbLogin.Items.Add("Todos");

                foreach (var usr in bll.ObtenerUsuarios())
                {
                    if (!cmbLogin.Items.Contains(usr.nombreUsuario))
                    {
                        cmbLogin.Items.Add(usr.nombreUsuario);
                    }
                }
                cmbLogin.SelectedIndex = 0;
            }
            catch (Exception)
            {
                cmbLogin.Items.Add("Todos");
                cmbLogin.SelectedIndex = 0;
            }
        }

        private void RestablecerFiltrosPorDefecto()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            cmbLogin.SelectedIndex = 0;
            cmbModulo.SelectedIndex = 0;
            cmbEvento.SelectedIndex = 0;
            cmbCriticidad.SelectedIndex = 0;
            dtpFechaInicio.Value = DateTime.Today.AddDays(-3);
            dtpFechaFin.Value = DateTime.Today;
            BLLEvento bitacora = new BLLEvento();
            MostrarGrilla(bitacora.ConsultarEventosPorDefecto());
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = true;
                ActualizarDatosUsuarioSeleccionado();
            }
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

        private void ActualizarDatosUsuarioSeleccionado()
        {
            if (dataGridView1.CurrentRow != null)
            {
                ServicioEvento registroSeleccionado = (ServicioEvento)dataGridView1.CurrentRow.DataBoundItem;

                if (registroSeleccionado != null)
                {
                    DALUsuario dalUser = new DALUsuario();
                    ServicioUsuario operario = dalUser.BuscarUsuarioPorDniOMail(registroSeleccionado.DNI, "x");
                    if (operario != null)
                    {
                        txtNombre.Text = operario.Nombre;
                        txtApellido.Text = operario.Apellido;
                    }
                    else
                    {
                        txtNombre.Text = "SISTEMA";
                        txtApellido.Text = "EVERGLOW";
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                DALEvento dal = new DALEvento();
                List<ServicioEvento> eventosFiltrados = dal.ObtenerEventos(dtpFechaInicio.Value.Date);
                eventosFiltrados = eventosFiltrados.Where(evt => evt.Fecha.Date <= dtpFechaFin.Value.Date).ToList();
                if (cmbLogin.Text != "Todos" && cmbLogin.SelectedIndex != -1)
                    eventosFiltrados = eventosFiltrados.Where(evt => evt.Login == cmbLogin.Text).ToList();
                if (cmbModulo.Text != "Todos" && cmbModulo.SelectedIndex != -1)
                    eventosFiltrados = eventosFiltrados.Where(evt => evt.Modulo == cmbModulo.Text).ToList();
                if (cmbEvento.Text != "Todos" && cmbEvento.SelectedIndex != -1)
                    eventosFiltrados = eventosFiltrados.Where(evt => evt.NombreEvento.StartsWith(cmbEvento.Text)).ToList();
                if (cmbCriticidad.Text != "Todos" && cmbCriticidad.SelectedIndex != -1)
                {
                    int nivelBuscar = int.Parse(cmbCriticidad.Text.Substring(0, 1));
                    eventosFiltrados = eventosFiltrados.Where(evt => evt.Criticidad == nivelBuscar).ToList();
                }
                MostrarGrilla(eventosFiltrados);
            }
            catch (Exception ex)
            {
                string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Error al aplicar filtros: ") + errorTraducido, ServicioSessionManager.GetInstance().Traducir("Error de Consulta"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RestablecerFiltrosPorDefecto();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ActualizarDatosUsuarioSeleccionado();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("No existen registros en la grilla actual para exportar a PDF."),
                                ServicioSessionManager.GetInstance().Traducir("Atención"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveFileDialog selectorDestino = new SaveFileDialog();
            selectorDestino.Filter = "Documento PDF (*.pdf)|*.pdf";
            selectorDestino.FileName = $"Reporte_Auditoria_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            if (selectorDestino.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Document documentoPdf = new Document(PageSize.A4.Rotate(), 20f, 20f, 30f, 30f);

                    using (FileStream fs = new FileStream(selectorDestino.FileName, FileMode.Create))
                    {
                        PdfWriter.GetInstance(documentoPdf, fs);
                        documentoPdf.Open();

                        iTextSharp.text.Font fuenteTitulo = FontFactory.GetFont("Segoe UI", 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                        iTextSharp.text.Font fuenteSubtitulo = FontFactory.GetFont("Segoe UI", 10, iTextSharp.text.Font.ITALIC, BaseColor.DARK_GRAY);
                        iTextSharp.text.Font fuenteCabeceraTabla = FontFactory.GetFont("Segoe UI", 9, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                        iTextSharp.text.Font fuenteCuerpoTabla = FontFactory.GetFont("Segoe UI", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                        Paragraph titulo = new Paragraph("EVERGLOW COSMÉTICOS", fuenteTitulo);
                        titulo.Alignment = Element.ALIGN_LEFT;
                        documentoPdf.Add(titulo);

                        Paragraph subtitulo = new Paragraph($"Reporte Oficial de Auditoría de Sistemas e Historial de Bitácora\nFecha de emisión: {DateTime.Now:dd/MM/yyyy HH:mm:ss}\n\n", fuenteSubtitulo);
                        subtitulo.Alignment = Element.ALIGN_LEFT;
                        documentoPdf.Add(subtitulo);

                        PdfPTable tablaPdf = new PdfPTable(7);
                        tablaPdf.WidthPercentage = 100;
                        float[] anchosColumnas = new float[] { 8f, 15f, 12f, 13f, 24f, 13f, 15f };
                        tablaPdf.SetWidths(anchosColumnas);

                        string[] cabeceras = { "ID Evento", "Fecha y Hora", "Usuario", "Módulo", "Acción / Evento", "Criticidad", "DNI" };
                        foreach (string columna in cabeceras)
                        {
                            PdfPCell celdaCabecera = new PdfPCell(new Phrase(columna, fuenteCabeceraTabla));
                            celdaCabecera.BackgroundColor = new BaseColor(45, 55, 72);
                            celdaCabecera.HorizontalAlignment = Element.ALIGN_CENTER;
                            celdaCabecera.Padding = 6f;
                            tablaPdf.AddCell(celdaCabecera);
                        }

                        foreach (DataGridViewRow fila in dataGridView1.Rows)
                        {
                            ServicioEvento registro = (ServicioEvento)fila.DataBoundItem;

                            if (registro != null)
                            {
                                tablaPdf.AddCell(new PdfPCell(new Phrase(registro.IdEvento.ToString(), fuenteCuerpoTabla)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4f });
                                tablaPdf.AddCell(new PdfPCell(new Phrase($"{registro.Fecha:dd/MM/yyyy} {registro.Hora}", fuenteCuerpoTabla)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                tablaPdf.AddCell(new PdfPCell(new Phrase(registro.Login ?? "SISTEMA", fuenteCuerpoTabla)) { PaddingLeft = 5f });
                                tablaPdf.AddCell(new PdfPCell(new Phrase(registro.Modulo, fuenteCuerpoTabla)) { PaddingLeft = 5f });
                                tablaPdf.AddCell(new PdfPCell(new Phrase(registro.NombreEvento, fuenteCuerpoTabla)) { PaddingLeft = 5f });
                                tablaPdf.AddCell(new PdfPCell(new Phrase(registro.Criticidad.ToString(), fuenteCuerpoTabla)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                tablaPdf.AddCell(new PdfPCell(new Phrase(registro.DNI.ToString(), fuenteCuerpoTabla)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            }
                        }
                        documentoPdf.Add(tablaPdf);
                        documentoPdf.Close();
                    }

                    MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("El reporte de auditoría en PDF ha sido generado y guardado correctamente."),
                                    ServicioSessionManager.GetInstance().Traducir("Impresión Exitosa"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                    MessageBox.Show(ServicioSessionManager.GetInstance().Traducir("Error físico al construir el documento PDF: ") + errorTraducido ,
                                    ServicioSessionManager.GetInstance().Traducir("Error de Exportación"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void FormBitacora_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void FormBitacora_FormClosing(object sender, FormClosingEventArgs e)
        {
            bllIdioma.BorrarSuscriptor(this);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
