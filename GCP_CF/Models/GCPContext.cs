using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class GCPContext : DbContext
    {
        public GCPContext()
           : base("DefaultConnection")
        {

        }

        public System.Data.Entity.DbSet<GCP_CF.Models.Contratos> Contratos { get; set; }
        public System.Data.Entity.DbSet<GCP_CF.Models.ContratosMarco> ContratosMarcoes { get; set; }
        public System.Data.Entity.DbSet<GCP_CF.Models.TiposContratos> TiposContratos { get; set; }
        public System.Data.Entity.DbSet<GCP_CF.Models.TiposEstadoContrato> TiposEstadoContratoes { get; set; }


        public System.Data.Entity.DbSet<GCP_CF.Models.Personas> Personas { get; set; }
        public System.Data.Entity.DbSet<GCP_CF.Models.TiposPersonas> TiposPersonas { get; set; }
        public System.Data.Entity.DbSet<GCP_CF.Models.TiposNaturalezas> TiposNaturalezas { get; set; }

        public System.Data.Entity.DbSet<GCP_CF.Models.EstadosActividad> EstadosActividads { get; set; }

        public System.Data.Entity.DbSet<GCP_CF.Models.FasesContrato> FasesContratoes { get; set; }

        public System.Data.Entity.DbSet<GCP_CF.Models.ActividadesFases> ActividadesFases { get; set; }

        public System.Data.Entity.DbSet<GCP_CF.Models.actividades> actividades { get; set; }

        public System.Data.Entity.DbSet<GCP_CF.Models.registrofacescontratos> registrofacescontratos { get; set; }
    }
}