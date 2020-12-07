using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosArzys;
using System.Linq.Dynamic;
using WebAppProduccion.Filters;
using System.IO;

namespace WebAppProduccion.Controllers.ControlUnidadesARZYS
{
    public class arz_unidadesController : Controller
    {
        private DB_A3F19C_producccionEntities4 db = new DB_A3F19C_producccionEntities4();
        CultureInfo culture = new CultureInfo("es-MX");

        // GET: arz_unidades
        [AuthorizeUser(IdOperacion: 37)]
        public ActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerUnidades()
        {
            var Draw = Request.Form.GetValues("draw").FirstOrDefault();
            var Start = Request.Form.GetValues("start").FirstOrDefault();
            var Length = Request.Form.GetValues("length").FirstOrDefault();
            var SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
            var SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            var economico = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var idstatus = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();

            int PageSize = Length != null ? Convert.ToInt32(Length) : 0;
            int Skip = Start != null ? Convert.ToInt32(Start) : 0;
            int TotalRecords = 0;

            List<arz_unidades> lista = new List<arz_unidades>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionGlobal"].ToString()))
            {
                con.Open();

                string sql = "exec [SP_Unidades_Index] @economico, @statusid";
                var query = new SqlCommand(sql, con);

                if (economico != "")
                {
                    query.Parameters.AddWithValue("@economico", economico);
                }
                else
                {
                    query.Parameters.AddWithValue("@economico", DBNull.Value);
                }

                if (idstatus != "" && idstatus != "0" && idstatus != null)
                {
                    query.Parameters.AddWithValue("@statusid", idstatus);
                }
                else
                {
                    query.Parameters.AddWithValue("@statusid", DBNull.Value);
                }

                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // Unidades
                        var unidades = new arz_unidades();

                        unidades.Id = Convert.ToInt32(dr["Id"]);
                        unidades.Economico = dr["Economico"].ToString();
                        unidades.lineatransporte = dr["Linea"].ToString();
                        unidades.destino = dr["Destino"].ToString();
                        unidades.statusunidad = dr["EstadoUnidad"].ToString();
                        unidades.retrabajo = dr["Retrabajo"].ToString();

                        if (dr["CitaArribo"].ToString() != "")
                        {
                            DateTime citaarribo = DateTime.Parse(dr["CitaArribo"].ToString());
                            unidades.citaarribostring = citaarribo.ToString("dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("es-MX"));
                        }
                        else
                        {
                            unidades.CitaArribo = DateTime.MinValue;
                        }                       

                        if (dr["CitaDestino"].ToString() != "")
                        {                           
                            DateTime citadestino = DateTime.Parse(dr["CitaDestino"].ToString());
                            unidades.citadestinostring = citadestino.ToString("dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("es-MX"));
                        }
                        else
                        {
                            unidades.CitaDestino = DateTime.MinValue;
                        }

                        lista.Add(unidades);
                    }
                }
            }

            if (!(string.IsNullOrEmpty(SortColumn) && string.IsNullOrEmpty(SortColumnDir)))
            {
                lista = lista.OrderBy(SortColumn + " " + SortColumnDir).ToList();
            }

            TotalRecords = lista.ToList().Count();
            var NewItems = lista.Skip(Skip).Take(PageSize == -1 ? TotalRecords : PageSize).ToList();

            return Json(new { draw = Draw, recordsFiltered = TotalRecords, recordsTotal = TotalRecords, data = NewItems }, JsonRequestBehavior.AllowGet);
        }

        // GET: arz_unidades/Create
        [AuthorizeUser(IdOperacion: 38)]
        public ActionResult Create()
        {
            ViewBag.arz_destinos_Id = new SelectList(db.arz_destinos, "Id", "Descripcion");
            ViewBag.arz_lineatransporte_Id = new SelectList(db.arz_lineatransporte, "Id", "Descripcion");
            ViewBag.arz_statusunidad_Id = new SelectList(db.arz_statusunidad, "Id", "Descripcion");
            return View();
        }

        // POST: arz_unidades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(arz_unidades arz_unidades)
        {
            try
            {
                // Display using pt-BR culture's short date format            
                CultureInfo culture = new CultureInfo("es-MX");

                DateTime date = Convert.ToDateTime(arz_unidades.citaarribostring, culture);

                DateTime date2 = Convert.ToDateTime(arz_unidades.citadestinostring, culture);

                arz_unidades.FechaHoraAlta = DateTime.Now.AddHours(2);

                arz_unidades.CitaArribo = date;

                arz_unidades.CitaDestino = date2;

                arz_unidades.arz_statusunidad_Id = 1;

                if (arz_unidades.citadestinostring == null)
                {
                    arz_unidades.CitaDestino = null;
                }

                if (ModelState.IsValid)
                {
                    db.arz_unidades.Add(arz_unidades);
                    db.SaveChanges();

                    int idunidad = arz_unidades.Id;

                    AgregarProcesos(idunidad);

                    AgregarRetrabajo(idunidad);

                    return RedirectToAction("Index");
                }

                ViewBag.arz_destinos_Id = new SelectList(db.arz_destinos, "Id", "Descripcion", arz_unidades.arz_destinos_Id);
                ViewBag.arz_lineatransporte_Id = new SelectList(db.arz_lineatransporte, "Id", "Descripcion", arz_unidades.arz_lineatransporte_Id);
                ViewBag.arz_statusunidad_Id = new SelectList(db.arz_statusunidad, "Id", "Descripcion", arz_unidades.arz_statusunidad_Id);
                return View(arz_unidades);
            }
            catch (Exception _ex)
            {
                ViewBag.Error = _ex.Message.ToString();
                return View(arz_unidades);
            }
        }

        [AuthorizeUser(IdOperacion: 39)]
        public ActionResult CerrarTodasLasUnidades(int id)
        {
            return View();
        }

        [HttpPost]
        public JsonResult CerrarTodasLasUnidades(arz_unidades unidad)
        {
            var unidades = from u in db.arz_unidades
                           join dtp in db.arz_detunidadproceso on u.Id equals dtp.arz_unidades_Id
                           where u.arz_statusunidad_Id == 3 && dtp.arz_proceso_Id == 6 && dtp.CheckIn == true
                           select new { u.Id, u.Economico, dtp.FechaHoraInicio } ;

            foreach (var item in unidades)
            {
                DateTime fechaHoy = DateTime.Now.Date;
                DateTime fechaDespachada = item.FechaHoraInicio.Value.Date;

                if (fechaDespachada < fechaHoy)
                {
                    arz_unidades unidadTemp = db.arz_unidades.Find(item.Id);
                    unidadTemp.arz_statusunidad_Id = 4;
                }
            }

            db.SaveChanges();
            
            return Json(new { respuesta = true }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeUser(IdOperacion: 40)]
        public ActionResult CancelarUnidad(int idunidad) 
        {
            arz_unidades unidad = db.arz_unidades.Find(idunidad);
            return View(unidad);
        }

        [HttpPost]
        public ActionResult CancelarUnidad(arz_unidades unidad) 
        {
            try
            {
                arz_unidades unidadTemp = db.arz_unidades.Where(x => x.Id == unidad.Id).FirstOrDefault();

                if (unidadTemp.arz_statusunidad_Id == 1)
                {
                    unidadTemp.arz_statusunidad_Id = 5;
                    db.SaveChanges();

                    return Json(new { respuesta = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { respuesta = false, msg = "No Se Puede Cancelar Esta Unidad." }, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception)
            {
                return Json(new { respuesta = false }, JsonRequestBehavior.AllowGet);                
            }            
        }

        [AuthorizeUser(IdOperacion: 41)]
        public ActionResult VerDetalle(int idunidad)
        {
            List<arz_detunidadproceso> detalle = db.arz_detunidadproceso.Where(x => x.arz_unidades_Id == idunidad).ToList();
            ViewBag.Economico = db.arz_unidades.Where(x => x.Id == idunidad).FirstOrDefault().Economico;

            return View(detalle);
        }

        [AuthorizeUser(IdOperacion: 42)]
        public ActionResult CerrarUnidad(int idunidad)
        {
            arz_unidades unidad = db.arz_unidades.Where(x => x.Id == idunidad).FirstOrDefault();
            return View(unidad);
        }

        [HttpPost]
        public ActionResult CerrarUnidad(arz_unidades unidad)
        {
            try
            {
                arz_unidades arz_unidades = db.arz_unidades.Find(unidad.Id);
                if (arz_unidades.arz_statusunidad_Id == 3)
                {
                    arz_unidades.arz_statusunidad_Id = 4;
                    db.SaveChanges();

                    return Json(new { respuesta = true }, JsonRequestBehavior.AllowGet);
                }
                if (arz_unidades.arz_statusunidad_Id == 4)
                {
                    return Json(new { respuesta = false, msg = "Esta Unidad Ya Está Cerrada." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { respuesta = false, msg = "Esta Unidad No Está Terminada." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { respuesta = false }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: arz_unidades/Edit/5
        [AuthorizeUser(IdOperacion: 43)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_unidades arz_unidades = db.arz_unidades.Find(id);

            CultureInfo culture = new CultureInfo("es-MX");

            DateTime citaarribo = Convert.ToDateTime(arz_unidades.CitaArribo, culture);
            arz_unidades.citaarribostring = citaarribo.ToString("dd:MM:yyyy HH:mm", culture);

            if (arz_unidades.CitaDestino == null)
            {
                arz_unidades.citadestinostring = "";
            }
            else
            {
                DateTime citadestino = Convert.ToDateTime(arz_unidades.CitaDestino, culture);
                arz_unidades.citadestinostring = citadestino.ToString("dd:MM:yyyy HH:mm", culture);
            }

            if (arz_unidades == null)
            {
                return HttpNotFound();
            }

            ViewBag.arz_destinos_Id = new SelectList(db.arz_destinos, "Id", "Descripcion", arz_unidades.arz_destinos_Id);
            ViewBag.arz_lineatransporte_Id = new SelectList(db.arz_lineatransporte, "Id", "Descripcion", arz_unidades.arz_lineatransporte_Id);
            ViewBag.arz_statusunidad_Id = new SelectList(db.arz_statusunidad, "Id", "Descripcion", arz_unidades.arz_statusunidad_Id);

            return View(arz_unidades);
        }

        // POST: arz_unidades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(arz_unidades arz_unidades)
        {
            CultureInfo culture = new CultureInfo("es-MX");

            DateTime date = Convert.ToDateTime(arz_unidades.citaarribostring, culture);
            arz_unidades.CitaArribo = date;

            if (arz_unidades.citadestinostring == "" || arz_unidades.citadestinostring == null)
            {
                arz_unidades.CitaDestino = null;
            }
            else
            {
                DateTime date2 = Convert.ToDateTime(arz_unidades.citadestinostring, culture);
                arz_unidades.CitaDestino = date2;
            }

            if (ModelState.IsValid)
            {
                db.Entry(arz_unidades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.arz_destinos_Id = new SelectList(db.arz_destinos, "Id", "Descripcion", arz_unidades.arz_destinos_Id);
            ViewBag.arz_lineatransporte_Id = new SelectList(db.arz_lineatransporte, "Id", "Descripcion", arz_unidades.arz_lineatransporte_Id);
            ViewBag.arz_statusunidad_Id = new SelectList(db.arz_statusunidad, "Id", "Descripcion", arz_unidades.arz_statusunidad_Id);
            return View(arz_unidades);
        }


        public ActionResult CargarUnidades()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportarUnidades(HttpPostedFileBase postedFileBase)
        {
            try
            {
                string filePath = string.Empty;

                if (postedFileBase != null)
                {
                    string path = Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFileBase.FileName);
                    string extension = Path.GetExtension(postedFileBase.FileName);
                    string numeromo = Path.GetFileName(postedFileBase.FileName);
                   
                }
                return RedirectToAction("Index");
            }
            catch (Exception _ex)
            {
                return RedirectToAction("CargarUnidades", new { mensaje = _ex.Message.ToString() });
            }
        }

        [HttpPost]
        public JsonResult ListaDestinos()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            foreach (var item in db.arz_destinos.OrderBy(x => x.Descripcion).ToList())
            {
                lista.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Descripcion
                });
            }

            return Json(lista);
        }

        [HttpPost]
        public JsonResult ListaLineas()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            foreach (var item in db.arz_lineatransporte.OrderBy(x => x.Descripcion).ToList())
            {
                lista.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Descripcion
                });
            }

            return Json(lista);
        }

        [HttpPost]
        public JsonResult ListaStatus()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            foreach (var item in db.arz_statusunidad.OrderBy(x => x.Id).ToList())
            {
                lista.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Descripcion
                });
            }

            return Json(lista);
        }

        public void AgregarProcesos(int idunidad)
        {
            List<arz_detunidadproceso> listaTemp = new List<arz_detunidadproceso>();
            foreach (var item in db.arz_proceso.ToList())
            {
                arz_detunidadproceso detunidadproceso = new arz_detunidadproceso();
                detunidadproceso.arz_unidades_Id = idunidad;
                detunidadproceso.arz_proceso_Id = item.Id;
                detunidadproceso.CheckIn = false;
                listaTemp.Add(detunidadproceso);
            }

            db.arz_detunidadproceso.AddRange(listaTemp);
            db.SaveChanges();
        }

        public void AgregarRetrabajo(int idUnidad)
        {
            arz_detunidadretrabajo detunidadretrabajo = new arz_detunidadretrabajo();
            detunidadretrabajo.arz_unidades_Id = idUnidad;
            detunidadretrabajo.arz_retrabajos_Id = 4;
            db.arz_detunidadretrabajo.Add(detunidadretrabajo);
            db.SaveChanges();
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
