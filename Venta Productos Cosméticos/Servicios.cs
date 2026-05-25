using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venta_Productos_Cosméticos
{
    internal class Servicios
    {
        public class SessionManager
        {
            private static SessionManager _instance;
            private BE.Usuario _usuarioActivo;
            private static object _lock = new Object();
            private SessionManager() { }
            public static SessionManager GetInstance()
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SessionManager();
                    }
                }
                return _instance;
            }
            public void IniciarSesion(BE.Usuario usuario)
            {
                _usuarioActivo = usuario;
            }
            public BE.Usuario ObtenerUsuario()
            {
                return _usuarioActivo;
            }
            public void CerrarSesion()
            {
                _usuarioActivo = null;
            }
        }

        public class Encriptador
        {
            public string Encriptar(string texto)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(texto));
            }
        }

        public class BitacoraEventos
        {
            public void RegistrarEvento(string mensaje)
            {
                Console.WriteLine($"[Bitácora] {DateTime.Now}: {mensaje}");
            }
        }
    }
}
