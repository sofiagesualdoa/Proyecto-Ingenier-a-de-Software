using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Venta_Productos_Cosméticos.BE;
using static Venta_Productos_Cosméticos.DAL;

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
                if (string.IsNullOrEmpty(texto)) return string.Empty;
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytesOriginales = Encoding.UTF8.GetBytes(texto);
                    byte[] bytesHasheados = sha256.ComputeHash(bytesOriginales);

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < bytesHasheados.Length; i++)
                    {
                        sb.Append(bytesHasheados[i].ToString("X2")); 
                    }
                    return sb.ToString();
                }
            }
        }

        public class BitacoraEventos
        {
            private readonly DALEvento dalEvento = new DALEvento();
            public void GrabarBitacora(string accion, string modulo, int nivelCriticidad)
            {
                BE.Evento registro = new BE.Evento();
                registro.NombreEvento = accion;
                registro.Modulo = modulo;
                registro.Criticidad = nivelCriticidad;
                registro.Fecha = DateTime.Today;
                registro.Hora = DateTime.Now.TimeOfDay;

                Usuario usuarioActivo = SessionManager.GetInstance().ObtenerUsuario();
                if (usuarioActivo != null)
                {
                    registro.Login = usuarioActivo.nombreUsuario;
                    registro.DNI = usuarioActivo.DNI;
                }
                else
                {
                    registro.Login = "Desconocido";
                    registro.DNI = 0;
                }
                dalEvento.RegistrarEvento(registro);
            }

            public List<BE.Evento> ConsultarEventosPorDefecto()
            {
                DateTime fechaFiltro = DateTime.Today.AddDays(-3);
                return dalEvento.ObtenerEventos(fechaFiltro);
            }
        }
    }
}
