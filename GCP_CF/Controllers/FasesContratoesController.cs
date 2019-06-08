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
    public class FasesContratoController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: FasesContrato
        public ActionResult Index()
        {
            return View(db.FasesContrato.ToList());
        }

        // GET: FasesContrato/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FasesContrato fasesContrato = db.FasesContrato.Find(id);
            if (fasesContrato == null)
            {
                return HttpNotFound();
            }
            return View(fasesContrato);
        }

        // GET: FasesContrato/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FasesContrato/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fase_Id,Descripcion")] FasesContrato fasesContrato)
        {
            if (ModelState.IsValid)
            {
                db.FasesContrato.Add(fasesContrato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fasesContrato);
        }

        // GET: FasesContrato/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FasesContrato fasesContrato = db.FasesContrato.Find(id);
            if (fasesContrato == null)
            {
                return HttpNotFound();
            }
            return View(fasesContrato);
        }

        // POST: FasesContrato/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "fase_Id,Descripcion")] FasesContrato fasesContrato)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fasesContrato).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fasesContrato);
        }

        // GET: FasesContrato/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FasesContrato fasesContrato = db.FasesContrato.Find(id);
            if (fasesContrato == null)
            {
                return HttpNotFound();
            }
            return View(fasesContrato);
        }

        // POST: FasesContrato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FasesContrato fasesContrato = db.FasesContrato.Find(id);
            db.FasesContrato.Remove(fasesContrato);
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
