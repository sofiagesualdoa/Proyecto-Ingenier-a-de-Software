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
    }
}
