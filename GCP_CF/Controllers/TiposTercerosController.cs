using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GCP_CF.Authorization;
using GCP_CF.Helpers;
using GCP_CF.Models;

namespace GCP_CF.Controllers
{
    [GCPAuthorize(Roles = RolHelper.SUPERUSUARIO)]
    public class TiposTercerosController : Controller
    {
        private readonly GCPContext db = new GCPContext();

        // GET: TiposPersona
        public ActionResult Index()
        {
            var TiposPersona = db.TiposPersona.Include(t => t.TiposNaturaleza);
            return View(TiposPersona.ToList());
        }

        // GET: TiposTerceros/Details/5
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
            return View(tiposPersona);
        }

        // GET: TiposTerceros/Create
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
        public ActionResult Create([Bind(Include = "TipoPersona_Id,Descripcion,Naturaleza_Id,Cargo")] TiposPersona tiposPersona)
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

        // GET: TiposTerceros/Edit/5
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

        // POST: TiposTerceros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoPersona_Id,Descripcion,Naturaleza_Id,Cargo")] TiposPersona tiposPersona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposPersona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Naturaleza_Id = new SelectList(db.TiposNaturaleza, "Naturaleza_Id", "Descripcion", tiposPersona.Naturaleza_Id);
            return View(tiposPersona);
        }

        // GET: TiposTerceros/Delete/5
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
            return View(tiposPersona);
        }

        // POST: TiposTerceros/Delete/5
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
