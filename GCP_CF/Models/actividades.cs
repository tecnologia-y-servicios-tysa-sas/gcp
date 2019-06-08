using System.ComponentModel.DataAnnotations;

namespace GCP_CF.Models
{
    public class actividades
    {
        [Key]
        public int Actividad_Id { get; set; }
        public int registrofacescontratos_id { get; set; }
        public int FaseContrato_Id { get; set; }
        public string Item { get; set; }
        public string Descripción { get; set; }
        public int DiasHabiles { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaFinal { get; set; }
        public int EstadoActividad_Id { get; set; }
        public string Observaciones { get; set; }

        public virtual registrofacescontratos registrofacescontratos { get; set; }
        public virtual EstadosActividad EstadosActividad { get; set; }
    }
}