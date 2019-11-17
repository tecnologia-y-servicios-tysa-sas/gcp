using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Mvc;
using GCP_CF.Authorization;
using GCP_CF.Helpers;
using GCP_CF.Models;

namespace GCP_CF.Controllers
{
    public class ContratosController : Controller
    {
        private readonly GCPContext db = new GCPContext();
        private const string CREAR = "Crear Contrato";
        private const string EDITAR = "Editar Contrato";

        private string tipoContratoMarco = System.Configuration.ConfigurationManager.AppSettings["tipoContratoMarco"];

        // GET: Contratos
        [GCPAuthorize(Roles = RolHelper.TODOS)]
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

        [GCPAuthorize(Roles = RolHelper.TODOS)]
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
        [GCPAuthorize(Roles = RolHelper.TODOS)]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            else
                if (!AutorizacionContrato(id, false)) return RedirectToAction("AccessDenied", "Account");

            Contratos contratos = GetContratosMarcoById(id).FirstOrDefault();
            if (contratos == null)
            {
                return HttpNotFound();
            }

            return View(contratos);
        }

        // GET: Contratos/Create
        [GCPAuthorize(Roles = RolHelper.PUEDE_ESCRIBIR)]
        public ActionResult Create(bool isInterAdmin)
        {
            if (!AutorizacionContrato(null, true)) return RedirectToAction("AccessDenied", "Account");
            ViewBag.IsInterAdmin = isInterAdmin;
            ViewBag.Accion = CREAR;
            ViewBag.IsEdit = false;
            ViewBag.idTipoContratoCIAD = new ContratosHelper().ObtenerIdCIAD();

            ViewBag.Persona_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto");
            ViewBag.PersonaAbogado_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 1), "Persona_Id", "NombreCompleto");
            ViewBag.PersonaSuperviosr_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 2), "Persona_Id", "NombreCompleto");
            ViewBag.PersonaSupervisorTecnico_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 4), "Persona_Id", "NombreCompleto");
            ViewBag.TipoEstadoContrato_Id = new SelectList(db.TiposEstadoContrato, "TiposEstadoContrato_Id", "Descripcion");
            ViewBag.ContratoMarco_Id = new SelectList(db.Contratos.Where(c => c.TipoContrato.Termino == tipoContratoMarco), "Contrato_Id", "NumeroContrato");
            ViewBag.PersonaNotificar_Id = new SelectList(db.Personas, "Persona_Id", "NombreCompleto");

            if (isInterAdmin)
            {
                ViewBag.TipoContrato_Id = new SelectList(db.TiposContratos, "TipoContrato_Id", "Descripcion",3);
            }
            else
            {
                ViewBag.TipoContrato_Id = new SelectList(db.TiposContratos, "TipoContrato_Id", "Descripcion");
            }
            ViewBag.FormaPagoId = new SelectList(db.FormaPagoes, "Id", "Descripcion");

            return View();
        }

        [GCPAuthorize(Roles = RolHelper.PUEDE_ESCRIBIR)]
        private ActionResult GuardarContrato(Contratos contratos, FormCollection form, bool esModificado, String[] PersonaNotificar_Id)
        {
            bool exito = false;
            string mensaje = string.Empty;

            try
            {
                if (contratos == null) return HttpNotFound();

                if (!AutorizacionContrato(contratos.Contrato_Id, true)) return RedirectToAction("AccessDenied", "Account");

                //if (ModelState.IsValid)
                //{
                    double valorContratoAux = Convert.ToDouble(contratos.ValorContratoAux.Replace(",", "").Replace(".00", ""));
                    contratos.ValorContrato = valorContratoAux;

                    double valorAdministrarAux = Convert.ToDouble(contratos.ValorAdministrarAux.Replace(",", "").Replace(".00", ""));
                    contratos.ValorAdministrar = valorAdministrarAux;

                    if (!string.IsNullOrEmpty(contratos.HonorariosAux))
                    {
                        double honorariosAux = Convert.ToDouble(contratos.HonorariosAux.Replace(",", "").Replace(".00", ""));
                        contratos.Honorarios = honorariosAux;
                    }

                    if (!string.IsNullOrEmpty(contratos.ValorPolizaAux))
                    {
                        double valorPolizaAux = Convert.ToDouble(contratos.ValorPolizaAux.Replace(",", "").Replace(".00", ""));
                        contratos.ValorPoliza = valorPolizaAux;
                    }

                    if (!string.IsNullOrEmpty(contratos.ValorCdpAux))
                    {
                        double valorCdpAux = Convert.ToDouble(contratos.ValorCdpAux.Replace(",", "").Replace(".00", ""));
                        contratos.ValorCDP = valorCdpAux;
                    }

                    if (!string.IsNullOrEmpty(contratos.ValorCrpAux))
                    {
                        double valorCrpAux = Convert.ToDouble(contratos.ValorCrpAux.Replace(",", "").Replace(".00", ""));
                        contratos.ValorCRP = valorCrpAux;
                    }

                if (esModificado)
                    { 
                        db.Entry(contratos).State = EntityState.Modified; 
                    }
                    else
                    {
                        db.Contratos.Add(contratos);
                        db.SaveChanges();
                    }

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

                    //Almaceno las personas a notificar
                    if (PersonaNotificar_Id != null)
                    {
                        foreach (var item in PersonaNotificar_Id)
                        {
                            Notificaciones notificacion = new Notificaciones()
                            {
                                ContractId = id,
                                PersonId = Convert.ToInt32(item)  
                            };
                            db.Notificaciones.Add(notificacion);
                        }
                    }

                    // Revisión de pagos - No aplican para los contratos que sean CIAD
                    //Se desactiva el condicional para que guarde los pagos al contrato asi no sean interadministrativos
                    //int idTipoContratoCIAD = (new ContratosHelper()).ObtenerIdCIAD();
                    //if (contratos.TipoContrato_Id != idTipoContratoCIAD)
                    //{
                        List<PagoContrato> pagosActuales = null;
                        List<int> pagosModificados = new List<int>();
                        if (esModificado) pagosActuales = db.PagoContrato.Where(p => p.Contrato_Id == id).ToList<PagoContrato>();

                        string strNumeroPagos = form["numeroPagos"];

                        if (!string.IsNullOrEmpty(strNumeroPagos))
                        {
                            int numeroPagos = int.Parse(strNumeroPagos);
                            for (int i = 0; i < numeroPagos; i++)
                            {
                                string idPagoAux = form["idPago_" + i];

                                double valorPagoAux = Convert.ToDouble(form["valorPago_" + i].Replace(",", "").Replace(".00", ""));

                                PagoContrato pago = null;
                                if (esModificado && !string.IsNullOrEmpty(idPagoAux))
                                {
                                    int idPago = int.Parse(idPagoAux);
                                    pagosModificados.Add(idPago);
                                    pago = pagosActuales.Where(p => p.PagosContrato_Id == idPago).FirstOrDefault<PagoContrato>();
                                    pago.Valor = valorPagoAux;
                                    pago.Fecha = DateTime.Parse(form["fechaPago_" + i]);
                                    pago.Notas = !string.IsNullOrEmpty(form["notasPago_" + i]) ? form["notasPago_" + i] : string.Empty;
                                    db.Entry(pago).State = EntityState.Modified;
                                }
                                else
                                {
                                    pago = new PagoContrato
                                    {
                                        Contrato_Id = id,
                                        Valor = valorPagoAux,
                                        Fecha = DateTime.Parse(form["fechaPago_" + i]),
                                        Notas = !string.IsNullOrEmpty(form["notasPago_" + i]) ? form["notasPago_" + i] : string.Empty
                                    };
                                    db.PagoContrato.Add(pago);
                                }
                            }
                        }
                        else
                        {
                            mensaje = "No se agregaron pagos al contrato. Por favor verifique.";
                        }

                //if (pagosActuales != null && pagosActuales.Count > 0 && pagosModificados != null && pagosModificados.Count > 0)
                //{
                //    List<PagoContrato> pagosContrato = new List<PagoContrato>();
                //    foreach (var pago in pagosActuales)
                //    {
                //        if (!pagosModificados.Contains(pago.PagosContrato_Id))
                //            db.Entry(pago).State = EntityState.Deleted;
                //        else
                //            pagosContrato.Add(pago);
                //    }

                //    contratos.PagoContrato = pagosContrato;

                //}
                //}

                // Solamente se deben guardar los cambios cuando NADA falle
                exito = (db.SaveChanges() > 0);
                //}
                //else
                //{
                //    mensaje = "No fue posible " + (ViewBag.IsEdit ? "actualizar" : "crear") + " el contrato";
                //}

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
                ViewBag.PersonaNotificar_Id = new SelectList(db.Personas, "Persona_Id", "NombreCompleto");

                ViewBag.MensajeError = mensaje;

                return View(contratos);
            }
        }

        // POST: Contratos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [GCPAuthorize(Roles = RolHelper.PUEDE_ESCRIBIR)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contratos contratos, FormCollection form, String[] PersonaNotificar_Id)
        {
            if (!AutorizacionContrato(null, true)) return RedirectToAction("AccessDenied", "Account");

            ViewBag.Accion = CREAR;
            ViewBag.IsEdit = false;
            ViewBag.idTipoContratoCIAD = new ContratosHelper().ObtenerIdCIAD();
            return GuardarContrato(contratos, form, ViewBag.IsEdit, PersonaNotificar_Id);
        }

        [GCPAuthorize(Roles = RolHelper.TODOS)]
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
        [GCPAuthorize(Roles = RolHelper.PUEDE_ESCRIBIR)]
        public ActionResult Edit(int? id)
        {
            ViewBag.Accion = EDITAR;
            ViewBag.IsEdit = true;
            ViewBag.idTipoContratoCIAD = new ContratosHelper().ObtenerIdCIAD();

            int? TipoContrato_Id = db.Contratos.Where(x => x.Contrato_Id == id).Select(y => y.TipoContrato_Id).FirstOrDefault();
            if (TipoContrato_Id == 3)
            {
                ViewBag.IsInterAdmin = true;
            }
            else
            {
                ViewBag.IsInterAdmin = false;
            }

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Contratos contratos = db.Contratos.Find(id);

            if (contratos == null) return HttpNotFound();
            else
                if (!AutorizacionContrato(id, true)) return RedirectToAction("AccessDenied", "Account");

            var formatter = new CultureInfo("es-CO", false).NumberFormat;
            formatter.NumberGroupSeparator = ",";
            formatter.NumberDecimalSeparator = ".";
            formatter.NumberDecimalDigits = 2;

            contratos.ValorContratoAux = contratos.ValorContrato.ToString("#,###.#0", formatter);
            contratos.ValorAdministrarAux = contratos.ValorAdministrar.ToString("#,###.#0", formatter);
            contratos.HonorariosAux = contratos.Honorarios.HasValue ? contratos.Honorarios.Value.ToString("#,###.#0", formatter) : "";
            contratos.ValorPolizaAux = contratos.ValorPoliza.HasValue ? contratos.ValorPoliza.Value.ToString("#,###.#0", formatter) : "";
            contratos.ValorCdpAux = contratos.ValorCDP.HasValue ? contratos.ValorCDP.Value.ToString("#,###.#0", formatter) : "";
            contratos.ValorCrpAux = contratos.ValorCRP.HasValue ? contratos.ValorCRP.Value.ToString("#,###.#0", formatter) : "";


            List<PagoContrato> pagosContrato = db.PagoContrato.Where(p => p.Contrato_Id == id).ToList<PagoContrato>();
            contratos.PagoContrato = pagosContrato;

            ViewBag.Persona_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto", contratos.Persona_Id);
            ViewBag.PersonaAbogado_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 1), "Persona_Id", "NombreCompleto",contratos.PersonaAbogado_Id);
            ViewBag.PersonaSuperviosr_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 2), "Persona_Id", "NombreCompleto", contratos.PersonaSuperviosr_Id);
            ViewBag.PersonaSupervisorTecnico_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 4), "Persona_Id", "NombreCompleto",contratos.PersonaSupervisorTecnico_Id);
            ViewBag.TipoEstadoContrato_Id = new SelectList(db.TiposEstadoContrato, "TiposEstadoContrato_Id", "Descripcion",contratos.TipoEstadoContrato_Id);
            ViewBag.ContratoMarco_Id = new SelectList(db.Contratos.Where(c => c.ContratoMarco_Id == null), "Contrato_Id", "NumeroContrato",contratos.ContratoMarco_Id);
            ViewBag.TipoContrato_Id_Aux = new SelectList(db.TiposContratos, "TipoContrato_Id", "Descripcion", contratos.TipoContrato_Id);
            ViewBag.FormaPagoId = new SelectList(db.FormaPagoes, "Id", "Descripcion");


            ViewBag.PersonasNotificar= (from p in db.Personas
                                                join n in db.Notificaciones on p.Persona_Id equals n.PersonId
                                                where n.ContractId == contratos.Contrato_Id
                                                select new PersonasNotificacion
                                                {
                                                    Persona_Id =  p.Persona_Id,
                                                    NombreCompleto=  p.Nombres +" "+ p.Apellidos,
     
                                                }).ToList();

            ViewBag.PersonaNotificar_Id = new SelectList(db.Personas, "Persona_Id", "NombreCompleto");

            return View(contratos);
        }

        // POST: Contratos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [GCPAuthorize(Roles = RolHelper.PUEDE_ESCRIBIR)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contratos contratos, FormCollection form, String[]  PersonaNotificar_Id)
        {
            if (!AutorizacionContrato(contratos.Contrato_Id, true)) return RedirectToAction("AccessDenied", "Account");
            int? TipoContrato_Id = db.Contratos.Where(x=> x.Contrato_Id == contratos.Contrato_Id).Select(y=> y.TipoContrato_Id).FirstOrDefault();
            if(TipoContrato_Id ==3)
            {
                ViewBag.IsInterAdmin = true;
            }
            else
            {
                ViewBag.IsInterAdmin = false;
            }

            ViewBag.Accion = EDITAR;
            ViewBag.IsEdit = true;
            ViewBag.idTipoContratoCIAD = new ContratosHelper().ObtenerIdCIAD();
            return GuardarContrato(contratos, form, ViewBag.IsEdit, PersonaNotificar_Id);
        }

        // GET: Contratos/Delete/5
        [GCPAuthorize(Roles = RolHelper.PUEDE_ESCRIBIR)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Contratos contratos = db.Contratos.Find(id);
            if (contratos == null) return HttpNotFound();
            else
                if (!AutorizacionContrato(id, true)) return RedirectToAction("AccessDenied", "Account");

            return View(contratos);
        }

        // POST: Contratos/Delete/5
        [GCPAuthorize(Roles = RolHelper.PUEDE_ESCRIBIR)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!AutorizacionContrato(id, true)) return RedirectToAction("AccessDenied", "Account");

            Contratos contratos = db.Contratos.Find(id);

            db.Contratos.Remove(contratos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [GCPAuthorize(Roles = RolHelper.TODOS)]
        [HttpGet]
        public PartialViewResult PartialContratos(int id)
        {
            List<Contratos> list = GetContratosById(id);
            return PartialView("PartialContratos", list.ToList());
        }

        [GCPAuthorize(Roles = RolHelper.TODOS)]
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

        [GCPAuthorize(Roles = RolHelper.TODOS)]
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

        [GCPAuthorize(Roles = RolHelper.TODOS)]
        public JsonResult GetSearchNumeroContrato(string numeroContrato)
        {
            var allsearch = db.Contratos.Where(x=> x.ContratoMarco_Id == null && x.NumeroContrato.Contains(numeroContrato)).ToList();
            var list = (from N in allsearch
                        select new {  N.NumeroContrato }).Distinct();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [GCPAuthorize(Roles = RolHelper.TODOS)]
        public JsonResult GetSearchEntidadContratante(string entidad)
        {
            var allsearch = db.Personas.Where(x => (x.TipoPersona_Id == 3 && x.Nombres.Contains(entidad)) || (x.TipoPersona_Id == 3 && x.Apellidos.Contains(entidad))).ToList();
            var list = (from N in allsearch
                        select new { Entidad=N.Nombres +" " +N.Apellidos }).Distinct();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [GCPAuthorize(Roles = RolHelper.TODOS)]
        public JsonResult GetObservaciones(int id)
        {
            var datos = (from h in db.HistoriaObservaciones
                         //join u in db.Users on h.UsuarioCrea equals u.Email
                         where h.ContratoId == id
                         select new { h.Fecha, h.Observaciones}
                        );


            return Json(datos.Select(o => new { o.Fecha, o.Observaciones }).OrderBy(o => o.Fecha), JsonRequestBehavior.AllowGet);
        }

        [GCPAuthorize(Roles = RolHelper.TODOS)]
        private bool AutorizacionContrato(int? contratoId, bool esEscritura)
        {
            bool tienePermiso = false;
            if (User.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)User.Identity;
                bool esSuperUsuario = User.IsInRole(RolHelper.SUPERUSUARIO);
                bool todosLosContratos = User.IsInRole(RolHelper.TODOS_LOS_CONTRATOS);

                if (esSuperUsuario)
                    tienePermiso = true;
                else
                {
                    if (!todosLosContratos)
                    {
                        if (contratoId.HasValue)
                        {
                            string idContratos = identity.Claims.Where(c => c.Type == RolHelper.LISTADO_CONTRATOS).Select(c => c.Value).FirstOrDefault();
                            List<int> listadoIdContratos = idContratos.Split(',').Select(int.Parse).ToList();
                            tienePermiso = listadoIdContratos.Contains(contratoId.Value);
                        }
                    }
                    else
                    {
                        if (esEscritura)
                            tienePermiso = User.IsInRole(RolHelper.ESCRITURA);
                        else
                            tienePermiso = User.IsInRole(RolHelper.LECTURA);
                    }
                }
            }

            return tienePermiso;

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
