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
    public class ContratosMarcoesController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: ContratosMarcoes
        public ActionResult Index()
        {
            return View(db.ContratosMarcoes.ToList());
        }

        // GET: ContratosMarcoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContratosMarco contratosMarco = db.ContratosMarcoes.Find(id);
            if (contratosMarco == null)
            {
                return HttpNotFound();
            }
            return View(contratosMarco);
        }

        // GET: ContratosMarcoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContratosMarcoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContratoMarco_Id,Descripcion")] ContratosMarco contratosMarco)
        {
            if (ModelState.IsValid)
            {
                db.ContratosMarcoes.Add(contratosMarco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contratosMarco);
        }

        // GET: ContratosMarcoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContratosMarco contratosMarco = db.ContratosMarcoes.Find(id);
            if (contratosMarco == null)
            {
                return HttpNotFound();
            }
            return View(contratosMarco);
        }

        // POST: ContratosMarcoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContratoMarco_Id,Descripcion")] ContratosMarco contratosMarco)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contratosMarco).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contratosMarco);
        }

        // GET: ContratosMarcoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContratosMarco contratosMarco = db.ContratosMarcoes.Find(id);
            if (contratosMarco == null)
            {
                return HttpNotFound();
            }
            return View(contratosMarco);
        }

        // POST: ContratosMarcoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContratosMarco contratosMarco = db.ContratosMarcoes.Find(id);
            db.ContratosMarcoes.Remove(contratosMarco);
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
