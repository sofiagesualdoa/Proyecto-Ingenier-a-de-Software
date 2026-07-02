using Microsoft.Data.SqlClient;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALPerfil
    {
        string conexionString = "Data Source=.;Initial Catalog=EverGlow;Integrated Security=True;Trust Server Certificate=True";
        
        public List<ServicioPerfil> ObtenerPerfiles()
        {
            List<ServicioPerfil> perfiles = new List<ServicioPerfil>();

            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "SELECT IdPerfil, Nombre, DVH FROM Perfil ORDER BY Nombre";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        perfiles.Add(new ServicioFamilia(
                            Convert.ToInt32(reader["IdPerfil"]),
                            reader["Nombre"].ToString(),
                            reader["DVH"].ToString()
                        ));
                    }
                }
            }

            return perfiles;    
        }

        public ServicioPerfil ObtenerPerfilUsuario(int idPerfilUsuario)
        {
            ServicioFamilia perfilRaiz = null;

            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "SELECT IdPerfil, Nombre, DVH FROM Perfil WHERE IdPerfil = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", idPerfilUsuario);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        perfilRaiz = new ServicioFamilia(
                            Convert.ToInt32(reader["IdPerfil"]),
                            reader["Nombre"].ToString(),
                            reader["DVH"].ToString()
                        );
                    }
                }
            }

            if (perfilRaiz != null)
            {
                List<ServicioPermiso> permisosSueltos = ObtenerPermisosDirectosDelPerfil(perfilRaiz.IdPerfil);
                foreach (var permiso in permisosSueltos)
                {
                    perfilRaiz.Agregar(permiso);
                }

                List<ServicioFamilia> familiasDelPerfil = ObtenerFamiliasDelPerfil(perfilRaiz.IdPerfil);
                foreach (var familia in familiasDelPerfil)
                {
                    ArmarArbolRecursivo(familia);

                    perfilRaiz.Agregar(familia);
                }
            }

            return perfilRaiz;
        }

        private void ArmarArbolRecursivo(ServicioFamilia padre)
        {
            List<ServicioPermiso> permisosDeFamilia = ObtenerPermisosDeFamilia(padre.IdPerfil);
            foreach (var permiso in permisosDeFamilia)
            {
                padre.Agregar(permiso);
            }

            List<ServicioFamilia> subFamilias = ObtenerSubFamilias(padre.IdPerfil);
            foreach (var subFamilia in subFamilias)
            {
                padre.Agregar(subFamilia);

                ArmarArbolRecursivo(subFamilia);
            }
        }

        private List<ServicioPermiso> ObtenerPermisosDirectosDelPerfil(int idPerfil)
        {
            List<ServicioPermiso> lista = new List<ServicioPermiso>();
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = @"SELECT p.IdPermiso, p.Nombre, p.DVH 
                                 FROM Permiso p 
                                 INNER JOIN Perfil_x_Permiso pp ON p.IdPermiso = pp.IdPermiso 
                                 WHERE pp.IdPerfil = @idPerfil";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idPerfil", idPerfil);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ServicioPermiso(Convert.ToInt32(reader["IdPermiso"]), reader["Nombre"].ToString(), reader["DVH"].ToString()));
                    }
                }
            }
            return lista;
        }

        private List<ServicioFamilia> ObtenerFamiliasDelPerfil(int idPerfil)
        {
            List<ServicioFamilia> lista = new List<ServicioFamilia>();
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = @"SELECT f.IdFamilia, f.Nombre, f.DVH 
                                 FROM Familia f 
                                 INNER JOIN Perfil_x_Familia pf ON f.IdFamilia = pf.IdFamilia 
                                 WHERE pf.IdPerfil = @idPerfil";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idPerfil", idPerfil);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ServicioFamilia(Convert.ToInt32(reader["IdFamilia"]), reader["Nombre"].ToString(), reader["DVH"].ToString()));
                    }
                }
            }
            return lista;
        }

        private List<ServicioPermiso> ObtenerPermisosDeFamilia(int idFamilia)
        {
            List<ServicioPermiso> lista = new List<ServicioPermiso>();
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = @"SELECT p.IdPermiso, p.Nombre, p.DVH 
                                 FROM Permiso p 
                                 INNER JOIN Permiso_x_Familia pf ON p.IdPermiso = pf.IdPermiso 
                                 WHERE pf.IdFamilia = @idFamilia";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idFamilia", idFamilia);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ServicioPermiso(Convert.ToInt32(reader["IdPermiso"]), reader["Nombre"].ToString(), reader["DVH"].ToString()));
                    }
                }
            }
            return lista;
        }

        private List<ServicioFamilia> ObtenerSubFamilias(int idFamiliaPadre)
        {
            List<ServicioFamilia> lista = new List<ServicioFamilia>();
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = @"SELECT f.IdFamilia, f.Nombre, f.DVH 
                                 FROM Familia f 
                                 INNER JOIN Familia_x_Familia ff ON f.IdFamilia = ff.IdFamiliaHijo 
                                 WHERE ff.IdFamiliaPadre = @idPadre";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idPadre", idFamiliaPadre);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ServicioFamilia(Convert.ToInt32(reader["IdFamilia"]), reader["Nombre"].ToString(), reader["DVH"].ToString()));
                    }
                }
            }
            return lista;
        }

        public int GuardarPerfil(ServicioPerfil perfil)
        {
            int idGenerado = 0;

            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "INSERT INTO Perfil (Nombre, DVH) VALUES (@Nombre, @DVH); SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", perfil.Nombre);
                    cmd.Parameters.AddWithValue("@DVH", perfil.DVH);

                    con.Open();
                    idGenerado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return idGenerado;
        }

        public void GuardarRelacionesPerfil(int idPerfilPadre, List<ServicioPerfil> hijos, List<string> dvhsRelaciones)
        {
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                con.Open();

                for (int i = 0; i < hijos.Count; i++)
                {
                    ServicioPerfil hijo = hijos[i];
                    string dvhRelacion = dvhsRelaciones[i];

                    string query = "";

                    if (hijo is ServicioPermiso)
                    {
                        query = "INSERT INTO Perfil_x_Permiso (IdPerfil, IdPermiso, DVH) VALUES (@idPadre, @idHijo, @DVH)";
                    }
                    else if (hijo is ServicioFamilia)
                    {
                        query = "INSERT INTO Perfil_x_Familia (IdPerfil, IdFamilia, DVH) VALUES (@idPadre, @idHijo, @DVH)";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@idPadre", idPerfilPadre);
                        cmd.Parameters.AddWithValue("@idHijo", hijo.IdPerfil);
                        cmd.Parameters.AddWithValue("@DVH", dvhRelacion);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public bool PerfilEstaAsignadoAUsuario(int idPerfil)
        {
            bool estaAsignado = false;
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "SELECT COUNT(1) FROM Usuario WHERE IdPerfil = @idPerfil";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idPerfil", idPerfil);
                try
                {
                    con.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    estaAsignado = count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error al verificar asignación del perfil: ") + ex.Message);
                }
            }
            return estaAsignado;
        }

        public void EliminarPerfil(int idPerfil)
        {
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        string query1 = "DELETE FROM Perfil_x_Permiso WHERE IdPerfil = @id";
                        SqlCommand cmd1 = new SqlCommand(query1, con, tran);
                        cmd1.Parameters.AddWithValue("@id", idPerfil);
                        cmd1.ExecuteNonQuery();
                        string query2 = "DELETE FROM Perfil_x_Familia WHERE IdPerfil = @id";
                        SqlCommand cmd2 = new SqlCommand(query2, con, tran);
                        cmd2.Parameters.AddWithValue("@id", idPerfil);
                        cmd2.ExecuteNonQuery();
                        string query3 = "DELETE FROM Perfil WHERE IdPerfil = @id";
                        SqlCommand cmd3 = new SqlCommand(query3, con, tran);
                        cmd3.Parameters.AddWithValue("@id", idPerfil);
                        cmd3.ExecuteNonQuery();

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error al eliminar físicamente el perfil: ") + ex.Message);
                    }
                }
            }
        }

        public List<int> ObtenerPerfilesQueQuedarianVaciosPorFamilia(int idFamiliaAEliminar)
        {
            List<int> perfilesAfectados = new List<int>();
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = @"
            SELECT pxf.IdPerfil 
            FROM Perfil_x_Familia pxf
            WHERE pxf.IdFamilia = @idFamilia
              AND (
                  (SELECT COUNT(1) FROM Perfil_x_Permiso WHERE IdPerfil = pxf.IdPerfil) +
                  (SELECT COUNT(1) FROM Perfil_x_Familia WHERE IdPerfil = pxf.IdPerfil)
              ) <= 1"; 

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idFamilia", idFamiliaAEliminar);
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            perfilesAfectados.Add(Convert.ToInt32(reader["IdPerfil"]));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error al verificar la integridad de componentes del perfil: ") + ex.Message);
                }
            }
            return perfilesAfectados;
        }

        public void AgregarRelacionPerfilPermiso(int idPerfilPadre, ServicioPerfil hijo, string dvhRelacion)
        {
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "INSERT INTO Perfil_x_Permiso (IdPerfil, IdPermiso, DVH) VALUES (@idPadre, @idHijo, @DVH)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idPadre", idPerfilPadre);
                    cmd.Parameters.AddWithValue("@idHijo", hijo.IdPerfil);
                    cmd.Parameters.AddWithValue("@DVH", dvhRelacion);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void QuitarRelacionPerfilPermiso(int idPerfilPadre, ServicioPerfil hijo)
        {
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "";
                if (hijo is ServicioPermiso)
                {
                    query = "DELETE FROM Perfil_x_Permiso WHERE IdPerfil = @idPadre AND IdPermiso = @idHijo";
                }
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idPadre", idPerfilPadre);
                cmd.Parameters.AddWithValue("@idHijo", hijo.IdPerfil);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error físico al quitar el componente del perfil: ") + ex.Message);
                }
            }
        }

        public int ObtenerCantidadHijosPerfil(int idPerfil)
        {
            int totalHijos = 0;
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = @"
            SELECT 
                (SELECT COUNT(1) FROM Perfil_x_Permiso WHERE IdPerfil = @id) + 
                (SELECT COUNT(1) FROM Perfil_x_Familia WHERE IdPerfil = @id)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", idPerfil);

                try
                {
                    con.Open();
                    totalHijos = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error al contar los componentes del perfil: ") + ex.Message);
                }
            }
            return totalHijos;
        }

        public void AgregarRelacionPerfilFamilia(int idPerfilPadre, ServicioFamilia hijo, string dvhRelacion)
        {
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "INSERT INTO Perfil_x_Familia (IdPerfil, IdFamilia, DVH) VALUES (@idPadre, @idHijo, @DVH)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idPadre", idPerfilPadre);
                    cmd.Parameters.AddWithValue("@idHijo", hijo.IdPerfil);
                    cmd.Parameters.AddWithValue("@DVH", dvhRelacion);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void QuitarRelacionPerfilFamilia(int idPerfilPadre, ServicioFamilia hijo)
        {
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "DELETE FROM Perfil_x_Familia WHERE IdPerfil = @idPadre AND IdFamilia = @idHijo";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idPadre", idPerfilPadre);
                cmd.Parameters.AddWithValue("@idHijo", hijo.IdPerfil);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error físico al quitar la familia del perfil: ") + ex.Message);
                }
            }
        }

        public List<ServicioPerfilPermiso> ObtenerRelacionesPerfilPermiso()
        {
            List<ServicioPerfilPermiso> lista = new List<ServicioPerfilPermiso>();
            string query = "SELECT IdPerfil, IdPermiso, DVH FROM Perfil_x_Permiso";

            using (SqlConnection con = new SqlConnection(conexionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ServicioPerfilPermiso
                        {
                            IdPerfil = Convert.ToInt32(reader["IdPerfil"]),
                            IdPermiso = Convert.ToInt32(reader["IdPermiso"]),
                            DVH = reader["DVH"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public List<ServicioPerfilFamilia> ObtenerRelacionesPerfilFamilia()
        {
            List<ServicioPerfilFamilia> lista = new List<ServicioPerfilFamilia>();
            string query = "SELECT IdPerfil, IdFamilia, DVH FROM Perfil_x_Familia";

            using (SqlConnection con = new SqlConnection(conexionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ServicioPerfilFamilia
                        {
                            IdPerfil = Convert.ToInt32(reader["IdPerfil"]),
                            IdFamilia = Convert.ToInt32(reader["IdFamilia"]),
                            DVH = reader["DVH"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void ActualizarDVHPerfil(int idPerfil, string dvh)
        {
            string query = "UPDATE Perfil SET DVH = @DVH WHERE IdPerfil = @IdPerfil";

            using (SqlConnection con = new SqlConnection(conexionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdPerfil", idPerfil);
                cmd.Parameters.AddWithValue("@DVH", dvh);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarDVHPerfilPermiso(int idPerfil, int idPermiso, string dvh)
        {
            string query = @"UPDATE Perfil_x_Permiso 
                     SET DVH = @DVH 
                     WHERE IdPerfil = @IdPerfil AND IdPermiso = @IdPermiso";

            using (SqlConnection con = new SqlConnection(conexionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdPerfil", idPerfil);
                cmd.Parameters.AddWithValue("@IdPermiso", idPermiso);
                cmd.Parameters.AddWithValue("@DVH", dvh);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarDVHPerfilFamilia(int idPerfil, int idFamilia, string dvh)
        {
            string query = @"UPDATE Perfil_x_Familia 
                     SET DVH = @DVH 
                     WHERE IdPerfil = @IdPerfil AND IdFamilia = @IdFamilia";

            using (SqlConnection con = new SqlConnection(conexionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdPerfil", idPerfil);
                cmd.Parameters.AddWithValue("@IdFamilia", idFamilia);
                cmd.Parameters.AddWithValue("@DVH", dvh);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
