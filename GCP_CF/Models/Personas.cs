using System;
using System.ComponentModel.DataAnnotations;

namespace GCP_CF.Models
{
    public class Personas
    {
        [Key]
        public int Persona_Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public Nullable<int> TipoPersona_Id { get; set; }

        public virtual TiposPersonas TiposPersonas { get; set; }
    }
}