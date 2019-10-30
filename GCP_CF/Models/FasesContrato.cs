using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class FasesContrato
    {
        [Key]
        public int fase_Id { get; set; }
        public string Descripcion { get; set; }

        
        public virtual ICollection<Registrofacescontratos> Registrofacescontratos { get; set; }
        public virtual ICollection<FasesContratosAcividades> GetFasesContratosAcividades { get; set; }

        public List<FasesContrato> Listar()
        {
            var fases = new List<FasesContrato>();
            try
            {
                using (var context = new GCPContext())
                {
                    fases = context.FasesContrato.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return fases;
        }
    }
}