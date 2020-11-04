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
    public class hd_statusordenController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: hd_statusorden
        public ActionResult Index()
        {
            return View(db.hd_statusorden.ToList());
        }

        // GET: hd_statusorden/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hd_statusorden hd_statusorden = db.hd_statusorden.Find(id);
            if (hd_statusorden == null)
            {
                return HttpNotFound();
            }
            return View(hd_statusorden);
        }

        // GET: hd_statusorden/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: hd_statusorden/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion")] hd_statusorden hd_statusorden)
        {
            if (ModelState.IsValid)
            {
                db.hd_statusorden.Add(hd_statusorden);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hd_statusorden);
        }

        // GET: hd_statusorden/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hd_statusorden hd_statusorden = db.hd_statusorden.Find(id);
            if (hd_statusorden == null)
            {
                return HttpNotFound();
            }
            return View(hd_statusorden);
        }

        // POST: hd_statusorden/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion")] hd_statusorden hd_statusorden)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hd_statusorden).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hd_statusorden);
        }

        // GET: hd_statusorden/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hd_statusorden hd_statusorden = db.hd_statusorden.Find(id);
            if (hd_statusorden == null)
            {
                return HttpNotFound();
            }
            return View(hd_statusorden);
        }

        // POST: hd_statusorden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hd_statusorden hd_statusorden = db.hd_statusorden.Find(id);
            db.hd_statusorden.Remove(hd_statusorden);
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
