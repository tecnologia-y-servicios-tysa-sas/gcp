using System;
using System.Collections.Generic;
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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha de Terminación")]
        public DateTime FechaTerminacion { get; set; }

        [Display(Name = "Seleccione Apoyo Jurídico")]
        public Nullable<int> PersonaAbogado_Id { get; set; }

        [Display(Name = "Seleccione Apoyo Técnico")]
        public Nullable<int> PersonaSuperviosr_Id { get; set; }

        [Display(Name = "CRP")]
        public Nullable<int>  Crp { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha CRP")]
        public DateTime? FechaCrp { get; set; }

        [Display(Name = "CDP")]
        public Nullable<int>  Cdp { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha CDP")]
        public DateTime? FechaCdp { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha de Firma de Contrato")]
        public DateTime FechaFirmaContrato { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha de Firma de Acta")]
        public DateTime FechaActaInicio { get; set; }

        [Display(Name = "Seleccione Tipo de Contrato")]
        public Nullable<int> TipoEstadoContrato_Id { get; set; }

        [Display(Name = "Año")]
        public Nullable<int> Year { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Valor Contrato")]
        public double ValorContrato { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        [Display(Name = "Recursos a Administrar")]
        public double ValorAdministrar { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<double> Honorarios { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        [Display(Name = "Porcentaje IVA Honorarios")]
        public Nullable<double> PorcentajeIvaHonorarios { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Valor CRP")]
        public double? ValorCRP { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Valor CDP")]
        public double? ValorCDP { get; set; }

        [NotMapped]
        [Display(Name = "Valor Neto Honorarios")]
        public double ValorNetoHonorarios
        {
            get {
                double valorHonorarios = Honorarios ?? 0;
                double ivaHonorarios = PorcentajeIvaHonorarios ?? 0;
                return valorHonorarios - Math.Round(valorHonorarios * ivaHonorarios / 100, 2);
            }
            set { }
        }

        [NotMapped]
        public string ValorContratoAux { get; set; }

        [NotMapped]
        public string ValorAdministrarAux { get; set; }

        [NotMapped]
        public string HonorariosAux { get; set; }

        [NotMapped]
        public string ValorCdpAux { get; set; }

        [NotMapped]
        public string ValorCrpAux { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<double> Ejecucion { get; set; }

        [Display(Name = "Procentaje Facturado")]
        public decimal PorcentajeFacturado { get; set; }

        [Display(Name = "Porcentaje Facturado Honorarios")]
        public decimal PorcentajeFacturadoHonorarios { get; set; }

        [Display(Name = "Seleccione Supervisor Técnico")]
        public Nullable<int> PersonaSupervisorTecnico_Id { get; set; }

        [Display(Name = "Seleccione Contrato Interadministrativo")]
        public Nullable<int> ContratoMarco_Id { get; set; }

        [NotMapped]
        public string Estado { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string Observaciones { get; set; }

        public Nullable<int> FormaPagoId { get; set; }

        [Display(Name = "Número de Póliza")]
        public string NumeroPoliza { get; set; }

        [NotMapped]
        public string ValorPolizaAux { get; set; }

        [Display(Name = "Valor")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public double? ValorPoliza { get; set; }

        [Display(Name = "Aseguradora")]
        public string NombreAseguradora { get; set; }

        [Display(Name = "Notas")]
        [DataType(DataType.MultilineText)]
        public string NotasPoliza { get; set; }

        /* CAMPOS VIRTUALES - SOLO VISUALIZACIÓN */

        [ForeignKey("FormaPagoId")]
        public virtual FormaPago FormaPago { get; set; }

        [ForeignKey("Persona_Id")]
        public virtual Personas EntidadContratante { get; set; }

        [ForeignKey("PersonaAbogado_Id")]
        public virtual Personas PersonaAbogado { get; set; }

        [ForeignKey("PersonaSuperviosr_Id")]
        public virtual Personas PersonaSupervisor { get; set; }

        [ForeignKey("PersonaSupervisorTecnico_Id")]
        public virtual Personas PersonaSupervisorTecnico { get; set; }

        [ForeignKey("TipoContrato_Id")]
        public virtual TiposContratos TipoContrato { get; set; }

        public virtual ICollection<HistoriaObservaciones> HistoriaObservaciones { get; set; }

        public virtual double PorcentajeValorEjecutado 
        {
            get {
                //double valorHonorarios = Honorarios ?? 0;
                double valorEjecutado = Ejecucion ?? 0;
                return Math.Round(valorEjecutado / ValorAdministrar, 2);
            }
        }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Facturas> Facturas { get; set; }

        public virtual ICollection<PagosContrato> PagosContrato { get; set; }

        public virtual ICollection<Notificaciones> Notificaciones { get; set; }

        public virtual ICollection<Factura> Factura { get; set; }

        public virtual ICollection<PagoContrato> PagoContrato { get; set; }

    }
}