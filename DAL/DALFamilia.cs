using Microsoft.Data.SqlClient;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALFamilia
    {
        private string conexionString = "Data Source=.;Initial Catalog=EverGlow;Integrated Security=True;Trust Server Certificate=True";
       
        public List<ServicioFamilia> ObtenerFamilias()
        {
            List<ServicioFamilia> lista = new List<ServicioFamilia>();

            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "SELECT IdFamilia, Nombre, DVH FROM Familia ORDER BY Nombre";
                SqlCommand cmd = new SqlCommand(query, con);

                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ServicioFamilia(
                                Convert.ToInt32(reader["IdFamilia"]),
                                reader["Nombre"].ToString(),
                                reader["DVH"].ToString()
                            ));
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error físico al leer la tabla Familia: ") + errorTraducido);
                }
            }

            foreach (var familia in lista)
            {
                ArmarArbolRecursivo(familia);
            }

            return lista;
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

                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ServicioPermiso(Convert.ToInt32(reader["IdPermiso"]), reader["Nombre"].ToString(), reader["DVH"].ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error al obtener permisos de la familia: ") + errorTraducido);
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

                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ServicioFamilia(Convert.ToInt32(reader["IdFamilia"]), reader["Nombre"].ToString(), reader["DVH"].ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error al obtener subfamilias: ") + errorTraducido);
                }
            }
            return lista;
        }

        public int GuardarFamilia(ServicioFamilia familia)
        {
            int idGenerado = 0;

            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "INSERT INTO Familia (Nombre, DVH) VALUES (@Nombre, @DVH); SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", familia.Nombre);
                    cmd.Parameters.AddWithValue("@DVH", familia.DVH);

                    con.Open();
                    idGenerado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return idGenerado;
        }

        public void GuardarRelacionesFamilia(int idFamiliaPadre, List<ServicioPerfil> hijos, List<string> dvhsRelaciones)
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
                        query = "INSERT INTO Permiso_x_Familia (IdFamilia, IdPermiso, DVH) VALUES (@idPadre, @idHijo, @DVH)";
                    }
                    else if (hijo is ServicioFamilia)
                    {
                        query = "INSERT INTO Familia_x_Familia (IdFamiliaPadre, IdFamiliaHijo, DVH) VALUES (@idPadre, @idHijo, @DVH)";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@idPadre", idFamiliaPadre);
                        cmd.Parameters.AddWithValue("@idHijo", hijo.IdPerfil);
                        cmd.Parameters.AddWithValue("@DVH", dvhRelacion);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void EliminarFamilia(int idFamilia)
        {
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        string q1 = "DELETE FROM Familia_x_Familia WHERE IdFamiliaPadre = @id";
                        using (SqlCommand cmd1 = new SqlCommand(q1, con, tran))
                        {
                            cmd1.Parameters.AddWithValue("@id", idFamilia);
                            cmd1.ExecuteNonQuery();
                        }

                        string q2 = "DELETE FROM Familia_x_Familia WHERE IdFamiliaHijo = @id";
                        using (SqlCommand cmd2 = new SqlCommand(q2, con, tran))
                        {
                            cmd2.Parameters.AddWithValue("@id", idFamilia);
                            cmd2.ExecuteNonQuery();
                        }

                        string q3 = "DELETE FROM Permiso_x_Familia WHERE IdFamilia = @id";
                        using (SqlCommand cmd3 = new SqlCommand(q3, con, tran))
                        {
                            cmd3.Parameters.AddWithValue("@id", idFamilia);
                            cmd3.ExecuteNonQuery();
                        }

                        string q4 = "DELETE FROM Perfil_x_Familia WHERE IdFamilia = @id";
                        using (SqlCommand cmd4 = new SqlCommand(q4, con, tran))
                        {
                            cmd4.Parameters.AddWithValue("@id", idFamilia);
                            cmd4.ExecuteNonQuery();
                        }

                        string q5 = "DELETE FROM Familia WHERE IdFamilia = @id";
                        using (SqlCommand cmd5 = new SqlCommand(q5, con, tran))
                        {
                            cmd5.Parameters.AddWithValue("@id", idFamilia);
                            cmd5.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error al eliminar físicamente la familia: ") + errorTraducido);
                    }
                }
            }
        }

        public List<int> ObtenerFamiliasPadreQueQuedarianVacias(int idFamiliaHijoAEliminar)
        {
            List<int> familiasPadreAfectadas = new List<int>();
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = @"
                    SELECT fxf.IdFamiliaPadre, fxf.DVH 
                    FROM Familia_x_Familia fxf
                    WHERE fxf.IdFamiliaHijo = @idHijo
                      AND (
                          (SELECT COUNT(1) FROM Permiso_x_Familia WHERE IdFamilia = fxf.IdFamiliaPadre) +
                          (SELECT COUNT(1) FROM Familia_x_Familia WHERE IdFamiliaPadre = fxf.IdFamiliaPadre)
                      ) <= 1";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idHijo", idFamiliaHijoAEliminar);
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            familiasPadreAfectadas.Add(Convert.ToInt32(reader["IdFamiliaPadre"]));
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error al verificar la integridad de componentes de las familias: ") + errorTraducido);
                }
            }
            return familiasPadreAfectadas;
        }

        public void AgregarRelacionFamiliaPermiso(int idFamiliaPadre, ServicioPerfil hijo, string dvhRelacion)
        {
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "";

                if (hijo is ServicioPermiso)
                {
                    query = "INSERT INTO Permiso_x_Familia (IdFamilia, IdPermiso, DVH) VALUES (@idPadre, @idHijo, @DVH)";
                }
                else if (hijo is ServicioFamilia)
                {
                    query = "INSERT INTO Familia_x_Familia (IdFamiliaPadre, IdFamiliaHijo, DVH) VALUES (@idPadre, @idHijo, @DVH)";
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@idPadre", idFamiliaPadre);
                    cmd.Parameters.AddWithValue("@idHijo", hijo.IdPerfil);
                    cmd.Parameters.AddWithValue("@DVH", dvhRelacion);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void QuitarRelacionFamiliaPermiso(int idFamiliaPadre, ServicioPerfil hijo)
        {
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "";
                if (hijo is ServicioPermiso)
                {
                    query = "DELETE FROM Permiso_x_Familia WHERE IdFamilia = @idPadre AND IdPermiso = @idHijo";
                }
                else if (hijo is ServicioFamilia)
                {
                    query = "DELETE FROM Familia_x_Familia WHERE IdFamiliaPadre = @idPadre AND IdFamiliaHijo = @idHijo";
                }

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idPadre", idFamiliaPadre);
                cmd.Parameters.AddWithValue("@idHijo", hijo.IdPerfil);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error físico al quitar el componente de la familia: ") + errorTraducido);
                }
            }
        }

        public int ObtenerCantidadHijosFamilia(int idFamilia)
        {
            int totalHijos = 0;
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = @"
                    SELECT 
                        (SELECT COUNT(1) FROM Permiso_x_Familia WHERE IdFamilia = @id) + 
                        (SELECT COUNT(1) FROM Familia_x_Familia WHERE IdFamiliaPadre = @id)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", idFamilia);

                try
                {
                    con.Open();
                    totalHijos = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error al contar los componentes de la familia: ") + errorTraducido);
                }
            }
            return totalHijos;
        }

        public List<ServicioPermisoFamilia> ObtenerRelacionesPermisoFamilia()
        {
            List<ServicioPermisoFamilia> lista = new List<ServicioPermisoFamilia>();
            string query = "SELECT IdPermiso, IdFamilia, DVH FROM Permiso_x_Familia";

            using (SqlConnection con = new SqlConnection(conexionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ServicioPermisoFamilia
                        {
                            IdPermiso = Convert.ToInt32(reader["IdPermiso"]),
                            IdFamilia = Convert.ToInt32(reader["IdFamilia"]),
                            DVH = reader["DVH"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public List<ServicioFamiliaFamilia> ObtenerRelacionesFamiliaFamilia()
        {
            List<ServicioFamiliaFamilia> lista = new List<ServicioFamiliaFamilia>();
            string query = "SELECT IdFamiliaPadre, IdFamiliaHijo, DVH FROM Familia_x_Familia";

            using (SqlConnection con = new SqlConnection(conexionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ServicioFamiliaFamilia
                        {
                            IdFamiliaPadre = Convert.ToInt32(reader["IdFamiliaPadre"]),
                            IdFamiliaHijo = Convert.ToInt32(reader["IdFamiliaHijo"]),
                            DVH = reader["DVH"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void ActualizarDVHFamilia(int idFamilia, string dvh)
        {
            string query = "UPDATE Familia SET DVH = @DVH WHERE IdFamilia = @IdFamilia";

            using (SqlConnection con = new SqlConnection(conexionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdFamilia", idFamilia);
                cmd.Parameters.AddWithValue("@DVH", dvh);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarDVHPermisoFamilia(int idFamilia, int idPermiso, string dvh)
        {
            string query = @"UPDATE Permiso_x_Familia 
                     SET DVH = @DVH 
                     WHERE IdFamilia = @IdFamilia AND IdPermiso = @IdPermiso";

            using (SqlConnection con = new SqlConnection(conexionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdFamilia", idFamilia);
                cmd.Parameters.AddWithValue("@IdPermiso", idPermiso);
                cmd.Parameters.AddWithValue("@DVH", dvh);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarDVHFamiliaFamilia(int idFamiliaPadre, int idFamiliaHijo, string dvh)
        {
            string query = @"UPDATE Familia_x_Familia 
                     SET DVH = @DVH 
                     WHERE IdFamiliaPadre = @IdFamiliaPadre 
                       AND IdFamiliaHijo = @IdFamiliaHijo";

            using (SqlConnection con = new SqlConnection(conexionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@IdFamiliaPadre", idFamiliaPadre);
                cmd.Parameters.AddWithValue("@IdFamiliaHijo", idFamiliaHijo);
                cmd.Parameters.AddWithValue("@DVH", dvh);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
