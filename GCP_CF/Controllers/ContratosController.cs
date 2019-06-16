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
            List<Contratos> list = GetContratos();

            return View(list.ToList());
        }

        [HttpPost]
        public ActionResult Index( string numero, string chkCerrados, string chkTodos, string chkAbiertos,
                                   string entidad, DateTime? fechaInicio, DateTime? fechaFin)
        {
            if(string.IsNullOrEmpty(numero) && chkCerrados== "false" && chkTodos=="false" &&
               chkAbiertos=="false" && string.IsNullOrEmpty(entidad) && fechaInicio == null &&  fechaFin == null)
            {
                List<Contratos> list = GetContratos();
                return View(list.ToList());
            }
            else
            {
                var listAll = db.Contratos.Where(c => c.ContratoMarco_Id == null).ToList();
                if (!string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "false" &&
                   chkAbiertos == "false" && !string.IsNullOrEmpty(entidad) && fechaInicio != null && fechaFin != null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.NumeroContrato.Contains(numero)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Nombres.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Apellidos.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.FechaInicio >= fechaInicio) &&
                                                      (c.ContratoMarco_Id == null && c.FechaTerminacion <= fechaFin)).ToList();
                }
                if (!string.IsNullOrEmpty(numero) && chkCerrados == "true" && chkTodos == "false" &&
                   chkAbiertos == "false" && !string.IsNullOrEmpty(entidad) && fechaInicio != null && fechaFin != null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.NumeroContrato.Contains(numero)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Nombres.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Apellidos.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.FechaInicio >= fechaInicio) &&
                                                      (c.ContratoMarco_Id == null && c.FechaTerminacion <= fechaFin)&&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id== 2)).ToList();
                }
                if (!string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "false" &&
                   chkAbiertos == "true" && !string.IsNullOrEmpty(entidad) && fechaInicio != null && fechaFin != null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.NumeroContrato.Contains(numero)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Nombres.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Apellidos.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.FechaInicio >= fechaInicio) &&
                                                      (c.ContratoMarco_Id == null && c.FechaTerminacion <= fechaFin) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id == 3)).ToList();
                }
                if (!string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "true" &&
                  chkAbiertos == "false" && !string.IsNullOrEmpty(entidad) && fechaInicio != null && fechaFin != null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.NumeroContrato.Contains(numero)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Nombres.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Apellidos.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.FechaInicio >= fechaInicio) &&
                                                      (c.ContratoMarco_Id == null && c.FechaTerminacion <= fechaFin) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id >= 2)).ToList();
                }
                if (!string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "true" &&
                 chkAbiertos == "false" && !string.IsNullOrEmpty(entidad) && fechaInicio != null && fechaFin == null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.NumeroContrato.Contains(numero)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Nombres.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Apellidos.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.FechaInicio >= fechaInicio) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id >= 2)).ToList();
                }
                if (string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "true" &&
                chkAbiertos == "false" && string.IsNullOrEmpty(entidad) && fechaInicio != null && fechaFin == null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.FechaInicio >= fechaInicio) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id >= 2)
                                                      ).ToList();
                }
                if (string.IsNullOrEmpty(numero) && chkCerrados == "true" && chkTodos == "false" &&
                chkAbiertos == "false" && string.IsNullOrEmpty(entidad) && fechaInicio != null && fechaFin == null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.FechaInicio >= fechaInicio) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id == 2) ).ToList();
                }
                if (string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "false" &&
                chkAbiertos == "true" && string.IsNullOrEmpty(entidad) && fechaInicio != null && fechaFin == null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.FechaInicio >= fechaInicio) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id == 3)).ToList();
                }
                if (!string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "true" &&
                 chkAbiertos == "false" && !string.IsNullOrEmpty(entidad) && fechaInicio == null && fechaFin != null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.NumeroContrato.Contains(numero)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Nombres.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Apellidos.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.FechaTerminacion <= fechaFin) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id >= 2) ).ToList();
                }
                if (string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "true" &&
                chkAbiertos == "false" && string.IsNullOrEmpty(entidad) && fechaInicio == null && fechaFin != null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.FechaTerminacion <= fechaFin) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id >= 2)).ToList();
                }
                if (string.IsNullOrEmpty(numero) && chkCerrados == "true" && chkTodos == "false" &&
                chkAbiertos == "false" && string.IsNullOrEmpty(entidad) && fechaInicio == null && fechaFin != null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.FechaTerminacion <= fechaFin) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id == 2)).ToList();
                }
                if (string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "false" &&
               chkAbiertos == "true" && string.IsNullOrEmpty(entidad) && fechaInicio == null && fechaFin != null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.FechaTerminacion <= fechaFin) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id == 3)).ToList();
                }
                if (!string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "true" &&
                  chkAbiertos == "false" && !string.IsNullOrEmpty(entidad) && fechaInicio == null && fechaFin == null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.NumeroContrato.Contains(numero)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Nombres.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Apellidos.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id >= 2)).ToList();
                }
                if (!string.IsNullOrEmpty(numero) && chkCerrados == "true" && chkTodos == "false" &&
                    chkAbiertos == "false" && !string.IsNullOrEmpty(entidad) && fechaInicio == null && fechaFin == null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.NumeroContrato.Contains(numero)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Nombres.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Apellidos.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id == 2)).ToList();
                }
                if (!string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "false" &&
                   chkAbiertos == "true" && !string.IsNullOrEmpty(entidad) && fechaInicio == null && fechaFin == null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.NumeroContrato.Contains(numero)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Nombres.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Apellidos.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id == 3)).ToList();
                }
                if (string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "true" &&
                chkAbiertos == "false" && !string.IsNullOrEmpty(entidad) && fechaInicio != null && fechaFin != null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.EntidadContratante.Nombres.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.EntidadContratante.Apellidos.Contains(entidad)) &&
                                                      (c.ContratoMarco_Id == null && c.FechaInicio >= fechaInicio) &&
                                                      (c.ContratoMarco_Id == null && c.FechaTerminacion <= fechaFin) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id >= 2)).ToList();
                }
                if (!string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "true" &&
                chkAbiertos == "false" && string.IsNullOrEmpty(entidad) && fechaInicio != null && fechaFin != null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.NumeroContrato.Contains(numero)) &&
                                                      (c.ContratoMarco_Id == null && c.FechaInicio >= fechaInicio) &&
                                                      (c.ContratoMarco_Id == null && c.FechaTerminacion <= fechaFin) &&
                                                      (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id >= 2) ).ToList();
                }
                if (string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "true" &&
                chkAbiertos == "false" && string.IsNullOrEmpty(entidad) && fechaInicio == null && fechaFin == null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id >= 2)).ToList();
                }
                if (string.IsNullOrEmpty(numero) && chkCerrados == "true" && chkTodos == "false" &&
                chkAbiertos == "false" && string.IsNullOrEmpty(entidad) && fechaInicio == null && fechaFin == null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id == 2)).ToList();
                }
                if (string.IsNullOrEmpty(numero) && chkCerrados == "false" && chkTodos == "false" &&
               chkAbiertos == "true" && string.IsNullOrEmpty(entidad) && fechaInicio == null && fechaFin == null)
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.TipoEstadoContrato_Id == 3)).ToList();
                }
                else if (!string.IsNullOrEmpty(numero))
                {
                    listAll = db.Contratos.Where(c => c.ContratoMarco_Id == null && c.NumeroContrato.Contains(numero)).ToList();
                }
                else if (!string.IsNullOrEmpty(entidad))
                {
                    listAll = db.Contratos.Where(c => (c.ContratoMarco_Id == null && c.EntidadContratante.Nombres.Contains(entidad)) && (c.ContratoMarco_Id == null && c.EntidadContratante.Apellidos.Contains(entidad))).ToList();
                }

                AddEstados(listAll);
                return View(listAll.ToList());
            }

        }

        private void AddEstados(List<Contratos> listAll)
        {
            var estados = db.TiposEstadoContrato.ToList();

            foreach (var item in listAll)
            {
                foreach (var estado in estados)
                {
                    if (item.TipoEstadoContrato_Id == estado.TiposEstadoContrato_Id)
                    {
                        item.Estado = estado.Descripcion;
                    }
                }
            }
        }

        private List<Contratos> GetContratos()
        {
            //Consulto Estados
            var estados = db.TiposEstadoContrato.ToList();

            //Son marco

            var list = (from contratos in db.Contratos
                        join contratos2 in db.Contratos on contratos.Contrato_Id equals contratos2.Contrato_Id
                        where contratos.ContratoMarco_Id == null && contratos.TipoEstadoContrato_Id == 3
                        select contratos).ToList();

            foreach (var item in list)
            {
                foreach (var estado in estados)
                {
                    if (item.TipoEstadoContrato_Id == estado.TiposEstadoContrato_Id)
                    {
                        item.Estado = estado.Descripcion;
                    }
                }
            }

            return list;
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

            ViewBag.Persona_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto", contratos.EntidadContratante.Persona_Id);
            ViewBag.PersonaAbogado_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 1), "Persona_Id", "NombreCompleto",contratos.PersonaAbogado_Id);
            ViewBag.PersonaSuperviosr_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 2), "Persona_Id", "NombreCompleto", contratos.PersonaSuperviosr_Id);
            ViewBag.PersonaSupervisorTecnico_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 4), "Persona_Id", "NombreCompleto",contratos.PersonaSupervisorTecnico_Id);
            ViewBag.TipoEstadoContrato_Id = new SelectList(db.TiposEstadoContrato, "TiposEstadoContrato_Id", "Descripcion",contratos.TipoEstadoContrato_Id);
            ViewBag.ContratoMarco_Id = new SelectList(db.Contratos.Where(c => c.ContratoMarco_Id == null), "Contrato_Id", "NumeroContrato",contratos.ContratoMarco_Id);
            ViewBag.TipoContrato_Id = new SelectList(db.TiposContratos, "TipoContrato_Id", "Descripcion", contratos.TipoContrato_Id);
            return View(contratos);
        }

        // POST: Contratos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contratos contratos)
        {
            try
            {
                db.Entry(contratos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Persona_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto", contratos.EntidadContratante.Persona_Id);
                ViewBag.PersonaAbogado_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 1), "Persona_Id", "NombreCompleto", contratos.PersonaAbogado_Id);
                ViewBag.PersonaSuperviosr_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 2), "Persona_Id", "NombreCompleto", contratos.PersonaSuperviosr_Id);
                ViewBag.PersonaSupervisorTecnico_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 4), "Persona_Id", "NombreCompleto", contratos.PersonaSupervisorTecnico_Id);
                ViewBag.TipoEstadoContrato_Id = new SelectList(db.TiposEstadoContrato, "TiposEstadoContrato_Id", "Descripcion", contratos.TipoEstadoContrato_Id);
                ViewBag.ContratoMarco_Id = new SelectList(db.Contratos.Where(c => c.ContratoMarco_Id == null), "Contrato_Id", "NumeroContrato", contratos.ContratoMarco_Id);
                ViewBag.TipoContrato_Id = new SelectList(db.TiposContratos, "TipoContrato_Id", "Descripcion", contratos.TipoContrato_Id);

                return View(contratos);
            }
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

        public JsonResult GetSearchNumeroContrato(string numeroContrato)
        {
            var allsearch = db.Contratos.Where(x=> x.ContratoMarco_Id == null && x.NumeroContrato.Contains(numeroContrato)).ToList();
            var list = (from N in allsearch
                        select new {  N.NumeroContrato }).Distinct();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSearchEntidadContratante(string entidad)
        {
            var allsearch = db.Personas.Where(x => (x.TipoPersona_Id == 3 && x.Nombres.Contains(entidad)) || (x.TipoPersona_Id == 3 && x.Apellidos.Contains(entidad))).ToList();
            var list = (from N in allsearch
                        select new { Entidad=N.Nombres +" " +N.Apellidos }).Distinct();
            return Json(list, JsonRequestBehavior.AllowGet);
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
