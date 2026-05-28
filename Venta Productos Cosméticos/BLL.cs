using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Venta_Productos_Cosméticos.BE;
using static Venta_Productos_Cosméticos.BLL;
using static Venta_Productos_Cosméticos.DAL;
using static Venta_Productos_Cosméticos.Servicios;

namespace Venta_Productos_Cosméticos
{
    internal class BLL
    {
        public class BLLUsuario
        {

            Servicios.BitacoraEventos bitacora = new Servicios.BitacoraEventos();

            public void ModificarClave(string claveActual, string claveNueva)
            {
                try
                {
                    BE.Usuario usuarioActivo = Servicios.SessionManager.GetInstance().ObtenerUsuario();
                    Servicios.Encriptador encriptador = new Servicios.Encriptador();
                    string hashClaveActual = encriptador.Encriptar(claveActual);
                    if (!usuarioActivo.ValidarPassword(hashClaveActual))
                    {
                        throw new Exception("La contraseña actual ingresada es incorrecta.");
                    }
                    string hashClaveNueva = encriptador.Encriptar(claveNueva);
                    DAL.DALUsuario dal = new DAL.DALUsuario();
                    dal.GuardarNuevaClave(usuarioActivo.nombreUsuario, hashClaveNueva);
                    usuarioActivo.ActualizarPasswordMemoria(hashClaveNueva);
                    bitacora.GrabarBitacora("Cambiar Clave", "Usuario", 1);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error en la capa de negocio al intentar cambiar la contraseña: " + ex.Message);
                }
            }

            public bool IniciarSesion(string nombreUsuario, string passwordIngresado)
            {
                Encriptador encriptador = new Encriptador();
                string hashIngresado = encriptador.Encriptar(passwordIngresado);

                DALUsuario dal = new DALUsuario();
                Usuario usuario = dal.ObtenerUsuario(nombreUsuario);

                if (usuario == null)
                {
                    throw new Exception("El nombre de usuario ingresado no existe.");
                }
                if (usuario.Bloqueado)
                {
                    throw new Exception("La cuenta se encuentra bloqueada por seguridad. Contacte a un administrador.");
                }
                if (!usuario.Activo)
                {
                    throw new Exception("El usuario se encuentra dado de baja en el sistema.");
                }
                if (!usuario.ValidarPassword(hashIngresado))
                {
                    
                    dal.SumarIntentoFallido(nombreUsuario);

                    if ((usuario.IntentosInicio + 1) >= 3)
                    {
                        dal.BloquearUsuario(usuario.DNI);
                        bitacora.GrabarBitacora($"Bloquear Usuario: {nombreUsuario}", "Usuario", 1);
                        throw new Exception("La cuenta ha sido bloqueada de forma automática por superar los 3 intentos fallidos.");
                    }

                    throw new Exception("La contraseña ingresada es incorrecta.");
                }

                if (usuario.IntentosInicio > 0)
                {
                    dal.DesbloquearUsuario(usuario.DNI);
                }
                dal.CargarPermisos(usuario);
                SessionManager.GetInstance().IniciarSesion(usuario);
                bitacora.GrabarBitacora("Login", "Usuario", 1);
                return true;
            }

            public bool CerrarSesion()
            {
                Usuario usuarioActivo = SessionManager.GetInstance().ObtenerUsuario();
                if (usuarioActivo != null)
                {
                    Servicios.BitacoraEventos bitacora = new Servicios.BitacoraEventos();
                    bitacora.GrabarBitacora("Logout", "Usuario", 1);
                    SessionManager.GetInstance().CerrarSesion();                    
                    return true;
                }
                return false;
            }

            public void CrearUsuario(Usuario usuario)
            {
                if (string.IsNullOrWhiteSpace(usuario.Nombre) ||
                    string.IsNullOrWhiteSpace(usuario.Apellido) ||
                    string.IsNullOrWhiteSpace(usuario.Email) ||
                    string.IsNullOrWhiteSpace(usuario.nombreUsuario) ||
                    string.IsNullOrWhiteSpace(usuario.Rol))
                {
                    throw new Exception("Debe completar todos los campos.");
                }

                DALUsuario dal = new DALUsuario();

                Usuario existente = dal.BuscarUsuarioPorDniOMail(
                    usuario.DNI,
                    usuario.Email);

                if (existente != null)
                {
                    throw new Exception("Ya existe un usuario con ese DNI o email.");
                }

                string clavePlana = usuario.DNI.ToString() + usuario.Apellido;
                Encriptador encriptador = new Encriptador();
                string hashClave = encriptador.Encriptar(clavePlana);
                usuario.SetPassword(hashClave);
                usuario.Bloqueado = false;
                usuario.IntentosInicio = 0;
                dal.GuardarUsuario(usuario);
                bitacora.GrabarBitacora($"Crear Usuario", "Usuario", 1);
            }


            public List<Usuario> ObtenerUsuarios()
            {
                DALUsuario dal = new DALUsuario();
                return dal.ObtenerUsuarios();
            }

            public void DesbloquearUsuario(int dni)
            {
                DALUsuario dal = new DALUsuario();
                Usuario usuario = dal.BuscarUsuarioPorDniOMail(dni, "x");

                if (usuario == null)
                {
                    throw new Exception("El usuario no existe.");
                }

                if (!usuario.Bloqueado)
                {
                    throw new Exception(
                        "El usuario ya se encuentra desbloqueado.");
                }

                dal.DesbloquearUsuario(dni); 
                bitacora.GrabarBitacora($"Desbloquear Usuario: {usuario.nombreUsuario}", "Usuario", 1);
            }
            public void ModificarUsuario(Usuario usuarioModificado)
            {
                DALUsuario dal = new DALUsuario();

                Usuario usuarioExistente =
                    dal.BuscarUsuarioPorDniOMail(usuarioModificado.DNI, "x");

                if (usuarioExistente == null)
                {
                    throw new Exception("El usuario no existe.");
                }

                dal.ModificarUsuario(usuarioModificado);
                bitacora.GrabarBitacora($"Modificar Usuario: {usuarioModificado.nombreUsuario}", "Usuario", 1);
            }

            public bool ModificarEstado(int DNIUsuarioSeleccionado)
            {
                Usuario logueado = Servicios.SessionManager.GetInstance().ObtenerUsuario();
                if (logueado != null && logueado.DNI == DNIUsuarioSeleccionado)
                {
                    throw new Exception("Operación inválida. No es posible desactivar la cuenta con la que se encuentra logueado actualmente.");
                }
                DALUsuario dal = new DALUsuario();
                bool exito = dal.ModificarEstado(DNIUsuarioSeleccionado);

                if (exito)
                {
                    Usuario usuarioAfectado = dal.BuscarUsuarioPorDniOMail(DNIUsuarioSeleccionado, "x");
                    bitacora.GrabarBitacora($"Activar / Desactivar Usuario", "Usuario", 1);
                    return true;
                }
                return false;
            }
        }
    }
}
