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
    public class TiposNaturalezaController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: TiposNaturaleza
        public ActionResult Index()
        {
            return View(db.TiposNaturaleza.ToList());
        }

        // GET: TiposNaturaleza/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposNaturaleza tiposNaturaleza = db.TiposNaturaleza.Find(id);
            if (tiposNaturaleza == null)
            {
                return HttpNotFound();
            }
            return View(tiposNaturalezas);
        }

        // GET: TiposNaturaleza/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposNaturaleza/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Naturaleza_Id,Descripcion")] TiposNaturalezas tiposNaturalezas)
        {
            if (ModelState.IsValid)
            {
                db.TiposNaturaleza.Add(tiposNaturaleza);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposNaturalezas);
        }

        // GET: TiposNaturaleza/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposNaturaleza tiposNaturaleza = db.TiposNaturaleza.Find(id);
            if (tiposNaturaleza == null)
            {
                return HttpNotFound();
            }
            return View(tiposNaturalezas);
        }

        // POST: TiposNaturaleza/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Naturaleza_Id,Descripcion")] TiposNaturalezas tiposNaturalezas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposNaturalezas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposNaturalezas);
        }

        // GET: TiposNaturaleza/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposNaturaleza tiposNaturaleza = db.TiposNaturaleza.Find(id);
            if (tiposNaturaleza == null)
            {
                return HttpNotFound();
            }
            return View(tiposNaturalezas);
        }

        // POST: TiposNaturaleza/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TiposNaturaleza tiposNaturaleza = db.TiposNaturaleza.Find(id);
            db.TiposNaturaleza.Remove(tiposNaturaleza);
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
