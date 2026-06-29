using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLPerfil
    {
        private DALPerfil dalPerfil = new DALPerfil();

        public ServicioPerfil CargarPerfilUsuario(int idPerfilUsuario)
        {
            if (idPerfilUsuario <= 0) return null;

            return dalPerfil.ObtenerPerfilUsuario(idPerfilUsuario);
        }

        public List<ServicioPerfil> ObtenerPerfiles()
        {
            return dalPerfil.ObtenerPerfiles();
        }

        public bool TienePermiso(ServicioUsuario usuario, string nombrePermisoABuscar)
        {
            if (usuario == null || usuario.PerfilUsuario == null) return false;
            if (string.IsNullOrEmpty(nombrePermisoABuscar)) return false;

            ServicioPerfil encontrado = usuario.PerfilUsuario.Buscar(nombrePermisoABuscar);

            return encontrado != null;
        }

        public void CrearPerfil(string nombrePerfil, List<ServicioPerfil> componentesSeleccionados)
        {
            if (string.IsNullOrWhiteSpace(nombrePerfil))
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("El nombre del perfil no puede estar vacío."));

            if (componentesSeleccionados == null || componentesSeleccionados.Count == 0)
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("No se puede crear un perfil vacío. Debe seleccionar al menos un permiso o familia."));

            ValidarNombrePerfilDisponible(nombrePerfil);

            ServicioFamilia nuevoPerfil = new ServicioFamilia(0, nombrePerfil.Trim());
            int idAsignado = dalPerfil.GuardarPerfil(nuevoPerfil);
            dalPerfil.GuardarRelacionesPerfil(idAsignado, componentesSeleccionados);
            BLLEvento bitacora = new BLLEvento();
            bitacora.GrabarBitacora("Creación de nuevo Perfil", "Perfiles", 1);
        }

        private void ValidarNombrePerfilDisponible(string nombrePerfil)
        {
            bool existe = ObtenerPerfiles()
                .Any(p => p.Nombre.Equals(nombrePerfil.Trim(), StringComparison.OrdinalIgnoreCase));

            if (existe)
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("Ya existe un perfil con el nombre ") + nombrePerfil);
        }

        public void EliminarPerfil(int idPerfil, string nombrePerfil)
        {
            if (dalPerfil.PerfilEstaAsignadoAUsuario(idPerfil))
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("El perfil ") + nombrePerfil + ServicioSessionManager.GetInstance().Traducir("no se puede eliminar porque está asignado actualmente a uno o más usuarios."));
            }
            dalPerfil.EliminarPerfil(idPerfil);
            BLLEvento bitacora = new BLLEvento();
            bitacora.GrabarBitacora("Eliminación de Perfil", "Perfiles", 1);
        }

        public void AgregarPermisoAPerfil(int idPerfilPadre, string nombrePerfil, ServicioPerfil hijo)
        {
            if (hijo == null) throw new Exception(ServicioSessionManager.GetInstance().Traducir("Debe seleccionar un componente válido para agregar."));

            ServicioPerfil perfilCompleto = dalPerfil.ObtenerPerfilUsuario(idPerfilPadre);
            if (perfilCompleto != null)
            {
                ServicioPerfil encontrado = perfilCompleto.Buscar(hijo.Nombre);
                if (encontrado != null && encontrado is ServicioPermiso)
                {
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("El perfil ") + nombrePerfil + ServicioSessionManager.GetInstance().Traducir("ya posee el componente ") + hijo.Nombre + ServicioSessionManager.GetInstance().Traducir("de forma directa o heredada a través de una familia."));
                }
            }
            dalPerfil.AgregarRelacionPerfilPermiso(idPerfilPadre, hijo);
            BLLEvento bitacora = new BLLEvento();
            bitacora.GrabarBitacora("Modificación Perfil", "Perfiles", 1);
        }

        public void QuitarPermisoDePerfil(int idPerfilPadre, string nombrePerfil, ServicioPerfil hijo)
        {
            if (hijo == null) throw new Exception(ServicioSessionManager.GetInstance().Traducir("Debe seleccionar un componente válido para quitar."));

            ServicioPerfil perfilCompleto = dalPerfil.ObtenerPerfilUsuario(idPerfilPadre);
            if (perfilCompleto == null || perfilCompleto.Hijos == null || !perfilCompleto.Hijos.Any(h => h.IdPerfil == hijo.IdPerfil && h.GetType() == hijo.GetType()))
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("El perfil ") + nombrePerfil + ServicioSessionManager.GetInstance().Traducir("no tiene asignado directamente el componente ") + hijo.Nombre + ServicioSessionManager.GetInstance().Traducir(", por lo que no puede ser removido."));
            }

            ValidarQueNoQuedeVacio(idPerfilPadre, nombrePerfil);
            dalPerfil.QuitarRelacionPerfilPermiso(idPerfilPadre, hijo);
            BLLEvento bitacora = new BLLEvento();
            bitacora.GrabarBitacora($"Modificación Perfil", "Perfiles", 1);
        }

        public void ValidarQueNoQuedeVacio(int idPerfil, string nombrePerfil)
        {
            int cantidadComponentes = dalPerfil.ObtenerCantidadHijosPerfil(idPerfil);
            if (cantidadComponentes <= 1)
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("Operación denegada. El perfil ") + nombrePerfil + ServicioSessionManager.GetInstance().Traducir("no puede quedarse vacío. Debe conservar al menos un permiso o familia asignado en su raíz."));
            }
        }

        public void AgregarFamiliaAPerfil(int idPerfilPadre, string nombrePerfil, ServicioFamilia familiaHijo)
        {
            if (familiaHijo == null) throw new Exception(ServicioSessionManager.GetInstance().Traducir("Debe seleccionar una familia válida para agregar."));

            ServicioPerfil perfilCompleto = dalPerfil.ObtenerPerfilUsuario(idPerfilPadre);
            if (perfilCompleto != null)
            {
                if (perfilCompleto.Hijos != null && perfilCompleto.Hijos.Any(h => h.IdPerfil == familiaHijo.IdPerfil && h is ServicioFamilia))
                {
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("El perfil ") + nombrePerfil + ServicioSessionManager.GetInstance().Traducir("ya posee la familia ") + familiaHijo.Nombre + ServicioSessionManager.GetInstance().Traducir("asignada directamente."));
                }

                BLLFamilia bllFamilia = new BLLFamilia();
                List<ServicioFamilia> todasLasFamilias = bllFamilia.ObtenerFamilias();
                ServicioFamilia familiaHijoCompleta = todasLasFamilias.FirstOrDefault(f => f.IdPerfil == familiaHijo.IdPerfil);

                if (familiaHijoCompleta != null && familiaHijoCompleta.Hijos != null)
                {
                    foreach (var componenteHijo in familiaHijoCompleta.Hijos)
                    {
                        ServicioPerfil componenteDuplicado = perfilCompleto.Buscar(componenteHijo.Nombre);
                        if (componenteDuplicado != null && componenteDuplicado is ServicioPermiso)
                        {
                            throw new Exception(ServicioSessionManager.GetInstance().Traducir("No se puede agregar la familia ") + familiaHijo.Nombre + ServicioSessionManager.GetInstance().Traducir("porque contiene el permiso ") + componenteHijo.Nombre + ServicioSessionManager.GetInstance().Traducir(", el cual ya existe en el perfil ") + nombrePerfil);
                        }
                    }
                }
            }

            dalPerfil.AgregarRelacionPerfilFamilia(idPerfilPadre, familiaHijo);
            BLLEvento bitacora = new BLLEvento();
            bitacora.GrabarBitacora("Modificación Perfil", "Perfiles", 1);
        }

        public void QuitarFamiliaDePerfil(int idPerfilPadre, string nombrePerfil, ServicioFamilia familiaHijo)
        {
            if (familiaHijo == null) throw new Exception(ServicioSessionManager.GetInstance().Traducir("Debe seleccionar una familia válida para quitar."));

            ServicioPerfil perfilCompleto = dalPerfil.ObtenerPerfilUsuario(idPerfilPadre);
            if (perfilCompleto == null || perfilCompleto.Hijos == null || !perfilCompleto.Hijos.Any(h => h.IdPerfil == familiaHijo.IdPerfil && h is ServicioFamilia))
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("El perfil " + nombrePerfil + ServicioSessionManager.GetInstance().Traducir("no tiene asignada directamente la familia ") + familiaHijo.Nombre + ServicioSessionManager.GetInstance().Traducir(", por lo que no puede ser removida.")));
            }

            ValidarQueNoQuedeVacio(idPerfilPadre, nombrePerfil);
            dalPerfil.QuitarRelacionPerfilFamilia(idPerfilPadre, familiaHijo);
            BLLEvento bitacora = new BLLEvento();
            bitacora.GrabarBitacora("Modificación Perfil", "Perfiles", 1);
        }
    }
}