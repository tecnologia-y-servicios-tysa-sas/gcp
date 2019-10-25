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
    public class RolesController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: Roles
        public ActionResult Index()
        {
            return View(db.Rols.ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = db.Rols.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            ViewBag.PermisosId = new SelectList(db.Permisos.OrderBy(x => x.Descripción), "PermisoId", "Descripción");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Rol rol, String[] PermisosId)
        {
            if (ModelState.IsValid)
            {
                using (var transacction = db.Database.BeginTransaction())
                {

                    try
                    {
                        db.Rols.Add(rol);
                        

                        if (PermisosId != null)
                        {
                            foreach (var item in PermisosId)
                            {
                                db.PermisosRoles.Add(new PermisosRoles { RolId = rol.RolId, PermisoId = Convert.ToInt32(item), Estado = true });
                            }

                        }

                        db.SaveChanges();
                        transacction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {

                        transacction.Rollback();
                       ModelState.AddModelError("", "Error " +  ex + " " + "Favor validar información o comunicarse con el administrador del sistema");
                        return View();
                    }
                }


            }

            ViewBag.PermisosId = new SelectList(db.Permisos.OrderBy(x => x.Descripción), "PermisoId", "Descripción");
            return View(rol);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = db.Rols.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rol rol)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rol).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rol);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = db.Rols.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rol rol = db.Rols.Find(id);
            db.Rols.Remove(rol);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult PermisoRoles(int? id)
        {
            return PartialView(db.PermisosRoles.Where(x => x.RolId == id).ToList());
        }

        public ActionResult EditPermisos(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermisosRoles rol = db.PermisosRoles.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            //return PartialView(rol);

            return View(rol);
        }


        [HttpPost]
       
        public ActionResult EditPermisos(PermisosRoles rol)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rol).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddPermisosRol","Roles",new { id = rol.RolId });
            }
            return View(rol);
        }

        public ActionResult AddPermisosRol(int id)
        {

            var permisos = db.PermisosRoles.Where(x => x.RolId == id).ToList().OrderBy(x => x.Permisos.Descripción);

            List<int> Npermisos = new List<int>();

            foreach (var item in permisos)
            {
                Npermisos.Add(item.PermisoId);

            }

           

            ViewBag.RolId = id;
            ViewBag.NombreRol = db.Rols.Where(x => x.RolId == id).FirstOrDefault().Descripción;
            ViewBag.PermisosId = new SelectList(db.Permisos.Where(x=> !(Npermisos.Contains(x.PermisoId))).OrderBy(x => x.Descripción), "PermisoId", "Descripción");

            return View(permisos.ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPermisosRol( PermisosRoles permisosRoles,  string[] PermisosId, int? rolesId)
        {
            if (ModelState.IsValid)
            {
                using (var transacction = db.Database.BeginTransaction())
                {

                    try
                    {
                       
                        if (PermisosId != null)
                        {
                            foreach (var item in PermisosId)
                            {
                                db.PermisosRoles.Add(new PermisosRoles { RolId = rolesId.Value, PermisoId = Convert.ToInt32(item), Estado = true });
                            }

                        }

                        db.SaveChanges();
                        transacction.Commit();
                        //return RedirectToAction("Index");
                        return RedirectToAction("AddPermisosRol", "Roles", new { id = rolesId });
                    }
                    catch (Exception ex)
                    {

                        transacction.Rollback();
                        ModelState.AddModelError("", "Error " + ex + " " + "Favor validar información o comunicarse con el administrador del sistema");
                        return View();
                    }
                }


            }

            ViewBag.PermisosId = new SelectList(db.Permisos.OrderBy(x => x.Descripción), "PermisoId", "Descripción");
            return View();
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
