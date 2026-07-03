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
            string query = @"SELECT DNI, Nombre, Apellido, NombreUsuario, Bloqueado, Activo, Contraseña, IntentosInicio, Email, IdPerfil, IdIdioma, DVH 
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
                                usuarioEncontrado.IdIdioma = reader["IdIdioma"] != DBNull.Value ? Convert.ToInt32(reader["IdIdioma"]) : 1;
                                usuarioEncontrado.SetPassword(reader["Contraseña"].ToString());
                                usuarioEncontrado.DVH = reader["DVH"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error al recuperar usuario: ") + ex.Message);
                    }
                }
            }
            return usuarioEncontrado;
        }

        public List<ServicioUsuario> ObtenerUsuarios()
        {
            usuarios.Clear();
            string query = @"SELECT u.DNI, u.Nombre, u.Apellido, u.NombreUsuario, u.Bloqueado, u.Activo, u.Contraseña, 
                            u.IntentosInicio, u.Email, u.IdPerfil, u.IdIdioma, u.DVH, p.Nombre AS NombrePerfil, p.DVH AS DVHPerfil
                     FROM Usuario u
                     LEFT JOIN Perfil p ON u.IdPerfil = p.IdPerfil;";

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
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
                            usr.DVH = reader["DVH"].ToString();
                            usr.IdIdioma = reader["IdIdioma"] != DBNull.Value ? Convert.ToInt32(reader["IdIdioma"]) : 1;
                            usr.PerfilUsuario = new ServicioFamilia(usr.IdPerfil, reader["NombrePerfil"].ToString(), reader["DVHPerfil"].ToString());
                            usr.SetPassword(reader["Contraseña"].ToString());

                            usuarios.Add(usr);
                        }
                    }
                }
            }
            return usuarios;
        }

        public void GuardarUsuario(ServicioUsuario usuario)
        {
            int idiomaAGuardar = (usuario.IdIdioma > 0) ? usuario.IdIdioma : 1;

            string query = @"INSERT INTO Usuario (DNI, Nombre, Apellido, NombreUsuario, Bloqueado, Activo, Contraseña, IntentosInicio, Email, IdPerfil, IdIdioma, DVH) 
                     VALUES (@DNI, @Nombre, @Apellido, @NombreUsuario, @Bloqueado, @Activo, @Contraseña, @IntentosInicio, @Email, @IdPerfil, @IdIdioma, @DVH);";

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
                    comando.Parameters.Add("@IdIdioma", SqlDbType.Int).Value = idiomaAGuardar;
                    comando.Parameters.Add("@DVH", SqlDbType.VarChar, 64).Value = usuario.DVH;
                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error físico al intentar insertar el nuevo usuario en la base de datos: ") + ex.Message);
                    }
                }
            }

            usuario.IdIdioma = idiomaAGuardar;
            usuarios.Add(usuario);
        }

        public ServicioUsuario BuscarUsuarioPorDniOMail(int dni, string email)
        {
            ServicioUsuario usuarioEncontrado = null;
            string query = @"SELECT DNI, Nombre, Apellido, NombreUsuario, Bloqueado, Activo, Contraseña, IntentosInicio, Email, IdPerfil, IdIdioma, DVH 
                     FROM Usuario 
                     WHERE DNI = @DNI OR Email = @Email;";
            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@DNI", dni);
                    comando.Parameters.AddWithValue("@Email", email);
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
                            usuarioEncontrado.IdIdioma = reader["IdIdioma"] != DBNull.Value ? Convert.ToInt32(reader["IdIdioma"]) : 1;
                            usuarioEncontrado.SetPassword(reader["Contraseña"].ToString());
                            usuarioEncontrado.DVH = reader["DVH"].ToString();
                        }
                    }
                }
            }
            return usuarioEncontrado;
        }

        public void DesbloquearUsuario(int dni, string dvh)
        {
            string query = @"UPDATE Usuario 
                                 SET Bloqueado = 0, IntentosInicio = 0, DVH = @DVH
                                 WHERE DNI = @DNI;";

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@DNI", SqlDbType.Int).Value = dni;
                    comando.Parameters.Add("@DVH", SqlDbType.VarChar, 64).Value = dvh;
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
                        string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error físico al intentar desbloquear el usuario en SQL Server: ") + errorTraducido);
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

        public void BloquearUsuario(int dni,string dvh)
        {
            string query = @"UPDATE Usuario 
                     SET Bloqueado = 1,  DVH = @DVH
                     WHERE DNI = @DNI;";

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@DNI", SqlDbType.Int).Value = dni;
                    comando.Parameters.Add("@DVH", SqlDbType.VarChar, 64).Value = dvh;
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
                        string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error físico al intentar bloquear el usuario en SQL Server: ") + errorTraducido);
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
            int idiomaAGuardar = (usuarioModificado.IdIdioma > 0) ? usuarioModificado.IdIdioma : 1;
            string query = @"UPDATE Usuario 
                     SET Nombre = @Nombre, 
                         Apellido = @Apellido, 
                         Email = @Email, 
                         NombreUsuario = @NombreUsuario, 
                         IdPerfil = @IdPerfil, 
                         Activo = @Activo,
                         IdIdioma = @IdIdioma, 
                         DVH = @DVH
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
                    comando.Parameters.Add("@IdIdioma", SqlDbType.Int).Value = idiomaAGuardar;
                    comando.Parameters.Add("@DVH", SqlDbType.VarChar, 64).Value = usuarioModificado.DVH;
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
                        string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error en la capa de datos al modificar el usuario: ") + errorTraducido);
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
                usuarioMemoria.IdIdioma = usuarioModificado.IdIdioma;
                usuarioMemoria.PerfilUsuario = usuarioModificado.PerfilUsuario;
                usuarioMemoria.Activo = usuarioModificado.Activo;
                usuarioMemoria.DVH = usuarioModificado.DVH;
            }
        }

        public bool ModificarEstado(int DNIUsuario, string dvh)
        {
            ServicioUsuario u = BuscarUsuarioPorDniOMail(DNIUsuario, "x");
            if (u == null)
            {
                throw new Exception("Usuario no encontrado en la base de datos.");
            }
            bool nuevoEstado = !u.Activo;
            string query = @"UPDATE Usuario 
                                 SET Activo = @Activo, DVH = @DVH 
                                 WHERE DNI = @DNI;";
            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@DNI", SqlDbType.Int).Value = DNIUsuario;
                    comando.Parameters.Add("@Activo", SqlDbType.Bit).Value = nuevoEstado;
                    comando.Parameters.Add("@DVH", SqlDbType.VarChar, 64).Value = dvh;
                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error físico al intentar cambiar el estado del usuario en SQL Server: ") + errorTraducido);
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

        public void GuardarNuevaClave(string nombreUsuario, string hashClaveNueva, string dvh)
        {
            string query = @"UPDATE Usuario 
                                 SET Contraseña = @Contraseña, DVH = @DVH 
                                 WHERE NombreUsuario = @NombreUsuario;";

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 50).Value = nombreUsuario;
                    comando.Parameters.Add("@Contraseña", SqlDbType.VarChar, 65).Value = hashClaveNueva;
                    comando.Parameters.Add("@DVH", SqlDbType.VarChar, 64).Value = dvh;
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
                        string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error en la capa de datos al guardar la nueva contraseña: ") + errorTraducido);
                    }
                }
            }
            ServicioUsuario usuarioMemoria = usuarios.FirstOrDefault(u => u.nombreUsuario == nombreUsuario);
            if (usuarioMemoria != null)
            {
                usuarioMemoria.SetPassword(hashClaveNueva);
            }
        }
        public void SumarIntentoFallido(string nombreUsuario, string dvh)
        {
            string query = @"UPDATE Usuario 
                                 SET IntentosInicio = IntentosInicio + 1, DVH = @DVH 
                                 WHERE NombreUsuario = @NombreUsuario;";

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 50).Value = nombreUsuario;
                    comando.Parameters.Add("@DVH", SqlDbType.VarChar, 64).Value = dvh;
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
                        string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error en la capa de datos al incrementar el intento fallido: ") + errorTraducido);
                    }
                }
            }
            ServicioUsuario usuarioMemoria = usuarios.FirstOrDefault(u => u.nombreUsuario == nombreUsuario);
            if (usuarioMemoria != null)
            {
                usuarioMemoria.IntentosInicio++;
            }
        }

        public void ActualizarIdiomaUsuario(int dni, int idIdioma, string dvh)
        {
            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                string query = "UPDATE Usuario SET IdIdioma = @IdIdioma, DVH = @DVH WHERE DNI = @DNI";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@IdIdioma", idIdioma);
                    cmd.Parameters.AddWithValue("@DNI", dni);
                    cmd.Parameters.Add("@DVH", SqlDbType.VarChar, 64).Value = dvh;
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarDVHUsuario(int dni, string dvh)
        {
            string query = @"UPDATE Usuario 
                     SET DVH = @DVH 
                     WHERE DNI = @DNI;";

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@DNI", SqlDbType.Int).Value = dni;
                    comando.Parameters.Add("@DVH", SqlDbType.VarChar, 64).Value = dvh;

                    try
                    {
                        conexion.Open();

                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            throw new Exception("No se encontró ningún usuario con el DNI especificado para actualizar el DVH.");
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error físico al actualizar el DVH del usuario: ") + errorTraducido);
                    }
                }
            }
        }
    }
}
