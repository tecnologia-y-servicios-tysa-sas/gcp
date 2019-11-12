using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace GCP_CF.Models
{
    public class Facturas
    {
        [Key]
        public int Factura_Id { get; set; }

        [Display(Name = "Año")]
        [Required(ErrorMessage = "Debe seleccionar el año")]
        public int Anio { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Debe seleccionar el estado")]
        public int Estado_Id { get; set; }

        [ForeignKey("Estado_Id")]
        public virtual EstadosFactura Estado { get; set; }

        [Display(Name = "Número")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar el número de la factura")]
        public string Numero { get; set; }

        [Display(Name = "Mes")]
        [Required(ErrorMessage = "Debe seleccionar el mes al que corresponde la factura")]
        public int Mes { get; set; }

        public virtual string NombreMes
        {
            get
            {
                DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
                return CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(dtinfo.GetMonthName(Mes));
            }
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha de Pago")]
        [Required(ErrorMessage = "Debe especificar la fecha del pago de la factura")]
        public DateTime FechaPago { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha de Vencimiento")]
        [Required(ErrorMessage = "Debe especificar la fecha de vencimiento de la factura")]
        public DateTime FechaVencimiento { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha de Entrega")]
        [Required(ErrorMessage = "Debe especificar la fecha de entrega de la factura")]
        public DateTime FechaEntrega { get; set; }


        [Display(Name = "Municipio o Entidad")]
        [Required(ErrorMessage = "Debe seleccionar el Municipio o Entidad")]
        public int Municipio_Id { get; set; }

        [ForeignKey("Municipio_Id")]
        public virtual Personas Municipio { get; set; }

        [Display(Name = "Concepto")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debe ingresar el concepto por el cual se crea la factura")]
        public string Concepto { get; set; }

        [Display(Name = "Contrato")]
        [Required(ErrorMessage = "Debe especificar el contrato al cual está asociada la factura")]
        public int Contrato_Id { get; set; }

        [ForeignKey("Contrato_Id")]
        public virtual Contratos Contrato { get; set; }

        [Display(Name = "Objeto")]
        [DataType(DataType.MultilineText)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe especificar el objeto de la factura")]
        public string Objeto { get; set; }

        [Display(Name = "Valor Base")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public virtual double ValorBase { get; set; }

        [Display(Name = "% IVA")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe especificar el porcentaje de IVA")]
        public Nullable<double> PorcentajeIva { get; set; }

        [Display(Name = "Valor IVA")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public Nullable<double> ValorIva { get; set; }

        [Display(Name = "Honorarios")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        [Required(ErrorMessage = "Debe especificar el valor de honorarios")]
        public Nullable<double> TotalHonorarios { get; set; }

        [Display(Name = "Valor Cancelado")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public double ValorCancelado { get; set; }

        [Display(Name = "Observaciones")]
        [DataType(DataType.MultilineText)]
        public string Observaciones { get; set; }

        public List<PagosContrato> PagosContrato { get; set; }

        [NotMapped]
        public string TotalHonorariosAux { get; set; }

        [NotMapped]
        public string ValorBaseAux { get; set; }

        [NotMapped]
        public string ValorIvaAux { get; set; }

        [NotMapped]
        public string ValorCanceladoAux { get; set; }

        [NotMapped]
        public string NumeroContrato { get; set; }
    }
}