using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GCP_CF.Models;

namespace GCP_CF.Controllers
{
    public class TiposEstadoContratoController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: TiposEstadoContrato
        public ActionResult Index()
        {
            return View(db.TiposEstadoContrato.ToList());
        }

        // GET: TiposEstadoContrato/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposEstadoContrato tiposEstadoContrato = db.TiposEstadoContrato.Find(id);
            if (tiposEstadoContrato == null)
            {
                return HttpNotFound();
            }
            return View(tiposEstadoContrato);
        }

        // GET: TiposEstadoContrato/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposEstadoContrato/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TiposEstadoContrato_Id,Descripcion")] TiposEstadoContrato tiposEstadoContrato)
        {
            if (ModelState.IsValid)
            {
                db.TiposEstadoContrato.Add(tiposEstadoContrato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposEstadoContrato);
        }

        // GET: TiposEstadoContrato/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposEstadoContrato tiposEstadoContrato = db.TiposEstadoContrato.Find(id);
            if (tiposEstadoContrato == null)
            {
                return HttpNotFound();
            }
            return View(tiposEstadoContrato);
        }

        // POST: TiposEstadoContrato/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TiposEstadoContrato_Id,Descripcion")] TiposEstadoContrato tiposEstadoContrato)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposEstadoContrato).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposEstadoContrato);
        }

        // GET: TiposEstadoContrato/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposEstadoContrato tiposEstadoContrato = db.TiposEstadoContrato.Find(id);
            if (tiposEstadoContrato == null)
            {
                return HttpNotFound();
            }
            return View(tiposEstadoContrato);
        }

        // POST: TiposEstadoContrato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TiposEstadoContrato tiposEstadoContrato = db.TiposEstadoContrato.Find(id);
            db.TiposEstadoContrato.Remove(tiposEstadoContrato);
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
