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
    public class arz_retrabajosController : Controller
    {
        private DB_A3F19C_producccionEntities4 db = new DB_A3F19C_producccionEntities4();

        // GET: arz_retrabajos
        public ActionResult Index()
        {
            return View(db.arz_retrabajos.ToList());
        }

        // GET: arz_retrabajos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_retrabajos arz_retrabajos = db.arz_retrabajos.Find(id);
            if (arz_retrabajos == null)
            {
                return HttpNotFound();
            }
            return View(arz_retrabajos);
        }

        // GET: arz_retrabajos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: arz_retrabajos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Duracion")] arz_retrabajos arz_retrabajos)
        {
            if (ModelState.IsValid)
            {
                db.arz_retrabajos.Add(arz_retrabajos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(arz_retrabajos);
        }

        // GET: arz_retrabajos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_retrabajos arz_retrabajos = db.arz_retrabajos.Find(id);
            if (arz_retrabajos == null)
            {
                return HttpNotFound();
            }
            return View(arz_retrabajos);
        }

        // POST: arz_retrabajos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Duracion")] arz_retrabajos arz_retrabajos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arz_retrabajos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arz_retrabajos);
        }

        // GET: arz_retrabajos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            arz_retrabajos arz_retrabajos = db.arz_retrabajos.Find(id);
            if (arz_retrabajos == null)
            {
                return HttpNotFound();
            }
            return View(arz_retrabajos);
        }

        // POST: arz_retrabajos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            arz_retrabajos arz_retrabajos = db.arz_retrabajos.Find(id);
            db.arz_retrabajos.Remove(arz_retrabajos);
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
