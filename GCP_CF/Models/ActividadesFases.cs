using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class ActividadesFases
    {
        [Key]
        public int Actividad_Id { get; set; }
        //public int Contratos_Id { get; set; }
        //public int FaseContrato_Id { get; set; }

        [Display(Name = "Ítem")]
        public string Item { get; set; }

        public string Descripción { get; set; }

        [Display(Name = "Días Hábiles")]
        public Nullable<int> DiasHabiles { get; set; }

        [Required(ErrorMessage = "{0} es Requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Inicio")]
        public Nullable<System.DateTime> FechaInicio { get; set; }

        [Required(ErrorMessage = "{0} es Requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Final")]
        public Nullable<System.DateTime> FechaFinal { get; set; }

        [Display(Name = "Estado Actividad")]
        public Nullable<int> EstadoActividad_Id { get; set; }

        public int Contratos_Contrato_Id1 { get; set; }

        public int FasesContrato_fase_Id1 { get; set; }

        public virtual Contratos Contratos { get; set; }

        public virtual EstadosActividad EstadosActividad { get; set; }

        public virtual FasesContrato FasesContrato { get; set; }
    }
}