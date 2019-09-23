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
    public class FormaPagosController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: FormaPagos
        public ActionResult Index()
        {
            return View(db.FormaPagoes.ToList());
        }

        // GET: FormaPagos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormaPago formaPago = db.FormaPagoes.Find(id);
            if (formaPago == null)
            {
                return HttpNotFound();
            }
            return View(formaPago);
        }

        // GET: FormaPagos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FormaPagos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion")] FormaPago formaPago)
        {
            if (ModelState.IsValid)
            {
                db.FormaPagoes.Add(formaPago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(formaPago);
        }

        // GET: FormaPagos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormaPago formaPago = db.FormaPagoes.Find(id);
            if (formaPago == null)
            {
                return HttpNotFound();
            }
            return View(formaPago);
        }

        // POST: FormaPagos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion")] FormaPago formaPago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formaPago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(formaPago);
        }

        // GET: FormaPagos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormaPago formaPago = db.FormaPagoes.Find(id);
            if (formaPago == null)
            {
                return HttpNotFound();
            }
            return View(formaPago);
        }

        // POST: FormaPagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FormaPago formaPago = db.FormaPagoes.Find(id);
            db.FormaPagoes.Remove(formaPago);
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
