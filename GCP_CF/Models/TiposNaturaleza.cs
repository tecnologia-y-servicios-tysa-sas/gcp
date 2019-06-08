using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCP_CF.Models
{
    public class TiposNaturaleza
    {
        [Key]
        public int Naturaleza_Id { get; set; }
        public string Descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TiposPersona> TiposPersona { get; set; }
    }
}