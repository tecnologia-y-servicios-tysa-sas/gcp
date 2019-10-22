﻿using System.Web;
using System.Web.Optimization;

namespace GCP_CF
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/moment.js",
                      "~/Scripts/moment-with-locales.js",
                      "~/Scripts/bootstrap-datetimepicker.js",
                      "~/Scripts/moment-es.js",
                       "~/Scripts/chosen.jquery.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/non-Generic-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrapCustom.css",
                      "~/Content/site.css",
                       "~/Content/chosen.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/StyleTable.css"
                     ));
        }
    }
}
