using Microsoft.Data.SqlClient;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL
{
    public class DALIdioma
    {
        private readonly string cadenaConexion = "Data Source=.;Initial Catalog=EverGlow;Integrated Security=True;Trust Server Certificate=True";

        public List<ServicioIdioma> ListarIdiomas()
        {
            List<ServicioIdioma> lista = new List<ServicioIdioma>();

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                string query = "SELECT IdIdioma, Nombre, Codigo, DVH FROM Idioma";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServicioIdioma idioma = new ServicioIdioma
                            {
                                IdIdioma = Convert.ToInt32(reader["IdIdioma"]),
                                Nombre = reader["Nombre"].ToString(),
                                CodigoIdioma = reader["Codigo"].ToString(),
                                DVH = reader["DVH"].ToString()
                            };
                            lista.Add(idioma);
                        }
                    }
                }
            }
            return lista;
        }

        private readonly string rutaJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "traducciones.json");

        public Dictionary<string, string> ObtenerTraducciones()
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();

            if (!File.Exists(rutaJson))
            {
                throw new FileNotFoundException("No se encontró el archivo traducciones.json en: " + rutaJson);
            }

            try
            {
                string jsonString = File.ReadAllText(rutaJson);

                using (JsonDocument doc = JsonDocument.Parse(jsonString))
                {
                    foreach (JsonProperty propiedad in doc.RootElement.EnumerateObject())
                    {
                        diccionario[propiedad.Name] = propiedad.Value.GetString();
                    }
                }
            }
            catch (JsonException ex)
            {
                throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error al procesar el formato del JSON."), ex);
            }

            return diccionario;
        }

        public void ActualizarDVHIdioma(int idIdioma, string dvh)
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                string query = "UPDATE Idioma SET DVH = @DVH WHERE IdIdioma = @IdIdioma";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@DVH", dvh);
                cmd.Parameters.AddWithValue("@IdIdioma", idIdioma);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
