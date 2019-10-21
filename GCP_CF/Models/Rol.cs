using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class Rol
    {

        [Key]
        public int RolId { get; set; }

        [Required(ErrorMessage = "Debe la Descripción del Rol ")]
        [Display(Name = "Descripción ")]
        [MaxLength(100, ErrorMessage = "El {0} no puede contener más de {1} caracteres.")]
        public string Descripción { get; set; }
    }
}