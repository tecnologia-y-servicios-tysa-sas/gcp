using System.Web.Mvc;

namespace GCP_CF.Controllers
{
    public class ReportesController : Controller
    {
        public ActionResult Contratos()
        {
            return View();
        }

        public ActionResult Facturas()
        {
            return View();
        }
    }
}