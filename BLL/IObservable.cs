using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IObservable
    {
        void AgregarSuscriptor(IObserver suscriptor);
        void BorrarSuscriptor(IObserver suscriptor);
        void NotificarSuscriptores(ServicioIdioma idioma);
    }
}
