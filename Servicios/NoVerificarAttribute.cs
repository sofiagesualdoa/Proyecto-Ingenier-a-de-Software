using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NoVerificarAttribute : Attribute
    {
    }
}
