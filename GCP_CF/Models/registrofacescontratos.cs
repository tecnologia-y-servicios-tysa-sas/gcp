using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCP_CF.Models
{
    public class Registrofacescontratos
    {
        [Key]
        public int Registrofacescontratos_id { get; set; }
        public int Fase_Id { get; set; }
        public int Contrato_Id { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Actividades> Actividades { get; set; }
        public virtual Contratos Contratos { get; set; }
        public virtual FasesContrato FasesContrato { get; set; }
                
    }
}