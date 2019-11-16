namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("PagoContrato", "Factura_Id", "Facturas");
        }
        
        public override void Down()
        {
            AddForeignKey("PagoContrato", "Factura_Id", "Facturas", "Factura_Id");
        }
    }
}
