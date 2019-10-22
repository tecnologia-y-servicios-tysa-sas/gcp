namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PermisosEstado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permisos", "Estado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Permisos", "Estado");
        }
    }
}
