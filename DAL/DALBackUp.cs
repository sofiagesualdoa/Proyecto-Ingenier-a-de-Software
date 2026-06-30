using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicios;

namespace DAL
{
    public class DALBackUp
    {
        public void EjecutarBackup(string connectionString, ServicioBackUp backup)
        {
            string fullPath = Path.Combine(backup.PathDestino, backup.NombreArchivo);
            string sql = $"BACKUP DATABASE [EverGlow] TO DISK = '{fullPath}' WITH FORMAT;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = 300;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EjecutarRestore(string rutaCompletaArchivo)
        {
            string cadena = "Data Source=.;Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True";
            string query = @"
                            USE [master];
                            ALTER DATABASE [EverGlow] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                            RESTORE DATABASE [EverGlow] FROM DISK = @Ruta WITH REPLACE;
                            ALTER DATABASE [EverGlow] SET MULTI_USER;";

            using (SqlConnection conexion = new SqlConnection(cadena))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Ruta", rutaCompletaArchivo);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
