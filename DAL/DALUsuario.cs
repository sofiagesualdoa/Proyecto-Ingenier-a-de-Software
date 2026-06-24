using System.Data;
using Microsoft.Data.SqlClient;
using Servicios;

namespace DALs
{
    public class DALUsuario
    {
        string cadena = "Data Source=.;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;Initial Catalog=EverGlow;";
        private static List<ServicioUsuario> usuarios = new List<ServicioUsuario>();
        public ServicioUsuario ObtenerUsuario(string nombreUsuario)
        {
            ServicioUsuario usuarioEncontrado = null;
            string query = @"SELECT DNI, Nombre, Apellido, NombreUsuario, Bloqueado, Activo, Contraseña, IntentosInicio, Email, IdPerfil 
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
                                usuarioEncontrado = new ServicioUsuario();
                                usuarioEncontrado.DNI = Convert.ToInt32(reader["DNI"]);
                                usuarioEncontrado.Nombre = reader["Nombre"].ToString();
                                usuarioEncontrado.Apellido = reader["Apellido"].ToString();
                                usuarioEncontrado.nombreUsuario = reader["NombreUsuario"].ToString();
                                usuarioEncontrado.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                                usuarioEncontrado.Activo = Convert.ToBoolean(reader["Activo"]);
                                usuarioEncontrado.Email = reader["Email"].ToString();
                                usuarioEncontrado.IntentosInicio = Convert.ToInt32(reader["IntentosInicio"]);
                                usuarioEncontrado.IdPerfil = Convert.ToInt32(reader["IdPerfil"]);
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

        public List<ServicioUsuario> ObtenerUsuarios()
        {
            usuarios.Clear();
            string query = @"SELECT u.DNI, u.Nombre, u.Apellido, u.NombreUsuario, u.Bloqueado, u.Activo, u.Contraseña, 
                                    u.IntentosInicio, u.Email, u.IdPerfil, p.Nombre AS NombrePerfil
                             FROM Usuario u
                             LEFT JOIN Perfil p ON u.IdPerfil = p.IdPerfil;";

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    try
                    {
                        conexion.Open();
                        using (SqlDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ServicioUsuario usr = new ServicioUsuario();
                                usr.DNI = Convert.ToInt32(reader["DNI"]);
                                usr.Nombre = reader["Nombre"].ToString();
                                usr.Apellido = reader["Apellido"].ToString();
                                usr.nombreUsuario = reader["NombreUsuario"].ToString();
                                usr.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                                usr.Activo = Convert.ToBoolean(reader["Activo"]);
                                usr.Email = reader["Email"].ToString();
                                usr.IntentosInicio = Convert.ToInt32(reader["IntentosInicio"]);
                                usr.IdPerfil = Convert.ToInt32(reader["IdPerfil"]);
                                usr.PerfilUsuario = new ServicioFamilia(
                                    usr.IdPerfil,
                                    reader["NombrePerfil"].ToString()
                                );
                                usr.SetPassword(reader["Contraseña"].ToString());

                                usuarios.Add(usr);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al recuperar la lista completa de usuarios: " + ex.Message);
                    }
                }
            }
            return usuarios;
        }

        public void GuardarUsuario(ServicioUsuario usuario)
        {
            string query = @"INSERT INTO Usuario (DNI, Nombre, Apellido, NombreUsuario, Bloqueado, Activo, Contraseña, IntentosInicio, Email, IdPerfil) 
                     VALUES (@DNI, @Nombre, @Apellido, @NombreUsuario, @Bloqueado, @Activo, @Contraseña, @IntentosInicio, @Email, @IdPerfil);";

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@DNI", SqlDbType.Int).Value = usuario.DNI;
                    comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = usuario.Nombre;
                    comando.Parameters.Add("@Apellido", SqlDbType.VarChar, 50).Value = usuario.Apellido;
                    comando.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 50).Value = usuario.nombreUsuario;
                    comando.Parameters.Add("@Bloqueado", SqlDbType.Bit).Value = usuario.Bloqueado;
                    comando.Parameters.Add("@Activo", SqlDbType.Bit).Value = usuario.Activo;
                    comando.Parameters.Add("@IntentosInicio", SqlDbType.Int).Value = usuario.IntentosInicio;
                    comando.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = usuario.Email;
                    comando.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = usuario.IdPerfil;
                    comando.Parameters.Add("@Contraseña", SqlDbType.VarChar, 65).Value = usuario.GetPassword();

                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error físico al intentar insertar el nuevo usuario en la base de datos: " + ex.Message);
                    }
                }
            }
            usuarios.Add(usuario);
        }

        public ServicioUsuario BuscarUsuarioPorDniOMail(int dni, string email)
        {
            ServicioUsuario usuarioEncontrado = null;
            string query = @"SELECT DNI, Nombre, Apellido, NombreUsuario, Bloqueado, Activo, Contraseña, IntentosInicio, Email, IdPerfil 
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
                                usuarioEncontrado = new ServicioUsuario();
                                usuarioEncontrado.DNI = Convert.ToInt32(reader["DNI"]);
                                usuarioEncontrado.Nombre = reader["Nombre"].ToString();
                                usuarioEncontrado.Apellido = reader["Apellido"].ToString();
                                usuarioEncontrado.nombreUsuario = reader["NombreUsuario"].ToString();
                                usuarioEncontrado.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                                usuarioEncontrado.Activo = Convert.ToBoolean(reader["Activo"]);
                                usuarioEncontrado.Email = reader["Email"].ToString();
                                usuarioEncontrado.IntentosInicio = Convert.ToInt32(reader["IntentosInicio"]);
                                usuarioEncontrado.IdPerfil = Convert.ToInt32(reader["IdPerfil"]);
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
            ServicioUsuario usuarioMemoria = usuarios.FirstOrDefault(u => u.DNI == dni);
            if (usuarioMemoria != null)
            {
                usuarioMemoria.Bloqueado = false;
                usuarioMemoria.IntentosInicio = 0;
            }
        }

        public void BloquearUsuario(int dni)
        {
            string query = @"UPDATE Usuario 
                     SET Bloqueado = 1 
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
                            throw new Exception("No se encontró ningún usuario con el DNI especificado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error físico al intentar bloquear el usuario en SQL Server: " + ex.Message);
                    }
                }
            }
            ServicioUsuario usuarioMemoria = usuarios.FirstOrDefault(u => u.DNI == dni);
            if (usuarioMemoria != null)
            {
                usuarioMemoria.Bloqueado = true;
            }
        }

        public void ModificarUsuario(ServicioUsuario usuarioModificado)
        {
            string query = @"UPDATE Usuario 
                                 SET Nombre = @Nombre, 
                                     Apellido = @Apellido, 
                                     Email = @Email, 
                                     NombreUsuario = @NombreUsuario, 
                                     IdPerfil = @IdPerfil, 
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
                    comando.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = usuarioModificado.IdPerfil;
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
            ServicioUsuario usuarioMemoria = usuarios.FirstOrDefault(u => u.DNI == usuarioModificado.DNI);

            if (usuarioMemoria != null)
            {
                usuarioMemoria.Nombre = usuarioModificado.Nombre;
                usuarioMemoria.Apellido = usuarioModificado.Apellido;
                usuarioMemoria.Email = usuarioModificado.Email;
                usuarioMemoria.nombreUsuario = usuarioModificado.nombreUsuario;
                usuarioMemoria.IdPerfil = usuarioModificado.IdPerfil;
                usuarioMemoria.PerfilUsuario = usuarioModificado.PerfilUsuario;
                usuarioMemoria.Activo = usuarioModificado.Activo;
            }
        }

        public bool ModificarEstado(int DNIUsuario)
        {
            ServicioUsuario u = BuscarUsuarioPorDniOMail(DNIUsuario, "x");
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
            ServicioUsuario usuarioMemoria = usuarios.FirstOrDefault(user => user.DNI == DNIUsuario);
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
                    comando.Parameters.Add("@Contraseña", SqlDbType.VarChar, 65).Value = hashClaveNueva;
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
            ServicioUsuario usuarioMemoria = usuarios.FirstOrDefault(u => u.nombreUsuario == nombreUsuario);
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
            ServicioUsuario usuarioMemoria = usuarios.FirstOrDefault(u => u.nombreUsuario == nombreUsuario);
            if (usuarioMemoria != null)
            {
                usuarioMemoria.IntentosInicio++;
            }
        }

        public void CargarPermisos(ServicioUsuario usuario)
        {
            // todo: lógica ADO.NET para hacer un SELECT p.nombre FROM Permisos p JOIN UsuarioPermisos up ON p.id = up.idPermiso JOIN Usuarios u ON up.idUsuario = u.id WHERE u.nombreUsuario = @user
        }
    }
}
