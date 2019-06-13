using GCP_CF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GCP_CF.Controllers
{
    public class ActividadesController : Controller
    {
        private GCPContext db = new GCPContext();

        // GET: actividades
        public ActionResult Index()
        {
            List<Contratos> ListadoContratos;
            ListadoContratos = (from e in db.Contratos select e).ToList();
            if (ListadoContratos == null)
            {
                ListadoContratos = new List<Contratos>();
            }

            ViewBag.ListadoContratos = ListadoContratos;
            List<FasesContrato> fases = db.FasesContrato.ToList();
            ViewBag.ListaFases = fases;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearContratoFase([Bind(Include = "Fase_Id,Contrato_Id")] Registrofacescontratos registrofacescontratos)
        {
            if (ModelState.IsValid)
            {
                if (registrofacescontratos.Fase_Id != 0)
                    if (db.Registrofacescontratos.Where(x => x.Fase_Id == registrofacescontratos.Fase_Id && x.Contrato_Id == registrofacescontratos.Contrato_Id).Select(x => x).ToList().Count() == 0)
                    {
                        db.Registrofacescontratos.Add(registrofacescontratos);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else {
                        TempData["Error"]  = "Error. Este fase ya se encuentra asignada al contrato";
                        return RedirectToAction("Index");
                    }
                TempData["Error"]  = "Error. Seleciona una Fase.";

            }
            return RedirectToAction("Index");
        }




        public JsonResult ObtenerFasesContratos(int idContrato)
        {
            var response = new { html = "", numContrato = "" };
            List<Registrofacescontratos> listadoFasesContrato;
            listadoFasesContrato = (from e in db.Registrofacescontratos.Include(a => a.FasesContrato)
                                    where e.Contrato_Id == idContrato
                                    select e).ToList();
            string table = "<table><thead><tr><th>Fase</th>";
            table = table + "<th>Acciones</th></tr></thead><tbody>";
            int indexrow = 0;
            string classname = "";
            string numContrato = "";
            if (listadoFasesContrato != null)
            {
                if (listadoFasesContrato.Count > 0)
                {
                    foreach (Registrofacescontratos item in listadoFasesContrato)
                    {
                        //ViewBag.ContratoNo = item.Contratos.NumeroContrato;
                        numContrato = item.Contratos.NumeroContrato;
                        if (indexrow == 1)
                        {
                            classname = "alt";
                            indexrow = 0;
                        }
                        else
                        {
                            classname = "";
                            indexrow = 1;
                        }

                        table = table + "<tr class='" + classname + "'><td>" + item.FasesContrato.Descripcion + "</td>" +
                            "<td style='width: 83px;cursor:pointer'><span onclick='VerActividadesFaseContrato(" + item.Contrato_Id + "," + item.Fase_Id + ")' class='label label-default'>" +
                            "Ver Actividades</span></td></tr>";
                    }
                    table = table + "</tbody></table>";
                }
                else
                {
                    table = "<table><thead><tr><th style='width:147px;'>Fase</th>";
                    table = table + "<th>Acciones</th></tr></thead><tbody><tr><td colspan='2'>No hay datos.</td></tr></tbody></table>";
                }
            }

            response = new
            {
                html = table,
                numContrato = numContrato
            };
            return Json(response);
        }

        public JsonResult ObtenerActividadesFasesContratos(int idContrato, int idFase)
        {
            var response = new { html = "", numContrato = "", numFase = "", idContrato = 0, idFase = 0 };
            List<ActividadesFases> listadoActividadesFasesContrato = null;
            listadoActividadesFasesContrato = (from e in db.ActividadesFases
                                               where e.Contratos_Contrato_Id1 == idContrato && e.FasesContrato_fase_Id1 == idFase
                                               select e).ToList();
            string table = "<table><thead><tr><th style='width:100px;'>Item</th><th style='width:200px;'>Descripción</th><th style='width:147px;'>Días Hábiles</th>" +
                "<th style='width:110px'>F.Inicio</th><th style='width:110px'>F.Fin</th></tr></thead><tbody>";
            int indexrow = 0;
            string classname = "";
            string numFase = "";
            string numContrato = "";

            if (listadoActividadesFasesContrato != null)
            {
                if (listadoActividadesFasesContrato.Count > 0)
                {
                    foreach (ActividadesFases item in listadoActividadesFasesContrato)
                    {
                        numFase = item.FasesContrato.Descripcion;
                        numContrato = item.Contratos.NumeroContrato;
                        if (indexrow == 1)
                        {
                            classname = "alt";
                            indexrow = 0;
                        }
                        else
                        {
                            classname = "";
                            indexrow = 1;
                        }

                        table = table + "<tr class='" + classname + "'><td>" + item.Item + "</td><td>" + item.Descripción + "</td><td>" + item.DiasHabiles.ToString() + "</td>" +
                            "<td>" + Convert.ToDateTime(item.FechaInicio).ToShortDateString() + "</td><td>" + Convert.ToDateTime(item.FechaFinal).ToShortDateString() + "</td></tr>";
                    }
                    table = table + "</tbody></table>";
                }
                else
                {
                    table = "<table><thead><tr><th style='width:147px;'>Fase</th>";
                    table = table + "<th>Acciones</th></tr></thead><tbody><tr><td colspan='2'>No hay datos.</td></tr></tbody></table>";
                }
            }

            response = new
            {
                html = table,
                numContrato = numContrato,
                numFase = numFase,
                idContrato = idContrato,
                idFase = idFase
            };
            return Json(response);
        }

        public JsonResult GuardarActividadFase(int idContrato, int idFase, string item, string descripcion, string diashabiles, string finicio,
           string ffin, string estadoactividad)
        {
            try
            {
                ActividadesFases actividad = new ActividadesFases
                {
                    Contratos_Contrato_Id1 = idContrato,
                    FasesContrato_fase_Id1 = idFase,
                    Item = item,
                    Descripción = descripcion,
                    DiasHabiles = Convert.ToInt32(diashabiles),
                    FechaInicio = Convert.ToDateTime(finicio),
                    FechaFinal = Convert.ToDateTime(ffin),
                    EstadoActividad_Id = Convert.ToInt32(estadoactividad)
                };

                db.ActividadesFases.Add(actividad);
                db.SaveChanges();
                var response = new
                {
                    response = "true"
                };

                return ObtenerActividadesFasesContratos(idContrato, idFase);
            }
            catch (Exception)
            {
                var response = new
                {
                    response = "false"
                };
                return Json(true);
            }

        }

        // GET: actividades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividades actividades = db.Actividades.Find(id);
            if (actividades == null)
            {
                return HttpNotFound();
            }
            return View(actividades);
        }

        // GET: actividades/Create
        public ActionResult Create()
        {
            ViewBag.EstadoActividad_Id = new SelectList(db.EstadosActividad, "EstadoActividad_Id", "Descripcion");
            ViewBag.registrofacescontratos_id = new SelectList(db.Registrofacescontratos, "registrofacescontratos_id", "registrofacescontratos_id");
            return View();
        }

        // POST: actividades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Actividad_Id,registrofacescontratos_id,FaseContrato_Id,Item,Descripción,DiasHabiles,FechaInicio,FechaFinal,EstadoActividad_Id,Observaciones")] Actividades actividades)
        {
            if (ModelState.IsValid)
            {
                db.Actividades.Add(actividades);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EstadoActividad_Id = new SelectList(db.EstadosActividad, "EstadoActividad_Id", "Descripcion", actividades.EstadoActividad_Id);
            ViewBag.registrofacescontratos_id = new SelectList(db.Registrofacescontratos, "registrofacescontratos_id", "registrofacescontratos_id", actividades.Registrofacescontratos_id);
            return View(actividades);
        }

        // GET: actividades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividades actividades = db.Actividades.Find(id);
            if (actividades == null)
            {
                return HttpNotFound();
            }
            ViewBag.EstadoActividad_Id = new SelectList(db.EstadosActividad, "EstadoActividad_Id", "Descripcion", actividades.EstadoActividad_Id);
            ViewBag.registrofacescontratos_id = new SelectList(db.Registrofacescontratos, "registrofacescontratos_id", "registrofacescontratos_id", actividades.Registrofacescontratos_id);
            return View(actividades);
        }

        // POST: actividades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Actividad_Id,registrofacescontratos_id,FaseContrato_Id,Item,Descripción,DiasHabiles,FechaInicio,FechaFinal,EstadoActividad_Id,Observaciones")] Actividades actividades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actividades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EstadoActividad_Id = new SelectList(db.EstadosActividad, "EstadoActividad_Id", "Descripcion", actividades.EstadoActividad_Id);
            ViewBag.registrofacescontratos_id = new SelectList(db.Registrofacescontratos, "registrofacescontratos_id", "registrofacescontratos_id", actividades.Registrofacescontratos_id);
            return View(actividades);
        }

        // GET: actividades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividades actividades = db.Actividades.Find(id);
            if (actividades == null)
            {
                return HttpNotFound();
            }
            return View(actividades);
        }

        // POST: actividades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Actividades actividades = db.Actividades.Find(id);
            db.Actividades.Remove(actividades);
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

