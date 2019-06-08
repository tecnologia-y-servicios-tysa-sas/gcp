using System;
using System.ComponentModel.DataAnnotations;

namespace GCP_CF.Models
{
    public class Contratos
    {
        [Key]
        public int Contrato_Id { get; set; }

        [Display(Name = "Numero de Contrato")]
        public string NumeroContrato { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Seleccione Contrato")]
        public Nullable<int> TipoContrato_Id { get; set; }

        [Display(Name = "Entidad Contratante")]
        public int Persona_Id { get; set; }

        [Display(Name = "Objeto Contractual")]
        public string ObjetoContractual { get; set; }

        public int Plazo { get; set; }

        [Display(Name = "Fecha de Terminacion")]
        public DateTime FechaTerminacion { get; set; }

        [Display(Name = "Seleccione Abogado")]
        public Nullable<int> PersonaAbogado_Id { get; set; }

        [Display(Name = "Seleccione Supervisor")]
        public Nullable<int> PersonaSuperviosr_Id { get; set; }

        [Display(Name = "Seleccione CRP")]
        public int Crp { get; set; }

        [Display(Name = "Seleccione CDP")]
        public int Cdp { get; set; }

        [Display(Name = "Fecha Acta Inicio")]
        public DateTime FechaActaInicio { get; set; }

        [Display(Name = "Seleccione Tipo de Contrato")]
        public Nullable<int> TipoEstadoContrato_Id { get; set; }

        [Display(Name = "Año")]
        public int Year { get; set; }

        [Display(Name = "Valor Contrato")]
        public decimal ValorContrato { get; set; }

        [Display(Name = "Valor Administrar")]
        public decimal ValorAdministrar { get; set; }

        public decimal Honorarios { get; set; }

        public int Ejecucion { get; set; }

        [Display(Name = "Procentaje Facturado")]
        public decimal PorcentajeFacturado { get; set; }

        [Display(Name = "Porcentaje Facturado Honorarios")]
        public decimal PorcentajeFacturadoHonorarios { get; set; }

        [Display(Name = "Seleccione Supervisor Técnico")]
        public Nullable<int> PersonaSupervisorTecnico_Id { get; set; }

        [Display(Name = "Seleccione Contrato Marco")]
        public Nullable<int> ContratoMarco_Id { get; set; }
    }
}