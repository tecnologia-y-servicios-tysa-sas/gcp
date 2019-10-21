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
    public class UsuariosRolesController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: UsuariosRoles
        public ActionResult Index()
        {
            return View(db.usuariosroles.ToList());
        }

        // GET: UsuariosRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosRoles usuariosRoles = db.usuariosroles.Find(id);
            if (usuariosRoles == null)
            {
                return HttpNotFound();
            }
            return View(usuariosRoles);
        }

        // GET: UsuariosRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuariosRoles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Usuario_Id,RolId")] UsuariosRoles usuariosRoles)
        {
            if (ModelState.IsValid)
            {
                db.usuariosroles.Add(usuariosRoles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuariosRoles);
        }

        // GET: UsuariosRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosRoles usuariosRoles = db.usuariosroles.Find(id);
            if (usuariosRoles == null)
            {
                return HttpNotFound();
            }
            return View(usuariosRoles);
        }

        // POST: UsuariosRoles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Usuario_Id,RolId")] UsuariosRoles usuariosRoles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuariosRoles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuariosRoles);
        }

        // GET: UsuariosRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosRoles usuariosRoles = db.usuariosroles.Find(id);
            if (usuariosRoles == null)
            {
                return HttpNotFound();
            }
            return View(usuariosRoles);
        }

        // POST: UsuariosRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UsuariosRoles usuariosRoles = db.usuariosroles.Find(id);
            db.usuariosroles.Remove(usuariosRoles);
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
