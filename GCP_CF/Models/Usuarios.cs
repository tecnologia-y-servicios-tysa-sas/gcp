using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GCP_CF.Helpers;

namespace GCP_CF.Models
{
    public class Usuarios
    {
        [Key]
        public int Usuario_Id { get; set; }

        [MaxLength(20, ErrorMessage = "El {0} ingresado no puede contener más de {1} caracteres.")]
        [Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [MaxLength(20, ErrorMessage = "La {0} ingresada no puede contener más de {1} caracteres.")]
        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [MaxLength(30)]
        [Display(Name = "Roles")]
        public string IdRoles { get; set; }

        [NotMapped]
        public List<KeyValuePair<int, string>> ListadoRoles
        {
            get { return RolHelper.ObtenerRolesUsuario(IdRoles); }
        }

        [MaxLength(30, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres.")]
        [Required(ErrorMessage = "Debe ingresar el nombre del usuario")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }

        [MaxLength(30, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres.")]
        [Required(ErrorMessage = "Debe ingresar los apellidos del usuario")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [NotMapped]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get { return Nombres + " " + Apellidos; } }

        [MaxLength(50, ErrorMessage = "El {0} no puede contener más de {1} caracteres.")]
        [Required(ErrorMessage = "Debe ingresar el correo electrónico del usuario")]
        [Display(Name = "Correo Electrónico")]
        [RegularExpression(@"\b[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b", ErrorMessage = "Correo electrónico no es valido")]
        public string CorreoElectronico { get; set; }

        [Display(Name = "Activar Usuario")]
        public bool EsActivo { get; set; }
    }
}