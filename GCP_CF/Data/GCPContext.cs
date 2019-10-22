//using MySql.Data.EntityFramework;
//using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
   //[DbConfigurationType(typeof(MySqlConfiguration))]
    public class GCPContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(100).IsRequired();
            //modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(200).IsRequired();
        }

        public DbSet<Actividades> Actividades { get; set; }

        public DbSet<ActividadesFases> ActividadesFases { get; set; }

        public DbSet<Contratos> Contratos { get; set; }

        //public DbSet<ContratosMarco> ContratosMarco { get; set; }

        public DbSet<EstadosActividad> EstadosActividad { get; set; }

        public DbSet<FasesContrato> FasesContrato { get; set; }

        public DbSet<Personas> Personas { get; set; }

        public DbSet<Registrofacescontratos> Registrofacescontratos { get; set; }

        public DbSet<TiposContratos> TiposContratos { get; set; }

        public DbSet<TiposEstadoContrato> TiposEstadoContrato { get; set; }

        public DbSet<TiposNaturaleza> TiposNaturaleza { get; set; }

        public DbSet<TiposPersona> TiposPersona { get; set; }

        public DbSet<HistoriaObservaciones> HistoriaObservaciones { get; set; }

        public DbSet<TiposDocumentos> TiposDocumentos { get; set; }

        public DbSet<FormaPago> FormaPagoes { get; set; }

        public DbSet<Facturas> Facturas { get; set; }

        public DbSet<EstadosFactura> EstadosFactura { get; set; }

        public DbSet<PagosContrato> PagosContrato { get; set; }

        public DbSet<Usuarios> Usuarios { get; set; }

   

        public System.Data.Entity.DbSet<GCP_CF.Models.UsuariosRoles> usuariosroles { get; set; }

        public System.Data.Entity.DbSet<GCP_CF.Models.Rol> Rols { get; set; }

        public System.Data.Entity.DbSet<GCP_CF.Models.Permisos> Permisos { get; set; }
        public System.Data.Entity.DbSet<GCP_CF.Models.PermisosRoles> PermisosRoles { get; set; }
    }
}
