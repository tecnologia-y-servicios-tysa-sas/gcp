using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace GCP_CF.Models
{
    /// <summary>
    /// Esta es la clase que gestiona a los Terceros de la aplicación, y es totalmente independiente de los usuarios del sistema.
    /// </summary>
    public class Personas
    {
        [Key]
        public int Persona_Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        [MaxLength(50, ErrorMessage = "Correo {0} no puede ser más largo de {1} caracteres.")]
        [RegularExpression(@"\b[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}\b", ErrorMessage = "Correo electrónico no es valido")]
        public string Correo { get; set; }

        [Display(Name = "Número de Identificación")]
        public string NumeroDocumento { get; set; }

        public Nullable<int> TipoPersona_Id { get; set; }

        public int? TipoDocumentoId { get; set; }

        [NotMapped]
        [Display(Name = "Nombre Completo")]
        public String NombreCompleto { get { return Nombres + ' ' + Apellidos; } }

        [Display(Name = "Recibir notificaciones")]
        public bool RecibirNotificaciones { get; set; }

        public virtual TiposPersona TiposPersona { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contratos> Contratos { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contratos> Contratos1 { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contratos> Contratos2 { get; set; }

        [ForeignKey("TipoDocumentoId")]
        public virtual TiposDocumentos TiposDocumentos { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Facturas> Facturas { get; set; }
    }
}