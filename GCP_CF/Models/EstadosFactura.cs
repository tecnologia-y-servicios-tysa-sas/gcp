using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCP_CF.Models
{
    public class EstadosFactura
    {
        [Key]
        [Display(Name = "ID")]
        public int EstadoFactura_Id { get; set; }

        [Display(Name = "Descripción")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar la descripción del estado")]
        [MaxLength(255)]
        public string Descripcion { get; set; }

        [Display(Name = "Término")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar el término para reconocer este tipo")]
        [MaxLength(50)]
        public string Termino { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Facturas> Facturas { get; set; }

        public virtual ICollection<Factura> Factura { get; set; }
    }
}