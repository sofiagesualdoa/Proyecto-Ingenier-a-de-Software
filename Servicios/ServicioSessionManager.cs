using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioSessionManager
    {
        private static ServicioSessionManager _instance;
        private ServicioUsuario _usuarioActivo;
        private static object _lock = new Object();
        private ServicioSessionManager() { }
        public static ServicioSessionManager GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new ServicioSessionManager();
                }
            }
            return _instance;
        }
        public void IniciarSesion(ServicioUsuario usuario)
        {
            _usuarioActivo = usuario;
        }
        public ServicioUsuario ObtenerUsuario()
        {
            return _usuarioActivo;
        }
        public void CerrarSesion()
        {
            _usuarioActivo = null;
        }
    }
}
