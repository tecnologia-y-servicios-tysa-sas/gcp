namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Camposd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.actividades",
                c => new
                    {
                        Actividad_Id = c.Int(nullable: false, identity: true),
                        registrofacescontratos_id = c.Int(nullable: false),
                        FaseContrato_Id = c.Int(nullable: false),
                        Item = c.String(),
                        Descripción = c.String(),
                        DiasHabiles = c.Int(nullable: false),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFinal = c.DateTime(nullable: false),
                        EstadoActividad_Id = c.Int(nullable: false),
                        Observaciones = c.String(),
                    })
                .PrimaryKey(t => t.Actividad_Id)
                .ForeignKey("dbo.registrofacescontratos", t => t.registrofacescontratos_id, cascadeDelete: true)
                .ForeignKey("dbo.EstadosActividads", t => t.EstadoActividad_Id, cascadeDelete: true)
                .Index(t => t.registrofacescontratos_id)
                .Index(t => t.EstadoActividad_Id);
            
            CreateTable(
                "dbo.EstadosActividads",
                c => new
                    {
                        EstadoActividad_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.EstadoActividad_Id);
            
            CreateTable(
                "dbo.ActividadesFases",
                c => new
                    {
                        Actividad_Id = c.Int(nullable: false, identity: true),
                        Item = c.String(),
                        Descripción = c.String(),
                        DiasHabiles = c.Int(),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFinal = c.DateTime(nullable: false),
                        EstadoActividad_Id = c.Int(),
                        Contratos_Contrato_Id = c.Int(),
                        FasesContrato_fase_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Actividad_Id)
                .ForeignKey("dbo.Contratos", t => t.Contratos_Contrato_Id)
                .ForeignKey("dbo.EstadosActividads", t => t.EstadoActividad_Id)
                .ForeignKey("dbo.FasesContratoes", t => t.FasesContrato_fase_Id)
                .Index(t => t.EstadoActividad_Id)
                .Index(t => t.Contratos_Contrato_Id)
                .Index(t => t.FasesContrato_fase_Id);
            
            CreateTable(
                "dbo.Contratos",
                c => new
                    {
                        Contrato_Id = c.Int(nullable: false, identity: true),
                        NumeroContrato = c.String(),
                        FechaInicio = c.DateTime(nullable: false),
                        TipoContrato_Id = c.Int(),
                        Persona_Id = c.Int(nullable: false),
                        ObjetoContractual = c.String(),
                        Plazo = c.Int(nullable: false),
                        FechaTerminacion = c.DateTime(nullable: false),
                        PersonaAbogado_Id = c.Int(),
                        PersonaSuperviosr_Id = c.Int(),
                        Crp = c.Int(nullable: false),
                        Cdp = c.Int(nullable: false),
                        FechaActaInicio = c.DateTime(nullable: false),
                        TipoEstadoContrato_Id = c.Int(),
                        Year = c.Int(nullable: false),
                        ValorContrato = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorAdministrar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Honorarios = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ejecucion = c.Int(nullable: false),
                        PorcentajeFacturado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PorcentajeFacturadoHonorarios = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PersonaSupervisorTecnico_Id = c.Int(),
                        ContratoMarco_Id = c.Int(),
                        Personas_Persona_Id = c.Int(),
                        Personas_Persona_Id1 = c.Int(),
                        Personas_Persona_Id2 = c.Int(),
                        TiposEstadoContrato_TiposEstadoContrato_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Contrato_Id)
                .ForeignKey("dbo.ContratosMarcoes", t => t.ContratoMarco_Id)
                .ForeignKey("dbo.Personas", t => t.Personas_Persona_Id)
                .ForeignKey("dbo.Personas", t => t.Personas_Persona_Id1)
                .ForeignKey("dbo.Personas", t => t.Personas_Persona_Id2)
                .ForeignKey("dbo.TiposContratos", t => t.TipoContrato_Id)
                .ForeignKey("dbo.TiposEstadoContratoes", t => t.TiposEstadoContrato_TiposEstadoContrato_Id)
                .Index(t => t.TipoContrato_Id)
                .Index(t => t.ContratoMarco_Id)
                .Index(t => t.Personas_Persona_Id)
                .Index(t => t.Personas_Persona_Id1)
                .Index(t => t.Personas_Persona_Id2)
                .Index(t => t.TiposEstadoContrato_TiposEstadoContrato_Id);
            
            CreateTable(
                "dbo.FasesContratoes",
                c => new
                    {
                        fase_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.fase_Id);
            
            CreateTable(
                "dbo.registrofacescontratos",
                c => new
                    {
                        registrofacescontratos_id = c.Int(nullable: false, identity: true),
                        fase_Id = c.Int(nullable: false),
                        Contrato_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.registrofacescontratos_id)
                .ForeignKey("dbo.Contratos", t => t.Contrato_Id, cascadeDelete: true)
                .ForeignKey("dbo.FasesContratoes", t => t.fase_Id, cascadeDelete: true)
                .Index(t => t.fase_Id)
                .Index(t => t.Contrato_Id);
            
            CreateTable(
                "dbo.ContratosMarcoes",
                c => new
                    {
                        ContratoMarco_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.ContratoMarco_Id);
            
            CreateTable(
                "dbo.Personas",
                c => new
                    {
                        Persona_Id = c.Int(nullable: false, identity: true),
                        Nombres = c.String(),
                        Apellidos = c.String(),
                        Direccion = c.String(),
                        Telefono = c.String(),
                        Correoo = c.String(),
                        TipoPersona_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Persona_Id)
                .ForeignKey("dbo.TiposPersonas", t => t.TipoPersona_Id)
                .Index(t => t.TipoPersona_Id);
            
            CreateTable(
                "dbo.TiposPersonas",
                c => new
                    {
                        TipoPersona_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                        Naturaleza_Id = c.Int(),
                        Cargo = c.String(),
                    })
                .PrimaryKey(t => t.TipoPersona_Id)
                .ForeignKey("dbo.TiposNaturalezas", t => t.Naturaleza_Id)
                .Index(t => t.Naturaleza_Id);
            
            CreateTable(
                "dbo.TiposNaturalezas",
                c => new
                    {
                        Naturaleza_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Naturaleza_Id);
            
            CreateTable(
                "dbo.TiposContratos",
                c => new
                    {
                        TipoContrato_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.TipoContrato_Id);
            
            CreateTable(
                "dbo.TiposEstadoContratoes",
                c => new
                    {
                        TiposEstadoContrato_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.TiposEstadoContrato_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contratos", "TiposEstadoContrato_TiposEstadoContrato_Id", "dbo.TiposEstadoContratoes");
            DropForeignKey("dbo.Contratos", "TipoContrato_Id", "dbo.TiposContratos");
            DropForeignKey("dbo.TiposPersonas", "Naturaleza_Id", "dbo.TiposNaturalezas");
            DropForeignKey("dbo.Personas", "TipoPersona_Id", "dbo.TiposPersonas");
            DropForeignKey("dbo.Contratos", "Personas_Persona_Id2", "dbo.Personas");
            DropForeignKey("dbo.Contratos", "Personas_Persona_Id1", "dbo.Personas");
            DropForeignKey("dbo.Contratos", "Personas_Persona_Id", "dbo.Personas");
            DropForeignKey("dbo.Contratos", "ContratoMarco_Id", "dbo.ContratosMarcoes");
            DropForeignKey("dbo.actividades", "EstadoActividad_Id", "dbo.EstadosActividads");
            DropForeignKey("dbo.ActividadesFases", "FasesContrato_fase_Id", "dbo.FasesContratoes");
            DropForeignKey("dbo.registrofacescontratos", "fase_Id", "dbo.FasesContratoes");
            DropForeignKey("dbo.registrofacescontratos", "Contrato_Id", "dbo.Contratos");
            DropForeignKey("dbo.actividades", "registrofacescontratos_id", "dbo.registrofacescontratos");
            DropForeignKey("dbo.ActividadesFases", "EstadoActividad_Id", "dbo.EstadosActividads");
            DropForeignKey("dbo.ActividadesFases", "Contratos_Contrato_Id", "dbo.Contratos");
            DropIndex("dbo.TiposPersonas", new[] { "Naturaleza_Id" });
            DropIndex("dbo.Personas", new[] { "TipoPersona_Id" });
            DropIndex("dbo.registrofacescontratos", new[] { "Contrato_Id" });
            DropIndex("dbo.registrofacescontratos", new[] { "fase_Id" });
            DropIndex("dbo.Contratos", new[] { "TiposEstadoContrato_TiposEstadoContrato_Id" });
            DropIndex("dbo.Contratos", new[] { "Personas_Persona_Id2" });
            DropIndex("dbo.Contratos", new[] { "Personas_Persona_Id1" });
            DropIndex("dbo.Contratos", new[] { "Personas_Persona_Id" });
            DropIndex("dbo.Contratos", new[] { "ContratoMarco_Id" });
            DropIndex("dbo.Contratos", new[] { "TipoContrato_Id" });
            DropIndex("dbo.ActividadesFases", new[] { "FasesContrato_fase_Id" });
            DropIndex("dbo.ActividadesFases", new[] { "Contratos_Contrato_Id" });
            DropIndex("dbo.ActividadesFases", new[] { "EstadoActividad_Id" });
            DropIndex("dbo.actividades", new[] { "EstadoActividad_Id" });
            DropIndex("dbo.actividades", new[] { "registrofacescontratos_id" });
            DropTable("dbo.TiposEstadoContratoes");
            DropTable("dbo.TiposContratos");
            DropTable("dbo.TiposNaturalezas");
            DropTable("dbo.TiposPersonas");
            DropTable("dbo.Personas");
            DropTable("dbo.ContratosMarcoes");
            DropTable("dbo.registrofacescontratos");
            DropTable("dbo.FasesContratoes");
            DropTable("dbo.Contratos");
            DropTable("dbo.ActividadesFases");
            DropTable("dbo.EstadosActividads");
            DropTable("dbo.actividades");
        }
    }
}
