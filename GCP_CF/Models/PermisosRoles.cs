using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class PermisosRoles
    {
        [Key]
        public int PermisosRolesId { get; set; }

        [Index("IndexPermisos", IsClustered = false, IsUnique = true, Order = 0)]
        public int RolId { get; set; }

        [Index("IndexPermisos", IsClustered = false, IsUnique = true, Order = 1)]
        public int PermisoId { get; set; }

        public bool Estado { get; set; }

        public virtual Rol Rol { get; set; }
        public virtual Permisos Permisos { get; set;}

    }
}