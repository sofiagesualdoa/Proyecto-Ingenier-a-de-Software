using Microsoft.Data.SqlClient;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALPermiso
    {
        private string conexionString = "Data Source=.;Initial Catalog=EverGlow;Integrated Security=True;Trust Server Certificate=True";
        public List<ServicioPermiso> ObtenerTodos()
        {
            List<ServicioPermiso> lista = new List<ServicioPermiso>();
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "SELECT IdPermiso, Nombre, DVH FROM Permiso ORDER BY Nombre";
                SqlCommand cmd = new SqlCommand(query, con);
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ServicioPermiso(
                                Convert.ToInt32(reader["IdPermiso"]),
                                reader["Nombre"].ToString(),
                                reader["DVH"].ToString()
                            ));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error físico al leer los permisos base: ") + ex.Message);
                }
            }
            return lista;
        }

        public void ActualizarDVHPermiso(int idPermiso, string dvh)
        {
            using (SqlConnection con = new SqlConnection(conexionString))
            {
                string query = "UPDATE Permiso SET DVH = @DVH WHERE IdPermiso = @IdPermiso";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DVH", dvh);
                cmd.Parameters.AddWithValue("@IdPermiso", idPermiso);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
