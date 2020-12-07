using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosSistemas;

namespace WebAppProduccion.Controllers
{
    public class HomeController : Controller
    {
        DB_A3F19C_producccionEntities1 db = new DB_A3F19C_producccionEntities1();
        public ActionResult Index()
        {
            Session["ua"] = User.Identity.Name;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string NombreUsuario(string email)
        {
            var usuario = db.empleados.Where(x => x.Email.Equals(email)).FirstOrDefault();
            string user;
            if (usuario != null)
            {
                return user = usuario.Nombres + " " + usuario.ApellidoPaterno;
            }
            else
            {
                return "Sin Usuario";
            }            
        }
    }
}