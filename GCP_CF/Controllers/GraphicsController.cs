using GCP_CF.Authorization;
using GCP_CF.Helpers;
using GCP_CF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace GCP_CF.Controllers
{
    [GCPAuthorize(Roles = RolHelper.TODOS)]
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

        [HttpPost]
        public JsonResult PresupuestoPorAnio(int? anioSeleccionado, int? idEntidadContratante, string numeroContrato, int? idEstadoContrato, int? idTipoContrato)
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

            if (idTipoContrato.HasValue && idTipoContrato.Value != -1)
            {
                if (idTipoContrato == 3)
                {
                    contratos = contratos.Where(c => c.TipoContrato_Id == idTipoContrato).ToList();
                    List<Contratos> contratosTemp = new List<Contratos>();
                    foreach (var contrato in contratos)
                    {
                        contratosTemp.AddRange(db.Contratos.Where(c => c.ContratoMarco_Id == contrato.Contrato_Id).ToList());
                    }
                    contratos = contratosTemp;
                }
                else
                {
                    contratos = contratos.Where(c => c.TipoContrato_Id == idTipoContrato).ToList();
                }
            }

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
                    .Select(c => c.TipoContrato != null ? c.ValorContrato : 0)
                    .DefaultIfEmpty(0).Sum();
                totalContratos += valorContratos;

                double valorRecursos = contratosAnio
                    .Select(c => c.TipoContrato != null ? c.ValorAdministrar : 0)
                    .DefaultIfEmpty(0)
                    .Sum();
                totalRecursos += valorRecursos;

                double valorHonorarios = contratosAnio
                    .Select(c => c.TipoContrato != null ? (c.Honorarios ?? 0) : 0)
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

        /*[HttpPost]
        public JsonResult PresupuestoPorMunicipio(FormCollection filterForm)
        {
            int anioActual = DateTime.Now.Year;

            List<Personas> municipios = db.Personas.Where(p => p.TipoPersona_Id == tipoEntidadContratante).ToList();

            ChartData chartData = new ChartData();
            chartData.Initialize();

            Dictionary<string, List<double>> valoresContratoMunicipio = new Dictionary<string, List<double>>();
            Dictionary<string, List<double>> valoresRecursosMunicipio = new Dictionary<string, List<double>>();

            // TODO: Agrupar por el municipio!!
            foreach (Personas municipio in municipios)
            {
                double valorContratos = 0;
                double valorRecursos = 0;
                string labelActualContratos = string.Empty;
                string labelActualRecursos = string.Empty;
                for (int anio = anioActual - 2; anio <= anioActual; anio++)
                {
                    labelActualContratos = municipio.NombreCompleto.ToUpper() + " - " + VALOR_CONTRATOS;
                    labelActualRecursos = municipio.NombreCompleto.ToUpper() + " - " + RECURSOS_ADMINISTRADOS;
                    if (!valoresContratoMunicipio.ContainsKey(labelActualContratos)) valoresContratoMunicipio.Add(labelActualContratos, new List<double>());
                    if (!valoresRecursosMunicipio.ContainsKey(labelActualRecursos)) valoresRecursosMunicipio.Add(labelActualRecursos, new List<double>());

                    try
                    {
                        valorContratos = db.Contratos.Where(c => c.Persona_Id == municipio.Persona_Id && c.FechaInicio.Year >= anio && c.FechaTerminacion.Year <= anio)
                            .Select(c => c.TipoContrato != null && c.TipoContrato.Termino != tipoContratoMarco ? c.ValorContrato : 0).DefaultIfEmpty(0).Sum();

                        valorRecursos = db.Contratos.Where(c => c.Persona_Id == municipio.Persona_Id && c.FechaInicio.Year >= anio && c.FechaTerminacion.Year <= anio)
                            .Select(c => c.TipoContrato != null && c.TipoContrato.Termino != tipoContratoMarco ? c.ValorAdministrar : 0).DefaultIfEmpty(0).Sum();
                    }
                    catch (Exception)
                    {
                        chartData.datasets.Add(new ChartDataSet { label = "Error", data = new List<double>() });
                    }                    
                }
                valoresContratoMunicipio[labelActualContratos].Add(valorContratos);
                valoresRecursosMunicipio[labelActualRecursos].Add(valorRecursos);
                chartData.labels.Add(municipio.NombreCompleto);
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
        }*/

        public JsonResult PresupuestoPorMunicipio(int? anioSeleccionado, int? idEntidadContratante, string numeroContrato, int? idEstadoContrato, int? idTipoContrato)
        {
            int anioActual = DateTime.Now.Year;

            List<Contratos> contratos = db.Contratos.ToList();

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

            if (idTipoContrato.HasValue && idTipoContrato.Value != -1)
            {
                if (idTipoContrato == 3)
                {
                    contratos = contratos.Where(c => c.TipoContrato_Id == idTipoContrato).ToList();
                    List<Contratos> contratosTemp = new List<Contratos>();
                    foreach (var contrato in contratos)
                    {
                        contratosTemp.AddRange(db.Contratos.Where(c => c.ContratoMarco_Id == contrato.Contrato_Id).ToList());
                    }
                    contratos = contratosTemp;
                }
                else
                {
                    contratos = contratos.Where(c => c.TipoContrato_Id == idTipoContrato).ToList();
                }
            }

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

            List<Personas> municipios = new List<Personas>();

            foreach (var contrato in contratos.Select(c=> c.Persona_Id).Distinct().ToList())
            {
                municipios.Add(db.Personas.Where(p => p.TipoPersona_Id == tipoEntidadContratante && p.Persona_Id == contrato).FirstOrDefault());
            }
            //List<Personas> municipios = db.Personas.Where(p => p.TipoPersona_Id == tipoEntidadContratante).ToList();            

            Dictionary<string, List<double>> valoresContratoMunicipio = new Dictionary<string, List<double>>();
            Dictionary<string, List<double>> valoresRecursosMunicipio = new Dictionary<string, List<double>>();
            List<double> valoresContratos = new List<double>();
            List<double> valoresResursos = new List<double>();
            List<double> valoresParticipacion = new List<double>();
            // TODO: Agrupar por el municipio!!
            foreach (Personas municipio in municipios)
            {
                double valorContratos = 0;
                double valorRecursos = 0;
                double valorParticipaciones = 0;
                string labelActualContratos = string.Empty;
                string labelActualRecursos = string.Empty;
                labelActualContratos = VALOR_CONTRATOS;
                labelActualRecursos = RECURSOS_ADMINISTRADOS;
                for (int anio = anioInferior; anio <= anioSuperior; anio++)
                {
                    try
                    {
                        double valorContrato = contratos.Where(c => c.Persona_Id == municipio.Persona_Id && c.FechaInicio.Year == anio && c.FechaTerminacion.Year == anio)
                            .Select(c => c.TipoContrato != null ? c.ValorContrato : 0).DefaultIfEmpty(0).Sum();
                        valorContratos += valorContrato;

                        double valorRecurso = contratos.Where(c => c.Persona_Id == municipio.Persona_Id && c.FechaInicio.Year == anio && c.FechaTerminacion.Year == anio)
                            .Select(c => c.TipoContrato != null ? c.ValorAdministrar : 0).DefaultIfEmpty(0).Sum();
                        valorRecursos += valorRecurso;

                        double valorParticipacion = contratos.Where(c => c.Persona_Id == municipio.Persona_Id && c.FechaInicio.Year == anio && c.FechaTerminacion.Year == anio)
                            .Select(c => c.TipoContrato != null ? c.ValorAdministrar : 0).DefaultIfEmpty(0).Sum();
                        valorParticipaciones += valorParticipacion;
                    }
                    catch (Exception)
                    {
                        chartData.datasets.Add(new ChartDataSet { label = "Error", data = new List<double>() });
                    }
                }
                valoresContratos.Add(valorContratos);
                valoresResursos.Add(valorRecursos);
                valoresParticipacion.Add(valorParticipaciones);
                chartData.labels.Add(municipio.NombreCompleto);
            }

            for (int i = 0; i < valoresParticipacion.Count; i++)
            {
                valoresParticipacion[i] = valoresParticipacion[i] / valoresContratos.Sum();
            }

            chartData.datasets.Add(new ChartDataSet { label = "VALOR_CONTRATOS", data = valoresContratos });
            chartData.datasets.Add(new ChartDataSet { label = "RECURSOS_ADMINISTRADOS", data = valoresResursos });

            // Datasets
            //foreach (string lblItem in valoresContratoMunicipio.Select(v => v.Key))
            //{
            //    valoresContratoMunicipio.TryGetValue(lblItem, out List<double> listaValoresContratos);
            //    chartData.datasets.Add(new ChartDataSet { label = lblItem, data = listaValoresContratos });
            //}

            //foreach (string lblItem in valoresRecursosMunicipio.Select(v => v.Key))
            //{
            //    valoresRecursosMunicipio.TryGetValue(lblItem, out List<double> listaRecursosMunicipio);
            //    chartData.datasets.Add(new ChartDataSet { label = lblItem, data = listaRecursosMunicipio });
            //}

            return Json(JsonConvert.SerializeObject(chartData, _jsonSetting));
        }

        [HttpPost]
        public JsonResult ValorContratadoDirectamentePorAnio(int? anioSeleccionado, int? idEntidadContratante, string numeroContrato, int? idEstadoContrato, int? idTipoContrato)
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

            if (idTipoContrato.HasValue && idTipoContrato.Value != -1)
            {
                if (idTipoContrato == 3)
                {
                    contratos = contratos.Where(c => c.TipoContrato_Id == idTipoContrato).ToList();
                    List<Contratos> contratosTemp = new List<Contratos>();
                    foreach (var contrato in contratos)
                    {
                        contratosTemp.AddRange(db.Contratos.Where(c => c.ContratoMarco_Id == contrato.Contrato_Id).ToList());
                    }
                    contratos = contratosTemp;
                }
                else
                {
                    contratos = contratos.Where(c => c.TipoContrato_Id == idTipoContrato).ToList();
                }
            }

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
                    .Select(c => c.TipoContrato != null ? c.ValorContrato : 0)
                    .DefaultIfEmpty(0).Sum();
                totalContratos += valorContratos;

                chartData.labels.Add(anio.ToString());
                valoresContratos.Add(valorContratos);
            }

            // Datasets
            chartData.datasets.Add(new ChartDataSet { label = VALOR_CONTRATOS, data = valoresContratos });

            return Json(JsonConvert.SerializeObject(chartData, _jsonSetting));
        }

        [HttpPost]
        public JsonResult ValorCIADS(int? anioSeleccionado, int? idEntidadContratante, string numeroContrato, int? idEstadoContrato, int? idTipoContrato)
        {
            int anioActual = DateTime.Now.Year;

            List<Contratos> contratos = db.Contratos.ToList();
            List<PagoContrato> pagoContratos = db.PagoContrato.ToList();
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

            if (idTipoContrato.HasValue && idTipoContrato.Value != -1)
            {
                if (idTipoContrato == 3)
                {
                    contratos = contratos.Where(c => c.TipoContrato_Id == idTipoContrato).ToList();
                    List<Contratos> contratosTemp = new List<Contratos>();
                    foreach (var contrato in contratos)
                    {
                        contratosTemp.AddRange(db.Contratos.Where(c => c.ContratoMarco_Id == contrato.Contrato_Id).ToList());
                    }
                    contratos = contratosTemp;
                }
                else
                {
                    contratos = contratos.Where(c => c.TipoContrato_Id == idTipoContrato).ToList();
                }
            }
                

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

            double valorContratos = 0;
            double valorCDP = 0;
            double valorCRP = 0;
            double valorPagos = 0;

            for (int anio = anioInferior; anio <= anioSuperior; anio++)
            {
                var contratosAnio = contratos.Where(c => c.FechaInicio.Year >= anio && c.FechaTerminacion.Year <= anio);

                valorContratos = contratosAnio
                    .Select(c => c.TipoContrato != null ? c.ValorContrato : 0)
                    .DefaultIfEmpty(0).Sum();
                totalContratos += valorContratos;

                valorCDP = contratosAnio
                    .Select(c => c.TipoContrato != null ? !c.ValorCDP.HasValue ? 0 : c.ValorCDP.Value : 0)
                    .DefaultIfEmpty(0).Sum();
                valorCDP += valorCDP;

                valorCRP = contratosAnio
                    .Select(c => c.TipoContrato != null ? !c.ValorCRP.HasValue ? 0 : c.ValorCRP.Value : 0)
                    .DefaultIfEmpty(0).Sum();
                valorCRP += valorCRP;                

                foreach (var contrato in contratosAnio.Where(c => c.TipoContrato != null).ToList())
                {
                    valorPagos = pagoContratos.Where(c => c.Fecha.Year >= anio && c.Contrato_Id == contrato.Contrato_Id).Select(c => c.Valor).DefaultIfEmpty(0).Sum();
                    valorPagos += valorPagos;
                }
                
            }

            valoresContratos.Add(totalContratos);
            valoresContratos.Add(valorCDP);
            valoresContratos.Add(valorCRP);
            valoresContratos.Add(valorPagos);

            if (anioSeleccionado < 0)
            {
                chartData.labels.Add("PRESUPUESTO");
            }
            else
            {
                chartData.labels.Add("PRESUPUESTO "+ anioSeleccionado.ToString());
            }
            chartData.labels.Add("CDP");
            chartData.labels.Add("CRP");
            chartData.labels.Add("PAGOS");

            // Datasets
            chartData.datasets.Add(new ChartDataSet { label = "VALOR", data = valoresContratos });

            return Json(JsonConvert.SerializeObject(chartData, _jsonSetting));
        }
    }
}