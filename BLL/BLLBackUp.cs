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

        string cadena = "Data Source=.;Initial Catalog=EverGlow;Integrated Security=True;Trust Server Certificate=True";
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
            dalBackUp.EjecutarBackup(cadena, backup);
            bitacora.GrabarBitacora("Creación de backup", "Respaldos", 1);
        }

        public void RealizarRestore(string rutaCompletaArchivo)
        {
            if (!File.Exists(rutaCompletaArchivo) || Path.GetExtension(rutaCompletaArchivo).ToLower() != ".bak")
            {
                throw new ArgumentException(ServicioSessionManager.GetInstance().Traducir("El archivo seleccionado no es un backup válido o está corrupto."));
            }
            dalBackUp.EjecutarRestore(rutaCompletaArchivo);
            bitacora.GrabarBitacora("Realizar Restore", "Respaldos", 1);
        }
    }
}
