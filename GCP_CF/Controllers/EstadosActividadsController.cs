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
    public class EstadosActividadsController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: EstadosActividads
        public ActionResult Index()
        {
            return View(db.EstadosActividads.ToList());
        }

        // GET: EstadosActividads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadosActividad estadosActividad = db.EstadosActividads.Find(id);
            if (estadosActividad == null)
            {
                return HttpNotFound();
            }
            return View(estadosActividad);
        }

        // GET: EstadosActividads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstadosActividads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EstadoActividad_Id,Descripcion")] EstadosActividad estadosActividad)
        {
            if (ModelState.IsValid)
            {
                db.EstadosActividads.Add(estadosActividad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estadosActividad);
        }

        // GET: EstadosActividads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadosActividad estadosActividad = db.EstadosActividads.Find(id);
            if (estadosActividad == null)
            {
                return HttpNotFound();
            }
            return View(estadosActividad);
        }

        // POST: EstadosActividads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EstadoActividad_Id,Descripcion")] EstadosActividad estadosActividad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estadosActividad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estadosActividad);
        }

        // GET: EstadosActividads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadosActividad estadosActividad = db.EstadosActividads.Find(id);
            if (estadosActividad == null)
            {
                return HttpNotFound();
            }
            return View(estadosActividad);
        }

        // POST: EstadosActividads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EstadosActividad estadosActividad = db.EstadosActividads.Find(id);
            db.EstadosActividads.Remove(estadosActividad);
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
