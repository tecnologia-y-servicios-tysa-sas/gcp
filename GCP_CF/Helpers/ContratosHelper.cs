using GCP_CF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCP_CF.Helpers
{
    public class ContratosHelper
    {
        private readonly GCPContext db = new GCPContext();

        public int ObtenerIdCIAD()
        {
            string tipoContratoCIAD = System.Configuration.ConfigurationManager.AppSettings["tipoContratoMarco"];
            try
            {
                return db.TiposContratos.Where(tc => tc.Termino == tipoContratoCIAD).Select(tc => tc.TipoContrato_Id).FirstOrDefault();
            } catch
            {
                return -1;
            }
        }
    }
}