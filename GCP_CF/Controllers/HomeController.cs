using GCP_CF.Authorization;
using GCP_CF.Helpers;
using GCP_CF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GCP_CF.Controllers
{
    public class HomeController : Controller
    {
        private readonly GCPContext db = new GCPContext();

        [GCPAuthorize(Roles = RolHelper.TODOS)]
        public ActionResult Index(FormCollection filterForm)
        {
            return View(ListarContratos(filterForm));
        }

        public List<Contratos> ListarContratos(FormCollection filterForm)
        {
            int anio = !string.IsNullOrEmpty(filterForm["Anio"]) ? int.Parse(filterForm["Anio"]) : 0;
            int idEntidadContratante = !string.IsNullOrEmpty(filterForm["IdEntidadContratante"]) ? int.Parse(filterForm["IdEntidadContratante"]) : 0;
            string numeroContrato = filterForm["NumeroContrato"];
            int idEstadoContrato = !string.IsNullOrEmpty(filterForm["IdEstadoContrato"]) ? int.Parse(filterForm["IdEstadoContrato"]) : 3;
            int idTipoContrato = !string.IsNullOrEmpty(filterForm["IdTipoContrato"]) ? int.Parse(filterForm["IdTipoContrato"]) : 3;

            ViewBag.IdEstadoContrato = new SelectList(db.TiposEstadoContrato.OrderBy(x => x.Descripcion), "TiposEstadoContrato_Id", "Descripcion", idEstadoContrato);
            var tipoContratos = new List<TiposContratos>();
            tipoContratos.Add(new TiposContratos { TipoContrato_Id = 0, Descripcion = "TODOS" });
            tipoContratos.AddRange(db.TiposContratos.OrderBy(x => x.Descripcion).ToList());
            ViewBag.IdTipoContrato = new SelectList(tipoContratos, "TipoContrato_Id", "Descripcion", idTipoContrato);
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

            // Guardar valores seleccionados para los gráficos
            ViewBag.AnioSeleccionado = anio;
            ViewBag.IdEntidadSeleccionada = idEntidadContratante;
            ViewBag.NumeroContratoEnviado = numeroContrato;
            ViewBag.IdEstadoSeleccionado = idEstadoContrato;
            ViewBag.IdTipoSeleccionado = idTipoContrato;

            List<Contratos> list = ObtenerContratos(anio, idEntidadContratante, numeroContrato, idEstadoContrato, idTipoContrato);
            return list != null ? list.OrderBy(x => x.Contrato_Id).ToList() : new List<Contratos>();
        }        

        private List<Contratos> ObtenerContratos(int anio, int idEntidadContratante, string numeroContrato, int idEstadoContrato, int idTipoContrato)
        {
            DateTime? fechaInicio = anio > 0 ? new DateTime(anio, 1, 1) : new DateTime?();
            DateTime? fechaFin = anio > 0 ? new DateTime(anio, 12, 31) : new DateTime?();

            bool hayFechas = fechaInicio.HasValue && fechaFin.HasValue;
            bool hayEntidadContratante = idEntidadContratante > 0;
            bool hayNumeroContrato = !string.IsNullOrEmpty(numeroContrato);
            bool hayEstadoContrato = idEstadoContrato > 0;
            bool hayTipoContrato = idTipoContrato > 0;

            var contratos = (from c in db.Contratos select c).Include(c => c.HistoriaObservaciones).ToList();

            if (hayFechas)
            {
                contratos = contratos.Where(c => (fechaInicio.Value <= c.FechaInicio && c.FechaInicio <= fechaFin.Value) ||
                                                 (fechaInicio.Value <= c.FechaTerminacion && c.FechaInicio <= fechaFin.Value) ||
                                                 (fechaInicio.Value <= c.FechaActaInicio && c.FechaInicio <= fechaFin.Value) ||
                                                 (fechaInicio.Value <= c.FechaCdp && c.FechaInicio <= fechaFin.Value) ||
                                                 (fechaInicio.Value <= c.FechaCrp && c.FechaInicio <= fechaFin.Value)).ToList();
            }

            if (hayEntidadContratante)
                contratos = contratos.Where(c => c.EntidadContratante.Persona_Id == idEntidadContratante).ToList();

            if (hayNumeroContrato)
            {
                int idContratoMarco = (from cm in db.Contratos
                                       where cm.NumeroContrato.ToUpper().Trim() == numeroContrato.ToUpper().Trim()
                                       select cm.Contrato_Id).FirstOrDefault();

                contratos = contratos.Where(c => (idContratoMarco > 0 ? c.Contrato_Id == idContratoMarco || c.ContratoMarco_Id.Value == idContratoMarco : true))
                                     .OrderByDescending(c => c.ContratoMarco_Id)
                                     .OrderByDescending(c => c.NumeroContrato).ToList();
            }

            if (hayEstadoContrato)
                contratos = contratos.Where(c => c.TipoEstadoContrato_Id == idEstadoContrato).ToList();

            if (hayTipoContrato)
            {
                if (idTipoContrato == 3)
                {
                    contratos = contratos.Where(c => c.TipoContrato_Id == idTipoContrato).ToList();
                    List<Contratos> contratosTemp = new List<Contratos>();
                    //foreach (var contrato in contratos)
                    //{
                    //    contratosTemp.AddRange(db.Contratos.Where(c => c.ContratoMarco_Id == contrato.Contrato_Id).ToList());
                    //}
                    contratos = contratosTemp.ToList();
                }
                else
                {
                    contratos = contratos.Where(c => c.TipoContrato_Id == idTipoContrato).ToList();
                }
            }

            if (contratos != null)
            {
                var estados = db.TiposEstadoContrato.ToList();
                foreach (Contratos item in contratos)
                {
                    foreach (var estado in estados)
                    {
                        if (item.TipoEstadoContrato_Id == estado.TiposEstadoContrato_Id)
                        {
                            item.Estado = estado.Descripcion;
                        }
                    }
                    //item.PorcentajeValorEjecutado = Math.Round (item.Ejecucion ?? 0 / item.ValorAdministrar, 2 );
                }

                return contratos.ToList<Contratos>();
            }

            return null;
        }

        [GCPAuthorize(Roles = RolHelper.TODOS)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [GCPAuthorize(Roles = RolHelper.TODOS)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}