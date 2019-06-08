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
            var tiposPersonas = db.TiposPersonas.Include(t => t.TiposNaturalezas);
            return View(tiposPersonas.ToList());
        }

        // GET: TiposPersonas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPersonas tiposPersonas = db.TiposPersonas.Find(id);
            if (tiposPersonas == null)
            {
                return HttpNotFound();
            }
            return View(tiposPersonas);
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
        public ActionResult Create([Bind(Include = "TipoPersona_Id,Descripcion,Naturaleza_Id,Cargo")] TiposPersonas tiposPersonas)
        {
            if (ModelState.IsValid)
            {
                db.TiposPersonas.Add(tiposPersonas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Naturaleza_Id = new SelectList(db.TiposNaturalezas, "Naturaleza_Id", "Descripcion", tiposPersonas.Naturaleza_Id);
            return View(tiposPersonas);
        }

        // GET: TiposPersonas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPersonas tiposPersonas = db.TiposPersonas.Find(id);
            if (tiposPersonas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Naturaleza_Id = new SelectList(db.TiposNaturalezas, "Naturaleza_Id", "Descripcion", tiposPersonas.Naturaleza_Id);
            return View(tiposPersonas);
        }

        // POST: TiposPersonas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoPersona_Id,Descripcion,Naturaleza_Id,Cargo")] TiposPersonas tiposPersonas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposPersonas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Naturaleza_Id = new SelectList(db.TiposNaturalezas, "Naturaleza_Id", "Descripcion", tiposPersonas.Naturaleza_Id);
            return View(tiposPersonas);
        }

        // GET: TiposPersonas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPersonas tiposPersonas = db.TiposPersonas.Find(id);
            if (tiposPersonas == null)
            {
                return HttpNotFound();
            }
            return View(tiposPersonas);
        }

        // POST: TiposPersonas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TiposPersonas tiposPersonas = db.TiposPersonas.Find(id);
            db.TiposPersonas.Remove(tiposPersonas);
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
