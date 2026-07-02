using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioEvento
    {
        [NoVerificar]
        public int IdEvento { get; set; }
        public string Login { get; set; }
        public int Criticidad { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string NombreEvento { get; set; }
        public string Modulo { get; set; }
        public int DNI { get; set; }
        public string DVH { get; set; }

    }
}
