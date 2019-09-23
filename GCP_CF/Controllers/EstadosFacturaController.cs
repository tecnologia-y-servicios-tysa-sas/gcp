using GCP_CF.Authorization;
using GCP_CF.Helpers;
using GCP_CF.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GCP_CF.Controllers
{
    [GCPAuthorize(Roles = RolHelper.SUPERUSUARIO)]
    public class EstadosFacturaController : Controller
    {
        private readonly GCPContext db = new GCPContext();

        // GET: EstadosFactura
        public ActionResult Index()
        {
            return View(db.EstadosFactura.ToList());
        }

        // GET: EstadosFactura/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EstadosFactura estadosFactura = db.EstadosFactura.Find(id);
            if (estadosFactura == null) return HttpNotFound();

            return View(estadosFactura);
        }

        // GET: EstadosFactura/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstadosFactura/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EstadoFactura_Id,Descripcion,Termino")] EstadosFactura estadosFactura)
        {
            if (ModelState.IsValid)
            {
                db.EstadosFactura.Add(estadosFactura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estadosFactura);
        }

        // GET: EstadosFactura/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EstadosFactura estadosFactura = db.EstadosFactura.Find(id);
            if (estadosFactura == null) return HttpNotFound();

            return View(estadosFactura);
        }

        // POST: EstadosFactura/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EstadoFactura_Id,Descripcion,Termino")] EstadosFactura estadosFactura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estadosFactura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estadosFactura);
        }

        // GET: EstadosFactura/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            EstadosFactura estadosFactura = db.EstadosFactura.Find(id);
            if (estadosFactura == null) return HttpNotFound();

            return View(estadosFactura);
        }

        // POST: TiposContratos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool exito = false;

            EstadosFactura estadosFactura = db.EstadosFactura.Find(id);
            if (estadosFactura == null) return HttpNotFound();

            try
            {
                db.EstadosFactura.Remove(estadosFactura);
                exito = db.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                ViewBag.MensajeError = "Ha ocurrido un error al eliminar la factura: " + e.Message;
            }

            if (exito)
                return RedirectToAction("Index");
            else
                return View(estadosFactura);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();

            base.Dispose(disposing);
        }

    }
}