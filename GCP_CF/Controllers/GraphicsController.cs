using GCP_CF.Helpers;
using GCP_CF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace GCP_CF.Controllers
{
    public class GraphicsController : Controller
    {
        private readonly GCPContext db = new GCPContext();

        private const string ANIO = "AÑO";
        private const string TOTAL = "Total General";
        private const string VALOR_CONTRATOS = "VALOR CONTRATOS";
        private const string RECURSOS_ADMINISTRADOS = "RECURSOS ADMINISTRADOS";
        private const string HONORARIOS = "HONORARIOS";

        string tipoContratoMarco = System.Configuration.ConfigurationManager.AppSettings["tipoContratoMarco"];
        int tipoEntidadContratante = int.Parse(System.Configuration.ConfigurationManager.AppSettings["tipoEntidadContratante"]);

        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        // GET: Graphics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PresupuestoPorAnio_Old(FormCollection filterForm)
        {
            int anioActual = DateTime.Now.Year;

            var contratos = db.Contratos;
            double totalContratos = 0;
            double totalRecursos = 0;
            double totalHonorarios = 0;

            DataTable dt = new DataTable("DatosPresupuesto");
            dt.Columns.Add(ANIO, typeof(string));
            dt.Columns.Add(VALOR_CONTRATOS, typeof(double));
            dt.Columns.Add(RECURSOS_ADMINISTRADOS, typeof(double));
            dt.Columns.Add(HONORARIOS, typeof(double));

            for (int anio = anioActual - 2; anio <= anioActual; anio++)
            {
                var contratosAnio = contratos.Where(c => c.FechaInicio.Year >= anio && c.FechaTerminacion.Year <= anio);

                double valorContratos = contratosAnio
                    .Select(c => c.TipoContrato == null || c.TipoContrato.Termino != tipoContratoMarco ? c.ValorContrato : 0)
                    .DefaultIfEmpty(0).Sum();
                totalContratos += valorContratos;

                double valorRecursos = contratosAnio
                    .Select(c => c.TipoContrato == null || c.TipoContrato.Termino != tipoContratoMarco ? c.ValorAdministrar : 0)
                    .DefaultIfEmpty(0)
                    .Sum();
                totalRecursos += valorRecursos;
                
                double valorHonorarios = contratosAnio
                    .Select(c => c.TipoContrato == null || c.TipoContrato.Termino != tipoContratoMarco ? (c.Honorarios ?? 0) : 0)
                    .DefaultIfEmpty(0)
                    .Sum();
                totalHonorarios += valorHonorarios;

                DataRow dr = dt.NewRow();
                dr[ANIO] = anio.ToString();
                dr[VALOR_CONTRATOS] = valorContratos;
                dr[RECURSOS_ADMINISTRADOS] = valorRecursos;
                dr[HONORARIOS] = valorHonorarios;
                dt.Rows.Add(dr);
            }

            // Nueva fila para los totales
            DataRow drt = dt.NewRow();
            drt[ANIO] = TOTAL;
            drt[VALOR_CONTRATOS] = totalContratos;
            drt[RECURSOS_ADMINISTRADOS] = totalRecursos;
            drt[HONORARIOS] = totalHonorarios;
            dt.Rows.Add(drt);

            var dataView = new DataView(dt);
            var myChart = new Chart(width: 600, height: 400)
                .AddTitle("Presupuesto por Año")
                .AddLegend()
                .AddSeries(
                    chartType: "bar",
                    name: VALOR_CONTRATOS,
                    xValue: dataView, xField: ANIO,
                    yValues: dataView, yFields: VALOR_CONTRATOS)
                .AddSeries(
                    chartType: "bar",
                    name: RECURSOS_ADMINISTRADOS,
                    xValue: dataView, xField: ANIO,
                    yValues: dataView, yFields: RECURSOS_ADMINISTRADOS)
                .AddSeries(
                    chartType: "bar",
                    name: HONORARIOS,
                    xValue: dataView, xField: ANIO,
                    yValues: dataView, yFields: HONORARIOS)
                .Write();

            return null;
        }

        [HttpPost]
        public JsonResult PresupuestoPorAnio(int? anioSeleccionado, int? idEntidadContratante, string numeroContrato, int? idEstadoContrato)
        {
            int anioActual = DateTime.Now.Year;

            List<Contratos> contratos = db.Contratos.ToList();
            double totalContratos = 0;
            double totalRecursos = 0;
            double totalHonorarios = 0;

            ChartData chartData = new ChartData();
            chartData.Initialize();

            if (idEntidadContratante.HasValue && idEntidadContratante.Value != -1)
                contratos = contratos.Where(c => c.Persona_Id == idEntidadContratante).ToList();

            if (!string.IsNullOrEmpty(numeroContrato))
            {
                int idContrato = (from cm in db.Contratos
                                 where cm.NumeroContrato.ToUpper().Trim() == numeroContrato.ToUpper().Trim()
                                 select cm.Contrato_Id).FirstOrDefault();

                contratos = contratos.Where(c => (idContrato > 0 ? 
                            c.Contrato_Id == idContrato || (c.ContratoMarco_Id.HasValue && c.ContratoMarco_Id.Value == idContrato) : true))
                            .ToList();
            }

            if (idEstadoContrato.HasValue && idEstadoContrato.Value != -1)
                contratos = contratos.Where(c => c.TipoEstadoContrato_Id == idEstadoContrato).ToList();

            List<double> valoresContratos = new List<double>();
            List<double> valoresRecursos = new List<double>();
            List<double> valoresHonorarios = new List<double>();

            int limiteAnios = 0;
            int anioSuperior = anioActual;
            if (!anioSeleccionado.HasValue || anioSeleccionado.Value <= 0)
            {
                limiteAnios = 2;
            }
            else
            {
                if (anioActual > anioSeleccionado.Value)
                    limiteAnios = anioActual - anioSeleccionado.Value;
                else
                {
                    anioSuperior = anioSeleccionado.Value;
                    limiteAnios = 0;
                }
            }

            if (limiteAnios > 2) limiteAnios = 2;
            int anioInferior = anioActual - limiteAnios;

            for (int anio = anioInferior; anio <= anioSuperior; anio++)
            {
                var contratosAnio = contratos.Where(c => c.FechaInicio.Year >= anio && c.FechaTerminacion.Year <= anio);

                double valorContratos = contratosAnio
                    .Select(c => c.TipoContrato == null || c.TipoContrato.Termino != tipoContratoMarco ? c.ValorContrato : 0)
                    .DefaultIfEmpty(0).Sum();
                totalContratos += valorContratos;

                double valorRecursos = contratosAnio
                    .Select(c => c.TipoContrato == null || c.TipoContrato.Termino != tipoContratoMarco ? c.ValorAdministrar : 0)
                    .DefaultIfEmpty(0)
                    .Sum();
                totalRecursos += valorRecursos;

                double valorHonorarios = contratosAnio
                    .Select(c => c.TipoContrato == null || c.TipoContrato.Termino != tipoContratoMarco ? (c.Honorarios ?? 0) : 0)
                    .DefaultIfEmpty(0)
                    .Sum();
                totalHonorarios += valorHonorarios;

                chartData.labels.Add(anio.ToString());
                valoresContratos.Add(valorContratos);
                valoresRecursos.Add(valorRecursos);
                valoresHonorarios.Add(valorHonorarios);
            }

            chartData.labels.Add(TOTAL);
            valoresContratos.Add(totalContratos);
            valoresRecursos.Add(totalRecursos);
            valoresHonorarios.Add(totalHonorarios);

            // Datasets
            chartData.datasets.Add(new ChartDataSet { label = VALOR_CONTRATOS, data = valoresContratos });
            chartData.datasets.Add(new ChartDataSet { label = RECURSOS_ADMINISTRADOS, data = valoresRecursos });
            chartData.datasets.Add(new ChartDataSet { label = HONORARIOS, data = valoresHonorarios });

            return Json(JsonConvert.SerializeObject(chartData, _jsonSetting));
        }

        [HttpPost]
        public JsonResult PresupuestoPorMunicipio(FormCollection filterForm)
        {
            int anioActual = DateTime.Now.Year;

            List<Personas> municipios = db.Personas.Where(p => p.TipoPersona_Id == tipoEntidadContratante).ToList();

            ChartData chartData = new ChartData();
            chartData.Initialize();

            Dictionary<string, List<double>> valoresContratoMunicipio = new Dictionary<string, List<double>>();
            Dictionary<string, List<double>> valoresRecursosMunicipio = new Dictionary<string, List<double>>();

            // TODO: Agrupar por el municipio!!!
            for (int anio = anioActual - 2; anio <= anioActual; anio++)
            {
                foreach (Personas municipio in municipios)
                {
                    string labelActualContratos = municipio.NombreCompleto.ToUpper() + " - " + VALOR_CONTRATOS;
                    string labelActualRecursos = municipio.NombreCompleto.ToUpper() + " - " + RECURSOS_ADMINISTRADOS;
                    if (!valoresContratoMunicipio.ContainsKey(labelActualContratos)) valoresContratoMunicipio.Add(labelActualContratos, new List<double>());
                    if (!valoresRecursosMunicipio.ContainsKey(labelActualRecursos)) valoresRecursosMunicipio.Add(labelActualRecursos, new List<double>());

                    try
                    {
                        double valorContratos = db.Contratos.Where(c => c.Persona_Id == municipio.Persona_Id && c.FechaInicio.Year >= anio && c.FechaTerminacion.Year <= anio)
                            .Select(c => c.TipoContrato == null || c.TipoContrato.Termino != tipoContratoMarco ? c.ValorContrato : 0).DefaultIfEmpty(0).Sum();

                        double valorRecursos = db.Contratos.Where(c => c.Persona_Id == municipio.Persona_Id && c.FechaInicio.Year >= anio && c.FechaTerminacion.Year <= anio)
                            .Select(c => c.TipoContrato == null || c.TipoContrato.Termino != tipoContratoMarco ? c.ValorAdministrar : 0).DefaultIfEmpty(0).Sum();

                        valoresContratoMunicipio[labelActualContratos].Add(valorContratos);
                        valoresRecursosMunicipio[labelActualRecursos].Add(valorRecursos);
                    }
                    catch (Exception e)
                    {
                        chartData.datasets.Add(new ChartDataSet { label = "Error", data = new List<double>() });
                    }
                }

                chartData.labels.Add(anio.ToString());
            }

            // Datasets
            foreach (string lblItem in valoresContratoMunicipio.Select(v => v.Key))
            {
                valoresContratoMunicipio.TryGetValue(lblItem, out List<double> listaValoresContratos);
                chartData.datasets.Add(new ChartDataSet { label = lblItem, data = listaValoresContratos });
            }

            foreach (string lblItem in valoresRecursosMunicipio.Select(v => v.Key))
            {
                valoresRecursosMunicipio.TryGetValue(lblItem, out List<double> listaRecursosMunicipio);
                chartData.datasets.Add(new ChartDataSet { label = lblItem, data = listaRecursosMunicipio });
            }

            return Json(JsonConvert.SerializeObject(chartData, _jsonSetting));
        }

        [HttpPost]
        public JsonResult ValorContratadoDirectamentePorAnio(int? anioSeleccionado, int? idEntidadContratante, string numeroContrato, int? idEstadoContrato)
        {
            int anioActual = DateTime.Now.Year;

            List<Contratos> contratos = db.Contratos.ToList();
            double totalContratos = 0;

            ChartData chartData = new ChartData();
            chartData.Initialize();

            if (idEntidadContratante.HasValue && idEntidadContratante.Value != -1)
                contratos = contratos.Where(c => c.Persona_Id == idEntidadContratante).ToList();

            if (!string.IsNullOrEmpty(numeroContrato))
            {
                int idContrato = (from cm in db.Contratos
                                  where cm.NumeroContrato.ToUpper().Trim() == numeroContrato.ToUpper().Trim()
                                  select cm.Contrato_Id).FirstOrDefault();

                contratos = contratos.Where(c => (idContrato > 0 ?
                            c.Contrato_Id == idContrato || (c.ContratoMarco_Id.HasValue && c.ContratoMarco_Id.Value == idContrato) : true))
                            .ToList();
            }

            if (idEstadoContrato.HasValue && idEstadoContrato.Value != -1)
                contratos = contratos.Where(c => c.TipoEstadoContrato_Id == idEstadoContrato).ToList();

            List<double> valoresContratos = new List<double>();

            int limiteAnios = 0;
            int anioSuperior = anioActual;
            if (!anioSeleccionado.HasValue || anioSeleccionado.Value <= 0)
            {
                limiteAnios = 3;
            }
            else
            {
                if (anioActual > anioSeleccionado.Value)
                    limiteAnios = anioActual - anioSeleccionado.Value;
                else
                {
                    anioSuperior = anioSeleccionado.Value;
                    limiteAnios = 0;
                }
            }

            if (limiteAnios > 3) limiteAnios = 3;
            int anioInferior = anioActual - limiteAnios;

            for (int anio = anioInferior; anio <= anioSuperior; anio++)
            {
                var contratosAnio = contratos.Where(c => c.FechaInicio.Year >= anio && c.FechaTerminacion.Year <= anio);

                double valorContratos = contratosAnio
                    .Select(c => c.TipoContrato == null || c.TipoContrato.Termino != tipoContratoMarco ? c.ValorContrato : 0)
                    .DefaultIfEmpty(0).Sum();
                totalContratos += valorContratos;

                chartData.labels.Add(anio.ToString());
                valoresContratos.Add(valorContratos);
            }

            // Datasets
            chartData.datasets.Add(new ChartDataSet { label = VALOR_CONTRATOS, data = valoresContratos });

            return Json(JsonConvert.SerializeObject(chartData, _jsonSetting));
        }
    }
}