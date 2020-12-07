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
    public class arz_destinosController : Controller
    {
        private DB_A3F19C_producccionEntities4 db = new DB_A3F19C_producccionEntities4();

        // GET: arz_destinos
        public ActionResult Index()
        {
            return View(db.arz_destinos.ToList());
        }

        // GET: arz_destinos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_destinos arz_destinos = db.arz_destinos.Find(id);
            if (arz_destinos == null)
            {
                return HttpNotFound();
            }
            return View(arz_destinos);
        }

        // GET: arz_destinos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: arz_destinos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,TiempoArribo")] arz_destinos arz_destinos)
        {
            if (ModelState.IsValid)
            {
                db.arz_destinos.Add(arz_destinos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(arz_destinos);
        }

        // GET: arz_destinos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_destinos arz_destinos = db.arz_destinos.Find(id);
            if (arz_destinos == null)
            {
                return HttpNotFound();
            }
            return View(arz_destinos);
        }

        // POST: arz_destinos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,TiempoArribo")] arz_destinos arz_destinos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arz_destinos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arz_destinos);
        }

        // GET: arz_destinos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_destinos arz_destinos = db.arz_destinos.Find(id);
            if (arz_destinos == null)
            {
                return HttpNotFound();
            }
            return View(arz_destinos);
        }

        // POST: arz_destinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            arz_destinos arz_destinos = db.arz_destinos.Find(id);
            db.arz_destinos.Remove(arz_destinos);
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
