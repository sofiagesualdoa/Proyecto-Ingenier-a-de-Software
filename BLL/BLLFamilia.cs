using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLFamilia
    {
        private DALFamilia dalFamilia = new DALFamilia();
        public List<ServicioFamilia> ObtenerFamilias()
        {
            return dalFamilia.ObtenerFamilias();
        }
        public void CrearFamilia(string nombreFamilia, List<ServicioPerfil> componentesSeleccionados)
        {
            if (string.IsNullOrWhiteSpace(nombreFamilia))
                throw new Exception("El nombre de la familia no puede estar vacío.");

            if (componentesSeleccionados == null || componentesSeleccionados.Count == 0)
                throw new Exception("No se puede crear una familia vacía. Debe seleccionar al menos un permiso o familia hijo.");

            ServicioFamilia nuevaFamilia = new ServicioFamilia(0, nombreFamilia.Trim());
            int idAsignado = dalFamilia.GuardarFamilia(nuevaFamilia);
            dalFamilia.GuardarRelacionesFamilia(idAsignado, componentesSeleccionados);
            BLLEvento bitacora = new BLLEvento();
            bitacora.GrabarBitacora("Creación de nueva Familia", "Perfiles", 1);
        }

        public void EliminarFamilia(int idFamilia, string nombreFamilia)
        {
            DAL.DALPerfil dalPerfil = new DAL.DALPerfil();
            List<int> perfilesVacios = dalPerfil.ObtenerPerfilesQueQuedarianVaciosPorFamilia(idFamilia);
            if (perfilesVacios.Count > 0)
            {
                throw new Exception($"Operación denegada. La familia '{nombreFamilia}' no se puede eliminar porque es el único componente asignado a uno o más Perfiles de Usuario. Modifique primero esos perfiles para que no queden vacíos.");
            }
            List<int> familiasVacias = dalFamilia.ObtenerFamiliasPadreQueQuedarianVacias(idFamilia);
            if (familiasVacias.Count > 0)
            {
                throw new Exception($"Operación denegada. La familia '{nombreFamilia}' no se puede eliminar porque es el único componente de otra Familia del sistema. Modifique la familia contenedora primero.");
            }
            dalFamilia.EliminarFamilia(idFamilia);
            BLLEvento bitacora = new BLLEvento();
            bitacora.GrabarBitacora("Eliminación de Familia", "Perfiles", 1);
        }

        public void AgregarPermisoAFamilia(int idFamiliaPadre, string nombreFamilia, ServicioPerfil hijo)
        {
            if (hijo == null) throw new Exception("Debe seleccionar un componente válido para agregar.");

            BLLFamilia bllFamilia = new BLLFamilia();
            List<ServicioFamilia> todasLasFamilias = bllFamilia.ObtenerFamilias();
            ServicioFamilia familiaCompleta = todasLasFamilias.FirstOrDefault(f => f.IdPerfil == idFamiliaPadre);

            if (familiaCompleta != null)
            {
                ServicioPerfil encontrado = familiaCompleta.Buscar(hijo.Nombre);
                if (encontrado != null)
                {
                    throw new Exception($"La familia '{nombreFamilia}' ya posee el componente '{hijo.Nombre}' de forma directa o heredada.");
                }
            }

            dalFamilia.AgregarRelacionFamiliaPermiso(idFamiliaPadre, hijo);
            BLLEvento bitacora = new BLLEvento();
            bitacora.GrabarBitacora("Modificación Familia", "Perfiles", 1);
        }

        public void QuitarPermisoDeFamilia(int idFamiliaPadre, string nombreFamilia, ServicioPerfil hijo)
        {
            if (hijo == null) throw new Exception("Debe seleccionar un componente válido para quitar.");

            BLLFamilia bllFamilia = new BLLFamilia();
            List<ServicioFamilia> todasLasFamilias = bllFamilia.ObtenerFamilias();
            ServicioFamilia familiaCompleta = todasLasFamilias.FirstOrDefault(f => f.IdPerfil == idFamiliaPadre);

            if (familiaCompleta == null || familiaCompleta.Hijos == null || !familiaCompleta.Hijos.Any(h => h.IdPerfil == hijo.IdPerfil && h.GetType() == hijo.GetType()))
            {
                throw new Exception($"La familia '{nombreFamilia}' no tiene asignado directamente el componente '{hijo.Nombre}', por lo que no puede ser removido.");
            }

            ValidarQueFamiliaNoQuedeVacia(idFamiliaPadre, nombreFamilia);
            dalFamilia.QuitarRelacionFamiliaPermiso(idFamiliaPadre, hijo);
            BLLEvento bitacora = new BLLEvento();
            bitacora.GrabarBitacora("Modificación Familia", "Perfiles", 1);
        }

        public void ValidarQueFamiliaNoQuedeVacia(int idFamilia, string nombreFamilia)
        {
            int cantidadComponentes = dalFamilia.ObtenerCantidadHijosFamilia(idFamilia);
            if (cantidadComponentes <= 1)
            {
                throw new Exception($"Operación denegada. La familia '{nombreFamilia}' no puede quedarse vacía. Debe conservar al menos un permiso o subfamilia asignado.");
            }
        }
    }
}
