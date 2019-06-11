using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCP_CF.Models
{
    public class TiposPersona
    {
        [Key]
        public int TipoPersona_Id { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> Naturaleza_Id { get; set; }
        public string Cargo { get; set; }

        public virtual TiposNaturaleza TiposNaturaleza { get; set; } 
        public virtual ICollection<Personas> Personas { get; set; }
    }
}