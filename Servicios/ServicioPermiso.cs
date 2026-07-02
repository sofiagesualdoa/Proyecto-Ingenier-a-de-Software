using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioPermiso : ServicioPerfil
    {
        public ServicioPermiso(int id, string nombre, string dvh) : base(id, nombre, dvh) { }

        public override List<ServicioPerfil> Hijos => null;

        public override void Agregar(ServicioPerfil c)
        {
            throw new InvalidOperationException("No se puede agregar en hojas");
        }

        public override void Eliminar(string nombre)
        {
            throw new InvalidOperationException("No se puede eliminar en hojas");
        }

        public override ServicioPerfil Buscar(string nombre)
        {
            if (Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)) return this;
            return null;
        }
    }
}
