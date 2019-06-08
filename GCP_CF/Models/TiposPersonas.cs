﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCP_CF.Models
{
    public class TiposPersonas
    {
        [Key]
        public int TipoPersona_Id { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> Naturaleza_Id { get; set; }
        public string Cargo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Personas> Personas { get; set; }
        public virtual TiposNaturalezas TiposNaturalezas { get; set; }
    }
}