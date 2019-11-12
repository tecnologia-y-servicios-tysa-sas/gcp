using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class Notificaciones
    {
        [Key]
        public int Id { get; set; }

        public int ContractId { get; set; }

        public int PersonId { get; set; }

        public virtual Contratos Contratos { get; set; }
    }
}