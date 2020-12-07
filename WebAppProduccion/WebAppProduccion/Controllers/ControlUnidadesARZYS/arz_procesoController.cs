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
    public class arz_procesoController : Controller
    {
        private DB_A3F19C_producccionEntities4 db = new DB_A3F19C_producccionEntities4();

        // GET: arz_proceso
        public ActionResult Index()
        {
            return View(db.arz_proceso.ToList());
        }

        // GET: arz_proceso/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_proceso arz_proceso = db.arz_proceso.Find(id);
            if (arz_proceso == null)
            {
                return HttpNotFound();
            }
            return View(arz_proceso);
        }

        // GET: arz_proceso/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: arz_proceso/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion")] arz_proceso arz_proceso)
        {
            if (ModelState.IsValid)
            {
                db.arz_proceso.Add(arz_proceso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(arz_proceso);
        }

        // GET: arz_proceso/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_proceso arz_proceso = db.arz_proceso.Find(id);
            if (arz_proceso == null)
            {
                return HttpNotFound();
            }
            return View(arz_proceso);
        }

        // POST: arz_proceso/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion")] arz_proceso arz_proceso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arz_proceso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arz_proceso);
        }

        // GET: arz_proceso/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_proceso arz_proceso = db.arz_proceso.Find(id);
            if (arz_proceso == null)
            {
                return HttpNotFound();
            }
            return View(arz_proceso);
        }

        // POST: arz_proceso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            arz_proceso arz_proceso = db.arz_proceso.Find(id);
            db.arz_proceso.Remove(arz_proceso);
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
