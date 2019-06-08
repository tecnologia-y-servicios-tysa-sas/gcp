using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCP_CF.Models
{
    public class registrofacescontratos
    {
        [Key]
        public int registrofacescontratos_id { get; set; }
        public int fase_Id { get; set; }
        public int Contrato_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<actividades> actividades { get; set; }
        public virtual Contratos Contratos { get; set; }
        public virtual FasesContrato FasesContrato { get; set; }
                
    }
}