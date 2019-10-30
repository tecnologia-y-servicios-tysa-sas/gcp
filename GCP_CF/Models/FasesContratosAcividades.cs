using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class FasesContratosAcividades
    {

        [Key]
        public int FasesContratosAcividadesId { get; set; }

        public int fase_Id { get; set; }

        public int ActividadesEtapasId { get; set; }
        public bool Estado { get; set; }

        public virtual FasesContrato FasesContrato { get; set; }

        public virtual ActividadesEtapas ActividadesEtapas { get; set; }

    }
}