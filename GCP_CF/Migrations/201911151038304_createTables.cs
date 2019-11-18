namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Factura",
                c => new
                {
                    Factura_Id = c.Int(nullable: false, identity: true),
                    Anio = c.Int(nullable: false),
                    Estado_Id = c.Int(nullable: false),
                    Numero = c.String(nullable: false, unicode: false),
                    Mes = c.Int(nullable: false),
                    FechaPago = c.DateTime(nullable: false, precision: 0),
                    FechaVencimiento = c.DateTime(nullable: false, precision: 0),
                    FechaEntrega = c.DateTime(nullable: false, precision: 0),
                    Municipio_Id = c.Int(nullable: false),
                    Concepto = c.String(nullable: false, unicode: false),
                    Contrato_Id = c.Int(nullable: false),
                    Objeto = c.String(nullable: false, unicode: false),
                    ValorBase = c.Double(nullable: false),
                    PorcentajeIva = c.Double(nullable: false),
                    ValorIva = c.Double(),
                    TotalHonorarios = c.Double(nullable: false),
                    ValorCancelado = c.Double(nullable: false),
                    Observaciones = c.String(unicode: false),
                })
                .PrimaryKey(t => t.Factura_Id)
                .ForeignKey("dbo.Contratos", t => t.Contrato_Id, cascadeDelete: true)
                .ForeignKey("dbo.EstadosFactura", t => t.Estado_Id, cascadeDelete: true)
                .ForeignKey("dbo.Personas", t => t.Municipio_Id, cascadeDelete: true)
                .Index(t => t.Estado_Id)
                .Index(t => t.Municipio_Id)
                .Index(t => t.Contrato_Id);

            //CreateTable(
            //    "dbo.PagoContrato",
            //    c => new
            //    {
            //        PagosContrato_Id = c.Int(nullable: false, identity: true),
            //        Contrato_Id = c.Int(nullable: false),
            //        NumeroPago = c.Int(nullable: false),
            //        Valor = c.Double(nullable: false),
            //        Fecha = c.DateTime(nullable: false, precision: 0),
            //        Notas = c.String(unicode: false),
            //        Factura_Id = c.Int(),
            //    })
            //    .PrimaryKey(t => t.PagosContrato_Id)
            //    .ForeignKey("dbo.Contratos", t => t.Contrato_Id, cascadeDelete: true)
            //    .ForeignKey("dbo.Factura", t => t.Factura_Id)
            //    .Index(t => t.Contrato_Id)
            //    .Index(t => t.Factura_Id);

            AddForeignKey("PagoContrato", "Factura_Id", "Factura", "Factura_Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PagoContrato", "Factura_Id", "dbo.Facturas");
            DropForeignKey("dbo.PagoContrato", "Contrato_Id", "dbo.Contratos");
            DropForeignKey("dbo.PagosContrato", "Factura_Id", "dbo.Factura");
            DropForeignKey("Factura", "Municipio_Id", "Personas");
            DropForeignKey("Factura", "Estado_Id", "EstadosFactura");
            DropForeignKey("Factura", "Contrato_Id", "Contratos");
            DropIndex("PagoContrato", new[] { "Factura_Id" });
            DropIndex("PagoContrato", new[] { "Contrato_Id" });
            DropIndex("Factura", new[] { "Contrato_Id" });
            DropIndex("Factura", new[] { "Municipio_Id" });
            DropIndex("Factura", new[] { "Estado_Id" });
            DropTable("PagoContrato");
            DropTable("Factura");
        }
    }
}
