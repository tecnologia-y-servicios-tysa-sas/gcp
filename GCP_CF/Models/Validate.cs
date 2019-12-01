using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class Validate
    {
        [NotMapped]
        public string FechaAuxiliar { get; set; }

        public DateTime Fecha { get; set; }
    }
}