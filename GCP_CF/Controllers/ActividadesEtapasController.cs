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
    public class ActividadesEtapasController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: ActividadesEtapas
        public ActionResult Index()
        {
            return View(db.ActividadesEtapas.ToList());
        }

        // GET: ActividadesEtapas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActividadesEtapas actividadesEtapas = db.ActividadesEtapas.Find(id);
            if (actividadesEtapas == null)
            {
                return HttpNotFound();
            }
            return View(actividadesEtapas);
        }

        // GET: ActividadesEtapas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActividadesEtapas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActividadesEtapasId,Descripción")] ActividadesEtapas actividadesEtapas)
        {
            if (ModelState.IsValid)
            {
                db.ActividadesEtapas.Add(actividadesEtapas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(actividadesEtapas);
        }

        // GET: ActividadesEtapas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActividadesEtapas actividadesEtapas = db.ActividadesEtapas.Find(id);
            if (actividadesEtapas == null)
            {
                return HttpNotFound();
            }
            return View(actividadesEtapas);
        }

        // POST: ActividadesEtapas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActividadesEtapasId,Descripción")] ActividadesEtapas actividadesEtapas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actividadesEtapas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actividadesEtapas);
        }

        // GET: ActividadesEtapas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActividadesEtapas actividadesEtapas = db.ActividadesEtapas.Find(id);
            if (actividadesEtapas == null)
            {
                return HttpNotFound();
            }
            return View(actividadesEtapas);
        }

        // POST: ActividadesEtapas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActividadesEtapas actividadesEtapas = db.ActividadesEtapas.Find(id);
            db.ActividadesEtapas.Remove(actividadesEtapas);
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
