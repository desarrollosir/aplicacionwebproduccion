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
    public class arz_statusunidadController : Controller
    {
        private DB_A3F19C_producccionEntities4 db = new DB_A3F19C_producccionEntities4();

        // GET: arz_statusunidad
        public ActionResult Index()
        {
            return View(db.arz_statusunidad.ToList());
        }

        // GET: arz_statusunidad/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_statusunidad arz_statusunidad = db.arz_statusunidad.Find(id);
            if (arz_statusunidad == null)
            {
                return HttpNotFound();
            }
            return View(arz_statusunidad);
        }

        // GET: arz_statusunidad/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: arz_statusunidad/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion")] arz_statusunidad arz_statusunidad)
        {
            if (ModelState.IsValid)
            {
                db.arz_statusunidad.Add(arz_statusunidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(arz_statusunidad);
        }

        // GET: arz_statusunidad/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_statusunidad arz_statusunidad = db.arz_statusunidad.Find(id);
            if (arz_statusunidad == null)
            {
                return HttpNotFound();
            }
            return View(arz_statusunidad);
        }

        // POST: arz_statusunidad/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion")] arz_statusunidad arz_statusunidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arz_statusunidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arz_statusunidad);
        }

        // GET: arz_statusunidad/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_statusunidad arz_statusunidad = db.arz_statusunidad.Find(id);
            if (arz_statusunidad == null)
            {
                return HttpNotFound();
            }
            return View(arz_statusunidad);
        }

        // POST: arz_statusunidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            arz_statusunidad arz_statusunidad = db.arz_statusunidad.Find(id);
            db.arz_statusunidad.Remove(arz_statusunidad);
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
