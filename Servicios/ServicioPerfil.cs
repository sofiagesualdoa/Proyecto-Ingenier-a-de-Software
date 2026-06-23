using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public abstract class ServicioPerfil
    {
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }

        public ServicioPerfil(int id, string nombre)
        {
            IdPerfil = id;
            Nombre = nombre;
        }
        public abstract void Agregar(ServicioPerfil c);
        public abstract void Eliminar(string nombre);
        public abstract ServicioPerfil Buscar(string nombre);
        public abstract List<ServicioPerfil> Hijos { get; }
        public override string ToString() => Nombre;

    }
}
