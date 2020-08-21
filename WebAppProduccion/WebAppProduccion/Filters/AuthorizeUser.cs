using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosSistemas;

namespace WebAppProduccion.Filters
{
    public class AuthorizeUser : AuthorizeAttribute
    {
        private empleados oUsuario;
        private DB_A3F19C_producccionEntities1 db = new DB_A3F19C_producccionEntities1();
        private int idOperacion;
        
        public AuthorizeUser(int IdOperacion = 0)
        {
            this.idOperacion = IdOperacion;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string nombreOperacion = "";
            string nombreModulo = "";
            string error = "No tiene permiso para realizar esta accion";
            try
            {
                string userlogin = HttpContext.Current.Session["ua"].ToString();


                oUsuario = db.empleados.Where(x => x.Email == userlogin).FirstOrDefault();
                var listaMisOperaciones = from m in db.rolesoperaciones
                                          where m.Rol_Id == oUsuario.Puestos_Id && m.Operaciones_Id == idOperacion
                                          select m;

                if (listaMisOperaciones.ToList().Count() == 0)
                {
                    var operacion = db.operaciones.Find(idOperacion);
                    int? idModulo = operacion.Modulos_Id;
                    nombreOperacion = ObtenerNombreOperacion(idOperacion);
                    nombreModulo = ObtenerNombreModulo(idModulo);
                    filterContext.Result = new RedirectResult("~/errores/UnauthorizedOperation?operacion=" + nombreOperacion + "&modulo=" + nombreModulo + "&msjeErrorExcepcion=" + error);
                }
            }
            catch (Exception ex)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }

        private string ObtenerNombreOperacion(int IdOperacion)
        {
            string nombreOperacion;

            try
            {
                var operacion = from op in db.operaciones
                                where op.id == IdOperacion
                                select op.Nombre;

                nombreOperacion = operacion.First();

                return nombreOperacion;
            }
            catch (Exception)
            {
                nombreOperacion = "";
            }

            return nombreOperacion;
        }

        public string ObtenerNombreModulo(int? idModulo)
        {
            string nombreModulo;

            try
            {
                var modulo = from m in db.modulos
                             where m.id == idModulo
                             select m.descripcion;

                nombreModulo = modulo.First();
            }
            catch (Exception)
            {
                nombreModulo = "";
            }

            return nombreModulo;
        }
    }
}