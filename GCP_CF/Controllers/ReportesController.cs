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

        public ActionResult Contratos(FormCollection filterForm)
        {
            int anio = !string.IsNullOrEmpty(filterForm["Anio"]) ? int.Parse(filterForm["Anio"]) : 0;
            int idEntidadContratante = !string.IsNullOrEmpty(filterForm["IdEntidadContratante"]) ? int.Parse(filterForm["IdEntidadContratante"]) : 0;
            string numeroContrato = filterForm["NumeroContrato"];
            int idEstadoContrato = !string.IsNullOrEmpty(filterForm["IdEstadoContrato"]) ? int.Parse(filterForm["IdEstadoContrato"]) : 0;

            ViewBag.IdEstadoContrato = new SelectList(db.TiposEstadoContrato.OrderBy(x => x.Descripcion), "TiposEstadoContrato_Id", "Descripcion", idEstadoContrato);
            ViewBag.IdEntidadContratante = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto", idEntidadContratante);
            ViewBag.NumeroContrato = numeroContrato;

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

            ViewBag.Anio = new SelectList(aniosContratos.Distinct().Where(x => x > 0).OrderByDescending(x => x), anio);

            List<Contratos> list = ObtenerContratos(anio, idEntidadContratante, numeroContrato, idEstadoContrato);
            return View(list != null ? list.OrderBy(x => x.Contrato_Id).ToList() : new List<Contratos>());
        }

        public ActionResult Facturas()
        {
            return View();
        }

        private List<Contratos> ObtenerContratos(int anio, int idEntidadContratante, string numeroContrato, int idEstadoContrato)
        {
            DateTime? fechaInicio = anio > 0 ? new DateTime(anio, 1, 1) : new DateTime?();
            DateTime? fechaFin = anio > 0 ? new DateTime(anio, 12, 31) : new DateTime?();

            bool hayFechas = fechaInicio.HasValue && fechaFin.HasValue;
            bool hayEntidadContratante = idEntidadContratante > 0;
            bool hayNumeroContrato = !string.IsNullOrEmpty(numeroContrato);
            bool hayEstadoContrato = idEstadoContrato > 0;

            List<Contratos> list = null;

            if (hayFechas || hayEntidadContratante || hayNumeroContrato || hayEstadoContrato)
            {
                list = (from c in db.Contratos
                         where (
                             (
                                 hayFechas ?
                                     (fechaInicio.Value <= c.FechaInicio && c.FechaInicio <= fechaFin.Value) ||
                                     (fechaInicio.Value <= c.FechaTerminacion && c.FechaInicio <= fechaFin.Value) ||
                                     (fechaInicio.Value <= c.FechaActaInicio && c.FechaInicio <= fechaFin.Value) ||
                                     (fechaInicio.Value <= c.FechaCdp && c.FechaInicio <= fechaFin.Value) ||
                                     (fechaInicio.Value <= c.FechaCrp && c.FechaInicio <= fechaFin.Value)
                                 : true
                             )
                             && (hayEntidadContratante ? c.EntidadContratante.Persona_Id == idEntidadContratante : true)
                             && (hayNumeroContrato ? c.NumeroContrato.Contains(numeroContrato) : true)
                             && (hayEstadoContrato ? c.TipoEstadoContrato_Id == idEstadoContrato : true)
                        )
                        select c).Include(c => c.HistoriaObservaciones).ToList<Contratos>();
            }

            if (list != null)
            {
                var estados = db.TiposEstadoContrato.ToList();
                foreach (Contratos item in list)
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

            return list;
        }
    }
}