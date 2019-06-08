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
    public class TiposEstadoContratoesController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: TiposEstadoContratoes
        public ActionResult Index()
        {
            return View(db.TiposEstadoContratoes.ToList());
        }

        // GET: TiposEstadoContratoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposEstadoContrato tiposEstadoContrato = db.TiposEstadoContratoes.Find(id);
            if (tiposEstadoContrato == null)
            {
                return HttpNotFound();
            }
            return View(tiposEstadoContrato);
        }

        // GET: TiposEstadoContratoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposEstadoContratoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TiposEstadoContrato_Id,Descripcion")] TiposEstadoContrato tiposEstadoContrato)
        {
            if (ModelState.IsValid)
            {
                db.TiposEstadoContratoes.Add(tiposEstadoContrato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposEstadoContrato);
        }

        // GET: TiposEstadoContratoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposEstadoContrato tiposEstadoContrato = db.TiposEstadoContratoes.Find(id);
            if (tiposEstadoContrato == null)
            {
                return HttpNotFound();
            }
            return View(tiposEstadoContrato);
        }

        // POST: TiposEstadoContratoes/Edit/5
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

        // GET: TiposEstadoContratoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposEstadoContrato tiposEstadoContrato = db.TiposEstadoContratoes.Find(id);
            if (tiposEstadoContrato == null)
            {
                return HttpNotFound();
            }
            return View(tiposEstadoContrato);
        }

        // POST: TiposEstadoContratoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TiposEstadoContrato tiposEstadoContrato = db.TiposEstadoContratoes.Find(id);
            db.TiposEstadoContratoes.Remove(tiposEstadoContrato);
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
