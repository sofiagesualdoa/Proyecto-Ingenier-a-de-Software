using BLL;
using Microsoft.VisualBasic;
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
    public partial class FormPerfil : Form, IObserver
    {
        public FormPerfil()
        {
            InitializeComponent();
        }

        private BLLPerfil bllPerfil = new BLLPerfil();
        private BLLFamilia bllFamilia = new BLLFamilia();
        private BLLPermiso bllPermiso = new BLLPermiso();
        private BLLEvento bitacora = new BLLEvento();
        private List<ServicioPermiso> todosLosPermisos = new List<ServicioPermiso>();
        private List<ServicioFamilia> todasLasFamilias = new List<ServicioFamilia>();
        private List<ServicioPerfil> todosLosPerfiles = new List<ServicioPerfil>();
        private BLLIdioma bllIdioma = new BLLIdioma();
        private Dictionary<Control, string> textosOriginales = new Dictionary<Control, string>();

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormSistema frmMenu = new FormSistema();
            frmMenu.Show();
            this.Close();
        }

        private void FormPerfil_Load(object sender, EventArgs e)
        {
            CargarGrillas();
            CargarTreeView();
            dgvFamilia.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPerfil.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPermiso.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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

        private void MostrarGrilla(DataGridView pGrilla, Object pVista)
        {
            pGrilla.DataSource = null;
            pGrilla.DataSource = pVista;
            pGrilla.ClearSelection();
        }

        private void CargarGrillas()
        {
            try
            {
                todosLosPerfiles = bllPerfil.ObtenerPerfiles();
                todasLasFamilias = bllFamilia.ObtenerFamilias();
                todosLosPermisos = bllPermiso.ObtenerPermisos();
                MostrarGrilla(dgvPerfil, todosLosPerfiles);
                MostrarGrilla(dgvFamilia, todasLasFamilias);
                MostrarGrilla(dgvPermiso, todosLosPermisos);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las grillas independientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarTreeView()
        {
            try
            {
                treeView1.Nodes.Clear();
                List<ServicioPerfil> listaRaiz = bllPerfil.ObtenerPerfiles();
                foreach (var perfilBase in listaRaiz)
                {
                    ServicioPerfil perfilCompleto = bllPerfil.CargarPerfilUsuario(perfilBase.IdPerfil);
                    if (perfilCompleto != null)
                    {
                        TreeNode nodoRaiz = new TreeNode(perfilCompleto.Nombre);
                        nodoRaiz.Tag = perfilCompleto;
                        if (perfilCompleto.Hijos != null && perfilCompleto.Hijos.Count > 0)
                        {
                            ArmarNodosTreeView(nodoRaiz, perfilCompleto.Hijos);
                        }
                        treeView1.Nodes.Add(nodoRaiz);
                    }
                }
                treeView1.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al armar el árbol de perfiles: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ArmarNodosTreeView(TreeNode nodoPadre, List<ServicioPerfil> hijos)
        {
            foreach (var hijo in hijos)
            {
                TreeNode nodoHijo = new TreeNode(hijo.Nombre);
                nodoHijo.Tag = hijo;
                if (hijo.Hijos != null && hijo.Hijos.Count > 0)
                {
                    ArmarNodosTreeView(nodoHijo, hijo.Hijos);
                }
                nodoPadre.Nodes.Add(nodoHijo);
            }
        }

        private void btnCrearPerfil_Click(object sender, EventArgs e)
        {

            try
            {
                if (dgvFamilia.SelectedRows.Count == 0 && dgvPermiso.SelectedRows.Count == 0)
                {
                    throw new Exception("No se han seleccionado familias o permisos. Se debe seleccionar algo para crear un nuevo perfil o familia.");
                }
                List<ServicioPerfil> seleccionados = ObtenerComponentesSeleccionadosDeGrillas();
                string nombre = Interaction.InputBox("Ingrese el nombre del nuevo perfil:", "Crear Perfil", "NuevoPerfil");
                bllPerfil.CrearPerfil(nombre, seleccionados);
                MessageBox.Show("Perfil creado con éxito junto a sus componentes básicos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrillas();
                CargarTreeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación / Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCrearFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvFamilia.SelectedRows.Count == 0 && dgvPermiso.SelectedRows.Count == 0)
                {
                    throw new Exception("No se han seleccionado familias o permisos. Se debe seleccionar algo para crear un nuevo perfil o familia.");
                }
                List<ServicioPerfil> seleccionados = ObtenerComponentesSeleccionadosDeGrillas();
                string nombre = Interaction.InputBox("Ingrese el nombre de la nueva familia:", "Crear Familia", "NuevaFamilia");
                bllFamilia.CrearFamilia(nombre, seleccionados);
                MessageBox.Show("Familia creada con éxito junto a sus componentes básicos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrillas();
                CargarTreeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación / Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnQuitarPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPerfil.SelectedRows.Count == 0)
                {
                    throw new Exception("Debe seleccionar el perfil que desea eliminar en la grilla de perfiles.");
                }
                var perfilSeleccionado = (ServicioPerfil)dgvPerfil.SelectedRows[0].DataBoundItem;
                DialogResult result = MessageBox.Show($"¿Está seguro de que desea eliminar el perfil '{perfilSeleccionado.Nombre}'? Esta acción no se puede deshacer.",
                    "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    bllPerfil.EliminarPerfil(perfilSeleccionado.IdPerfil, perfilSeleccionado.Nombre);
                    MessageBox.Show("Perfil eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrillas();
                    CargarTreeView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al eliminar perfil", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<ServicioPerfil> ObtenerComponentesSeleccionadosDeGrillas()
        {
            List<ServicioPerfil> lista = new List<ServicioPerfil>();
            if (dgvFamilia.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow fila in dgvFamilia.SelectedRows)
                {
                    if (fila.DataBoundItem is ServicioFamilia fam)
                    {
                        lista.Add(fam);
                    }
                }
            }
            if (dgvPermiso.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow fila in dgvPermiso.SelectedRows)
                {
                    if (fila.DataBoundItem is ServicioPermiso perm)
                    {
                        lista.Add(perm);
                    }
                }
            }
            return lista;
        }

        private List<ServicioPermiso> ObtenerPermisosSeleccionadosDeGrilla()
        {
            List<ServicioPermiso> lista = new List<ServicioPermiso>();

            if (dgvPermiso.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow fila in dgvPermiso.SelectedRows)
                {
                    if (fila.DataBoundItem is ServicioPermiso perm)
                    {
                        lista.Add(perm);
                    }
                }
            }
            return lista;
        }

        private void btnQuitarFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvFamilia.SelectedRows.Count == 0)
                {
                    throw new Exception("Debe seleccionar la familia que desea eliminar en la grilla de familias.");
                }
                var familiaSeleccionada = (ServicioFamilia)dgvFamilia.SelectedRows[0].DataBoundItem;
                DialogResult result = MessageBox.Show($"¿Está seguro de que desea eliminar la familia '{familiaSeleccionada.Nombre}'? Se desvinculará de todos los perfiles y subfamilias.",
                    "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    bllFamilia.EliminarFamilia(familiaSeleccionada.IdPerfil, familiaSeleccionada.Nombre);
                    MessageBox.Show("Familia eliminada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrillas();
                    CargarTreeView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al eliminar familia", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarPermPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                ServicioPerfil perfilPadre = ObtenerPerfilPadreDesdeTreeView();
                List<ServicioPermiso> permisosElegidos = ObtenerPermisosSeleccionadosDeGrilla();

                if (permisosElegidos.Count == 0)
                    throw new Exception("Debe seleccionar al menos un permiso de la grilla de permisos para agregar.");

                foreach (var permiso in permisosElegidos)
                {
                    bllPerfil.AgregarPermisoAPerfil(perfilPadre.IdPerfil, perfilPadre.Nombre, permiso);
                }

                MessageBox.Show("Permiso(s) agregado(s) con éxito al perfil.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrillas();
                CargarTreeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación / Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnQuitarPermPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                ServicioPerfil perfilPadre = ObtenerPerfilPadreDesdeTreeView();
                List<ServicioPermiso> permisosAQuitar = ObtenerPermisosSeleccionadosDeGrilla();

                if (permisosAQuitar.Count == 0)
                    throw new Exception("Debe seleccionar al menos un permiso de la grilla de permisos para quitar.");

                foreach (var permiso in permisosAQuitar)
                {
                    bllPerfil.QuitarPermisoDePerfil(perfilPadre.IdPerfil, perfilPadre.Nombre, permiso);
                }

                MessageBox.Show("Permiso(s) removido(s) con éxito del perfil.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrillas();
                CargarTreeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación de Integridad / Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAgregarPermFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                ServicioFamilia familiaPadre = ObtenerFamiliaPadreDesdeTreeView();
                List<ServicioPerfil> componentesElegidos = ObtenerComponentesSeleccionadosParaFamilia();

                if (componentesElegidos.Count == 0)
                    throw new Exception("Debe seleccionar al menos una familia o un permiso para agregar.");

                foreach (var componente in componentesElegidos)
                {
                    if (componente is ServicioFamilia familiaHija && familiaHija.IdPerfil == familiaPadre.IdPerfil)
                        throw new Exception("Una familia no puede agregarse a sí misma.");

                    bllFamilia.AgregarPermisoAFamilia(familiaPadre.IdPerfil, familiaPadre.Nombre, componente);
                }

                MessageBox.Show("Componente(s) agregado(s) con éxito a la familia.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrillas();
                CargarTreeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación / Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnQuitarPermFamilia_Click(object sender, EventArgs e)
        {
            try
            {
                ServicioFamilia familiaPadre = ObtenerFamiliaPadreDesdeTreeView();
                List<ServicioPerfil> componentesAQuitar = ObtenerComponentesSeleccionadosParaFamilia();

                if (componentesAQuitar.Count == 0)
                    throw new Exception("Debe seleccionar al menos una familia o un permiso para quitar.");

                foreach (var componente in componentesAQuitar)
                {
                    bllFamilia.QuitarPermisoDeFamilia(familiaPadre.IdPerfil, familiaPadre.Nombre, componente);
                }

                MessageBox.Show("Componente(s) removido(s) con éxito de la familia.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrillas();
                CargarTreeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación de Integridad / Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnAgregarFamPerfil_Click_1(object sender, EventArgs e)
        {
            try
            {
                ServicioPerfil perfilPadre = ObtenerPerfilPadreDesdeTreeView();
                List<ServicioFamilia> familiasElegidas = ObtenerFamiliasSeleccionadasDeGrilla();

                if (familiasElegidas.Count == 0)
                    throw new Exception("Debe seleccionar al menos una familia de la grilla.");

                foreach (var familia in familiasElegidas)
                {
                    bllPerfil.AgregarFamiliaAPerfil(perfilPadre.IdPerfil, perfilPadre.Nombre, familia);
                }

                MessageBox.Show("Familia(s) agregada(s) con éxito al perfil.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrillas();
                CargarTreeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación / Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnQuitarFamPerfil_Click_1(object sender, EventArgs e)
        {
            try
            {
                ServicioPerfil perfilPadre = ObtenerPerfilPadreDesdeTreeView();
                List<ServicioFamilia> familiasAQuitar = ObtenerFamiliasSeleccionadasDeGrilla();

                if (familiasAQuitar.Count == 0)
                    throw new Exception("Debe seleccionar al menos una familia de la grilla.");

                foreach (var familia in familiasAQuitar)
                {
                    bllPerfil.QuitarFamiliaDePerfil(perfilPadre.IdPerfil, perfilPadre.Nombre, familia);
                }

                MessageBox.Show("Familia(s) removida(s) con éxito del perfil.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrillas();
                CargarTreeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación de Integridad / Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormPerfil_FormClosing(object sender, FormClosingEventArgs e)
        {
            bllIdioma.BorrarSuscriptor(this);
        }

        private ServicioPerfil ObtenerPadreDesdeTreeView()
        {
            if (treeView1.SelectedNode == null)
                throw new Exception("Debe seleccionar en el árbol dónde desea agregar o quitar el componente.");

            if (treeView1.SelectedNode.Tag is ServicioPermiso)
                throw new Exception("No puede agregar componentes dentro de un permiso.");

            if (treeView1.SelectedNode.Tag is not ServicioPerfil padre)
                throw new Exception("El nodo seleccionado no es válido.");

            return padre;
        }

        private ServicioPerfil ObtenerPerfilPadreDesdeTreeView()
        {
            ServicioPerfil padre = ObtenerPadreDesdeTreeView();

            if (treeView1.SelectedNode.Level != 0)
                throw new Exception("Debe seleccionar un perfil del árbol.");

            return padre;
        }

        private ServicioFamilia ObtenerFamiliaPadreDesdeTreeView()
        {
            ServicioPerfil padre = ObtenerPadreDesdeTreeView();

            if (treeView1.SelectedNode.Level == 0)
                throw new Exception("Debe seleccionar una familia del árbol, no un perfil.");

            if (padre is not ServicioFamilia familiaPadre)
                throw new Exception("Debe seleccionar una familia válida del árbol.");

            return familiaPadre;
        }

        private List<ServicioFamilia> ObtenerFamiliasSeleccionadasDeGrilla()
        {
            List<ServicioFamilia> lista = new List<ServicioFamilia>();

            foreach (DataGridViewRow fila in dgvFamilia.SelectedRows)
            {
                if (fila.DataBoundItem is ServicioFamilia familia)
                {
                    lista.Add(familia);
                }
            }

            return lista;
        }

        private List<ServicioPerfil> ObtenerComponentesSeleccionadosParaFamilia()
        {
            List<ServicioPerfil> componentes = new List<ServicioPerfil>();

            componentes.AddRange(ObtenerFamiliasSeleccionadasDeGrilla());
            componentes.AddRange(ObtenerPermisosSeleccionadosDeGrilla());

            return componentes;
        }
    }
}
