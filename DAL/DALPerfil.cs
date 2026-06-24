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
                string query = "SELECT IdPerfil, Nombre FROM Perfil ORDER BY Nombre";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        perfiles.Add(new ServicioFamilia(
                            Convert.ToInt32(reader["IdPerfil"]),
                            reader["Nombre"].ToString()
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
                string query = "SELECT IdPerfil, Nombre FROM Perfil WHERE IdPerfil = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", idPerfilUsuario);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        perfilRaiz = new ServicioFamilia(
                            Convert.ToInt32(reader["IdPerfil"]),
                            reader["Nombre"].ToString()
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
                string query = @"SELECT p.IdPermiso, p.Nombre 
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
                        lista.Add(new ServicioPermiso(Convert.ToInt32(reader["IdPermiso"]), reader["Nombre"].ToString()));
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
                string query = @"SELECT f.IdFamilia, f.Nombre 
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
                        lista.Add(new ServicioFamilia(Convert.ToInt32(reader["IdFamilia"]), reader["Nombre"].ToString()));
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
                string query = @"SELECT p.IdPermiso, p.Nombre 
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
                        lista.Add(new ServicioPermiso(Convert.ToInt32(reader["IdPermiso"]), reader["Nombre"].ToString()));
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
                string query = @"SELECT f.IdFamilia, f.Nombre 
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
                        lista.Add(new ServicioFamilia(Convert.ToInt32(reader["IdFamilia"]), reader["Nombre"].ToString()));
                    }
                }
            }
            return lista;
        }
    }
}
