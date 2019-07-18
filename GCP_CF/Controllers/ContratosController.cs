using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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
        private const string CREAR = "Crear Contrato";
        private const string EDITAR = "Editar Contrato";

        private string tipoContratoMarco = System.Configuration.ConfigurationManager.AppSettings["tipoContratoMarco"];

        // GET: Contratos
        public ActionResult Index(Contratos contratos)
        {
            if (contratos.TipoEstadoContrato_Id == null)
            {
                contratos.TipoEstadoContrato_Id = 3;
                ViewBag.TipoEstadoContrato_Id = new SelectList(db.TiposEstadoContrato, "TiposEstadoContrato_Id", "Descripcion",3);
            }
            else
            {
                ViewBag.TipoEstadoContrato_Id = new SelectList(db.TiposEstadoContrato, "TiposEstadoContrato_Id", "Descripcion", contratos.TipoEstadoContrato_Id);
            }
            
            
            List<Contratos> list = GetContratos(Convert.ToInt16(contratos.TipoEstadoContrato_Id));
            return View(list.OrderBy(x=> x.Contrato_Id).ToList());
        }


        private List<Contratos> GetContratos(int estadoId)
        {
            //Consulto Estados
            var estados = db.TiposEstadoContrato.ToList();
            //Son marco
  
                var list = db.Contratos.Include(s => s.HistoriaObservaciones).Where(s => s.ContratoMarco_Id == null && s.TipoEstadoContrato_Id == estadoId).ToList();
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
            Contratos contratos = GetContratosMarcoById(id).FirstOrDefault();//db.Contratos.Find(id);
            if (contratos == null)
            {
                return HttpNotFound();
            }
            return View(contratos);
        }

        // GET: Contratos/Create
        public ActionResult Create()
        {
            ViewBag.Accion = CREAR;
            ViewBag.IsEdit = false;

            ViewBag.Persona_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto");
            ViewBag.PersonaAbogado_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 1), "Persona_Id", "NombreCompleto");
            ViewBag.PersonaSuperviosr_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 2), "Persona_Id", "NombreCompleto");
            ViewBag.PersonaSupervisorTecnico_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 4), "Persona_Id", "NombreCompleto");
            ViewBag.TipoEstadoContrato_Id = new SelectList(db.TiposEstadoContrato, "TiposEstadoContrato_Id", "Descripcion");
            ViewBag.ContratoMarco_Id = new SelectList(db.Contratos.Where(c => c.TipoContrato.Termino == tipoContratoMarco), "Contrato_Id", "NumeroContrato");
            ViewBag.TipoContrato_Id = new SelectList(db.TiposContratos, "TipoContrato_Id", "Descripcion");
            ViewBag.FormaPagoId = new SelectList(db.FormaPagoes, "Id", "Descripcion");

            return View();
        }

        private ActionResult GuardarContrato(Contratos contratos, FormCollection form, bool esModificado)
        {
            bool exito = false;
            string mensaje = string.Empty;

            try
            {
                if (contratos == null) return HttpNotFound();

                if (ModelState.IsValid)
                {
                    double valorContratoAux = Convert.ToDouble(contratos.ValorContratoAux.Replace(",", "").Replace(".00", ""));
                    contratos.ValorContrato = valorContratoAux;

                    double valorAdministrarAux = Convert.ToDouble(contratos.ValorAdministrarAux.Replace(",", "").Replace(".00", ""));
                    contratos.ValorAdministrar = valorAdministrarAux;

                    double honorariosAux = Convert.ToDouble(contratos.HonorariosAux.Replace(",", "").Replace(".00", ""));
                    contratos.Honorarios = honorariosAux;

                    if (!string.IsNullOrEmpty(contratos.ValorPolizaAux))
                    {
                        double valorPolizaAux = Convert.ToDouble(contratos.ValorPolizaAux.Replace(",", "").Replace(".00", ""));
                        contratos.ValorPoliza = valorPolizaAux;
                    }

                    if (esModificado)
                        db.Entry(contratos).State = EntityState.Modified;
                    else
                        db.Contratos.Add(contratos);

                    int id = contratos.Contrato_Id;
                    if (!string.IsNullOrEmpty(contratos.Observaciones))
                    {
                        //Almaceno la observacion en la tabla historiaobservaciones
                        HistoriaObservaciones historiaObs = new HistoriaObservaciones
                        {
                            Observaciones = contratos.Observaciones,
                            Fecha = DateTime.Now,
                            ContratoId = id,
                        };
                        db.HistoriaObservaciones.Add(historiaObs);
                    }

                    // Revisión de pagos
                    List<PagosContrato> pagosActuales = null;
                    List<int> pagosModificados = new List<int>();
                    if (esModificado) pagosActuales = db.PagosContrato.Where(p => p.Contrato_Id == id).ToList<PagosContrato>();

                    string strNumeroPagos = form["numeroPagos"];

                    if (!string.IsNullOrEmpty(strNumeroPagos))
                    {
                        int numeroPagos = int.Parse(strNumeroPagos);
                        for (int i = 0; i < numeroPagos; i++)
                        {
                            string idPagoAux = form["idPago_" + i];

                            double valorPagoAux = Convert.ToDouble(form["valorPago_" + i].Replace(",", "").Replace(".00", ""));

                            PagosContrato pago = null;
                            if (esModificado && !string.IsNullOrEmpty(idPagoAux))
                            {
                                int idPago = int.Parse(idPagoAux);
                                pagosModificados.Add(idPago);
                                pago = pagosActuales.Where(p => p.PagosContrato_Id == idPago).FirstOrDefault<PagosContrato>();
                                pago.Valor = valorPagoAux;
                                pago.Fecha = DateTime.Parse(form["fechaPago_" + i]);
                                pago.Notas = !string.IsNullOrEmpty(form["notasPago_" + i]) ? form["notasPago_" + i] : string.Empty;
                                db.Entry(pago).State = EntityState.Modified;
                            }
                            else
                            {
                                pago = new PagosContrato
                                {
                                    Contrato_Id = id,
                                    Valor = valorPagoAux,
                                    Fecha = DateTime.Parse(form["fechaPago_" + i]),
                                    Notas = !string.IsNullOrEmpty(form["notasPago_" + i]) ? form["notasPago_" + i] : string.Empty
                                };
                                db.PagosContrato.Add(pago);
                            }
                        }
                    }
                    else
                    {
                        mensaje = "No se agregaron pagos al contrato. Por favor verifique.";
                    }

                    if (pagosActuales != null && pagosActuales.Count > 0 && pagosModificados != null && pagosModificados.Count > 0)
                    {
                        List<PagosContrato> pagosContrato = new List<PagosContrato>();
                        foreach (var pago in pagosActuales)
                        {
                            if (!pagosModificados.Contains(pago.PagosContrato_Id))
                                db.Entry(pago).State = EntityState.Deleted;
                            else
                                pagosContrato.Add(pago);
                        }

                        contratos.PagosContrato = pagosContrato;

                    }

                    // Solamente se deben guardar los cambios cuando NADA falle
                    exito = (db.SaveChanges() > 0);
                }
                else
                {
                    mensaje = "No fue posible " + (ViewBag.IsEdit ? "actualizar" : "crear") + " el contrato";
                }

            }
            catch (Exception e)
            {
                mensaje = "Ha ocurrido un error al " + (ViewBag.IsEdit ? "actualizar" : "crear") + " el contrato: " + e.Message;
            }

            if (exito)
                return RedirectToAction("Index");
            else
            {
                ViewBag.Persona_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto", contratos.Persona_Id);
                ViewBag.PersonaAbogado_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 1), "Persona_Id", "NombreCompleto", contratos.PersonaAbogado_Id);
                ViewBag.PersonaSuperviosr_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 2), "Persona_Id", "NombreCompleto", contratos.PersonaSuperviosr_Id);
                ViewBag.PersonaSupervisorTecnico_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 4), "Persona_Id", "NombreCompleto", contratos.PersonaSupervisorTecnico_Id);
                ViewBag.TipoEstadoContrato_Id = new SelectList(db.TiposEstadoContrato, "TiposEstadoContrato_Id", "Descripcion", contratos.TipoEstadoContrato_Id);
                ViewBag.ContratoMarco_Id = new SelectList(db.Contratos.Where(c => c.ContratoMarco_Id == null), "Contrato_Id", "NumeroContrato", contratos.ContratoMarco_Id);
                ViewBag.TipoContrato_Id_Aux = new SelectList(db.TiposContratos, "TipoContrato_Id", "Descripcion", contratos.TipoContrato_Id);
                ViewBag.FormaPagoId = new SelectList(db.FormaPagoes, "Id", "Descripcion");
                ViewBag.MensajeError = mensaje;

                return View(contratos);
            }
        }

        // POST: Contratos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contratos contratos, FormCollection form)
        {
            ViewBag.Accion = CREAR;
            ViewBag.IsEdit = false;
            return GuardarContrato(contratos, form, ViewBag.IsEdit);
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
            ViewBag.Accion = EDITAR;
            ViewBag.IsEdit = true;

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Contratos contratos = db.Contratos.Find(id);

            if (contratos == null)
                return HttpNotFound();

            var formatter = new CultureInfo("es-CO", false).NumberFormat;
            formatter.NumberGroupSeparator = ",";
            formatter.NumberDecimalSeparator = ".";
            formatter.NumberDecimalDigits = 2;

            contratos.ValorContratoAux = contratos.ValorContrato.ToString("#,###.#0", formatter);
            contratos.ValorAdministrarAux = contratos.ValorAdministrar.ToString("#,###.#0", formatter);
            contratos.HonorariosAux = contratos.Honorarios.HasValue ? contratos.Honorarios.Value.ToString("#,###.#0", formatter) : "";
            contratos.ValorPolizaAux = contratos.ValorPoliza.HasValue ? contratos.ValorPoliza.Value.ToString("#,###.#0", formatter) : "";

            List<PagosContrato> pagosContrato = db.PagosContrato.Where(p => p.Contrato_Id == id).ToList<PagosContrato>();
            contratos.PagosContrato = pagosContrato;

            ViewBag.Persona_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto", contratos.Persona_Id);
            ViewBag.PersonaAbogado_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 1), "Persona_Id", "NombreCompleto",contratos.PersonaAbogado_Id);
            ViewBag.PersonaSuperviosr_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 2), "Persona_Id", "NombreCompleto", contratos.PersonaSuperviosr_Id);
            ViewBag.PersonaSupervisorTecnico_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 4), "Persona_Id", "NombreCompleto",contratos.PersonaSupervisorTecnico_Id);
            ViewBag.TipoEstadoContrato_Id = new SelectList(db.TiposEstadoContrato, "TiposEstadoContrato_Id", "Descripcion",contratos.TipoEstadoContrato_Id);
            ViewBag.ContratoMarco_Id = new SelectList(db.Contratos.Where(c => c.ContratoMarco_Id == null), "Contrato_Id", "NumeroContrato",contratos.ContratoMarco_Id);
            ViewBag.TipoContrato_Id_Aux = new SelectList(db.TiposContratos, "TipoContrato_Id", "Descripcion", contratos.TipoContrato_Id);
            ViewBag.FormaPagoId = new SelectList(db.FormaPagoes, "Id", "Descripcion");

            return View(contratos);
        }

        // POST: Contratos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contratos contratos, FormCollection form)
        {
            ViewBag.Accion = EDITAR;
            ViewBag.IsEdit = true;
            return GuardarContrato(contratos, form, ViewBag.IsEdit);
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

        [HttpGet]
        public PartialViewResult PartialContratos(int id)
        {
            List<Contratos> list = GetContratosById(id);
            return PartialView("PartialContratos", list.ToList());
        }

        private List<Contratos> GetContratosById(int id)
        {
            //Consulto Estados
            var estados = db.TiposEstadoContrato.ToList();
            //No Son marco
            var list  = db.Contratos.Include(s => s.HistoriaObservaciones).Where(s => s.ContratoMarco_Id != null && s.ContratoMarco_Id==id).ToList();
            //(from contratos in db.Contratos
            //            join contratos2 in db.Contratos on contratos.Contrato_Id equals contratos2.Contrato_Id
            //            where contratos.ContratoMarco_Id != null && contratos.ContratoMarco_Id== id
            //            select contratos).ToList();

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

        private List<Contratos> GetContratosMarcoById(int? id)
        {
            //Consulto Estados
            var estados = db.TiposEstadoContrato.ToList();
            //No Son marco
            var list = db.Contratos.Include(s => s.HistoriaObservaciones).Where(s => s.ContratoMarco_Id == null && s.Contrato_Id == id).ToList();
            //(from contratos in db.Contratos
            //        join contratos2 in db.Contratos on contratos.Contrato_Id equals contratos2.Contrato_Id
            //        where contratos.ContratoMarco_Id == null && contratos.Contrato_Id == id
            //        select contratos).ToList();

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

        public JsonResult GetObservaciones(int id)
        {
            var datos = (from h in db.HistoriaObservaciones
                         //join u in db.Users on h.UsuarioCrea equals u.Email
                         where h.ContratoId == id
                         select new { h.Fecha, h.Observaciones}
                        );


            return Json(datos.Select(o => new { o.Fecha, o.Observaciones }).OrderBy(o => o.Fecha), JsonRequestBehavior.AllowGet);
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
