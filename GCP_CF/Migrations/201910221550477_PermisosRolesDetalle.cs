namespace GCP_CF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PermisosRolesDetalle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PermisosRoles",
                c => new
                    {
                        PermisosRolesId = c.Int(nullable: false, identity: true),
                        RolId = c.Int(nullable: false),
                        PermisoId = c.Int(nullable: false),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PermisosRolesId)
                .ForeignKey("dbo.Permisos", t => t.PermisoId, cascadeDelete: true)
                .ForeignKey("dbo.Rol", t => t.RolId, cascadeDelete: true)
                .Index(t => new { t.RolId, t.PermisoId }, unique: true, name: "IndexPermisos");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PermisosRoles", "RolId", "dbo.Rol");
            DropForeignKey("dbo.PermisosRoles", "PermisoId", "dbo.Permisos");
            DropIndex("dbo.PermisosRoles", "IndexPermisos");
            DropTable("dbo.PermisosRoles");
        }
    }
}
