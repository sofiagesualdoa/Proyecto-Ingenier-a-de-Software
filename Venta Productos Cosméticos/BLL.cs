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
            public void ModificarClave(string claveActual, string claveNueva)
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
                Servicios.BitacoraEventos bitacora = new Servicios.BitacoraEventos();
                bitacora.RegistrarEvento($"El usuario {usuarioActivo.nombreUsuario} modificó su contraseña de acceso de forma exitosa. Fecha y Hora: {DateTime.Now}");
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
                    throw new Exception("La cuenta se encuentra bloqueada temporalmente por seguridad.");
                }
                if (!usuario.Activo)
                {
                    throw new Exception("El usuario se encuentra dado de baja en el sistema.");
                }
                if (!usuario.ValidarPassword(hashIngresado))
                {
                    dal.SumarIntentoFallido(nombreUsuario);
                    throw new Exception("La contraseña ingresada es incorrecta.");
                }
                dal.CargarPermisos(usuario);
                SessionManager.GetInstance().IniciarSesion(usuario);

                return true;
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

                Encriptador encriptador = new Encriptador();

                usuario.SetPassword(
                    encriptador.Encriptar("1234"));

                //usuario.Bloqueado = false;
                usuario.Bloqueado = true;

                dal.GuardarUsuario(usuario);

                BitacoraEventos bitacora = new BitacoraEventos();

                bitacora.RegistrarEvento(
                    $"Se creó el usuario {usuario.nombreUsuario}");
            }


            public List<Usuario> ObtenerUsuarios()
            {
                DALUsuario dal = new DALUsuario();

                return dal.ObtenerUsuarios();
            }



            public void DesbloquearUsuario(int dni)
            {
                DALUsuario dal = new DALUsuario();

                Usuario usuario = dal.ObtenerUsuarioB(dni);

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

                BitacoraEventos bitacora = new BitacoraEventos();

                bitacora.RegistrarEvento($"Se desbloqueó el usuario {usuario.nombreUsuario}");
            }
            public void ModificarUsuario(Usuario usuarioModificado)
            {
                DALUsuario dal = new DALUsuario();

                Usuario usuarioExistente =
                    dal.ObtenerUsuarioB(usuarioModificado.DNI);

                if (usuarioExistente == null)
                {
                    throw new Exception("El usuario no existe.");
                }

                dal.ModificarUsuario(usuarioModificado);

                BitacoraEventos bitacora =
                    new BitacoraEventos();

                bitacora.RegistrarEvento(
                    $"Se modificó el usuario {usuarioModificado.nombreUsuario}");
            }
        }
    }
}
