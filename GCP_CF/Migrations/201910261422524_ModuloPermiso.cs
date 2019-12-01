namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModuloPermiso : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Permisos", "ModuloId", c => c.Int());
            //CreateIndex("dbo.Permisos", "ModuloId");
            //AddForeignKey("Permisos", "ModuloId", "Modulos", "ModuloId");
        }
        
        public override void Down()
        {
            //DropForeignKey("Permisos", "ModuloId", "Modulos");
            //DropIndex("dbo.Permisos", new[] { "ModuloId" });
            //DropColumn("dbo.Permisos", "ModuloId");
        }
    }
}
