namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMigration : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.ActividadesFases", "TipoTareas_TipoTareasId", "dbo.TipoTareas");
            //DropIndex("dbo.ActividadesFases", new[] { "TipoTareas_TipoTareasId" });
            //DropColumn("dbo.ActividadesFases", "TipoTareas_Id");
            //DropColumn("dbo.ActividadesFases", "TipoTareas_TipoTareasId");
            //DropTable("dbo.TipoTareas");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.TipoTareas",
            //    c => new
            //        {
            //            TipoTareasId = c.Int(nullable: false, identity: true),
            //            Descripcion = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
            //        })
            //    .PrimaryKey(t => t.TipoTareasId);
            
            //AddColumn("dbo.ActividadesFases", "TipoTareas_TipoTareasId", c => c.Int());
            //AddColumn("dbo.ActividadesFases", "TipoTareas_Id", c => c.Int(nullable: false));
            //CreateIndex("dbo.ActividadesFases", "TipoTareas_TipoTareasId");
            //AddForeignKey("dbo.ActividadesFases", "TipoTareas_TipoTareasId", "dbo.TipoTareas", "TipoTareasId");
        }
    }
}
