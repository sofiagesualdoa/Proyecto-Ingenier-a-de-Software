using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioFamilia : ServicioPerfil
    {
        private List<ServicioPerfil> listaperfil;

        public ServicioFamilia(int id, string nombre, string dvh) : base(id, nombre, dvh)
        {
            listaperfil = new List<ServicioPerfil>();
        }

        public override List<ServicioPerfil> Hijos => listaperfil;

        public override void Agregar(ServicioPerfil c)
        {
            listaperfil.Add(c);
        }

        public override void Eliminar(string nombre)
        {
            ServicioPerfil encontrar = Buscar(nombre);
            if (encontrar != null) listaperfil.Remove(encontrar);
        }

        public override ServicioPerfil Buscar(string nombre)
        {
            if (Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)) return this;

            foreach (var c in listaperfil)
            {
                ServicioPerfil enc = c.Buscar(nombre);
                if (enc != null) return enc;
            }
            return null;
        }

    }
}
