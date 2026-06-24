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
    public partial class FormPerfil : Form
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
                if (dgvPerfil.SelectedRows.Count == 0)
                {
                    throw new Exception("Debe seleccionar un Perfil en la grilla de perfiles (derecha) al cual desea agregar componentes.");
                }
                ServicioPerfil perfilPadre = (ServicioPerfil)dgvPerfil.SelectedRows[0].DataBoundItem;
                List<ServicioPerfil> componentesElegidos = ObtenerComponentesSeleccionadosDeGrillas();
                if (componentesElegidos.Count == 0)
                {
                    throw new Exception("Debe seleccionar al menos un Permiso de las grillas de la derecha para agregar.");
                }
                foreach (var comp in componentesElegidos)
                {
                    bllPerfil.AgregarPermisoAPerfil(perfilPadre.IdPerfil, perfilPadre.Nombre, comp);
                }
                MessageBox.Show("Componente(s) agregado(s) con éxito al perfil.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (dgvPerfil.SelectedRows.Count == 0)
                {
                    throw new Exception("Debe seleccionar un Perfil en la grilla de perfiles (derecha) al cual desea quitarle componentes.");
                }
                ServicioPerfil perfilPadre = (ServicioPerfil)dgvPerfil.SelectedRows[0].DataBoundItem;
                List<ServicioPerfil> componentesAQuitar = ObtenerComponentesSeleccionadosDeGrillas();
                if (componentesAQuitar.Count == 0)
                {
                    throw new Exception("Debe seleccionar al menos una Familia o un Permiso de las grillas de la derecha para quitar del perfil.");
                }
                string nombresComponentes = string.Join(", ", componentesAQuitar.Select(c => c.Nombre));
                DialogResult result = MessageBox.Show($"¿Desea quitar los siguientes componentes: [{nombresComponentes}] del perfil '{perfilPadre.Nombre}'?",
                    "Confirmar Quitar Componentes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    foreach (var comp in componentesAQuitar)
                    {
                        bllPerfil.QuitarPermisoDePerfil(perfilPadre.IdPerfil, perfilPadre.Nombre, comp);
                    }
                    MessageBox.Show("Componente(s) removido(s) con éxito del perfil.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrillas();
                    CargarTreeView();
                }
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
                if (dgvFamilia.SelectedRows.Count == 0)
                {
                    throw new Exception("Debe seleccionar una Familia en la grilla al cual desea agregar componentes.");
                }

                ServicioFamilia familiaPadre = (ServicioFamilia)dgvFamilia.SelectedRows[0].DataBoundItem;
                List<ServicioPermiso> permisosElegidos = ObtenerPermisosSeleccionadosDeGrilla();

                if (permisosElegidos.Count == 0)
                {
                    throw new Exception("Debe seleccionar al menos un Permiso de la grilla de permisos para agregar.");
                }

                foreach (var perm in permisosElegidos)
                {
                    bllFamilia.AgregarPermisoAFamilia(familiaPadre.IdPerfil, familiaPadre.Nombre, perm);
                }

                MessageBox.Show("Permiso(s) agregado(s) con éxito a la familia.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                if (dgvFamilia.SelectedRows.Count == 0)
                {
                    throw new Exception("Debe seleccionar una Familia en la grilla a la cual desea quitarle componentes.");
                }

                ServicioFamilia familiaPadre = (ServicioFamilia)dgvFamilia.SelectedRows[0].DataBoundItem;
                List<ServicioPermiso> permisosAQuitar = ObtenerPermisosSeleccionadosDeGrilla();

                if (permisosAQuitar.Count == 0)
                {
                    throw new Exception("Debe seleccionar al menos un Permiso de la grilla de permisos para quitar.");
                }

                string nombresComponentes = string.Join(", ", permisosAQuitar.Select(p => p.Nombre));
                DialogResult result = MessageBox.Show($"¿Desea quitar los siguientes permisos: [{nombresComponentes}] de la familia '{familiaPadre.Nombre}'?",
                    "Confirmar Acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (var perm in permisosAQuitar)
                    {
                        bllFamilia.QuitarPermisoDeFamilia(familiaPadre.IdPerfil, familiaPadre.Nombre, perm);
                    }

                    MessageBox.Show("Permiso(s) removido(s) con éxito de la familia.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarGrillas();
                    CargarTreeView();
                }
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
                if (dgvPerfil.SelectedRows.Count == 0)
                {
                    throw new Exception("Debe seleccionar un Perfil en la grilla al cual desea agregarle una familia.");
                }
                if (dgvFamilia.SelectedRows.Count == 0)
                {
                    throw new Exception("Debe seleccionar la Familia en la grilla que desea incorporar.");
                }

                ServicioPerfil perfilPadre = (ServicioPerfil)dgvPerfil.SelectedRows[0].DataBoundItem;
                ServicioFamilia familiaHijo = (ServicioFamilia)dgvFamilia.SelectedRows[0].DataBoundItem;

                bllPerfil.AgregarFamiliaAPerfil(perfilPadre.IdPerfil, perfilPadre.Nombre, familiaHijo);

                MessageBox.Show("Familia agregada con éxito al perfil.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (dgvPerfil.SelectedRows.Count == 0)
                {
                    throw new Exception("Debe seleccionar un Perfil en la grilla al cual desea quitarle una familia.");
                }
                if (dgvFamilia.SelectedRows.Count == 0)
                {
                    throw new Exception("Debe seleccionar la Familia en la grilla que desea remover.");
                }

                ServicioPerfil perfilPadre = (ServicioPerfil)dgvPerfil.SelectedRows[0].DataBoundItem;
                ServicioFamilia familiaHijo = (ServicioFamilia)dgvFamilia.SelectedRows[0].DataBoundItem;

                DialogResult result = MessageBox.Show($"¿Desea desasignar la familia '{familiaHijo.Nombre}' del perfil '{perfilPadre.Nombre}'?",
                    "Confirmar Acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bllPerfil.QuitarFamiliaDePerfil(perfilPadre.IdPerfil, perfilPadre.Nombre, familiaHijo);

                    MessageBox.Show("Familia removida con éxito del perfil.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarGrillas();
                    CargarTreeView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación de Integridad / Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
