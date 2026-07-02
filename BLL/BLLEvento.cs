using DALs;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLEvento
    {
        private readonly DALEvento dalEvento = new DALEvento();
        private readonly GeneradorDigVerificador generador = new GeneradorDigVerificador();
        public void GrabarBitacora(string accion, string modulo, int nivelCriticidad)
        {
            ServicioEvento registro = new ServicioEvento();
            registro.NombreEvento = accion;
            registro.Modulo = modulo;
            registro.Criticidad = nivelCriticidad;
            registro.Fecha = DateTime.Today;
            registro.Hora = DateTime.Now.TimeOfDay;

            ServicioUsuario usuarioActivo = ServicioSessionManager.GetInstance().ObtenerUsuario();
            if (usuarioActivo != null)
            {
                registro.Login = usuarioActivo.nombreUsuario;
                registro.DNI = usuarioActivo.DNI;
            }
            else
            {
                registro.Login = "Desconocido";
                registro.DNI = 0;
            }
            registro.DVH = generador.GenerarDVH(registro);
            dalEvento.RegistrarEvento(registro);
            new BLLDVV().RecalcularDVVEvento();
        }

        public List<ServicioEvento> ConsultarEventosPorDefecto()
        {
            DateTime fechaFiltro = DateTime.Today.AddDays(-3);
            return dalEvento.ObtenerEventos(fechaFiltro);
        }
    }
}
