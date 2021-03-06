﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosAdministracion;

namespace WebAppProduccion.Controllers.Administracion
{
    public class statusproveedorsController : Controller
    {
        private DB_A3F19C_producccionEntities2 db = new DB_A3F19C_producccionEntities2();

        // GET: statusproveedors
        public ActionResult Index()
        {
            return View(db.statusproveedor.ToList());
        }

        // GET: statusproveedors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            statusproveedor statusproveedor = db.statusproveedor.Find(id);
            if (statusproveedor == null)
            {
                return HttpNotFound();
            }
            return View(statusproveedor);
        }

        // GET: statusproveedors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: statusproveedors/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion")] statusproveedor statusproveedor)
        {
            if (ModelState.IsValid)
            {
                db.statusproveedor.Add(statusproveedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(statusproveedor);
        }

        // GET: statusproveedors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            statusproveedor statusproveedor = db.statusproveedor.Find(id);
            if (statusproveedor == null)
            {
                return HttpNotFound();
            }
            return View(statusproveedor);
        }

        // POST: statusproveedors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion")] statusproveedor statusproveedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(statusproveedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(statusproveedor);
        }

        // GET: statusproveedors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            statusproveedor statusproveedor = db.statusproveedor.Find(id);
            if (statusproveedor == null)
            {
                return HttpNotFound();
            }
            return View(statusproveedor);
        }

        // POST: statusproveedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            statusproveedor statusproveedor = db.statusproveedor.Find(id);
            db.statusproveedor.Remove(statusproveedor);
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
