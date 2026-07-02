using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public abstract class ServicioPerfil
    {
        [NoVerificar]
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }
        public string DVH { get; set; }

        public ServicioPerfil(int id, string nombre, string dvh)
        {
            IdPerfil = id;
            Nombre = nombre;
            DVH = dvh;
        }
        public abstract void Agregar(ServicioPerfil c);
        public abstract void Eliminar(string nombre);
        public abstract ServicioPerfil Buscar(string nombre);
        [NoVerificar]
        public abstract List<ServicioPerfil> Hijos { get; }
        public override string ToString() => Nombre;

    }
}
