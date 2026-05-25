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
            private string contraseña; 
            public string nombreUsuario { get; set; } 

            public bool Activo { get; set; }
            public bool Bloqueado { get; set; }
            public bool ValidarPassword(string hashClaveIngresada)
            {
                return this.contraseña == hashClaveIngresada;
            }

            public void ActualizarPasswordMemoria(string hashNuevaClave)
            {
                this.contraseña = hashNuevaClave;
            }
        }
    }
}
