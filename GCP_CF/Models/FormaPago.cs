using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class FormaPago
    {
        [Key]
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public virtual ICollection<Contratos> Contratos { get; set; }

    }
}