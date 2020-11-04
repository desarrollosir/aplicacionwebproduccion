using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosEscaneos;

namespace WebAppProduccion.Controllers.HomeDelivery
{
    public class hd_statusquickshipController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: hd_statusquickship
        public ActionResult Index()
        {
            return View(db.hd_statusquickship.ToList());
        }

        // GET: hd_statusquickship/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hd_statusquickship hd_statusquickship = db.hd_statusquickship.Find(id);
            if (hd_statusquickship == null)
            {
                return HttpNotFound();
            }
            return View(hd_statusquickship);
        }

        // GET: hd_statusquickship/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: hd_statusquickship/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion")] hd_statusquickship hd_statusquickship)
        {
            if (ModelState.IsValid)
            {
                db.hd_statusquickship.Add(hd_statusquickship);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hd_statusquickship);
        }

        // GET: hd_statusquickship/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hd_statusquickship hd_statusquickship = db.hd_statusquickship.Find(id);
            if (hd_statusquickship == null)
            {
                return HttpNotFound();
            }
            return View(hd_statusquickship);
        }

        // POST: hd_statusquickship/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion")] hd_statusquickship hd_statusquickship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hd_statusquickship).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hd_statusquickship);
        }

        // GET: hd_statusquickship/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hd_statusquickship hd_statusquickship = db.hd_statusquickship.Find(id);
            if (hd_statusquickship == null)
            {
                return HttpNotFound();
            }
            return View(hd_statusquickship);
        }

        // POST: hd_statusquickship/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hd_statusquickship hd_statusquickship = db.hd_statusquickship.Find(id);
            db.hd_statusquickship.Remove(hd_statusquickship);
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
