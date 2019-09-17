using GCP_CF.Authorization;
using GCP_CF.Helpers;
using System.Web.Mvc;

namespace GCP_CF.Controllers
{
    public class HomeController : Controller
    {
        [GCPAuthorize(Roles = RolHelper.TODOS)]
        public ActionResult Index()
        {
            return View();
        }

        [GCPAuthorize(Roles = RolHelper.TODOS)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [GCPAuthorize(Roles = RolHelper.TODOS)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}