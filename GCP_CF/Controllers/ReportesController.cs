using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GCP_CF.Models;

namespace GCP_CF.Controllers
{
    public class ReportesController : Controller
    {
        private GCPContext db = new GCPContext();

        public ActionResult Contratos(int? Anio, int? IdEntidadContratante, string NumeroContrato, int? IdEstadoContrato)
        {
            ViewBag.IdEstadoContrato = new SelectList(db.TiposEstadoContrato.OrderBy(x => x.Descripcion), "TiposEstadoContrato_Id", "Descripcion", IdEstadoContrato.GetValueOrDefault());
            ViewBag.IdEntidadContratante = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto", IdEntidadContratante.GetValueOrDefault());

            HashSet<int> aniosContratos = new HashSet<int>();
            var fechasContratos = db.Contratos.Select(c => new { c.FechaInicio, c.FechaTerminacion, c.FechaActaInicio, c.FechaCdp, c.FechaCrp });
            foreach (var fechaContrato in fechasContratos)
            {
                aniosContratos.Add(fechaContrato.FechaInicio.Year);
                aniosContratos.Add(fechaContrato.FechaTerminacion.Year);
                aniosContratos.Add(fechaContrato.FechaActaInicio.Year);
                aniosContratos.Add(fechaContrato.FechaCdp.GetValueOrDefault().Year);
                aniosContratos.Add(fechaContrato.FechaCrp.GetValueOrDefault().Year);
            }

            ViewBag.Anio = new SelectList(aniosContratos.Distinct().Where(x => x > 0).OrderByDescending(x => x), Anio.GetValueOrDefault());

            List<Contratos> list = ObtenerContratos(Anio, IdEntidadContratante, NumeroContrato, IdEstadoContrato);
            return View(list.OrderBy(x => x.Contrato_Id).ToList());
        }

        public ActionResult Facturas()
        {
            return View();
        }

        private List<Contratos> ObtenerContratos(int? anio, int? idEntidadContratante, string numeroContrato, int? idEstadoContrato)
        {
            DateTime? fechaInicio = anio.HasValue ? new DateTime(anio.Value, 1, 1) : new DateTime?();
            DateTime? fechaFin = anio.HasValue ? new DateTime(anio.Value, 12, 1) : new DateTime?();

            List<Contratos> list1 = new List<Contratos>();
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                ViewBag.Anio = anio.Value;
                list1 = db.Contratos.Include(c => c.HistoriaObservaciones)
                                    .Where(c => (fechaInicio.Value <= c.FechaInicio && c.FechaInicio <= fechaFin)
                                            || (fechaInicio.Value <= c.FechaTerminacion && c.FechaInicio <= fechaFin)
                                            || (fechaInicio.Value <= c.FechaActaInicio && c.FechaInicio <= fechaFin)
                                            || (fechaInicio.Value <= c.FechaCdp && c.FechaInicio <= fechaFin)
                                            || (fechaInicio.Value <= c.FechaCrp && c.FechaInicio <= fechaFin)).ToList<Contratos>();
            }

            List<Contratos> list2 = new List<Contratos>();
            if (idEntidadContratante.HasValue)
            {
                ViewBag.IdEntidadContratante = idEntidadContratante.Value;
                list2 = db.Contratos.Include(c => c.HistoriaObservaciones)
                                    .Where(c => c.EntidadContratante.Persona_Id == idEntidadContratante.Value).ToList<Contratos>();
            }

            List<Contratos> list3 = new List<Contratos>();
            if (numeroContrato != "")
            {
                ViewBag.NumeroContrato = numeroContrato;
                list3 = db.Contratos.Include(c => c.HistoriaObservaciones)
                                    .Where(c => c.NumeroContrato == numeroContrato).ToList<Contratos>();
            }

            List<Contratos> list4 = new List<Contratos>();
            if (idEstadoContrato.HasValue)
            {
                ViewBag.IdEstadoContrato = idEstadoContrato.Value;
                list4 = db.Contratos.Include(c => c.HistoriaObservaciones)
                                    .Where(c => c.TipoEstadoContrato_Id == idEstadoContrato).ToList<Contratos>();
            }

            List<Contratos> list = list1.Intersect(list2.Intersect(list3.Intersect(list4))).ToList<Contratos>();

            return list;
        }
    }
}