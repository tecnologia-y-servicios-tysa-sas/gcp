﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class TiposDocumentos
    {
        [Key]
        public int DocumentoId { get; set; }

        //[Index(IsUnique = true)]
        public string Descripcion { get; set; }

        public string Prefijo { get; set; }

        public virtual ICollection<TiposPersona> TiposPersona { get; set; }
    }
}