using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ClosedXML.Excel;
using GCP_CF.Models;
using MySql.Data.MySqlClient;

namespace GCP_CF.Controllers
{
    public class ReportesController : Controller
    {
        private readonly GCPContext db = new GCPContext();

        public ActionResult Contratos(FormCollection filterForm)
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

        public ActionResult Facturas(FormCollection filterForm)
        {
            return View(ListarFacturas(filterForm));
        }

        public string NombreMes(int mes)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(dtinfo.GetMonthName(mes));
        }

        private List<Factura> ListarFacturas(FormCollection filterForm)
        {
            int idEntidad = !string.IsNullOrEmpty(filterForm["IdMunicipio"]) ? int.Parse(filterForm["IdMunicipio"]) : 0;
            int mes = !string.IsNullOrEmpty(filterForm["Mes"]) ? int.Parse(filterForm["Mes"]) : 0;
            int anio = !string.IsNullOrEmpty(filterForm["Anio"]) ? int.Parse(filterForm["Anio"]) : 0;

            ViewBag.IdMunicipio = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto", idEntidad);
            var mesesFacturas = db.Factura.Select(f => f.Mes).Distinct();

            List<object> listadoMeses = new List<object>();
            foreach (var m in mesesFacturas)
                listadoMeses.Add(new { id = m, nombre = NombreMes(m) });

            ViewBag.Mes = new SelectList(listadoMeses, "id", "nombre", mes);

            List<int> aniosFacturas = db.Factura.Select(f => f.Anio).Distinct().ToList<int>();
            ViewBag.Anio = new SelectList(aniosFacturas.Where(x => x > 0).OrderByDescending(x => x), anio);

            List<Factura> list = ObtenerFacturas(idEntidad, mes, anio);
            return list != null ? list.OrderBy(x => x.Factura_Id).ToList() : new List<Factura>();
        }

        private List<Contratos> ObtenerContratos(int? anio, int? idEntidadContratante, string numeroContrato, int idEstadoContrato, int idTipoContrato)
        {
            var Lectura = db.Database.SqlQuery<Lectura>("call getValoresContrato (@yearId, @EntidadId, @numContrato, @estadoContratoId, @tipoContratoId)",
                   new MySqlParameter("@yearId", anio == 0 ? null : anio),
                   new MySqlParameter("@EntidadId", idEntidadContratante == 0 ? null : idEntidadContratante),
                   new MySqlParameter("@numContrato", numeroContrato == string.Empty ? null : numeroContrato),
                   new MySqlParameter("@estadoContratoId", idEstadoContrato),
                   new MySqlParameter("@tipoContratoId", 3)).ToList();

            var contratos = new List<Contratos>();

            if (Lectura != null)
            {
                var estados = db.TiposEstadoContrato.ToList();
                foreach (Lectura item in Lectura)
                {
                    var contrato = db.Contratos.Where(c => c.Contrato_Id == item.contrato_Id).FirstOrDefault();
                    if (contrato != null)
                    {
                        contrato.ValorAdministrar = item.Valor_Administrar;
                        contrato.ValorContrato = item.Valor_Contrato;
                        contrato.ValorCDP = item.Valor_CDP;
                        contrato.ValorCRP = item.Valor_CRP;
                        contrato.ValorNetoHonorarios = item.Valor_Honorarios;
                        contrato.Ejecucion = item.Valor_Ejecutado * 100;
                        foreach (var estado in estados)
                        {
                            if (item.TipoEstadoContrato_Id == estado.TiposEstadoContrato_Id)
                            {
                                contrato.Estado = estado.Descripcion;
                            }
                        }
                        if (contrato != null)
                        {
                            contratos.Add(contrato);
                        }
                    }
                    //item.PorcentajeValorEjecutado = Math.Round (item.Ejecucion ?? 0 / item.ValorAdministrar, 2 );
                }

                return contratos.ToList<Contratos>();
            }

            return null;
        }

        private List<Factura> ObtenerFacturas(int idEntidad, int mes, int anio)
        {
            bool hayMes = mes > 0;
            bool hayAnio = anio > 0;
            bool hayEntidad = idEntidad > 0;

            var facturas = (from f in db.Factura select f).Include(f => f.Contrato);
            
            if (hayEntidad)
                facturas = facturas.Where(f => f.Municipio_Id == idEntidad);

            if (hayMes)
                facturas = facturas.Where(c => c.Mes == mes);

            if (hayAnio)
                facturas = facturas.Where(c => c.Anio == anio);

            if (facturas != null)
                return facturas.ToList<Factura>();

            return null;
        }

        [HttpPost]
        public FileResult ExportarReporteContratos(FormCollection filterForm)
        {
            List<Contratos> contratos = ListarContratos(filterForm);

            DataTable dt = new DataTable("Contratos");
            dt.Columns.AddRange(new DataColumn[21]
            {
                new DataColumn("Tipo"),
                new DataColumn("Número"),
                new DataColumn("Valor Contrato"),
                new DataColumn("Valor Administrar"),
                new DataColumn("% Ejecutado"),
                new DataColumn("Valor Ejecutado"),
                new DataColumn("Honorarios"),
                new DataColumn("Entidad Contratante"),
                new DataColumn("Objeto Contractual"),
                new DataColumn("Fecha Inicio"),
                new DataColumn("Fecha Fin"),
                new DataColumn("Plazo"),
                new DataColumn("Estado"),
                new DataColumn("CRP"),
                new DataColumn("Fecha CRP"),
                new DataColumn("CDP"),
                new DataColumn("Fecha CDP"),
                new DataColumn("Fecha Acta Inicio"),
                new DataColumn("Abogado"),
                new DataColumn("Supervisor"),
                new DataColumn("Observaciones")
            });

            foreach (Contratos c in contratos)
            {
                dt.Rows.Add(c.TipoContrato != null ? c.TipoContrato.Termino : "N/A", 
                            c.NumeroContrato, c.ValorContrato, c.ValorAdministrar, c.PorcentajeValorEjecutado + "%", 
                            c.Ejecucion.HasValue ? c.Ejecucion.Value : 0, c.Honorarios,
                            c.EntidadContratante != null ? c.EntidadContratante.NombreCompleto : "N/A", 
                            c.ObjetoContractual, c.FechaInicio, c.FechaTerminacion, c.Plazo, 
                            c.Estado != null ? c.Estado : c.TipoEstadoContrato_Id.GetValueOrDefault().ToString(), 
                            c.Crp, c.FechaCrp, c.Cdp, c.FechaCdp, c.FechaActaInicio, 
                            c.PersonaAbogado != null ? c.PersonaAbogado.NombreCompleto: "N/A", 
                            c.PersonaSupervisor != null ? c.PersonaSupervisor.NombreCompleto : "N/A", 
                            c.Observaciones);
            }

            return ExportarAExcel("Contratos", dt);
        }

        [HttpPost]
        public FileResult ExportarReporteFacturas(FormCollection filterForm)
        {
            List<Factura> facturas = ListarFacturas(filterForm);

            DataTable dt = new DataTable("Facturas");
            dt.Columns.AddRange(new DataColumn[15]
            {
                new DataColumn("Año"),
                new DataColumn("Estado"),
                new DataColumn("Número"),
                new DataColumn("Mes"),
                new DataColumn("Fecha de Pago"),
                new DataColumn("Municipio"),
                new DataColumn("Conceptos"),
                new DataColumn("Contrato"),
                new DataColumn("Objeto"),
                new DataColumn("Base"),
                new DataColumn("%"),
                new DataColumn("IVA"),
                new DataColumn("Total Honorarios"),
                new DataColumn("Vr. Cancelado"),
                new DataColumn("Observaciones")
            });

            foreach (Factura f in facturas)
            {
                dt.Rows.Add(f.Anio, f.Estado.Termino, f.Numero, f.NombreMes, f.FechaPago.ToShortDateString(), 
                    f.Municipio.NombreCompleto, f.Concepto, f.Contrato.NumeroContrato, f.Objeto, 
                    f.ValorBase, f.PorcentajeIva, f.ValorIva, f.TotalHonorarios, f.ValorCancelado, f.Observaciones);
            }

            return ExportarAExcel("Facturas", dt);
        }

        public FileResult ExportarAExcel(string nombre, DataTable dt)
        {
            string nombreArchivo = DateTime.Now.ToString("yyyyMMddHHmmss") + "_Reporte" + nombre + ".xlsx";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                }
            }
        }
    }
}