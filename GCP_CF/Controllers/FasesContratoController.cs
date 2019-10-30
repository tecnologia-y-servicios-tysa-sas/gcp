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

            return View(db.FasesContrato.OrderBy(x=>x.Descripcion).ToList());
        }

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

     
        public ActionResult Create()
        {
            return View();
        }

    
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



        public ActionResult CreateEtapasActividades(int? id)
        {

            ViewBag.ActividadesEtapasId = new SelectList(db.ActividadesEtapas, "ActividadesEtapasId", "Descripción");
            ViewBag.fase_Id = new SelectList(db.FasesContrato, "fase_Id", "Descripcion", id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEtapasActividades(FasesContratosAcividades fasesContratosAcividades)
        {
            if (ModelState.IsValid)
            {
                db.FasesContratosAcividades.Add(fasesContratosAcividades);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ActividadesEtapasId = new SelectList(db.ActividadesEtapas, "ActividadesEtapasId", "Descripción", fasesContratosAcividades.ActividadesEtapasId);
            ViewBag.fase_Id = new SelectList(db.FasesContrato, "fase_Id", "Descripcion", fasesContratosAcividades.fase_Id);
            return View(fasesContratosAcividades);
        }


        public ActionResult DetalleActividadEtapa( int? id)
        {
            var fasesContratosAcividades = db.FasesContratosAcividades.Where(x=>x.fase_Id == id).Include(f => f.ActividadesEtapas).Include(f => f.FasesContrato);
           return PartialView (fasesContratosAcividades.ToList());
        }



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
