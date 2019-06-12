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
    public class ContratosController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: Contratos
        public ActionResult Index()
        {
            var estados = db.TiposEstadoContrato.ToList();
            //Son marco
            var list = (from contratos in db.Contratos
                        join contratos2 in db.Contratos on contratos.Contrato_Id equals contratos2.Contrato_Id
                        where contratos.ContratoMarco_Id == null 
                        select contratos).ToList();

            foreach (var item in list)
            {
                foreach (var estado in estados)
                {
                    if(item.TipoEstadoContrato_Id == estado.TiposEstadoContrato_Id)
                    {
                        item.Estado = estado.Descripcion;
                    }
                }
            }
 
            return View(list.ToList());
        }

        // GET: Contratos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contratos contratos = db.Contratos.Find(id);
            if (contratos == null)
            {
                return HttpNotFound();
            }
            return View(contratos);
        }

        // GET: Contratos/Create
        public ActionResult Create()
        {
            var list = db.Contratos.Where(c => c.ContratoMarco_Id == null).ToList();
            foreach (var item in list)
            {

            }

            ViewBag.Persona_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto");
            ViewBag.PersonaAbogado_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 1), "Persona_Id", "NombreCompleto");
            ViewBag.PersonaSuperviosr_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 2), "Persona_Id", "NombreCompleto");
            ViewBag.PersonaSupervisorTecnico_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 4), "Persona_Id", "NombreCompleto");
            ViewBag.TipoEstadoContrato_Id = new SelectList(db.TiposEstadoContrato, "TiposEstadoContrato_Id", "Descripcion");
            ViewBag.ContratoMarco_Id = new SelectList(db.Contratos.Where(c => c.ContratoMarco_Id == null), "Contrato_Id", "NumeroContrato");
            ViewBag.TipoContrato_Id = new SelectList(db.TiposContratos, "TipoContrato_Id", "Descripcion");


            return View();
        }

        // POST: Contratos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contratos contratos)
        {
            try
            {
                db.Contratos.Add(contratos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Persona_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto");
                ViewBag.PersonaAbogado_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 1), "Persona_Id", "NombreCompleto");
                ViewBag.PersonaSuperviosr_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 2), "Persona_Id", "NombreCompleto");
                ViewBag.PersonaSupervisorTecnico_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 4), "Persona_Id", "NombreCompleto");
                ViewBag.TiposEstadoContrato_Id = new SelectList(db.TiposEstadoContrato, "TiposEstadoContrato_Id", "Descripcion");
                ViewBag.ContratoMarco_Id = new SelectList(db.Contratos.Where(c => c.ContratoMarco_Id == null), "Contrato_Id", "NumeroContrato");
                ViewBag.TipoContrato_Id = new SelectList(db.TiposContratos, "TipoContrato_Id", "Descripcion");

                return View(contratos);
            }              
        }

        public JsonResult GetDocumento(int id)
        {
            string termino = db.TiposContratos.Where(x => x.TipoContrato_Id == id).Select(x=> x.Termino).FirstOrDefault();
            int? consecutivo = 0;
            try
            {
                consecutivo = (int)db.Contratos.Max(x => x.Contrato_Id) + 1;
            }
            catch (Exception)
            {

                consecutivo = 1;
            }

            return Json(termino+"-"+Convert.ToString(consecutivo) + "-" + Convert.ToString(DateTime.Now.Year), JsonRequestBehavior.AllowGet);
        }

        // GET: Contratos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contratos contratos = db.Contratos.Find(id);
            if (contratos == null)
            {
                return HttpNotFound();
            }
            return View(contratos);
        }

        // POST: Contratos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FechaInicio,TipoContrato_Id,Persona_Id,ObjetoContractual,Plazo,FechaTerminacion,PersonaAbogado_Id,PersonaSuperviosr_Id,Crp,Cdp,FechaActaInicio,TipoEstadoContrato_Id,Year,ValorContrato,ValorAdministrar,Honorarios,Ejecucion,PorcentajeFacturado,PorcentajeFacturadoHonorarios,PersonaSupervisorTecnico_Id,ContratoMarco_Id")] Contratos contratos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contratos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contratos);
        }

        // GET: Contratos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contratos contratos = db.Contratos.Find(id);
            if (contratos == null)
            {
                return HttpNotFound();
            }
            return View(contratos);
        }

        // POST: Contratos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contratos contratos = db.Contratos.Find(id);
            db.Contratos.Remove(contratos);
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
