namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Permisos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permisos",
                c => new
                    {
                        PermisoId = c.Int(nullable: false, identity: true),
                        Descripción = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Detalle = c.String(maxLength: 200, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.PermisoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Permisos");
        }
    }
}
