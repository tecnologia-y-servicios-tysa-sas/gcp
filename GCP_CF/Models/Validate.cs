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

        public DateTime FechaI { get; set; }

        public DateTime FechaT { get; set; }

        public string NumeroContrato { get; set; }
    }
}