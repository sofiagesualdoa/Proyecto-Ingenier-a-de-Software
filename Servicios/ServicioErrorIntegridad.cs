using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioErrorIntegridad
    {
        public string Tabla { get; set; }
        public string Registro { get; set; }
        public string TipoError { get; set; }
        public string ValorEsperado { get; set; }
        public string ValorActual { get; set; }
    }
}