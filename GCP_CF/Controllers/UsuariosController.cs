using GCP_CF.Helpers;
using GCP_CF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GCP_CF.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly GCPContext db = new GCPContext();
        private const string CREAR = "Crear Usuario";
        private const string EDITAR = "Editar Usuario";

        private string rolSuperUsuario = System.Configuration.ConfigurationManager.AppSettings["rolSuperUsuario"];
        private string rolUsuarioNormal = System.Configuration.ConfigurationManager.AppSettings["rolUsuarioNormal"];

        // GET: Usuarios
        public ActionResult Index()
        {
            List<Usuarios> usuarios = db.Usuarios.ToList();
            return View(usuarios);
        }
        
        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Usuarios usuario = db.Usuarios.Find(id);
            if (usuario == null) return HttpNotFound();

            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            List<Contratos> contratos = db.Contratos.OrderByDescending(c => c.NumeroContrato).ToList();

            ViewBag.Accion = CREAR;
            ViewBag.IsEdit = false;
            ViewBag.Contratos = contratos;

            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuarios usuarios, FormCollection form)
        {
            ViewBag.Accion = CREAR;
            ViewBag.IsEdit = false;
            return GuardarUsuario(usuarios, form, ViewBag.IsEdit);
        }

        // GET: Personas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Usuarios usuario = db.Usuarios.Find(id);
            if (usuario == null) return HttpNotFound();

            List<Contratos> contratos = db.Contratos.OrderByDescending(c => c.NumeroContrato).ToList();

            ViewBag.Accion = EDITAR;
            ViewBag.IsEdit = true;
            ViewBag.Contratos = contratos;

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuarios usuarios, FormCollection form)
        {
            ViewBag.Accion = EDITAR;
            ViewBag.IsEdit = true;
            return GuardarUsuario(usuarios, form, ViewBag.IsEdit);
        }

        private ActionResult GuardarUsuario(Usuarios usuarios, FormCollection form, bool esModificado)
        {
            bool exito = false;
            string mensaje = string.Empty;

            try
            {
                if (usuarios == null) return HttpNotFound();
                UserManager um = new UserManager();
                
                if (!string.IsNullOrEmpty(usuarios.PasswordAux))
                    usuarios.Password = um.Base64Encode(usuarios.PasswordAux);
                else
                    if (!esModificado) usuarios.Password = um.Base64Encode(usuarios.Password);

                if (ModelState.IsValid)
                {
                    // Lógica para recuperar valores de contratos
                    string esSuperUsuario = form["esSuperUsuario"];
                    string tipoPermiso = form["tipoPermiso"];
                    bool todosLosContratos = usuarios.TodosLosContratos;

                    if (string.IsNullOrEmpty(esSuperUsuario))
                    {
                        usuarios.IdRoles = rolUsuarioNormal;
                        if (tipoPermiso != null && tipoPermiso == "W")
                            usuarios.TipoPermisos = "W"; // Escritura
                        else
                            usuarios.TipoPermisos = "R"; // Lectura

                        if (!todosLosContratos)
                        {
                            string[] idContratos = form.GetValues("contratos");
                            if (idContratos != null && idContratos.Length > 0)
                                usuarios.IdContratos = string.Join(",", idContratos);
                            else
                                mensaje = "Debe elegir al menos un contrato sobre el cual el usuario debe tener permisos.";
                        }
                    }
                    else
                    {
                        if (esSuperUsuario == "0") usuarios.IdRoles = rolSuperUsuario;
                    }

                    if (string.IsNullOrEmpty(mensaje))
                    {
                        if (esModificado)
                            db.Entry(usuarios).State = EntityState.Modified;
                        else
                            db.Usuarios.Add(usuarios);

                        // Solamente se deben guardar los cambios cuando NADA falle
                        exito = (db.SaveChanges() > 0);
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    string errores = string.Empty;
                    foreach (var error in errors)
                    {
                        errores += error.ErrorMessage + "<br />";
                    }
                    mensaje = "No fue posible " + (ViewBag.IsEdit ? "actualizar" : "crear") + " el usuario: " + errores;
                }

            }
            catch (Exception e)
            {
                mensaje = "Ha ocurrido un error al " + (ViewBag.IsEdit ? "actualizar" : "crear") + " el usuario: " + e.Message;
            }

            if (exito)
                return RedirectToAction("Index");
            else
            {
                List<Contratos> contratos = db.Contratos.OrderByDescending(c => c.NumeroContrato).ToList();
                ViewBag.Contratos = contratos;
                ViewBag.MensajeError = mensaje;
                return View(usuarios);
            }
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Usuarios usuario = db.Usuarios.Find(id);
            if (usuario == null) return HttpNotFound();

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string mensaje = string.Empty;
            bool exito = false;
            Usuarios usuario = db.Usuarios.Find(id);

            try
            {
                db.Usuarios.Remove(usuario);
                exito = db.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                mensaje = "Ha ocurrido un error al eliminar el usuario: " + e.Message;
            }

            if (exito)
                return RedirectToAction("Index");
            else
            {
                ViewBag.MensajeError = mensaje;
                return View(usuario);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}