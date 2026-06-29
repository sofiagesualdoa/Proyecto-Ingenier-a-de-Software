using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicios;

namespace BLL
{
    public class BLLBackUp
    {
        private DALBackUp dalBackUp = new DALBackUp();
        private BLLEvento bitacora = new BLLEvento();

        public void RealizarBackup()
        {
            string carpetaBackup = @"C:\EverGlow\Backups";
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory;
            //string carpetaBackup = Path.Combine(rutaBase, "Backups");

            if (!Directory.Exists(carpetaBackup))
                Directory.CreateDirectory(carpetaBackup);

            ServicioBackUp backup = new ServicioBackUp();
            backup.PathDestino = carpetaBackup;
            backup.NombreArchivo = $"Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";

            dalBackUp.EjecutarBackup("Data Source=.;Initial Catalog=EverGlow;Integrated Security=True;Trust Server Certificate=True", backup);

            bitacora.GrabarBitacora($"Creación de backup", "Respaldos", 1);
        }
    }
}
