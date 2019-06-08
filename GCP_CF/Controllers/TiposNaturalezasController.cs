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
    public class TiposNaturalezasController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: TiposNaturalezas
        public ActionResult Index()
        {
            return View(db.TiposNaturalezas.ToList());
        }

        // GET: TiposNaturalezas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposNaturaleza tiposNaturaleza = db.TiposNaturalezas.Find(id);
            if (tiposNaturaleza == null)
            {
                return HttpNotFound();
            }
            return View(tiposNaturaleza);
        }

        // GET: TiposNaturalezas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposNaturalezas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Naturaleza_Id,Descripcion")] TiposNaturaleza tiposNaturaleza)
        {
            if (ModelState.IsValid)
            {
                db.TiposNaturalezas.Add(tiposNaturaleza);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposNaturaleza);
        }

        // GET: TiposNaturalezas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposNaturaleza tiposNaturaleza = db.TiposNaturalezas.Find(id);
            if (tiposNaturaleza == null)
            {
                return HttpNotFound();
            }
            return View(tiposNaturaleza);
        }

        // POST: TiposNaturalezas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Naturaleza_Id,Descripcion")] TiposNaturaleza tiposNaturaleza)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposNaturaleza).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposNaturaleza);
        }

        // GET: TiposNaturalezas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposNaturaleza tiposNaturaleza = db.TiposNaturalezas.Find(id);
            if (tiposNaturaleza == null)
            {
                return HttpNotFound();
            }
            return View(tiposNaturaleza);
        }

        // POST: TiposNaturalezas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TiposNaturaleza tiposNaturaleza = db.TiposNaturalezas.Find(id);
            db.TiposNaturalezas.Remove(tiposNaturaleza);
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
