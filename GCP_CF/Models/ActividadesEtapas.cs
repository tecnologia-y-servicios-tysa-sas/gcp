using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GCP_CF.Models
{
    public class ActividadesEtapas
    {
        [Key]
        public int ActividadesEtapasId { get; set; }

        [Required(ErrorMessage = "Debe Ingresar la Descripción de la Actividad")]
        [Display(Name = "Actividad Etapa")]
        [MaxLength(100, ErrorMessage = "El {0} no puede contener más de {1} caracteres.")]
        public string Descripción { get; set; }

        public virtual ICollection<FasesContratosAcividades> GetFasesContratosAcividades { get; set; }



    }
}