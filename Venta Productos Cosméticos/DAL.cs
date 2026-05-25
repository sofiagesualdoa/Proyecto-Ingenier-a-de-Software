using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Venta_Productos_Cosméticos.BE;

namespace Venta_Productos_Cosméticos
{
    internal class DAL
    {
        public class DALUsuario
        {
            public Usuario ObtenerUsuario(string nombreUsuario)
            {
                // lógica ADO.NET para conectarte a SQL Server y obtener el usuario por su nombre de usuario.
                return new Usuario
                {
                    nombreUsuario = nombreUsuario,
                    Activo = true,
                    Bloqueado = false
                };
            }

            public void GuardarNuevaClave(string nombreUsuario, string hashClaveNueva)
            {
                // lógica ADO.NET para hacer un UPDATE Usuarios SET contraseña = @hash WHERE nombreUsuario = @user
            }
            public void SumarIntentoFallido(string nombreUsuario)
            {
                // lógica ADO.NET para hacer un UPDATE Usuarios SET intentosFallidos = intentosFallidos + 1 WHERE nombreUsuario = @user
            }

            public void CargarPermisos(Usuario usuario)
            {
                // lógica ADO.NET para hacer un SELECT p.nombre FROM Permisos p JOIN UsuarioPermisos up ON p.id = up.idPermiso JOIN Usuarios u ON up.idUsuario = u.id WHERE u.nombreUsuario = @user
            }
        }
    }
}
