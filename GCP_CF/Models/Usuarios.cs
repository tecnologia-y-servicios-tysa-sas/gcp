using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using GCP_CF.Helpers;

namespace GCP_CF.Models
{
    public class Usuarios
    {
        [Key]
        public int Usuario_Id { get; set; }

        //[MaxLength(20, ErrorMessage = "El {0} ingresado no puede contener más de {1} caracteres.")]
        //[Required(ErrorMessage = "Debe ingresar el nombre de usuario")]
        //[Display(Name = "Usuario")]
        //public string Usuario { get; set; }

        [MaxLength(20, ErrorMessage = "La {0} ingresada no puede contener más de {1} caracteres.")]
        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [NotMapped]
        public string PasswordAux { get; set; }

        [MaxLength(30)]
        [Display(Name = "Roles")]
        public string IdRoles { get; set; }

        [Display(Name = "Permisos")]
        public string TipoPermisos { get; set; }

        [Display(Name = "Todos los contratos")]
        public bool TodosLosContratos { get; set; }

        [Column(TypeName = "longtext")]
        public string IdContratos { get; set; }

        [NotMapped]
        public List<int> ListadoIdContratos
        {
            get {
                if (!string.IsNullOrEmpty(IdContratos))
                    return IdContratos.Split(',').Select(int.Parse).ToList();
                else
                    return new List<int>();
            }

            set {
                if (!TodosLosContratos)
                    IdContratos = String.Join(",", value.Select(p => p.ToString()));
                else
                    IdContratos = "-1"; // Todos
            }
        }

        [NotMapped]
        public string Rol
        {
            get { return RolHelper.ObtenerRolUsuario(RolId); }
        }

        [NotMapped]
        public bool EsSuperUsuario
        {
            get { return RolHelper.EsSuperUsuario(IdRoles); }
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
        [Index(IsUnique = true)]
        public string CorreoElectronico { get; set; }

        [Display(Name = "Activar Usuario")]
        public bool EsActivo { get; set; }

        [NotMapped]
        public virtual string EstadoPermisos {
            get {
                string nombreRol = RolHelper.ObtenerRolUsuario(RolId).ToUpper();
                string lecturaEscritura = EsSuperUsuario ? "tiene poder" : (TipoPermisos == "W") ? "puede escribir" : "puede leer";
                string contratos = EsSuperUsuario ? "sobre todo" : (TodosLosContratos) ? "sobre todos los contratos" : "sobre algunos contratos";
                return nombreRol + ", " + lecturaEscritura + " " + contratos;
            }
        }


        [Display(Name = "Rol")]
        public int? RolId { get; set; }

        [ForeignKey("RolId")]
        public virtual Rol Roles { get; set; }

    }
}