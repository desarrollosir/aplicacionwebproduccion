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
    public class arz_lineatransporteController : Controller
    {
        private DB_A3F19C_producccionEntities4 db = new DB_A3F19C_producccionEntities4();

        // GET: arz_lineatransporte
        public ActionResult Index()
        {
            return View(db.arz_lineatransporte.ToList());
        }

        // GET: arz_lineatransporte/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_lineatransporte arz_lineatransporte = db.arz_lineatransporte.Find(id);
            if (arz_lineatransporte == null)
            {
                return HttpNotFound();
            }
            return View(arz_lineatransporte);
        }

        // GET: arz_lineatransporte/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: arz_lineatransporte/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion")] arz_lineatransporte arz_lineatransporte)
        {
            if (ModelState.IsValid)
            {
                db.arz_lineatransporte.Add(arz_lineatransporte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(arz_lineatransporte);
        }

        // GET: arz_lineatransporte/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_lineatransporte arz_lineatransporte = db.arz_lineatransporte.Find(id);
            if (arz_lineatransporte == null)
            {
                return HttpNotFound();
            }
            return View(arz_lineatransporte);
        }

        // POST: arz_lineatransporte/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion")] arz_lineatransporte arz_lineatransporte)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arz_lineatransporte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arz_lineatransporte);
        }

        // GET: arz_lineatransporte/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_lineatransporte arz_lineatransporte = db.arz_lineatransporte.Find(id);
            if (arz_lineatransporte == null)
            {
                return HttpNotFound();
            }
            return View(arz_lineatransporte);
        }

        // POST: arz_lineatransporte/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            arz_lineatransporte arz_lineatransporte = db.arz_lineatransporte.Find(id);
            db.arz_lineatransporte.Remove(arz_lineatransporte);
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
