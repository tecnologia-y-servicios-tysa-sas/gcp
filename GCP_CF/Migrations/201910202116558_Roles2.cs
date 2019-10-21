namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Roles2 : DbMigration
    {
        public override void Up()
        {
          //  DropTable("dbo.Roles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RolId = c.Int(nullable: false, identity: true),
                        Descripción = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.RolId);
            
        }
    }
}
