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
    public class hd_skushomedeliveryController : Controller
    {
        private DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        // GET: hd_skushomedelivery
        public ActionResult Index()
        {
            var hd_skushomedelivery = db.hd_skushomedelivery.Include(h => h.skus);
            return View(hd_skushomedelivery.ToList());
        }

        // GET: hd_skushomedelivery/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hd_skushomedelivery hd_skushomedelivery = db.hd_skushomedelivery.Find(id);
            if (hd_skushomedelivery == null)
            {
                return HttpNotFound();
            }
            return View(hd_skushomedelivery);
        }

        // GET: hd_skushomedelivery/Create
        public ActionResult Create()
        {
            ViewBag.skus_Id = new SelectList(db.skus, "id", "SKU");
            return View();
        }

        // POST: hd_skushomedelivery/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,QtyManual,QrCode,skus_Id")] hd_skushomedelivery hd_skushomedelivery)
        {
            if (ModelState.IsValid)
            {
                db.hd_skushomedelivery.Add(hd_skushomedelivery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.skus_Id = new SelectList(db.skus, "id", "SKU", hd_skushomedelivery.skus_Id);
            return View(hd_skushomedelivery);
        }

        // GET: hd_skushomedelivery/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hd_skushomedelivery hd_skushomedelivery = db.hd_skushomedelivery.Find(id);
            if (hd_skushomedelivery == null)
            {
                return HttpNotFound();
            }

            hd_skushomedelivery.QtyManualValor = (bool)hd_skushomedelivery.QtyManual;
            hd_skushomedelivery.QrCodeValor = (bool)hd_skushomedelivery.QrCode;

            ViewBag.skus_Id = new SelectList(db.skus, "id", "SKU", hd_skushomedelivery.skus_Id);
            return View(hd_skushomedelivery);
        }

        // POST: hd_skushomedelivery/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(hd_skushomedelivery hd_skushomedelivery)
        {            
            if (ModelState.IsValid)
            {
                hd_skushomedelivery sku = db.hd_skushomedelivery.Find(hd_skushomedelivery.Id);
                sku.QrCode = hd_skushomedelivery.QrCodeValor;
                sku.QtyManual = hd_skushomedelivery.QtyManualValor;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.skus_Id = new SelectList(db.skus, "id", "SKU", hd_skushomedelivery.skus_Id);
            return View(hd_skushomedelivery);
        }

        // GET: hd_skushomedelivery/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hd_skushomedelivery hd_skushomedelivery = db.hd_skushomedelivery.Find(id);
            if (hd_skushomedelivery == null)
            {
                return HttpNotFound();
            }
            return View(hd_skushomedelivery);
        }

        // POST: hd_skushomedelivery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hd_skushomedelivery hd_skushomedelivery = db.hd_skushomedelivery.Find(id);
            db.hd_skushomedelivery.Remove(hd_skushomedelivery);
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
