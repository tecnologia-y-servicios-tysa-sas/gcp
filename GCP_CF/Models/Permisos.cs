using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class Permisos
    {
        [Key]
        public int PermisoId { get; set; }

        [Required(ErrorMessage = "Debe Ingrsar la Descripción del Permiso ")]
        [Display(Name = "Descripción ")]
        [MaxLength(100, ErrorMessage = "El {0} no puede contener más de {1} caracteres.")]
        public string Descripción { get; set; }

        [Display(Name = "Detalle ")]
        [DataType(DataType.MultilineText)]
        [MaxLength(200, ErrorMessage = "El {0} no puede contener más de {1} caracteres.")]
        public string Detalle { get; set; }

        public bool Estado { get; set; }

        public virtual ICollection<PermisosRoles> PermisosRoles { get; set; }
    }
}