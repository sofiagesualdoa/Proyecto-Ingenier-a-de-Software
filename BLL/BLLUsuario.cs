using DALs;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLUsuario
    {
        BLLEvento bitacora = new BLLEvento();
        private readonly GeneradorDigVerificador generador = new GeneradorDigVerificador();
        public void ModificarClave(string claveActual, string claveNueva)
        {
            try
            {
                ServicioUsuario usuarioActivo = ServicioSessionManager.GetInstance().ObtenerUsuario();

                ServicioEncriptador encriptador = new ServicioEncriptador();
                string hashClaveActual = encriptador.Encriptar(claveActual);
                if (!usuarioActivo.ValidarPassword(hashClaveActual))
                {
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("La contraseña actual ingresada es incorrecta."));
                }
                string hashClaveNueva = encriptador.Encriptar(claveNueva);

                if (usuarioActivo.GetPassword().ToUpper().Trim() == hashClaveNueva.ToUpper().Trim())
                {
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("La nueva contraseña no puede ser igual a la contraseña actual. Por favor, elija una diferente."));
                }
                DALUsuario dal = new DALUsuario();
                usuarioActivo.ActualizarPasswordMemoria(hashClaveNueva);
                usuarioActivo.DVH = generador.GenerarDVH(usuarioActivo);
                dal.GuardarNuevaClave(usuarioActivo.nombreUsuario, hashClaveNueva, usuarioActivo.DVH);
                new BLLDVV().RecalcularDVVUsuario();
                bitacora.GrabarBitacora("Cambiar Clave", "Usuario", 1);
            }
            catch (Exception ex)
            {
                string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error al intentar cambiar la contraseña: ") + errorTraducido);
            }
        }

        public bool IniciarSesion(string nombreUsuario, string passwordIngresado)
        {
        
            ServicioEncriptador encriptador = new ServicioEncriptador();
            string hashIngresado = encriptador.Encriptar(passwordIngresado);

            DALUsuario dal = new DALUsuario();
            ServicioUsuario usuario = dal.ObtenerUsuario(nombreUsuario);

            if (usuario == null)
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("El nombre de usuario ingresado no existe."));
            }
            if (usuario.Bloqueado)
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("La cuenta se encuentra bloqueada por seguridad. Contacte a un administrador."));
            }
            if (!usuario.Activo)
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("El usuario se encuentra dado de baja en el sistema."));
            }
            if (!usuario.ValidarPassword(hashIngresado))
            {
                usuario.IntentosInicio++;
                usuario.DVH = generador.GenerarDVH(usuario);
                dal.SumarIntentoFallido(usuario.nombreUsuario, usuario.DVH);
                new BLLDVV().RecalcularDVVUsuario();
                if (usuario.IntentosInicio >= 3)
                {
                    usuario.Bloqueado = true;
                    usuario.DVH = generador.GenerarDVH(usuario);
                    dal.BloquearUsuario(usuario.DNI, usuario.DVH);
                    new BLLDVV().RecalcularDVVUsuario();
                    bitacora.GrabarBitacora($"Bloquear Usuario: {nombreUsuario}", "Usuario", 1);
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("La cuenta ha sido bloqueada de forma automática por superar los 3 intentos fallidos."));
                }

                throw new Exception(ServicioSessionManager.GetInstance().Traducir("La contraseña ingresada es incorrecta."));
            }

            if (usuario.IntentosInicio > 0)
            {
                usuario.IntentosInicio = 0;
                usuario.Bloqueado = false;
                usuario.DVH = generador.GenerarDVH(usuario);
                dal.DesbloquearUsuario(usuario.DNI, usuario.DVH);
                new BLLDVV().RecalcularDVVUsuario();
            }

            BLLPerfil bllPerfil = new BLLPerfil();
            usuario.PerfilUsuario = bllPerfil.CargarPerfilUsuario(usuario.IdPerfil);
            
            if (usuario.IdIdioma > 0)
            {
                BLLIdioma bllIdioma = new BLLIdioma();
                usuario.Idioma = bllIdioma.ListarIdiomas().FirstOrDefault(i => i.IdIdioma == usuario.IdIdioma);
                if (usuario.Idioma != null && usuario.Idioma.CodigoIdioma == "en")
                {
                    usuario.Idioma.DiccionarioLeyendas = bllIdioma.ObtenerTraducciones();
                }
            }
            BLLDVV bllDVV = new BLLDVV();
            if (!bllDVV.ValidarIntegridad())
            {
                if (bllPerfil.TienePermiso(usuario, "Administrador"))
                {
                    ServicioSessionManager.GetInstance().IniciarSesion(usuario);
                    throw new Exception("ERROR_INTEGRIDAD_ADMIN");
                }
                throw new Exception("ERROR_INTEGRIDAD_NO_ADMIN");
            }            

            ServicioSessionManager.GetInstance().IniciarSesion(usuario);
            bitacora.GrabarBitacora("Login", "Usuario", 1);
            return true;
        }

        public bool CerrarSesion()
        {
            ServicioUsuario usuarioActivo = ServicioSessionManager.GetInstance().ObtenerUsuario();
            if (usuarioActivo != null)
            {
                BLLEvento bitacora = new BLLEvento();
                bitacora.GrabarBitacora("Logout", "Usuario", 1);
                ServicioSessionManager.GetInstance().CerrarSesion();
                return true;
            }
            return false;
        }

        public void CrearUsuario(ServicioUsuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nombre) ||
                string.IsNullOrWhiteSpace(usuario.Apellido) ||
                string.IsNullOrWhiteSpace(usuario.Email) ||
                string.IsNullOrWhiteSpace(usuario.nombreUsuario) ||
                usuario.IdPerfil <= 0)
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("Debe completar todos los campos."));
            }

            DALUsuario dal = new DALUsuario();

            ServicioUsuario existente = dal.BuscarUsuarioPorDniOMail(
                usuario.DNI,
                usuario.Email);

            if (existente != null)
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("Ya existe un usuario con ese DNI o email."));
            }

            string clavePlana = usuario.DNI.ToString() + usuario.Apellido;
            ServicioEncriptador encriptador = new ServicioEncriptador();
            string hashClave = encriptador.Encriptar(clavePlana);
            usuario.SetPassword(hashClave);
            usuario.Bloqueado = false;
            usuario.IntentosInicio = 0;
            usuario.DVH = generador.GenerarDVH(usuario);
            dal.GuardarUsuario(usuario);
            new BLLDVV().RecalcularDVVUsuario();
            bitacora.GrabarBitacora("Crear Usuario", "Usuario", 1);
        }


        public List<ServicioUsuario> ObtenerUsuarios()
        {
            DALUsuario dal = new DALUsuario();
            return dal.ObtenerUsuarios();
        }

        public void DesbloquearUsuario(int dni)
        {
            DALUsuario dal = new DALUsuario();
            ServicioUsuario usuario = dal.BuscarUsuarioPorDniOMail(dni, "x");

            if (usuario == null)
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("El usuario no existe."));
            }

            if (!usuario.Bloqueado)
            {
                throw new Exception(
                    ServicioSessionManager.GetInstance().Traducir("El usuario ya se encuentra desbloqueado."));
            }
            usuario.Bloqueado = false;
            usuario.IntentosInicio = 0;
            usuario.DVH = generador.GenerarDVH(usuario);
            dal.DesbloquearUsuario(dni, usuario.DVH);
            new BLLDVV().RecalcularDVVUsuario();
            bitacora.GrabarBitacora($"Desbloquear Usuario: {usuario.nombreUsuario}", "Usuario", 1);
        }
        public void ModificarUsuario(ServicioUsuario usuarioModificado)
        {
            DALUsuario dal = new DALUsuario();

            ServicioUsuario usuarioExistente =
                dal.BuscarUsuarioPorDniOMail(usuarioModificado.DNI, "x");

            if (usuarioExistente == null)
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("El usuario no existe."));
            }
            usuarioModificado.DVH = generador.GenerarDVH(usuarioModificado);
            dal.ModificarUsuario(usuarioModificado);
            new BLLDVV().RecalcularDVVUsuario();
            bitacora.GrabarBitacora($"Modificar Usuario: {usuarioModificado.nombreUsuario}", "Usuario", 1);
        }

        public bool ModificarEstado(int DNIUsuarioSeleccionado)
        {
            ServicioUsuario logueado = ServicioSessionManager.GetInstance().ObtenerUsuario();
            if (logueado != null && logueado.DNI == DNIUsuarioSeleccionado)
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("Operación inválida. No es posible desactivar la cuenta con la que se encuentra logueado actualmente."));
            }
            DALUsuario dal = new DALUsuario();
            ServicioUsuario usuarioAfectado = dal.BuscarUsuarioPorDniOMail(DNIUsuarioSeleccionado, "x");
            usuarioAfectado.Activo = !usuarioAfectado.Activo;
            usuarioAfectado.DVH = generador.GenerarDVH(usuarioAfectado);
            bool exito = dal.ModificarEstado(DNIUsuarioSeleccionado, usuarioAfectado.DVH);
            new BLLDVV().RecalcularDVVUsuario();
            if (exito)
            {
                bitacora.GrabarBitacora($"Activar / Desactivar Usuario", "Usuario", 1);
                return true;
            }
            return false;
        }

        public void ActualizarIdiomaUsuario(int dni, int idIdioma)
        {
            DALUsuario dal = new DALUsuario();
            ServicioUsuario logueado = ServicioSessionManager.GetInstance().ObtenerUsuario();
            logueado.IdIdioma = idIdioma;
            logueado.DVH = generador.GenerarDVH(logueado);
            dal.ActualizarIdiomaUsuario(dni, idIdioma, logueado.DVH);
            new BLLDVV().RecalcularDVVUsuario();
        }
    }
}
