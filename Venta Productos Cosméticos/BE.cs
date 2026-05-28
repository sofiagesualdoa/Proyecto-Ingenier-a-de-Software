using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venta_Productos_Cosméticos
{
    internal class BE
    {
        public class Usuario
        {
            public int DNI {  get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Email { get; set; }
            private string contraseña; 
            public string nombreUsuario { get; set; } 
            public string Rol {  get; set; }

            public int IntentosInicio { get; set; }
            public bool Activo { get; set; }
            public bool Bloqueado { get; set; }
            public void SetPassword(string hash)
            {
                contraseña = hash.ToUpper().Trim();
            }

            public string GetPassword()
            {
                return contraseña;
            }

            public bool ValidarPassword(string hashIngresado)
            {
                return this.contraseña.ToUpper().Trim() == hashIngresado.ToUpper().Trim();
            }

            public void ActualizarPasswordMemoria(string nuevoHash)
            {
                this.contraseña = nuevoHash.ToUpper().Trim();
            }
        }

        public class Evento
        {
            public int IdEvento { get; set; }
            public string Login { get; set; }      
            public int Criticidad { get; set; }    
            public DateTime Fecha { get; set; }
            public TimeSpan Hora { get; set; }      
            public string NombreEvento { get; set; }     
            public string Modulo { get; set; }      
            public int DNI { get; set; }           
        }
    }
}
