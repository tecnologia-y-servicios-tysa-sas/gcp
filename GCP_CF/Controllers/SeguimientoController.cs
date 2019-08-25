using GCP_CF.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using PagedList;
using System;

namespace GCP_CF.Controllers
{
    public class SeguimientoController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: Seguimiento
        public ViewResult Index(string sortBy, string currentFilter, string searchString, int? page)
        {
            var listadoContratos = GetListadoContratos(sortBy, currentFilter, searchString, page);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(listadoContratos.ToPagedList<Contratos>(pageNumber, pageSize));
        }

        // GET: FasesContrato
        public ActionResult FasesContrato(int? idContrato)
        {
            if (idContrato == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Contratos contrato = db.Contratos.Find(idContrato);
            ViewBag.Contrato = contrato;

            ViewBag.EstadoActividad_Id = new SelectList(db.EstadosActividad.OrderBy(e => e.Descripcion), "EstadoActividad_Id", "Descripcion");

            List<Registrofacescontratos> listadoFasesContrato = (from e in db.Registrofacescontratos.Include(a => a.FasesContrato)
                                                                 where e.Contrato_Id == idContrato
                                                                 select e).ToList();
            ViewBag.FasesContrato = listadoFasesContrato;

            return View();
        }

        [HttpPost]
        public JsonResult ObtenerListadoFasesContrato(int idContrato)
        {
            // TODO: Mejorar esta respuesta construyendo objetos
            object response = null;

            try
            {
                List<Registrofacescontratos> listadoFasesContrato = db.Registrofacescontratos
                                                                        .Include(a => a.FasesContrato)
                                                                        .Where(e => e.Contrato_Id == idContrato)
                                                                        .Select(e => e)
                                                                        .ToList<Registrofacescontratos>();
                //.Select(e => new { e.FasesContrato.fase_Id, e.FasesContrato.Descripcion });

                if (listadoFasesContrato.Count() > 0)
                {
                    var listadoFases = new List<object>();

                    foreach (Registrofacescontratos fase in listadoFasesContrato) {
                        var listadoActividadesFase = db.ActividadesFases
                                                        .Include(a => a.EstadosActividad)
                                                        .Where(a => a.FasesContrato_fase_Id1 == fase.Fase_Id && a.Contratos_Contrato_Id1 == idContrato)
                                                        .OrderBy(a => new { a.FechaInicio, a.FechaFinal })
                                                        .Select(a => new {
                                                            Id = a.Actividad_Id, 
                                                            IdFase = a.FasesContrato_fase_Id1,
                                                            IdContrato = a.Contratos_Contrato_Id1,
                                                            IdActividad = a.Actividad_Id,
                                                            a.Item,
                                                            Descripcion = a.Descripción,
                                                            DiasEntreFechas = a.DiasHabiles,
                                                            FechaInicio = a.FechaInicio.HasValue ? a.FechaInicio.Value.ToString() : string.Empty,
                                                            FechaFinal = a.FechaFinal.HasValue ? a.FechaFinal.Value.ToString() : string.Empty,
                                                            CodigoEstado = a.EstadoActividad_Id.Value, 
                                                            DescripcionEstado = a.EstadosActividad.Descripcion
                                                        });

                        listadoFases.Add(new { Id = fase.FasesContrato.fase_Id, Descripcion = fase.FasesContrato.Descripcion, Actividades = listadoActividadesFase });
                    }

                    response = new { fases = listadoFases };
                } 
                else
                    response = new { mensaje = "No existen fases asociadas a este contrato" };

            } catch (Exception e)
            {
                response = new { mensaje = "Ha ocurrido un error al obtener las fases asociadas a este contrato", detalle = e.Message };
            }

            return Json(response);
        }

        [HttpPost]
        public JsonResult ObtenerFasesNoAsociadas(int idContrato)
        {
            List<int> listadoFasesContrato = (from fc in db.Registrofacescontratos.Include(a => a.FasesContrato)
                                              where fc.Contrato_Id == idContrato
                                              select fc.Fase_Id).ToList();

            var listadoFasesNoAsociadas = db.FasesContrato.Select(f => new { f.fase_Id, f.Descripcion }).Where(f => !listadoFasesContrato.Contains(f.fase_Id));

            var response = "{ \"fases\":" + JsonConvert.DeserializeObject(JsonConvert.SerializeObject(listadoFasesNoAsociadas)) + "}";
            return Json(response);
        }

        [HttpPost]
        public JsonResult GuardarFaseContrato(int idContrato, int idFase)
        {
            string mensaje = "";
            string error = "";

            string numeroContrato = db.Contratos.Where(c => c.Contrato_Id == idContrato).Select(c => c.NumeroContrato).FirstOrDefault();
            string nombreFase = db.FasesContrato.Where(f => f.fase_Id == idFase).Select(f => f.Descripcion).FirstOrDefault();

            Registrofacescontratos rfc = new Registrofacescontratos();
            rfc.Contrato_Id = idContrato;
            rfc.Fase_Id = idFase;

            if (rfc.Fase_Id != 0)
                if (db.Registrofacescontratos.Where(x => x.Fase_Id == rfc.Fase_Id && x.Contrato_Id == rfc.Contrato_Id).Select(x => x).ToList().Count() == 0)
                {
                    try
                    {
                        db.Registrofacescontratos.Add(rfc);
                        db.SaveChanges();
                        mensaje = "La fase &quot;" + nombreFase.Trim() + "&quot; fue agregada exitosamente al contrato " + numeroContrato + ".";
                    }
                    catch (Exception)
                    {
                        error = "Ocurrió un error al intentar agregar la fase &quot;" + nombreFase.Trim() + "&quot; al contrato " + numeroContrato + ".";
                    }
                }
                else
                {
                    error = "<b>Error:</b> La fase &quot;" + nombreFase.Trim() + "&quot; ya se encuentra asignada al contrato" + numeroContrato + ".";
                }

            return Json("{ \"mensaje\": \"" + mensaje + "\", \"error\": \"" + error + "\" }");
        }

        [HttpPost]
        public JsonResult EliminarFaseContrato(int idContrato, int idFase)
        {
            string mensaje = "";
            string error = "";

            string numeroContrato = db.Contratos.Where(c => c.Contrato_Id == idContrato).Select(c => c.NumeroContrato).FirstOrDefault();
            string nombreFase = db.FasesContrato.Where(f => f.fase_Id == idFase).Select(f => f.Descripcion).FirstOrDefault();

            Registrofacescontratos rfc = db.Registrofacescontratos
                                            .Where(x => x.Fase_Id == idFase && x.Contrato_Id == idContrato)
                                            .Select(x => x).FirstOrDefault();

            if (rfc.Fase_Id != 0)
                if (rfc != null)
                {
                    // Validar antes si la fase que se eliminará del contrato ya tiene al menos una actividad asociada
                    List<ActividadesFases> actividadesFase = db.ActividadesFases
                                                               .Where(af => af.Contratos_Contrato_Id1 == idContrato && af.FasesContrato_fase_Id1 == idFase)
                                                               .Select(af => af).ToList<ActividadesFases>();

                    // Si la fase tiene al menos una actividad asociada, no se dejará eliminar
                    if (actividadesFase.Count > 0)
                        error = "<b>Error:</b> No es posible eliminar la fase &quot;" + nombreFase.Trim() + "&quot;, ya que tiene " + actividadesFase.Count + " actividades asociadas.";
                    else
                    {
                        try
                        {
                            db.Registrofacescontratos.Remove(rfc);
                            db.SaveChanges();
                            mensaje = "La fase &quot;" + nombreFase.Trim() + "&quot; fue eliminada exitosamente del contrato " + numeroContrato + ".";
                        }
                        catch (Exception)
                        {
                            error = "Ocurrió un error al intentar eliminar la fase &quot;" + nombreFase.Trim() + "&quot; del contrato " + numeroContrato + ".";
                        }
                    }
                }
                else
                {
                    error = "<b>Error:</b> El identificador de la fase no es válido.";
                }

            return Json("{ \"mensaje\": \"" + mensaje + "\", \"error\": \"" + error + "\" }");
        }

        [HttpPost]
        public JsonResult GuardarActividadFase(int idActividad, int idContrato, int idFase, string item, string descripcion, 
            string diasHabiles, string fechaInicio, string fechaFin, string estadoActividad)
        {
            string accionVerbo = idActividad > 1 ? "actualizar" : "agregar";
            string accionEjecutada = idActividad > 1 ? "actualizada" : "agregada";

            string mensaje = "";
            string error = "";
            string detalleError = "";

            try
            {
                ActividadesFases actividad = null;

                if (idActividad > -1)
                {
                    actividad = (from a in db.ActividadesFases
                                 where a.Actividad_Id == idActividad
                                 select a).FirstOrDefault<ActividadesFases>();
                    if (actividad.Item != item) actividad.Item = item;
                    if (actividad.Descripción != descripcion) actividad.Descripción = descripcion;
                    if (actividad.DiasHabiles.GetValueOrDefault() != Convert.ToInt32(diasHabiles)) actividad.DiasHabiles = Convert.ToInt32(diasHabiles);
                    if (actividad.FechaInicio != Convert.ToDateTime(fechaInicio)) actividad.FechaInicio = Convert.ToDateTime(fechaInicio);
                    if (actividad.FechaFinal != Convert.ToDateTime(fechaFin)) actividad.FechaFinal = Convert.ToDateTime(fechaFin);
                    if (actividad.EstadoActividad_Id != Convert.ToInt32(estadoActividad)) actividad.EstadoActividad_Id = Convert.ToInt32(estadoActividad);
                }                    
                else
                {
                    actividad = new ActividadesFases
                    {
                        Contratos_Contrato_Id1 = idContrato,
                        FasesContrato_fase_Id1 = idFase,
                        Item = item,
                        Descripción = descripcion,
                        DiasHabiles = Convert.ToInt32(diasHabiles),
                        FechaInicio = Convert.ToDateTime(fechaInicio),
                        FechaFinal = Convert.ToDateTime(fechaFin),
                        EstadoActividad_Id = Convert.ToInt32(estadoActividad)
                    };
                    db.ActividadesFases.Add(actividad);
                }

                db.SaveChanges();
                mensaje = "La actividad ha sido " + accionEjecutada + " exitosamente.";

            }
            catch (Exception e)
            {
                error = "Ocurrió un error al " + accionVerbo + " la actividad.";
                detalleError = e.Message + (e.InnerException != null ? " - " + e.InnerException.Message : "N/A" );
            }

            return Json("{ \"mensaje\": \"" + mensaje + "\", \"error\": \"" + error + "\", \"detalleError\": \"" + detalleError + "\" }");

        }

        [HttpPost]
        public JsonResult ObtenerListadoActividadesFase(int idContrato, int idFase)
        {
            // TODO: Mejorar esta respuesta construyendo objetos
            object response = null;

            try
            {
                var listadoActividadesFase = db.ActividadesFases
                                            .Where(a => a.FasesContrato_fase_Id1 == idFase && a.Contratos_Contrato_Id1 == idContrato)
                                            .Select(a => a);

                if (listadoActividadesFase.Count() > 0)
                    response = new { fases = listadoActividadesFase };
                else
                    response = new { mensaje = "Esta fase aún no tiene actividades asociadas." };

            }
            catch (Exception e)
            {
                response = new { mensaje = "Ha ocurrido un error al obtener las actividades asociadas a esta fase.", detalle = e.Message };
            }

            return Json(response);
        }

        [HttpPost]
        public JsonResult ObtenerActividadFase(int idActividad)
        {
            // TODO: Mejorar esta respuesta construyendo objetos
            object response = null;

            var actividad = db.ActividadesFases
                              .Where(a => a.Actividad_Id == idActividad)
                              .Select(a => new {
                                  Id = a.Actividad_Id,
                                  IdFase = a.FasesContrato_fase_Id1,
                                  IdContrato = a.Contratos_Contrato_Id1,
                                  IdActividad = a.Actividad_Id,
                                  a.Item,
                                  Descripcion = a.Descripción,
                                  DiasEntreFechas = a.DiasHabiles,
                                  FechaInicio = a.FechaInicio.HasValue ? a.FechaInicio.Value.ToString() : string.Empty,
                                  FechaFinal = a.FechaFinal.HasValue ? a.FechaFinal.Value.ToString() : string.Empty,
                                  CodigoEstado = a.EstadoActividad_Id.Value,
                                  DescripcionEstado = a.EstadosActividad.Descripcion
                              }).FirstOrDefault();

            if (actividad != null && actividad.Id != 0)
            {
                try
                {
                    response = new { actividadFase = actividad };
                }
                catch (Exception e)
                {
                    response = new { mensaje = "Ha ocurrido un error al obtener el detalle de la actividad.", detalle = e.Message };
                }
            }
            else
            {
                response = new { error = "<b>Error:</b> El identificador de la actividad no es válido." };
            }

            return Json(response);
        }

        [HttpPost]
        public JsonResult EliminarActividadFase(int idActividad)
        {
            string mensaje = "";
            string error = "";

            ActividadesFases actividad = db.ActividadesFases
                                           .Where(x => x.Actividad_Id == idActividad)
                                           .Select(x => x).FirstOrDefault();

            if (actividad != null && actividad.Actividad_Id != 0)
            {
                try
                {
                    db.ActividadesFases.Remove(actividad);
                    db.SaveChanges();
                    mensaje = "La actividad &quot;" + actividad.Descripción.Trim() + "&quot; fue eliminada exitosamente.";
                }
                catch (Exception)
                {
                    error = "Ocurrió un error al intentar eliminar la actividad &quot;" + actividad.Descripción.Trim() + "&quot;.";
                }
            }
            else
            {
                error = "<b>Error:</b> El identificador de la actividad no es válido.";
            }

            return Json("{ \"mensaje\": \"" + mensaje + "\", \"error\": \"" + error + "\" }");
        }

        private IQueryable<Contratos> GetListadoContratos(string sortBy, string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.SortByIdContrato = sortBy == "idContrato|ASC" ? "idContrato|DESC" : "idContrato|ASC";
            ViewBag.SortByFechaInicio = sortBy == "fechaInicio|ASC" ? "fechaInicio|DESC" : "fechaInicio|ASC";
            ViewBag.SortByEntidadContratante = sortBy == "fechaInicio|ASC" ? "entidadContratante|DESC" : "entidadContratante|ASC";
            ViewBag.SortByObjetoContractual = sortBy == "objetoContractual|ASC" ? "objetoContractual|DESC" : "objetoContractual|ASC";
            ViewBag.SortByPlazo = sortBy == "objetoContractual|ASC" ? "plazo|DESC" : "plazo|ASC";
            ViewBag.SortByFechaTerminacion = sortBy == "fechaTerminacion|ASC" ? "fechaTerminacion|DESC" : "fechaTerminacion|ASC";
            ViewBag.SortByValorContrato = sortBy == "valorContrato|ASC"  ? "valorContrato|DESC" : "valorContrato|ASC";

            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentFilter = searchString;

            var listadoContratos = from c in db.Contratos
                                   select c;

            if (!string.IsNullOrEmpty(searchString))
                listadoContratos = listadoContratos.Where(c => c.NumeroContrato.Contains(searchString));

            switch (sortBy)
            {
                case "idContrato|DESC":
                    listadoContratos = listadoContratos.OrderByDescending(c => c.NumeroContrato);
                    break;
                case "idContrato|ASC":
                    listadoContratos = listadoContratos.OrderBy(c => c.NumeroContrato);
                    break;
                case "fechaInicio|DESC":
                    listadoContratos = listadoContratos.OrderByDescending(c => c.FechaInicio);
                    break;
                case "fechaInicio|ASC":
                    listadoContratos = listadoContratos.OrderBy(c => c.FechaInicio);
                    break;
                case "entidadContratante|DESC":
                    listadoContratos = listadoContratos.OrderByDescending(c => c.EntidadContratante);
                    break;
                case "entidadContratante|ASC":
                    listadoContratos = listadoContratos.OrderBy(c => c.EntidadContratante);
                    break;
                case "objetoContractual|DESC":
                    listadoContratos = listadoContratos.OrderByDescending(c => c.ObjetoContractual);
                    break;
                case "objetoContractual|ASC":
                    listadoContratos = listadoContratos.OrderBy(c => c.ObjetoContractual);
                    break;
                case "plazo|DESC":
                    listadoContratos = listadoContratos.OrderByDescending(c => c.Plazo);
                    break;
                case "plazo|ASC":
                    listadoContratos = listadoContratos.OrderBy(c => c.Plazo);
                    break;
                case "fechaTerminacion|DESC":
                    listadoContratos = listadoContratos.OrderByDescending(c => c.FechaTerminacion);
                    break;
                case "fechaTerminacion|ASC":
                    listadoContratos = listadoContratos.OrderBy(c => c.FechaTerminacion);
                    break;
                case "valorContrato|DESC":
                    listadoContratos = listadoContratos.OrderByDescending(c => c.ValorContrato);
                    break;
                case "valorContrato|ASC":
                    listadoContratos = listadoContratos.OrderBy(c => c.ValorContrato);
                    break;
                default:
                    listadoContratos = listadoContratos.OrderByDescending(c => c.Contrato_Id);
                    break;
            }

            return listadoContratos;
        }

        [HttpPost]
        public JsonResult ActualizarValorEjecutado(int idContrato, double? valorEjecutado)
        {
            double porcentajeEjecucion = 0;
            string mensaje = "";
            string error = "";

            if (valorEjecutado != null)
            {
                Contratos contrato = db.Contratos.Where(c => c.Contrato_Id == idContrato).FirstOrDefault();

                if (valorEjecutado > 0)
                {
                    try
                    {
                        contrato.Ejecucion = valorEjecutado;
                        db.Entry(contrato).State = EntityState.Modified;
                        if (db.SaveChanges() > 0)
                        {
                            mensaje = "El valor ejecutado del contrato " + contrato.NumeroContrato + " fue actualizado exitosamente.";
                            porcentajeEjecucion = Math.Round(contrato.PorcentajeValorEjecutado, 2);
                        }
                        else
                        {
                            mensaje = "No fue posible actualizar el valor ejecutado del contrato " + contrato.NumeroContrato + ".";
                        }
                    }
                    catch (Exception)
                    {
                        error = "Ocurrió un error al intentar actualizar el valor ejecutado del contrato " + contrato.NumeroContrato + ".";
                    }
                }
                else
                {
                    error = "<b>Error:</b> El valor ejecutado ingresado no es válido.";
                }

            } else
                error = "<b>Error:</b> Debe especificar el valor ejecutado.";

            return Json("{ \"porcentajeEjecucion\": " + porcentajeEjecucion + ", \"mensaje\": \"" + mensaje + "\", \"error\": \"" + error + "\" }");
        }
    }
}