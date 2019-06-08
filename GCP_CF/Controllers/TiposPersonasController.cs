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
    public class TiposPersonaController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: TiposPersona
        public ActionResult Index()
        {
            var TiposPersona = db.TiposPersona.Include(t => t.TiposNaturaleza);
            return View(TiposPersona.ToList());
        }

        // GET: TiposPersona/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPersona tiposPersona = db.TiposPersona.Find(id);
            if (tiposPersona == null)
            {
                return HttpNotFound();
            }
            return View(tiposPersonas);
        }

        // GET: TiposPersona/Create
        public ActionResult Create()
        {
            ViewBag.Naturaleza_Id = new SelectList(db.TiposNaturaleza, "Naturaleza_Id", "Descripcion");
            return View();
        }

        // POST: TiposPersona/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TipoPersona_Id,Descripcion,Naturaleza_Id,Cargo")] TiposPersonas tiposPersonas)
        {
            if (ModelState.IsValid)
            {
                db.TiposPersona.Add(tiposPersona);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Naturaleza_Id = new SelectList(db.TiposNaturaleza, "Naturaleza_Id", "Descripcion", tiposPersona.Naturaleza_Id);
            return View(tiposPersona);
        }

        // GET: TiposPersona/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPersona tiposPersona = db.TiposPersona.Find(id);
            if (tiposPersona == null)
            {
                return HttpNotFound();
            }
            ViewBag.Naturaleza_Id = new SelectList(db.TiposNaturaleza, "Naturaleza_Id", "Descripcion", tiposPersona.Naturaleza_Id);
            return View(tiposPersona);
        }

        // POST: TiposPersona/Edit/5
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
            ViewBag.Naturaleza_Id = new SelectList(db.TiposNaturaleza, "Naturaleza_Id", "Descripcion", tiposPersona.Naturaleza_Id);
            return View(tiposPersona);
        }

        // GET: TiposPersona/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPersona tiposPersona = db.TiposPersona.Find(id);
            if (tiposPersona == null)
            {
                return HttpNotFound();
            }
            return View(tiposPersonas);
        }

        // POST: TiposPersona/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TiposPersona tiposPersona = db.TiposPersona.Find(id);
            db.TiposPersona.Remove(tiposPersona);
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
