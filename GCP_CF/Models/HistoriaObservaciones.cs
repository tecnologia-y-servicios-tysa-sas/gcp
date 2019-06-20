
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GCP_CF.Models
{
    public class HistoriaObservaciones
    {
        [Key]

        public int ObservacionId { get; set; }

        [StringLength(500)]
        public string Observaciones { get; set; }

        //[StringLength(128)]
        //[Display(Name = "Usuario Crea")]
        //public string UsuarioCrea { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? Fecha { get; set; }

        public int? ContratoId { get; set; }


        [ForeignKey("ContratoId")]
        public virtual Contratos Contratos { get; set; }

    }
}