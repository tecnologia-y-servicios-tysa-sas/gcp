using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class UsuariosRoles
    {
        [Key]
        public int Usuario_Id { get; set; }

        public int RolId { get; set; }

    }
}