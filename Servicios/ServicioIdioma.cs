using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioIdioma
    {
        [NoVerificar]
        public int IdIdioma { get; set; }
        public string Nombre { get; set; } 
        public string CodigoIdioma { get; set; }
        public string DVH { get; set; }

        [NoVerificar]
        public Dictionary<string, string> DiccionarioLeyendas { get; set; }

        public ServicioIdioma()
        {
            DiccionarioLeyendas = new Dictionary<string, string>();
        }
    }
}
