using System;
using System.ComponentModel.DataAnnotations;

namespace GCP_CF.Models
{
    public class Actividades
    {
        [Key]
        public int Actividad_Id { get; set; }
        public int Registrofacescontratos_id { get; set; }
        public string Item { get; set; }
        public string Descripcion { get; set; }
        public int DiasHabiles { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int EstadoActividad_Id { get; set; }
        public string Observaciones { get; set; }

        public virtual Registrofacescontratos Registrofacescontratos { get; set; }
        public virtual EstadosActividad EstadosActividad { get; set; }
    }
}