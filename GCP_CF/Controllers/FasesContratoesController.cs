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
    public class FasesContratoesController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: FasesContratoes
        public ActionResult Index()
        {
            return View(db.FasesContratoes.ToList());
        }

        // GET: FasesContratoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FasesContrato fasesContrato = db.FasesContratoes.Find(id);
            if (fasesContrato == null)
            {
                return HttpNotFound();
            }
            return View(fasesContrato);
        }

        // GET: FasesContratoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FasesContratoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "fase_Id,Descripcion")] FasesContrato fasesContrato)
        {
            if (ModelState.IsValid)
            {
                db.FasesContratoes.Add(fasesContrato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fasesContrato);
        }

        // GET: FasesContratoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FasesContrato fasesContrato = db.FasesContratoes.Find(id);
            if (fasesContrato == null)
            {
                return HttpNotFound();
            }
            return View(fasesContrato);
        }

        // POST: FasesContratoes/Edit/5
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

        // GET: FasesContratoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FasesContrato fasesContrato = db.FasesContratoes.Find(id);
            if (fasesContrato == null)
            {
                return HttpNotFound();
            }
            return View(fasesContrato);
        }

        // POST: FasesContratoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FasesContrato fasesContrato = db.FasesContratoes.Find(id);
            db.FasesContratoes.Remove(fasesContrato);
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
