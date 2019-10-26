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
    public class ModulosController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: Modulos
        public ActionResult Index()
        {
            return View(db.Modulos.ToList());
        }

        // GET: Modulos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modulos modulos = db.Modulos.Find(id);
            if (modulos == null)
            {
                return HttpNotFound();
            }
            return View(modulos);
        }

        // GET: Modulos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Modulos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Modulos modulos)
        {
            if (ModelState.IsValid)
            {
                db.Modulos.Add(modulos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modulos);
        }

        // GET: Modulos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modulos modulos = db.Modulos.Find(id);
            if (modulos == null)
            {
                return HttpNotFound();
            }
            return View(modulos);
        }

        // POST: Modulos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Modulos modulos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modulos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modulos);
        }

        // GET: Modulos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modulos modulos = db.Modulos.Find(id);
            if (modulos == null)
            {
                return HttpNotFound();
            }
            return View(modulos);
        }

        // POST: Modulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Modulos modulos = db.Modulos.Find(id);
            db.Modulos.Remove(modulos);
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
