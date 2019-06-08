namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actividades",
                c => new
                    {
                        Actividad_Id = c.Int(nullable: false, identity: true),
                        Registrofacescontratos_id = c.Int(nullable: false),
                        Item = c.String(unicode: false),
                        Descripcion = c.String(unicode: false),
                        DiasHabiles = c.Int(nullable: false),
                        FechaInicio = c.DateTime(nullable: false, precision: 0),
                        FechaFinal = c.DateTime(nullable: false, precision: 0),
                        EstadoActividad_Id = c.Int(nullable: false),
                        Observaciones = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Actividad_Id)
                .ForeignKey("dbo.Registrofacescontratos", t => t.Registrofacescontratos_id, cascadeDelete: true)
                .ForeignKey("dbo.EstadosActividad", t => t.EstadoActividad_Id, cascadeDelete: true)
                .Index(t => t.Registrofacescontratos_id)
                .Index(t => t.EstadoActividad_Id);
            
            CreateTable(
                "dbo.EstadosActividad",
                c => new
                    {
                        EstadoActividad_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.EstadoActividad_Id);
            
            CreateTable(
                "dbo.ActividadesFases",
                c => new
                    {
                        Actividad_Id = c.Int(nullable: false, identity: true),
                        Item = c.String(unicode: false),
                        Descripción = c.String(unicode: false),
                        DiasHabiles = c.Int(),
                        FechaInicio = c.DateTime(nullable: false, precision: 0),
                        FechaFinal = c.DateTime(nullable: false, precision: 0),
                        EstadoActividad_Id = c.Int(),
                        Contratos_Contrato_Id1 = c.Int(nullable: false),
                        FasesContrato_fase_Id1 = c.Int(nullable: false),
                        Contratos_Contrato_Id = c.Int(),
                        FasesContrato_fase_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Actividad_Id)
                .ForeignKey("dbo.Contratos", t => t.Contratos_Contrato_Id)
                .ForeignKey("dbo.EstadosActividad", t => t.EstadoActividad_Id)
                .ForeignKey("dbo.FasesContrato", t => t.FasesContrato_fase_Id)
                .Index(t => t.EstadoActividad_Id)
                .Index(t => t.Contratos_Contrato_Id)
                .Index(t => t.FasesContrato_fase_Id);
            
            CreateTable(
                "dbo.Contratos",
                c => new
                    {
                        Contrato_Id = c.Int(nullable: false, identity: true),
                        NumeroContrato = c.String(unicode: false),
                        FechaInicio = c.DateTime(nullable: false, precision: 0),
                        TipoContrato_Id = c.Int(),
                        Persona_Id = c.Int(nullable: false),
                        ObjetoContractual = c.String(unicode: false),
                        Plazo = c.Int(nullable: false),
                        FechaTerminacion = c.DateTime(nullable: false, precision: 0),
                        PersonaAbogado_Id = c.Int(),
                        PersonaSuperviosr_Id = c.Int(),
                        Crp = c.Int(nullable: false),
                        FechaCrp = c.DateTime(nullable: false, precision: 0),
                        Cdp = c.Int(nullable: false),
                        FechaCdp = c.DateTime(nullable: false, precision: 0),
                        FechaActaInicio = c.DateTime(nullable: false, precision: 0),
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
                .ForeignKey("dbo.Personas", t => t.Personas_Persona_Id)
                .ForeignKey("dbo.Personas", t => t.Personas_Persona_Id1)
                .ForeignKey("dbo.Personas", t => t.Personas_Persona_Id2)
                .ForeignKey("dbo.Personas", t => t.Persona_Id, cascadeDelete: true)
                .ForeignKey("dbo.Personas", t => t.PersonaAbogado_Id)
                .ForeignKey("dbo.Personas", t => t.PersonaSuperviosr_Id)
                .ForeignKey("dbo.Personas", t => t.PersonaSupervisorTecnico_Id)
                .ForeignKey("dbo.ContratosMarco", t => t.ContratoMarco_Id)
                .ForeignKey("dbo.TiposContratos", t => t.TipoContrato_Id)
                .ForeignKey("dbo.TiposEstadoContrato", t => t.TiposEstadoContrato_TiposEstadoContrato_Id)
                .Index(t => t.TipoContrato_Id)
                .Index(t => t.Persona_Id)
                .Index(t => t.PersonaAbogado_Id)
                .Index(t => t.PersonaSuperviosr_Id)
                .Index(t => t.PersonaSupervisorTecnico_Id)
                .Index(t => t.ContratoMarco_Id)
                .Index(t => t.Personas_Persona_Id)
                .Index(t => t.Personas_Persona_Id1)
                .Index(t => t.Personas_Persona_Id2)
                .Index(t => t.TiposEstadoContrato_TiposEstadoContrato_Id);
            
            CreateTable(
                "dbo.Personas",
                c => new
                    {
                        Persona_Id = c.Int(nullable: false, identity: true),
                        Nombres = c.String(unicode: false),
                        Apellidos = c.String(unicode: false),
                        Direccion = c.String(unicode: false),
                        Telefono = c.String(unicode: false),
                        Correo = c.String(unicode: false),
                        TipoPersona_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Persona_Id)
                .ForeignKey("dbo.TiposPersona", t => t.TipoPersona_Id)
                .Index(t => t.TipoPersona_Id);
            
            CreateTable(
                "dbo.TiposPersona",
                c => new
                    {
                        TipoPersona_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(unicode: false),
                        Naturaleza_Id = c.Int(),
                        Cargo = c.String(unicode: false),
                        TiposNaturaleza_Naturaleza_Id = c.Int(),
                        TiposNaturaleza_Naturaleza_Id1 = c.Int(),
                        TiposNaturalezas_Naturaleza_Id = c.Int(),
                    })
                .PrimaryKey(t => t.TipoPersona_Id)
                .ForeignKey("dbo.TiposNaturaleza", t => t.TiposNaturaleza_Naturaleza_Id)
                .ForeignKey("dbo.TiposNaturaleza", t => t.TiposNaturaleza_Naturaleza_Id1)
                .ForeignKey("dbo.TiposNaturaleza", t => t.TiposNaturalezas_Naturaleza_Id)
                .Index(t => t.TiposNaturaleza_Naturaleza_Id)
                .Index(t => t.TiposNaturaleza_Naturaleza_Id1)
                .Index(t => t.TiposNaturalezas_Naturaleza_Id);
            
            CreateTable(
                "dbo.TiposNaturaleza",
                c => new
                    {
                        Naturaleza_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Naturaleza_Id);
            
            CreateTable(
                "dbo.FasesContrato",
                c => new
                    {
                        fase_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.fase_Id);
            
            CreateTable(
                "dbo.Registrofacescontratos",
                c => new
                    {
                        Registrofacescontratos_id = c.Int(nullable: false, identity: true),
                        Fase_Id = c.Int(nullable: false),
                        Contrato_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Registrofacescontratos_id)
                .ForeignKey("dbo.Contratos", t => t.Contrato_Id, cascadeDelete: true)
                .ForeignKey("dbo.FasesContrato", t => t.Fase_Id, cascadeDelete: true)
                .Index(t => t.Fase_Id)
                .Index(t => t.Contrato_Id);
            
            CreateTable(
                "dbo.ContratosMarco",
                c => new
                    {
                        ContratoMarco_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ContratoMarco_Id);
            
            CreateTable(
                "dbo.TiposContratos",
                c => new
                    {
                        TipoContrato_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.TipoContrato_Id);
            
            CreateTable(
                "dbo.TiposEstadoContrato",
                c => new
                    {
                        TiposEstadoContrato_Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.TiposEstadoContrato_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contratos", "TiposEstadoContrato_TiposEstadoContrato_Id", "dbo.TiposEstadoContrato");
            DropForeignKey("dbo.Contratos", "TipoContrato_Id", "dbo.TiposContratos");
            DropForeignKey("dbo.Contratos", "ContratoMarco_Id", "dbo.ContratosMarco");
            DropForeignKey("dbo.Actividades", "EstadoActividad_Id", "dbo.EstadosActividad");
            DropForeignKey("dbo.ActividadesFases", "FasesContrato_fase_Id", "dbo.FasesContrato");
            DropForeignKey("dbo.Registrofacescontratos", "Fase_Id", "dbo.FasesContrato");
            DropForeignKey("dbo.Registrofacescontratos", "Contrato_Id", "dbo.Contratos");
            DropForeignKey("dbo.Actividades", "Registrofacescontratos_id", "dbo.Registrofacescontratos");
            DropForeignKey("dbo.ActividadesFases", "EstadoActividad_Id", "dbo.EstadosActividad");
            DropForeignKey("dbo.ActividadesFases", "Contratos_Contrato_Id", "dbo.Contratos");
            DropForeignKey("dbo.Contratos", "PersonaSupervisorTecnico_Id", "dbo.Personas");
            DropForeignKey("dbo.Contratos", "PersonaSuperviosr_Id", "dbo.Personas");
            DropForeignKey("dbo.Contratos", "PersonaAbogado_Id", "dbo.Personas");
            DropForeignKey("dbo.Contratos", "Persona_Id", "dbo.Personas");
            DropForeignKey("dbo.TiposPersona", "TiposNaturalezas_Naturaleza_Id", "dbo.TiposNaturaleza");
            DropForeignKey("dbo.TiposPersona", "TiposNaturaleza_Naturaleza_Id1", "dbo.TiposNaturaleza");
            DropForeignKey("dbo.TiposPersona", "TiposNaturaleza_Naturaleza_Id", "dbo.TiposNaturaleza");
            DropForeignKey("dbo.Personas", "TipoPersona_Id", "dbo.TiposPersona");
            DropForeignKey("dbo.Contratos", "Personas_Persona_Id2", "dbo.Personas");
            DropForeignKey("dbo.Contratos", "Personas_Persona_Id1", "dbo.Personas");
            DropForeignKey("dbo.Contratos", "Personas_Persona_Id", "dbo.Personas");
            DropIndex("dbo.Registrofacescontratos", new[] { "Contrato_Id" });
            DropIndex("dbo.Registrofacescontratos", new[] { "Fase_Id" });
            DropIndex("dbo.TiposPersona", new[] { "TiposNaturalezas_Naturaleza_Id" });
            DropIndex("dbo.TiposPersona", new[] { "TiposNaturaleza_Naturaleza_Id1" });
            DropIndex("dbo.TiposPersona", new[] { "TiposNaturaleza_Naturaleza_Id" });
            DropIndex("dbo.Personas", new[] { "TipoPersona_Id" });
            DropIndex("dbo.Contratos", new[] { "TiposEstadoContrato_TiposEstadoContrato_Id" });
            DropIndex("dbo.Contratos", new[] { "Personas_Persona_Id2" });
            DropIndex("dbo.Contratos", new[] { "Personas_Persona_Id1" });
            DropIndex("dbo.Contratos", new[] { "Personas_Persona_Id" });
            DropIndex("dbo.Contratos", new[] { "ContratoMarco_Id" });
            DropIndex("dbo.Contratos", new[] { "PersonaSupervisorTecnico_Id" });
            DropIndex("dbo.Contratos", new[] { "PersonaSuperviosr_Id" });
            DropIndex("dbo.Contratos", new[] { "PersonaAbogado_Id" });
            DropIndex("dbo.Contratos", new[] { "Persona_Id" });
            DropIndex("dbo.Contratos", new[] { "TipoContrato_Id" });
            DropIndex("dbo.ActividadesFases", new[] { "FasesContrato_fase_Id" });
            DropIndex("dbo.ActividadesFases", new[] { "Contratos_Contrato_Id" });
            DropIndex("dbo.ActividadesFases", new[] { "EstadoActividad_Id" });
            DropIndex("dbo.Actividades", new[] { "EstadoActividad_Id" });
            DropIndex("dbo.Actividades", new[] { "Registrofacescontratos_id" });
            DropTable("dbo.TiposEstadoContrato");
            DropTable("dbo.TiposContratos");
            DropTable("dbo.ContratosMarco");
            DropTable("dbo.Registrofacescontratos");
            DropTable("dbo.FasesContrato");
            DropTable("dbo.TiposNaturaleza");
            DropTable("dbo.TiposPersona");
            DropTable("dbo.Personas");
            DropTable("dbo.Contratos");
            DropTable("dbo.ActividadesFases");
            DropTable("dbo.EstadosActividad");
            DropTable("dbo.Actividades");
        }
    }
}
