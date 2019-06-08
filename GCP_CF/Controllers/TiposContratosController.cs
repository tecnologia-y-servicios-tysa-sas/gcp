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
    public class TiposContratosController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: TiposContratos
        public ActionResult Index()
        {
            return View(db.TiposContratos.ToList());
        }

        // GET: TiposContratos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposContratos tiposContratos = db.TiposContratos.Find(id);
            if (tiposContratos == null)
            {
                return HttpNotFound();
            }
            return View(tiposContratos);
        }

        // GET: TiposContratos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposContratos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TipoContrato_Id,Descripcion")] TiposContratos tiposContratos)
        {
            if (ModelState.IsValid)
            {
                db.TiposContratos.Add(tiposContratos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposContratos);
        }

        // GET: TiposContratos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposContratos tiposContratos = db.TiposContratos.Find(id);
            if (tiposContratos == null)
            {
                return HttpNotFound();
            }
            return View(tiposContratos);
        }

        // POST: TiposContratos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoContrato_Id,Descripcion")] TiposContratos tiposContratos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposContratos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposContratos);
        }

        // GET: TiposContratos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposContratos tiposContratos = db.TiposContratos.Find(id);
            if (tiposContratos == null)
            {
                return HttpNotFound();
            }
            return View(tiposContratos);
        }

        // POST: TiposContratos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TiposContratos tiposContratos = db.TiposContratos.Find(id);
            db.TiposContratos.Remove(tiposContratos);
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
