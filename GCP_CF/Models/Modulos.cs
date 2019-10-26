using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class Modulos
    {

        [Key]
        public int ModuloId { get; set; }

        [Required(ErrorMessage = "Debe Ingrsar la Descripción del Modulo")]
        [Display(Name = "Modulo")]
        [MaxLength(100, ErrorMessage = "El {0} no puede contener más de {1} caracteres.")]
        public string Descripción { get; set; }

        [Display(Name = "Detalle ")]
        [DataType(DataType.MultilineText)]
        [MaxLength(200, ErrorMessage = "El {0} no puede contener más de {1} caracteres.")]
        public string Detalle { get; set; }

        public bool Estado { get; set; }

        public virtual ICollection<Permisos> Permisos { get; set; }


    }
}