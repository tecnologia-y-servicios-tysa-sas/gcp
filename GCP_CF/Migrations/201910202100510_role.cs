namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class role : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.UsuariosRoles", "RolId", "dbo.Roles");
            //DropForeignKey("dbo.UsuariosRoles", "Usuarios_Usuario_Id", "dbo.Usuarios");
            //DropIndex("dbo.UsuariosRoles", new[] { "RolId" });
            //DropIndex("dbo.UsuariosRoles", new[] { "Usuarios_Usuario_Id" });
            CreateTable(
                "dbo.UsuarioRoles",
                c => new
                    {
                        Usuario_Id = c.Int(nullable: false, identity: true),
                        RolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Usuario_Id);
            
           //DropColumn("dbo.UsuariosRoles", "Usuarios_Usuario_Id");
        }
        
        //public override void Down()
        //{
        //    AddColumn("dbo.UsuariosRoles", "Usuarios_Usuario_Id", c => c.Int());
        //    DropTable("dbo.UsuarioRoles");
        //    CreateIndex("dbo.UsuariosRoles", "Usuarios_Usuario_Id");
        //    CreateIndex("dbo.UsuariosRoles", "RolId");
        //    AddForeignKey("dbo.UsuariosRoles", "Usuarios_Usuario_Id", "dbo.Usuarios", "Usuario_Id");
        //    AddForeignKey("dbo.UsuariosRoles", "RolId", "dbo.Roles", "RolId", cascadeDelete: true);
        //}
    }
}
