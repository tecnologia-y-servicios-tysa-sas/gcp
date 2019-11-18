namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class salgado : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Notificaciones", "Contratos_Contrato_Id", "dbo.Contratos");
            //DropIndex("dbo.Notificaciones", new[] { "Contratos_Contrato_Id" });
            //DropColumn("dbo.Facturas", "FechaVencimiento");
            //DropColumn("dbo.Facturas", "FechaEntrega");
            //DropTable("dbo.Notificaciones");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.Notificaciones",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ContractId = c.Int(nullable: false),
            //            PersonId = c.Int(nullable: false),
            //            Contratos_Contrato_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //AddColumn("dbo.Facturas", "FechaEntrega", c => c.DateTime(nullable: false, precision: 0));
            //AddColumn("dbo.Facturas", "FechaVencimiento", c => c.DateTime(nullable: false, precision: 0));
            //CreateIndex("dbo.Notificaciones", "Contratos_Contrato_Id");
            //AddForeignKey("dbo.Notificaciones", "Contratos_Contrato_Id", "dbo.Contratos", "Contrato_Id");
        }
    }
}
