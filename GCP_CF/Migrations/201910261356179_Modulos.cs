namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modulos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Modulos",
                c => new
                    {
                        ModuloId = c.Int(nullable: false, identity: true),
                        Descripción = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Detalle = c.String(maxLength: 200, storeType: "nvarchar"),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ModuloId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Modulos");
        }
    }
}
