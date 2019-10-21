namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usuarioRole : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.UsuarioRoles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UsuarioRoles",
                c => new
                    {
                        Usuario_Id = c.Int(nullable: false, identity: true),
                        RolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Usuario_Id);
            
        }
    }
}
