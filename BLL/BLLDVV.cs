using DAL;
using DALs;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLDVV
    {
        private readonly GeneradorDigVerificador generador = new GeneradorDigVerificador();
        private readonly DALDVV dalDVV = new DALDVV();

        public void RecalcularDigitosVerificadores()
        {
            RecalcularDVHUsuario();
            RecalcularDVHPerfil();
            RecalcularDVHFamilia();
            RecalcularDVHEvento();
            RecalcularDVHPerfilPermiso();
            RecalcularDVHPerfilFamilia();
            RecalcularDVHPermisoFamilia();
            RecalcularDVHFamiliaFamilia();
            RecalcularDVHIdioma();
            RecalcularDVHPermiso();
            RecalcularTodosLosDVV();
        }

        public void RecalcularTodosLosDVV()
        {
            RecalcularDVVUsuario();
            RecalcularDVVPerfil();
            RecalcularDVVFamilia();
            RecalcularDVVEvento();
            RecalcularDVVPerfilPermiso();
            RecalcularDVVPerfilFamilia();
            RecalcularDVVPermisoFamilia();
            RecalcularDVVFamiliaFamilia();
            RecalcularDVVIdioma();
            RecalcularDVVPermiso();
        }

        public void RecalcularDVHUsuario()
        {
            DALUsuario dal = new DALUsuario();
            List<ServicioUsuario> usuarios = dal.ObtenerUsuarios();

            foreach (ServicioUsuario usuario in usuarios)
            {
                usuario.DVH = generador.GenerarDVH(usuario);
                dal.ActualizarDVHUsuario(usuario.DNI, usuario.DVH);
            }
        }
        public void RecalcularDVHPerfil()
        {
            DALPerfil dal = new DALPerfil();
            List<ServicioPerfil> perfiles = dal.ObtenerPerfiles();

            foreach (ServicioPerfil x in perfiles)
            {
                x.DVH = generador.GenerarDVH(x);
                dal.ActualizarDVHPerfil(x.IdPerfil, x.DVH);
            }
        }

        public void RecalcularDVHFamilia()
        {
            DALFamilia dal = new DALFamilia();
            List<ServicioFamilia> fams = dal.ObtenerFamilias();

            foreach (ServicioFamilia x in fams)
            {
                x.DVH = generador.GenerarDVH(x);
                dal.ActualizarDVHFamilia(x.IdPerfil, x.DVH);
            }
        }

        public void RecalcularDVHEvento()
        {
            DALEvento dal = new DALEvento();
            List<ServicioEvento> eventos = dal.ObtenerTodosLosEventos();

            foreach (ServicioEvento x in eventos)
            {
                x.DVH = generador.GenerarDVH(x);
                dal.ActualizarDVHEvento(x.IdEvento, x.DVH);
            }
        }

        public void RecalcularDVHPerfilPermiso()
        {
            DALPerfil dal = new DALPerfil();
            List<ServicioPerfilPermiso> lista = dal.ObtenerRelacionesPerfilPermiso();

            foreach (ServicioPerfilPermiso x in lista)
            {
                x.DVH = generador.GenerarDVH(x);
                dal.ActualizarDVHPerfilPermiso(x.IdPerfil, x.IdPermiso, x.DVH);
            }
        }

        public void RecalcularDVHPerfilFamilia()
        {
            DALPerfil dal = new DALPerfil();
            List<ServicioPerfilFamilia> lista = dal.ObtenerRelacionesPerfilFamilia();

            foreach (ServicioPerfilFamilia x in lista)
            {
                x.DVH = generador.GenerarDVH(x);
                dal.ActualizarDVHPerfilFamilia(x.IdPerfil, x.IdFamilia, x.DVH);
            }
        }

        public void RecalcularDVHPermisoFamilia()
        {
            DALFamilia dal = new DALFamilia();
            List<ServicioPermisoFamilia> lista = dal.ObtenerRelacionesPermisoFamilia();

            foreach (ServicioPermisoFamilia x in lista)
            {
                x.DVH = generador.GenerarDVH(x);
                dal.ActualizarDVHPermisoFamilia(x.IdFamilia, x.IdPermiso, x.DVH);
            }
        }

        public void RecalcularDVHFamiliaFamilia()
        {
            DALFamilia dal = new DALFamilia();
            List<ServicioFamiliaFamilia> lista = dal.ObtenerRelacionesFamiliaFamilia();

            foreach (ServicioFamiliaFamilia x in lista)
            {
                x.DVH = generador.GenerarDVH(x);
                dal.ActualizarDVHFamiliaFamilia(x.IdFamiliaPadre, x.IdFamiliaHijo, x.DVH);
            }
        }

        public void RecalcularDVHPermiso()
        {
            DALPermiso dal = new DALPermiso();
            List<ServicioPermiso> permisos = dal.ObtenerTodos();

            foreach (ServicioPermiso permiso in permisos)
            {
                permiso.DVH = generador.GenerarDVH(permiso);
                dal.ActualizarDVHPermiso(permiso.IdPerfil, permiso.DVH);
            }
        }

        public void RecalcularDVHIdioma()
        {
            DALIdioma dal = new DALIdioma();
            List<ServicioIdioma> idiomas = dal.ListarIdiomas();

            foreach (ServicioIdioma idioma in idiomas)
            {
                idioma.DVH = generador.GenerarDVH(idioma);
                dal.ActualizarDVHIdioma(idioma.IdIdioma, idioma.DVH);
            }
        }
        public void RecalcularDVVUsuario()
        {
            DALUsuario dal = new DALUsuario();
            GuardarDVV("Usuario", dal.ObtenerUsuarios().Cast<object>().ToList());
        }

        public void RecalcularDVVPerfil()
        {
            DALPerfil dal = new DALPerfil();
            GuardarDVV("Perfil", dal.ObtenerPerfiles().Cast<object>().ToList());
        }

        public void RecalcularDVVFamilia()
        {
            DALFamilia dal = new DALFamilia();
            GuardarDVV("Familia", dal.ObtenerFamilias().Cast<object>().ToList());
        }

        public void RecalcularDVVEvento()
        {
            DALEvento dal = new DALEvento();
            GuardarDVV("Evento", dal.ObtenerTodosLosEventos().Cast<object>().ToList());
        }

        public void RecalcularDVVPerfilPermiso()
        {
            DALPerfil dal = new DALPerfil();
            GuardarDVV("Perfil_x_Permiso", dal.ObtenerRelacionesPerfilPermiso().Cast<object>().ToList());
        }

        public void RecalcularDVVPerfilFamilia()
        {
            DALPerfil dal = new DALPerfil();
            GuardarDVV("Perfil_x_Familia", dal.ObtenerRelacionesPerfilFamilia().Cast<object>().ToList());
        }

        public void RecalcularDVVPermisoFamilia()
        {
            DALFamilia dal = new DALFamilia();
            GuardarDVV("Permiso_x_Familia", dal.ObtenerRelacionesPermisoFamilia().Cast<object>().ToList());
        }

        public void RecalcularDVVFamiliaFamilia()
        {
            DALFamilia dal = new DALFamilia();
            GuardarDVV("Familia_x_Familia", dal.ObtenerRelacionesFamiliaFamilia().Cast<object>().ToList());
        }
        public void RecalcularDVVPermiso()
        {
            DALPermiso dal = new DALPermiso();
            GuardarDVV("Permiso", dal.ObtenerTodos().Cast<object>().ToList());
        }

        public void RecalcularDVVIdioma()
        {
            DALIdioma dal = new DALIdioma();
            GuardarDVV("Idioma", dal.ListarIdiomas().Cast<object>().ToList());
        }

        private void GuardarDVV(string nombreTabla, List<object> registros)
        {
            string digito = CalcularDVV(nombreTabla, registros);

            dalDVV.GuardarDVV(new ServicioDVV
            {
                NombreTabla = nombreTabla,
                Digito = digito
            });
        }

        private string CalcularDVV(string nombreTabla, List<object> registros)
        {
            return new GeneradorDigVerificador().GenerarDVV(nombreTabla, registros);
        }

        public bool ValidarIntegridad()
        {
            return ObtenerErroresIntegridad().Count == 0;
        }
        public List<ServicioErrorIntegridad> ObtenerErroresIntegridad()
        {
            List<ServicioErrorIntegridad> errores = new List<ServicioErrorIntegridad>();

            errores.AddRange(ValidarDVHTabla("Usuario", new DALUsuario().ObtenerUsuarios().Cast<object>().ToList()));
            errores.AddRange(ValidarDVHTabla("Perfil", new DALPerfil().ObtenerPerfiles().Cast<object>().ToList()));
            errores.AddRange(ValidarDVHTabla("Familia", new DALFamilia().ObtenerFamilias().Cast<object>().ToList()));
            errores.AddRange(ValidarDVHTabla("Evento", new DALEvento().ObtenerTodosLosEventos().Cast<object>().ToList()));
            errores.AddRange(ValidarDVHTabla("Perfil_x_Permiso", new DALPerfil().ObtenerRelacionesPerfilPermiso().Cast<object>().ToList()));
            errores.AddRange(ValidarDVHTabla("Perfil_x_Familia", new DALPerfil().ObtenerRelacionesPerfilFamilia().Cast<object>().ToList()));
            errores.AddRange(ValidarDVHTabla("Permiso_x_Familia", new DALFamilia().ObtenerRelacionesPermisoFamilia().Cast<object>().ToList()));
            errores.AddRange(ValidarDVHTabla("Familia_x_Familia", new DALFamilia().ObtenerRelacionesFamiliaFamilia().Cast<object>().ToList()));
            errores.AddRange(ValidarDVHTabla("Permiso", new DALPermiso().ObtenerTodos().Cast<object>().ToList()));
            errores.AddRange(ValidarDVHTabla("Idioma", new DALIdioma().ListarIdiomas().Cast<object>().ToList()));
            errores.AddRange(ValidarDVVDetallado());

            return errores;
        }

        private List<ServicioErrorIntegridad> ValidarDVHTabla(string nombreTabla, List<object> registros)
        {
            List<ServicioErrorIntegridad> errores = new List<ServicioErrorIntegridad>();

            foreach (object registro in registros)
            {
                var propiedadDVH = registro.GetType().GetProperty("DVH");
                string dvhGuardado = propiedadDVH?.GetValue(registro)?.ToString();
                string dvhCalculado = generador.GenerarDVH(registro);

                if (dvhGuardado != dvhCalculado)
                {
                    errores.Add(new ServicioErrorIntegridad
                    {
                        Tabla = nombreTabla,
                        Registro = ObtenerIdentificadorRegistro(registro),
                        TipoError = "DVH incorrecto",
                        ValorEsperado = dvhCalculado,
                        ValorActual = dvhGuardado
                    });
                }
            }

            return errores;
        }

        private List<ServicioErrorIntegridad> ValidarDVVDetallado()
        {
            List<ServicioErrorIntegridad> errores = new List<ServicioErrorIntegridad>();
            List<ServicioDVV> dvvsGuardados = dalDVV.ObtenerDVV();

            ValidarDVVTablaDetallado("Usuario", new DALUsuario().ObtenerUsuarios().Cast<object>().ToList(), dvvsGuardados, errores);
            ValidarDVVTablaDetallado("Perfil", new DALPerfil().ObtenerPerfiles().Cast<object>().ToList(), dvvsGuardados, errores);
            ValidarDVVTablaDetallado("Familia", new DALFamilia().ObtenerFamilias().Cast<object>().ToList(), dvvsGuardados, errores);
            ValidarDVVTablaDetallado("Evento", new DALEvento().ObtenerTodosLosEventos().Cast<object>().ToList(), dvvsGuardados, errores);
            ValidarDVVTablaDetallado("Perfil_x_Permiso", new DALPerfil().ObtenerRelacionesPerfilPermiso().Cast<object>().ToList(), dvvsGuardados, errores);
            ValidarDVVTablaDetallado("Perfil_x_Familia", new DALPerfil().ObtenerRelacionesPerfilFamilia().Cast<object>().ToList(), dvvsGuardados, errores);
            ValidarDVVTablaDetallado("Permiso_x_Familia", new DALFamilia().ObtenerRelacionesPermisoFamilia().Cast<object>().ToList(), dvvsGuardados, errores);
            ValidarDVVTablaDetallado("Familia_x_Familia", new DALFamilia().ObtenerRelacionesFamiliaFamilia().Cast<object>().ToList(), dvvsGuardados, errores);
            ValidarDVVTablaDetallado("Permiso", new DALPermiso().ObtenerTodos().Cast<object>().ToList(), dvvsGuardados, errores);
            ValidarDVVTablaDetallado("Idioma", new DALIdioma().ListarIdiomas().Cast<object>().ToList(), dvvsGuardados, errores);
            return errores;
        }

        private void ValidarDVVTablaDetallado(
                                                string nombreTabla,
                                                List<object> registros,
                                                List<ServicioDVV> dvvsGuardados,
                                                List<ServicioErrorIntegridad> errores)
        {
            string dvvCalculado = CalcularDVV(nombreTabla, registros);
            string dvvGuardado = dvvsGuardados.FirstOrDefault(d => d.NombreTabla == nombreTabla)?.Digito;

            if (dvvGuardado != dvvCalculado)
            {
                errores.Add(new ServicioErrorIntegridad
                {
                    Tabla = nombreTabla,
                    Registro = "Tabla completa",
                    TipoError = "DVV incorrecto",
                    ValorEsperado = dvvCalculado,
                    ValorActual = dvvGuardado
                });
            }
        }

        private string ObtenerIdentificadorRegistro(object registro)
        {
            string[] posiblesIds =
            {
        "DNI",
        "IdPerfil",
        "IdFamilia",
        "IdPermiso",
        "IdEvento",
        "IdFamiliaPadre",
        "IdFamiliaHijo",
        "IdIdioma"
    };

            List<string> partes = new List<string>();

            foreach (string nombrePropiedad in posiblesIds)
            {
                var propiedad = registro.GetType().GetProperty(nombrePropiedad);

                if (propiedad != null)
                {
                    object valor = propiedad.GetValue(registro);

                    if (valor != null)
                    {
                        partes.Add(nombrePropiedad + ": " + valor);
                    }
                }
            }

            if (partes.Count == 0)
            {
                return registro.ToString();
            }

            return string.Join(" | ", partes);
        }
    }
}
