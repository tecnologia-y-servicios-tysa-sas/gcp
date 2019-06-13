using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class RelacionContratoFase
    {
        public int id_RealacionContratoFase { get; set; }
        public int Contrato_Id { get; set; }
        public int Fase_Id { get; set; }
    }
}