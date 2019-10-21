namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rol",
                c => new
                    {
                        RolId = c.Int(nullable: false, identity: true),
                        Descripción = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.RolId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Rol");
        }
    }
}
