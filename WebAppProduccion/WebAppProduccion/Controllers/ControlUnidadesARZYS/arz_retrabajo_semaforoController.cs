using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosArzys;

namespace WebAppProduccion.Controllers.ControlUnidadesARZYS
{
    public class arz_retrabajo_semaforoController : Controller
    {
        private DB_A3F19C_producccionEntities4 db = new DB_A3F19C_producccionEntities4();

        // GET: arz_retrabajo_semaforo
        public ActionResult Index()
        {
            var arz_retrabajo_semaforo = db.arz_retrabajo_semaforo.Include(a => a.arz_retrabajos);
            return View(arz_retrabajo_semaforo.ToList());
        }

        // GET: arz_retrabajo_semaforo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_retrabajo_semaforo arz_retrabajo_semaforo = db.arz_retrabajo_semaforo.Find(id);
            if (arz_retrabajo_semaforo == null)
            {
                return HttpNotFound();
            }
            return View(arz_retrabajo_semaforo);
        }

        // GET: arz_retrabajo_semaforo/Create
        public ActionResult Create()
        {
            ViewBag.arz_retrabajos_Id = new SelectList(db.arz_retrabajos, "Id", "Descripcion");
            return View();
        }

        // POST: arz_retrabajo_semaforo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MinutosParaAgregar,Semaforo,arz_retrabajos_Id")] arz_retrabajo_semaforo arz_retrabajo_semaforo)
        {
            if (ModelState.IsValid)
            {
                db.arz_retrabajo_semaforo.Add(arz_retrabajo_semaforo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.arz_retrabajos_Id = new SelectList(db.arz_retrabajos, "Id", "Descripcion", arz_retrabajo_semaforo.arz_retrabajos_Id);
            return View(arz_retrabajo_semaforo);
        }

        // GET: arz_retrabajo_semaforo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_retrabajo_semaforo arz_retrabajo_semaforo = db.arz_retrabajo_semaforo.Find(id);
            if (arz_retrabajo_semaforo == null)
            {
                return HttpNotFound();
            }
            ViewBag.arz_retrabajos_Id = new SelectList(db.arz_retrabajos, "Id", "Descripcion", arz_retrabajo_semaforo.arz_retrabajos_Id);
            return View(arz_retrabajo_semaforo);
        }

        // POST: arz_retrabajo_semaforo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MinutosParaAgregar,Semaforo,arz_retrabajos_Id")] arz_retrabajo_semaforo arz_retrabajo_semaforo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arz_retrabajo_semaforo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.arz_retrabajos_Id = new SelectList(db.arz_retrabajos, "Id", "Descripcion", arz_retrabajo_semaforo.arz_retrabajos_Id);
            return View(arz_retrabajo_semaforo);
        }

        // GET: arz_retrabajo_semaforo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_retrabajo_semaforo arz_retrabajo_semaforo = db.arz_retrabajo_semaforo.Find(id);
            if (arz_retrabajo_semaforo == null)
            {
                return HttpNotFound();
            }
            return View(arz_retrabajo_semaforo);
        }

        // POST: arz_retrabajo_semaforo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            arz_retrabajo_semaforo arz_retrabajo_semaforo = db.arz_retrabajo_semaforo.Find(id);
            db.arz_retrabajo_semaforo.Remove(arz_retrabajo_semaforo);
            db.SaveChanges();
            return RedirectToAction("Index");
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
