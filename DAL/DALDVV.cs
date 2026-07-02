using Microsoft.Data.SqlClient;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALDVV
    {
        string cadena = "Data Source=.;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;Initial Catalog=EverGlow;";

        public List<ServicioDVV> ObtenerDVV()
        {
            List<ServicioDVV> lista = new List<ServicioDVV>();
            string query = "SELECT NombreTabla, Digito FROM DVV;";

            using (SqlConnection conexion = new SqlConnection(cadena))
            using (SqlCommand comando = new SqlCommand(query, conexion))
            {
                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ServicioDVV
                        {
                            NombreTabla = reader["NombreTabla"].ToString(),
                            Digito = reader["Digito"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void GuardarDVV(ServicioDVV dvv)
        {
            string query = @"
                            IF EXISTS (SELECT 1 FROM DVV WHERE NombreTabla = @NombreTabla)
                            UPDATE DVV SET Digito = @Digito WHERE NombreTabla = @NombreTabla;
                            ELSE
                            INSERT INTO DVV (NombreTabla, Digito) VALUES (@NombreTabla, @Digito);";

            using (SqlConnection conexion = new SqlConnection(cadena))
            using (SqlCommand comando = new SqlCommand(query, conexion))
            {
                comando.Parameters.Add("@NombreTabla", SqlDbType.VarChar, 50).Value = dvv.NombreTabla;
                comando.Parameters.Add("@Digito", SqlDbType.VarChar, 64).Value = dvv.Digito;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
    }
}
