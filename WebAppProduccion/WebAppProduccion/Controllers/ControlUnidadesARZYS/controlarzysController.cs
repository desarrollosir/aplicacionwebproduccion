using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosArzys;
using WebAppProduccion.Filters;

namespace WebAppProduccion.Controllers.ControlUnidadesARZYS
{
    public class controlarzysController : Controller
    {
        private DB_A3F19C_producccionEntities4 db = new DB_A3F19C_producccionEntities4();

        // GET: controlarzys
        [AuthorizeUser(IdOperacion: 46)]
        public ActionResult Index()
        {

            var conteoenproceso = (from u in db.arz_unidades
                                   join dtp in db.arz_detunidadproceso on u.Id equals dtp.arz_unidades_Id
                                   where dtp.arz_proceso_Id == 5 && dtp.CheckIn == false && u.arz_statusunidad_Id == 2
                                   select u).Count();

            var conteoterminadas = (from u in db.arz_unidades
                                   join dtp in db.arz_detunidadproceso on u.Id equals dtp.arz_unidades_Id
                                   where dtp.arz_proceso_Id == 5 && dtp.CheckIn == true && u.arz_statusunidad_Id == 2
                                   select u).Count();

            var conteotransito = db.arz_unidades.Where(x => x.arz_statusunidad_Id == 1).Count();

            var conteoterminadasconcita = db.arz_unidades.Where(x => x.arz_statusunidad_Id == 3 && x.CitaDestino != null).Count();

            var conteoterminadassincita = db.arz_unidades.Where(x => x.arz_statusunidad_Id == 3 && x.CitaDestino == null).Count();

            var total = conteoenproceso + conteoterminadas + conteotransito + conteoterminadasconcita + conteoterminadassincita;

            ViewBag.CantidadEnProceso = conteoenproceso;
            ViewBag.CantidadListas = conteoterminadas;
            ViewBag.CantidadEnTransito = conteotransito;
            ViewBag.CantidadTerminadasConCita = conteoterminadasconcita;
            ViewBag.CantidadTerminadasSinCita = conteoterminadassincita;
            ViewBag.Total = total;

            return View();
        }

        [AuthorizeUser(IdOperacion: 44)]
        public ActionResult IndexPhone()
        {
            List<arz_unidades> listaRetorno = new List<arz_unidades>();
            var consulta = (from u in db.arz_unidades
                           join d in db.arz_detunidadproceso on u.Id equals d.arz_unidades_Id
                           where d.arz_proceso_Id == 6 && u.arz_statusunidad_Id != 5 && d.CheckIn == false 
                           select new { u.Id, u.Economico, u.arz_lineatransporte, u.arz_destinos }).ToList();

            foreach (var item in consulta)
            {
                arz_unidades ab = new arz_unidades();
                ab.Id = item.Id;
                ab.Economico = item.Economico;
                ab.arz_lineatransporte = item.arz_lineatransporte;
                ab.arz_destinos = item.arz_destinos;
                listaRetorno.Add(ab);                
            }
            
            return View(listaRetorno);
        }

        [AuthorizeUser(IdOperacion: 45)]
        public ActionResult ActividadesUnidad(int idunidad)
        {
            var modelo = db.arz_detunidadproceso.Where(x => x.arz_unidades_Id == idunidad).ToList();
            ViewBag.IdUnidad = idunidad;
            ViewBag.IdEstado = db.arz_unidades.Find(idunidad).arz_statusunidad_Id;
            return View(modelo);
        }

        [HttpPost]
        public ActionResult ActualizarDetalle(int idProceso, int idUnidad, int retrabajo) 
        {
            try
            {
                DateTime fechahora = DateTime.Now.AddHours(2);

                if (idProceso == 7)
                {
                    //Sin Retrabajos
                    arz_detunidadproceso inicioretrabajo = db.arz_detunidadproceso.Where(x => x.arz_proceso_Id == 3 && x.arz_unidades_Id == idUnidad).FirstOrDefault();
                    inicioretrabajo.CheckIn = true;
                    inicioretrabajo.FechaHoraInicio = fechahora;

                    arz_detunidadproceso finretrabajo = db.arz_detunidadproceso.Where(x => x.arz_proceso_Id == 4 && x.arz_unidades_Id == idUnidad).FirstOrDefault();
                    finretrabajo.CheckIn = true;
                    finretrabajo.FechaHoraInicio = fechahora;
                }
                else if (idProceso == 6)
                {
                    arz_unidades unidad = db.arz_unidades.Where(x => x.Id == idUnidad).FirstOrDefault();
                    unidad.arz_statusunidad_Id = 3;

                    arz_detunidadproceso detunidadproceso = db.arz_detunidadproceso.Where(x => x.arz_proceso_Id == idProceso && x.arz_unidades_Id == idUnidad).FirstOrDefault();
                    detunidadproceso.CheckIn = true;
                    detunidadproceso.FechaHoraInicio = fechahora;
                }
                else
                {
                    arz_detunidadproceso detunidadproceso = db.arz_detunidadproceso.Where(x => x.arz_proceso_Id == idProceso && x.arz_unidades_Id == idUnidad).FirstOrDefault();
                    detunidadproceso.CheckIn = true;
                    detunidadproceso.FechaHoraInicio = fechahora;

                    arz_unidades unidad = db.arz_unidades.Where(x => x.Id == idUnidad).FirstOrDefault();
                    unidad.arz_statusunidad_Id = 2;
                }

                if (retrabajo > 0)
                {
                    arz_detunidadretrabajo detunidadretrabajo = db.arz_detunidadretrabajo.Where(x => x.arz_unidades_Id == idUnidad).FirstOrDefault();                    
                    detunidadretrabajo.arz_retrabajos_Id = retrabajo;                    
                }

                db.SaveChanges();

                return Json(new { respuesta = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { respuesta = false }, JsonRequestBehavior.AllowGet);                
            }            
        }
    }
}