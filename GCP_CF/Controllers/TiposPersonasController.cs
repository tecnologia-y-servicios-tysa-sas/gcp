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
    public class TiposPersonasController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: TiposPersonas
        public ActionResult Index()
        {
            var tiposPersonas = db.TiposPersonas.Include(t => t.TiposNaturaleza);
            return View(tiposPersonas.ToList());
        }

        // GET: TiposPersonas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPersona tiposPersona = db.TiposPersonas.Find(id);
            if (tiposPersona == null)
            {
                return HttpNotFound();
            }
            return View(tiposPersona);
        }

        // GET: TiposPersonas/Create
        public ActionResult Create()
        {
            ViewBag.Naturaleza_Id = new SelectList(db.TiposNaturalezas, "Naturaleza_Id", "Descripcion");
            return View();
        }

        // POST: TiposPersonas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TipoPersona_Id,Descripcion,Naturaleza_Id,Cargo")] TiposPersona tiposPersona)
        {
            if (ModelState.IsValid)
            {
                db.TiposPersonas.Add(tiposPersona);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Naturaleza_Id = new SelectList(db.TiposNaturalezas, "Naturaleza_Id", "Descripcion", tiposPersona.Naturaleza_Id);
            return View(tiposPersona);
        }

        // GET: TiposPersonas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPersona tiposPersona = db.TiposPersonas.Find(id);
            if (tiposPersona == null)
            {
                return HttpNotFound();
            }
            ViewBag.Naturaleza_Id = new SelectList(db.TiposNaturalezas, "Naturaleza_Id", "Descripcion", tiposPersona.Naturaleza_Id);
            return View(tiposPersona);
        }

        // POST: TiposPersonas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoPersona_Id,Descripcion,Naturaleza_Id,Cargo")] TiposPersona tiposPersona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposPersona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Naturaleza_Id = new SelectList(db.TiposNaturalezas, "Naturaleza_Id", "Descripcion", tiposPersona.Naturaleza_Id);
            return View(tiposPersona);
        }

        // GET: TiposPersonas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPersona tiposPersona = db.TiposPersonas.Find(id);
            if (tiposPersona == null)
            {
                return HttpNotFound();
            }
            return View(tiposPersona);
        }

        // POST: TiposPersonas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TiposPersona tiposPersona = db.TiposPersonas.Find(id);
            db.TiposPersonas.Remove(tiposPersona);
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
