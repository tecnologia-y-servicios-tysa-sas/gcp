namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contratos", "FormaPagoId", "dbo.FormaPago");
            DropIndex("dbo.Contratos", new[] { "FormaPagoId" });
            DropColumn("dbo.Contratos", "FormaPagoId");
            DropTable("dbo.FormaPago");
        }
    }
}
