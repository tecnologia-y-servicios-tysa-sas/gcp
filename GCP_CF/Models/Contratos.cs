﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [DataType(DataType.MultilineText)]
        [Display(Name = "Objeto Contractual")]
        public string ObjetoContractual { get; set; }

        public int Plazo { get; set; }

        [Display(Name = "Fecha de Terminacion")]
        public DateTime FechaTerminacion { get; set; }

        [Display(Name = "Seleccione Abogado")]
        public Nullable<int> PersonaAbogado_Id { get; set; }

        [Display(Name = "Seleccione Supervisor")]
        public Nullable<int> PersonaSuperviosr_Id { get; set; }

        [Display(Name = "CRP")]
        public int Crp { get; set; }

        [Display(Name = "Fecha CRP")]
        public DateTime FechaCrp { get; set; }

        [Display(Name = "CDP")]
        public int Cdp { get; set; }

        [Display(Name = "Fecha CDP")]
        public DateTime FechaCdp { get; set; }

        [Display(Name = "Fecha Acta Inicio")]
        public DateTime FechaActaInicio { get; set; }

        [Display(Name = "Seleccione Tipo de Contrato")]
        public Nullable<int> TipoEstadoContrato_Id { get; set; }

        [Display(Name = "Año")]
        public int Year { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Display(Name = "Valor Contrato")]
        public decimal ValorContrato { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Display(Name = "Valor Administrar")]
        public decimal ValorAdministrar { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
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

        //[ForeignKey("ContratoMarco_Id")]
        //public virtual ContratosMarco ContratosMarco{ get; set; }

        [ForeignKey("Persona_Id")]
        public virtual Personas EntidadContratante { get; set; }

        [ForeignKey("PersonaAbogado_Id")]
        public virtual Personas PersonaAbogado { get; set; }

        [ForeignKey("PersonaSuperviosr_Id")]
        public virtual Personas PersonaSupervisor { get; set; }

        [ForeignKey("PersonaSupervisorTecnico_Id")]
        public virtual Personas PersonaSupervisorTecnico { get; set; }

        
    }
}