using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
            string cadena = "Data Source=.;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;Initial Catalog=EverGlow;";
            private static List<Usuario> usuarios = new List<Usuario>();
            public Usuario ObtenerUsuario(string nombreUsuario)
            {
                Usuario usuarioEncontrado = null;
                string query = @"SELECT DNI, Nombre, Apellido, NombreUsuario, Bloqueado, Activo, Contraseña, IntentosInicio, Email 
                                 FROM Usuario 
                                 WHERE NombreUsuario = @NombreUsuario;";
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                        try
                        {
                            conexion.Open();
                            using (SqlDataReader reader = comando.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    usuarioEncontrado = new Usuario();
                                    usuarioEncontrado.DNI = Convert.ToInt32(reader["DNI"]);
                                    usuarioEncontrado.Nombre = reader["Nombre"].ToString();
                                    usuarioEncontrado.Apellido = reader["Apellido"].ToString();
                                    usuarioEncontrado.nombreUsuario = reader["NombreUsuario"].ToString();
                                    usuarioEncontrado.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                                    usuarioEncontrado.Activo = Convert.ToBoolean(reader["Activo"]);
                                    usuarioEncontrado.Email = reader["Email"].ToString();
                                    usuarioEncontrado.IntentosInicio = Convert.ToInt32(reader["IntentosInicio"]);
                                    usuarioEncontrado.SetPassword(reader["Contraseña"].ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error físico en la base de datos al intentar recuperar el usuario: " + ex.Message);
                        }
                    }
                }
                return usuarioEncontrado;
            }

            public List<Usuario> ObtenerUsuarios()
            {
                return usuarios;
            }

            public void GuardarUsuario(Usuario usuario)
            {
                usuarios.Add(usuario);
            }

            public Usuario BuscarUsuarioPorDniOMail(int dni, string email)
            {
                Usuario usuarioEncontrado = null;
                string query = @"SELECT DNI, Nombre, Apellido, NombreUsuario, Bloqueado, Activo, Contraseña, IntentosInicio, Email 
                                 FROM Usuario 
                                 WHERE DNI = @DNI OR Email = @Email;";
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.Add("@DNI", SqlDbType.Int).Value = dni;
                        comando.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = email;
                        try
                        {
                            conexion.Open();
                            using (SqlDataReader reader = comando.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    usuarioEncontrado = new Usuario();
                                    usuarioEncontrado.DNI = Convert.ToInt32(reader["DNI"]);
                                    usuarioEncontrado.Nombre = reader["Nombre"].ToString();
                                    usuarioEncontrado.Apellido = reader["Apellido"].ToString();
                                    usuarioEncontrado.nombreUsuario = reader["NombreUsuario"].ToString();
                                    usuarioEncontrado.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                                    usuarioEncontrado.Activo = Convert.ToBoolean(reader["Activo"]);
                                    usuarioEncontrado.Email = reader["Email"].ToString();
                                    usuarioEncontrado.IntentosInicio = Convert.ToInt32(reader["IntentosInicio"]);
                                    usuarioEncontrado.SetPassword(reader["Contraseña"].ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error en la capa de datos al buscar duplicados de usuario: " + ex.Message);
                        }
                    }
                }

                return usuarioEncontrado;
            }

            public void DesbloquearUsuario(int dni)
            {
                string query = @"UPDATE Usuario 
                                 SET Bloqueado = 0, IntentosInicio = 0 
                                 WHERE DNI = @DNI;";

                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.Add("@DNI", SqlDbType.Int).Value = dni;
                        try
                        {
                            conexion.Open();
                            int filasAfectadas = comando.ExecuteNonQuery();
                            if (filasAfectadas == 0)
                            {
                                throw new Exception("No se encontró ningún usuario con el DNI especificado en la base de datos.");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error físico al intentar desbloquear el usuario en SQL Server: " + ex.Message);
                        }
                    }
                }
                Usuario usuarioMemoria = usuarios.FirstOrDefault(u => u.DNI == dni);
                if (usuarioMemoria != null)
                {
                    usuarioMemoria.Bloqueado = false;
                    usuarioMemoria.IntentosInicio = 0; 
                }
            }

            public void ModificarUsuario(Usuario usuarioModificado)
            {
                string query = @"UPDATE Usuario 
                                 SET Nombre = @Nombre, 
                                     Apellido = @Apellido, 
                                     Email = @Email, 
                                     NombreUsuario = @NombreUsuario, 
                                     Rol = @Rol, 
                                     Activo = @Activo
                                 WHERE DNI = @DNI;";

                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.Add("@DNI", SqlDbType.Int).Value = usuarioModificado.DNI;
                        comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = usuarioModificado.Nombre;
                        comando.Parameters.Add("@Apellido", SqlDbType.VarChar, 50).Value = usuarioModificado.Apellido;
                        comando.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = usuarioModificado.Email;
                        comando.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 50).Value = usuarioModificado.nombreUsuario;
                        comando.Parameters.Add("@Rol", SqlDbType.VarChar, 50).Value = usuarioModificado.Rol;
                        comando.Parameters.Add("@Activo", SqlDbType.Bit).Value = usuarioModificado.Activo;

                        try
                        {
                            conexion.Open();
                            int filasAfectadas = comando.ExecuteNonQuery();
                            if (filasAfectadas == 0)
                            {
                                throw new Exception("No se pudo actualizar. El usuario no existe en la base de datos.");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error en la capa de datos al modificar el usuario: " + ex.Message);
                        }
                    }
                }
                Usuario usuarioMemoria = usuarios.FirstOrDefault(u => u.DNI == usuarioModificado.DNI);

                if (usuarioMemoria != null)
                {
                    usuarioMemoria.Nombre = usuarioModificado.Nombre;
                    usuarioMemoria.Apellido = usuarioModificado.Apellido;
                    usuarioMemoria.Email = usuarioModificado.Email;
                    usuarioMemoria.nombreUsuario = usuarioModificado.nombreUsuario;
                    usuarioMemoria.Rol = usuarioModificado.Rol;
                    usuarioMemoria.Activo = usuarioModificado.Activo;
                }
            }

            public bool ModificarEstado(int DNIUsuario)
            {
                Usuario u = BuscarUsuarioPorDniOMail(DNIUsuario, "x");
                if (u == null)
                {
                    throw new Exception("Usuario no encontrado en la base de datos.");
                }
                bool nuevoEstado = !u.Activo;
                string query = @"UPDATE Usuario 
                                 SET Activo = @Activo 
                                 WHERE DNI = @DNI;";
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.Add("@DNI", SqlDbType.Int).Value = DNIUsuario;
                        comando.Parameters.Add("@Activo", SqlDbType.Bit).Value = nuevoEstado;
                        try
                        {
                            conexion.Open();
                            comando.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error físico al intentar cambiar el estado del usuario en SQL Server: " + ex.Message);
                        }
                    }
                }
                Usuario usuarioMemoria = usuarios.FirstOrDefault(user => user.DNI == DNIUsuario);
                if (usuarioMemoria != null)
                {
                    usuarioMemoria.Activo = nuevoEstado;
                }
                return true;
            }

            public void GuardarNuevaClave(string nombreUsuario, string hashClaveNueva)
            {
                string query = @"UPDATE Usuario 
                                 SET Contraseña = @Contraseña 
                                 WHERE NombreUsuario = @NombreUsuario;";

                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 50).Value = nombreUsuario;
                        comando.Parameters.Add("@Contraseña", SqlDbType.VarChar, 50).Value = hashClaveNueva;
                        try
                        {
                            conexion.Open();
                            int filasAfectadas = comando.ExecuteNonQuery();

                            if (filasAfectadas == 0)
                            {
                                throw new Exception("No se encontró ningún usuario con ese apodo para actualizar la contraseña.");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error en la capa de datos al guardar la nueva contraseña: " + ex.Message);
                        }
                    }
                }
                Usuario usuarioMemoria = usuarios.FirstOrDefault(u => u.nombreUsuario == nombreUsuario);
                if (usuarioMemoria != null)
                {
                    usuarioMemoria.SetPassword(hashClaveNueva);
                }
            }
            public void SumarIntentoFallido(string nombreUsuario)
            {
                string query = @"UPDATE Usuario 
                                 SET IntentosInicio = IntentosInicio + 1 
                                 WHERE NombreUsuario = @NombreUsuario;";

                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 50).Value = nombreUsuario;

                        try
                        {
                            conexion.Open();
                            int filasAfectadas = comando.ExecuteNonQuery();

                            if (filasAfectadas == 0)
                            {
                                throw new Exception("No se pudo registrar el intento fallido. El usuario no existe.");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error en la capa de datos al incrementar el intento fallido: " + ex.Message);
                        }
                    }
                }
                Usuario usuarioMemoria = usuarios.FirstOrDefault(u => u.nombreUsuario == nombreUsuario);
                if (usuarioMemoria != null)
                {
                    usuarioMemoria.IntentosInicio++;
                }
            }

            public void CargarPermisos(Usuario usuario)
            {
                // todo: lógica ADO.NET para hacer un SELECT p.nombre FROM Permisos p JOIN UsuarioPermisos up ON p.id = up.idPermiso JOIN Usuarios u ON up.idUsuario = u.id WHERE u.nombreUsuario = @user
            }
        }
    }
}
