using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GCP_CF.Models;

namespace GCP_CF.Controllers
{
    public class FacturasController : Controller
    {
        private readonly GCPContext db = new GCPContext();

        // GET: Facturas
        public ActionResult Index()
        {
            List<Facturas> facturas = db.Facturas.OrderByDescending(f => f.Factura_Id).ToList<Facturas>();
            return View(facturas);
        }

        [HttpPost]
        public JsonResult ConsultarIdContrato(string numeroContrato)
        {
            string mensaje = string.Empty;
            string idContrato = string.Empty;

            if (string.IsNullOrEmpty(numeroContrato))
                mensaje = "Debe ingresar un número de contrato";
            else
            {
                Contratos contrato = db.Contratos.Where(c => c.NumeroContrato.ToLower() == numeroContrato.Trim().ToLower()).FirstOrDefault();
                if (contrato != null)
                    idContrato = contrato.Contrato_Id.ToString();
                else
                    mensaje = "No se encontró un contrato con el número ingresado";
            }

            object response = new { id = idContrato, error = mensaje };
            return Json(response);
        }

        // GET: Facturas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Facturas factura = db.Facturas.Where(f => f.Factura_Id == id.Value).FirstOrDefault();
            if (factura == null)
                return HttpNotFound();

            return View(factura);
        }

        // GET: Facturas/Create
        public ActionResult Create()
        {
            CargarListados();
            return View();
        }

        private void CargarListados()
        {
            ViewBag.Estado_Id = new SelectList(db.EstadosFactura.OrderBy(e => e.Descripcion), "EstadoFactura_Id", "Descripcion");
            ViewBag.Municipio_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto");

            List<object> listadoMeses = new List<object>();
            for (int i = 1; i <= 12; i++)
                listadoMeses.Add(new { id = i, mes = NombreMes(i) });

            ViewBag.Mes = new SelectList(listadoMeses, "id", "mes");
        }

        public string NombreMes(int mes)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(dtinfo.GetMonthName(mes));
        }

        // POST: Facturas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Facturas factura, FormCollection collection)
        {
            try
            {
                double totalHonorariosAux = Convert.ToDouble(factura.TotalHonorariosAux.Replace(",", "").Replace(".00", ""));
                factura.TotalHonorarios = totalHonorariosAux;

                double valorBaseAux = Convert.ToDouble(factura.ValorBaseAux.Replace(",", "").Replace(".00", ""));
                factura.ValorBase = valorBaseAux;

                double valorIvaAux = Convert.ToDouble(factura.ValorIvaAux.Replace(",", "").Replace(".00", ""));
                factura.ValorIva = valorIvaAux;

                double valorCanceladoAux = Convert.ToDouble(factura.ValorCanceladoAux.Replace(",", "").Replace(".00", ""));
                factura.ValorCancelado = valorCanceladoAux;

                db.Facturas.Add(factura);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                CargarListados();
                ViewBag.MensajeError = "Ha ocurrido un error al crear la factura: " + e.Message;
                return View(factura);
            }
        }

        // GET: Facturas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Facturas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Facturas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Facturas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
