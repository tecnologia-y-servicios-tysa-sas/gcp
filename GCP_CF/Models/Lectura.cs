using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCP_CF.Models
{
    public class Lectura
    {
        public int contrato_Id { get; set; }

        public int Anio { get; set; }

        public int Persona_Id { get; set; }

        public int TipoContrato_Id { get; set; }

        public int TipoEstadoContrato_Id { get; set; }

        public double Valor_Contrato { get; set; }

        public double Valor_Administrar { get; set; }

        public double Valor_Honorarios { get; set; }

        public double Valor_CDP { get; set; }

        public double Valor_CRP { get; set; }

        public double Valor_Pagos { get; set; }

        public double Valor_Ejecutado { get; set; }

        public double? Valor_CuentasxCobrar { get; set; }

        public double? Valor_Facturado { get; set; }

        public double? Valor_Facturas_Canceladas { get; set; }

        public string Estado { get; set; }
    }
}