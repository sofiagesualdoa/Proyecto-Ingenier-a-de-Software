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

        public string CodigoIdiomaActual { get; private set; }

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
            if (_usuarioActivo != null && _usuarioActivo.Idioma != null)
            {
                CodigoIdiomaActual = _usuarioActivo.Idioma.CodigoIdioma;
            }
        }

        public ServicioUsuario ObtenerUsuario()
        {
            return _usuarioActivo;
        }

        public void CerrarSesion()
        {
            _usuarioActivo = null;
            CodigoIdiomaActual = null;
        }

        public void CambiarIdiomaSesion(string nuevoCodigoIdioma)
        {
            if (_usuarioActivo != null)
            {
                CodigoIdiomaActual = nuevoCodigoIdioma;
            }
        }

        public string Traducir(string textoBase)
        {
            if (_usuarioActivo?.Idioma?.DiccionarioLeyendas != null &&
                _usuarioActivo.Idioma.DiccionarioLeyendas.TryGetValue(textoBase, out string textoTraducido))
            {
                return textoTraducido;
            }
            return textoBase;
        }
    }
}
