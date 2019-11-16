using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCP_CF.Models
{
    public class PagoContrato
    {
        [Key]
        public int PagosContrato_Id { get; set; }

        public int Contrato_Id { get; set; }

        [Display(Name = "No. Pago")]
        public int NumeroPago { get; set; }

        [Display(Name = "Valor")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public double Valor { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Notas")]
        public string Notas { get; set; }

        public Nullable<int> Factura_Id { get; set; }

        [ForeignKey("Contrato_Id")]
        public virtual Contratos Contrato { get; set; }

        [ForeignKey("Factura_Id")]
        public virtual Factura Factura { get; set; }
    }
}