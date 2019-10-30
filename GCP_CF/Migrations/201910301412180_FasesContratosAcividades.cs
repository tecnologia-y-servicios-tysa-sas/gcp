namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FasesContratosAcividades : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FasesContratosAcividades",
                c => new
                    {
                        FasesContratosAcividadesId = c.Int(nullable: false, identity: true),
                        fase_Id = c.Int(nullable: false),
                        ActividadesEtapasId = c.Int(nullable: false),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FasesContratosAcividadesId)
                .ForeignKey("dbo.ActividadesEtapas", t => t.ActividadesEtapasId, cascadeDelete: true)
                .ForeignKey("dbo.FasesContrato", t => t.fase_Id, cascadeDelete: true)
                .Index(t => t.fase_Id)
                .Index(t => t.ActividadesEtapasId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FasesContratosAcividades", "fase_Id", "dbo.FasesContrato");
            DropForeignKey("dbo.FasesContratosAcividades", "ActividadesEtapasId", "dbo.ActividadesEtapas");
            DropIndex("dbo.FasesContratosAcividades", new[] { "ActividadesEtapasId" });
            DropIndex("dbo.FasesContratosAcividades", new[] { "fase_Id" });
            DropTable("dbo.FasesContratosAcividades");
        }
    }
}
