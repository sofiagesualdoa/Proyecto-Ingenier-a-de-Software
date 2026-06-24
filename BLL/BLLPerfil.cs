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
    }
}
