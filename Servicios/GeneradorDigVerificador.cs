using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class GeneradorDigVerificador
    {
        private readonly ServicioEncriptador encriptador = new ServicioEncriptador();

        public string GenerarDVH(object entidad)
        {
            Type tipo = entidad.GetType();
            string nombreClase = tipo.Name;
            int longitudClase = CalcularLongitud(entidad);

            StringBuilder cadena = new StringBuilder();
            cadena.Append(nombreClase);

            PropertyInfo[] propiedades = tipo.GetProperties()
            .Where(p => p.CanRead)
            .Where(p => p.Name != "DVH")
            .Where(p => !Attribute.IsDefined(p, typeof(NoVerificarAttribute)))
            .OrderBy(p => p.MetadataToken)
            .ToArray();

            foreach (PropertyInfo propiedad in propiedades)
            {
                object valor = propiedad.GetValue(entidad);
                if (valor != null)
                {
                    cadena.Append(valor.ToString());
                }
            }

            string transformado = Transformar(cadena.ToString(), longitudClase);
            return encriptador.EncriptarBase64(transformado);
        }

        public string GenerarDVV(string nombreTabla, IEnumerable<object> registros)
        {
            StringBuilder cadena = new StringBuilder();

            foreach (object registro in registros)
            {
                var propiedadDVH = registro.GetType().GetProperty("DVH");
                string dvh = propiedadDVH?.GetValue(registro)?.ToString();

                if (!string.IsNullOrEmpty(dvh))
                    cadena.Append(dvh);
            }

            cadena.Append(nombreTabla);

            int suma = 0;
            foreach (char caracter in cadena.ToString())
                suma += Convert.ToInt32(caracter);

            return suma.ToString();
        }

        private int CalcularLongitud(object entidad)
        {
            return entidad.GetType().Name.Length;
        }

        private string Transformar(string valor, int desplazamiento)
        {
            StringBuilder resultado = new StringBuilder();

            for (int i = 0; i < valor.Length; i++)
            {
                int posicion = i + 1;
                int ascii = TransformarASCII(valor[i]);
                int ponderado = ascii + posicion;
                int desplazado = ponderado + desplazamiento;

                resultado.Append(desplazado);
            }

            return resultado.ToString();
        }

        private int TransformarASCII(char caracter)
        {
            return Convert.ToInt32(caracter);
        }
    }
}
