using Microsoft.Data.SqlClient;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALs
{
    public class DALEvento
    {
        string cadena = "Data Source=.;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;Initial Catalog=EverGlow;";

        public void RegistrarEvento(ServicioEvento registro)
        {
            string query = @"INSERT INTO Evento (Login, Criticidad, Fecha, Hora, NombreEvento, Modulo, DNI, DVH) 
                                 VALUES (@Login, @Criticidad, @Fecha, @Hora, @NombreEvento, @Modulo, @DNI, @DVH);";

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@Login", SqlDbType.VarChar, 50).Value = (object)registro.Login ?? DBNull.Value;
                    comando.Parameters.Add("@Criticidad", SqlDbType.Int).Value = registro.Criticidad;
                    comando.Parameters.Add("@Fecha", SqlDbType.Date).Value = registro.Fecha;
                    comando.Parameters.Add("@Hora", SqlDbType.Time).Value = registro.Hora;
                    comando.Parameters.Add("@NombreEvento", SqlDbType.VarChar, 50).Value = registro.NombreEvento;
                    comando.Parameters.Add("@Modulo", SqlDbType.VarChar, 50).Value = registro.Modulo;
                    comando.Parameters.Add("@DNI", SqlDbType.Int).Value = registro.DNI;
                    comando.Parameters.Add("@DVH", SqlDbType.VarChar, 64).Value = registro.DVH;
                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error físico al escribir en la bitácora: ") + errorTraducido);
                    }
                }
            }
        }

        public List<ServicioEvento> ObtenerEventos(DateTime fechaDesde)
        {
            List<ServicioEvento> lista = new List<ServicioEvento>();
            string query = @"SELECT IdEvento, Login, Criticidad, Fecha, Hora, NombreEvento, Modulo, DNI, DVH 
                                 FROM Evento 
                                 WHERE Fecha >= @FechaDesde 
                                 ORDER BY Fecha DESC, Hora DESC;";

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@FechaDesde", SqlDbType.Date).Value = fechaDesde;

                    try
                    {
                        conexion.Open();
                        using (SqlDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ServicioEvento evt = new ServicioEvento();
                                evt.IdEvento = Convert.ToInt32(reader["IdEvento"]);
                                evt.Login = reader["Login"].ToString();
                                evt.Criticidad = Convert.ToInt32(reader["Criticidad"]);
                                evt.Fecha = Convert.ToDateTime(reader["Fecha"]);
                                evt.Hora = (TimeSpan)reader["Hora"];
                                evt.NombreEvento = reader["NombreEvento"].ToString();
                                evt.Modulo = reader["Modulo"].ToString();
                                evt.DNI = Convert.ToInt32(reader["DNI"]);
                                evt.DVH = (reader["DVH"]).ToString();
                                lista.Add(evt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorTraducido = ServicioSessionManager.GetInstance().Traducir(ex.Message);
                        throw new Exception(ServicioSessionManager.GetInstance().Traducir("Error en la DAL al recuperar eventos: ") + errorTraducido);
                    }
                }
            }
            return lista;
        }

        public List<ServicioEvento> ObtenerTodosLosEventos()
        {
            List<ServicioEvento> lista = new List<ServicioEvento>();
            string query = @"SELECT IdEvento, Login, Criticidad, Fecha, Hora, NombreEvento, Modulo, DNI, DVH FROM Evento";

            using (SqlConnection conexion = new SqlConnection(cadena))
            using (SqlCommand comando = new SqlCommand(query, conexion))
            {
                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new ServicioEvento
                        {
                            IdEvento = Convert.ToInt32(reader["IdEvento"]),
                            Login = reader["Login"].ToString(),
                            Criticidad = Convert.ToInt32(reader["Criticidad"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            Hora = (TimeSpan)reader["Hora"],
                            NombreEvento = reader["NombreEvento"].ToString(),
                            Modulo = reader["Modulo"].ToString(),
                            DNI = Convert.ToInt32(reader["DNI"]),
                            DVH = reader["DVH"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void ActualizarDVHEvento(int idEvento, string dvh)
        {
            string query = "UPDATE Evento SET DVH = @DVH WHERE IdEvento = @IdEvento";

            using (SqlConnection conexion = new SqlConnection(cadena))
            using (SqlCommand comando = new SqlCommand(query, conexion))
            {
                comando.Parameters.Add("@IdEvento", SqlDbType.Int).Value = idEvento;
                comando.Parameters.Add("@DVH", SqlDbType.VarChar, 64).Value = dvh;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
    }
}
