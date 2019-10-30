namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActividadesEtapas1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActividadesEtapas",
                c => new
                    {
                        ActividadesEtapasId = c.Int(nullable: false, identity: true),
                        Descripción = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.ActividadesEtapasId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ActividadesEtapas");
        }
    }
}
