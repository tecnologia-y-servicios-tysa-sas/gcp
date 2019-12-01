using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
     [NotMapped]
    public class PersonasNotificacion
    {
        public  int Persona_Id { get; set; }
        public string NombreCompleto { get; set; }
    }
}