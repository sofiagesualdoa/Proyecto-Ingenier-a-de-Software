using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLPermiso
    {
        private DALPermiso dalPermiso = new DALPermiso();
        public List<ServicioPermiso> ObtenerPermisos()
        {
            return dalPermiso.ObtenerTodos();
        }
    }
}
