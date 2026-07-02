using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioEncriptador
    {
        public string Encriptar(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytesOriginales = Encoding.UTF8.GetBytes(texto);
                byte[] bytesHasheados = sha256.ComputeHash(bytesOriginales);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytesHasheados.Length; i++)
                {
                    sb.Append(bytesHasheados[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public string EncriptarBase64(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytesOriginales = Encoding.UTF8.GetBytes(texto);
                byte[] bytesHasheados = sha256.ComputeHash(bytesOriginales);
                return Convert.ToBase64String(bytesHasheados);
            }
        }
    }
}
