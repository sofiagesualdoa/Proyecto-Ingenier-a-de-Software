using System.Windows.Forms;

namespace Servicios
{
    public class ServicioUsuario
    {
        public string DVH { get; set; }
        public int DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string contraseña { get; set; }
        public string nombreUsuario { get; set; }
        public int IdPerfil { get; set; }

        [NoVerificar]
        public ServicioPerfil PerfilUsuario { get; set; }

        public int IntentosInicio { get; set; }
        public bool Activo { get; set; }
        public bool Bloqueado { get; set; }
        public int IdIdioma { get; set; }
        
        [NoVerificar]
        public ServicioIdioma Idioma { get; set; }
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
}
