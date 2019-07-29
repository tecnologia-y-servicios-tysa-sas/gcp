using GCP_CF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCP_CF.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly GCPContext db = new GCPContext();

        // GET: Usuarios
        public ActionResult Index()
        {
            List<Usuarios> usuarios = db.Usuarios.ToList();
            return View(usuarios);
        }
    }
}