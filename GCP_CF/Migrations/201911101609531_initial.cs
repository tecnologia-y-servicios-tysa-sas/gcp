namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facturas", "FechaVencimiento", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Facturas", "FechaEntrega", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Facturas", "FechaEntrega");
            DropColumn("dbo.Facturas", "FechaVencimiento");
        }
    }
}
