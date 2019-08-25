﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GCP_CF.Models;

namespace GCP_CF.Controllers
{
    public class FacturasController : Controller
    {
        private readonly GCPContext db = new GCPContext();
        private const string CREAR = "Crear Factura";
        private const string EDITAR = "Editar Factura";

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
            object listaPagos = new { };
            var jsonSerializer = new JavaScriptSerializer();

            if (string.IsNullOrEmpty(numeroContrato))
                mensaje = "Debe ingresar un número de contrato";
            else
            {
                Contratos contrato = db.Contratos.Where(c => c.NumeroContrato.ToLower() == numeroContrato.Trim().ToLower()).FirstOrDefault();
                if (contrato != null) {
                    idContrato = contrato.Contrato_Id.ToString();

                    List<PagosContrato> pagosContrato = contrato.PagosContrato.Select(p => p).ToList<PagosContrato>();
                    if (pagosContrato == null || pagosContrato.Count == 0)
                        mensaje = "No existen pagos asociados al contrato " + numeroContrato + ".";
                    else
                    {
                        // Deben llegar los pagos que no estén asociados a una factura
                        pagosContrato = pagosContrato.Where(p => p.Factura_Id == null).ToList<PagosContrato>();

                        if (pagosContrato == null || pagosContrato.Count == 0)
                            mensaje = "Todos los pagos asociados al contrato " + numeroContrato + " han sido facturados.";
                        else
                        {
                            listaPagos = pagosContrato.Select(p => new {
                                id = p.PagosContrato_Id,
                                numero = p.NumeroPago,
                                valor = p.Valor,
                                fecha = p.Fecha.ToShortDateString(),
                                notas = p.Notas
                            });
                        }
                    }

                } else
                    mensaje = "No se encontró un contrato con el número ingresado";
            }

            object response = new { id = idContrato, pagos = listaPagos, error = mensaje };
            return Json(response);
        }

        // GET: Facturas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Facturas factura = db.Facturas.Where(f => f.Factura_Id == id.Value).Include(f => f.PagosContrato).FirstOrDefault();
            if (factura == null)
                return HttpNotFound();

            return View(factura);
        }

        // GET: Facturas/Create
        public ActionResult Create()
        {
            CargarListados(string.Empty, string.Empty, string.Empty);
            ViewBag.Accion = CREAR;
            ViewBag.IsEdit = false;

            return View();
        }

        private void CargarListados(string idEstado, string idPersona, string mes)
        {
            ViewBag.Estado_Id = new SelectList(db.EstadosFactura.OrderBy(e => e.Descripcion), "EstadoFactura_Id", "Descripcion", idEstado);
            ViewBag.Municipio_Id = new SelectList(db.Personas.Where(x => x.TipoPersona_Id == 3), "Persona_Id", "NombreCompleto", idPersona);

            List<object> listadoMeses = new List<object>();
            for (int i = 1; i <= 12; i++)
                listadoMeses.Add(new { id = i, mes = NombreMes(i) });

            ViewBag.Mes = new SelectList(listadoMeses, "id", "mes", mes);
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
            ViewBag.Accion = CREAR;
            ViewBag.IsEdit = false;

            try
            {
                // Validar si se seleccionó al menos un pago
                string[] idsPagosContrato = collection.GetValues("idPagoContrato");
                if (idsPagosContrato == null)
                    throw new Exception("Debe seleccionar al menos un pago del contrato " + factura.Contrato.NumeroContrato);

                // Validar si la factura tiene pagos asociados
                List<PagosContrato> pagosContrato = db.PagosContrato.Where(p => p.Contrato_Id == factura.Contrato_Id && p.Factura_Id == null).ToList();
                if (pagosContrato == null || pagosContrato.Count == 0)
                    throw new Exception("El contrato " + factura.Contrato.NumeroContrato + " no tiene pagos asociados.");

                // Se filtra el arreglo de pagos para las coincidencias con los pagos seleccionados
                pagosContrato = pagosContrato.Where(p => idsPagosContrato.Contains(p.PagosContrato_Id.ToString())).ToList();

                // Validación adicional de seguridad
                if (pagosContrato == null || pagosContrato.Count == 0)
                    throw new Exception("Los pagos seleccionados no coinciden con los pagos del contrato " + factura.Contrato.NumeroContrato + ".");

                double totalHonorariosAux = Convert.ToDouble(factura.TotalHonorariosAux.Replace(",", "").Replace(".00", ""));
                factura.TotalHonorarios = totalHonorariosAux;

                double valorBaseAux = Convert.ToDouble(factura.ValorBaseAux.Replace(",", "").Replace(".00", ""));
                factura.ValorBase = valorBaseAux;

                double valorIvaAux = Convert.ToDouble(factura.ValorIvaAux.Replace(",", "").Replace(".00", ""));
                factura.ValorIva = valorIvaAux;

                if (!string.IsNullOrEmpty(factura.ValorCanceladoAux))
                {
                    double valorCanceladoAux = Convert.ToDouble(factura.ValorCanceladoAux.Replace(",", "").Replace(".00", ""));
                    factura.ValorCancelado = valorCanceladoAux;
                }
                else
                    factura.ValorCancelado = 0;

                // Se asocian los pagos a la factura
                if (factura.PagosContrato == null) factura.PagosContrato = new List<PagosContrato>();
                factura.PagosContrato.AddRange(pagosContrato);

                db.Facturas.Add(factura);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                CargarListados(collection["Estado_Id"], collection["Municipio_Id"], collection["Mes"]);
                ViewBag.MensajeError = "Ha ocurrido un error al crear la factura: " + e.Message;
                ViewBag.Accion = CREAR;

                factura.ValorBase = 0;
                factura.ValorBaseAux = string.Empty;
                factura.ValorCancelado = 0;
                factura.ValorCanceladoAux = string.Empty;
                factura.ValorIva = 0;
                factura.ValorIvaAux = string.Empty;
                factura.TotalHonorarios = 0;
                factura.TotalHonorariosAux = string.Empty;

                return View(factura);
            }
        }

        // GET: Facturas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Facturas factura = db.Facturas.Where(f => f.Factura_Id == id.Value).Include(f => f.PagosContrato).FirstOrDefault();
            if (factura == null) return HttpNotFound();

            CargarListados(factura.Estado_Id.ToString(), factura.Municipio_Id.ToString(), factura.Mes.ToString());
            ViewBag.NumeroContrato = factura.Contrato.NumeroContrato;
            ViewBag.Accion = EDITAR;
            ViewBag.IsEdit = true;

            var formatter = new CultureInfo("es-CO", false).NumberFormat;
            formatter.NumberGroupSeparator = ",";
            formatter.NumberDecimalSeparator = ".";
            formatter.NumberDecimalDigits = 2;

            factura.TotalHonorariosAux = factura.TotalHonorarios.HasValue ? factura.TotalHonorarios.Value.ToString("#,###.#0", formatter) : string.Empty;
            factura.ValorBaseAux = factura.ValorBase.ToString("#,###.#0", formatter);
            factura.ValorIvaAux = factura.ValorIva.HasValue ? factura.ValorIva.Value.ToString("#,###.#0", formatter) : string.Empty;
            factura.ValorCanceladoAux = factura.ValorCancelado.ToString("#,###.#0", formatter);

            return View(factura);
        }

        // POST: Facturas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Facturas factura, FormCollection form)
        {
            bool exito = false;
            ViewBag.Accion = EDITAR;
            ViewBag.IsEdit = true;

            try
            {
                if (factura == null) return HttpNotFound();

                if (ModelState.IsValid)
                {
                    // Validar si se seleccionó al menos un pago
                    string[] idsPagosContrato = form.GetValues("idPagoContrato");
                    if (idsPagosContrato == null)
                        throw new Exception("Debe seleccionar al menos un pago del contrato " + factura.Contrato.NumeroContrato);

                    // Validar si la factura tiene pagos asociados
                    List<PagosContrato> pagosContrato = db.PagosContrato.Where(p => p.Contrato_Id == factura.Contrato_Id && p.Factura_Id == null).ToList();
                    if (pagosContrato == null || pagosContrato.Count == 0)
                        throw new Exception("El contrato " + factura.Contrato.NumeroContrato + " no tiene pagos asociados.");

                    // Se filtra el arreglo de pagos para las coincidencias con los pagos seleccionados
                    pagosContrato = pagosContrato.Where(p => idsPagosContrato.Contains(p.PagosContrato_Id.ToString())).ToList();

                    // Validación adicional de seguridad
                    if (pagosContrato == null || pagosContrato.Count == 0)
                        throw new Exception("Los pagos seleccionados no coinciden con los pagos del contrato " + factura.Contrato.NumeroContrato + ".");

                    double totalHonorariosAux = Convert.ToDouble(factura.TotalHonorariosAux.Replace(",", "").Replace(".", ","));
                    factura.TotalHonorarios = totalHonorariosAux;

                    double valorBaseAux = Convert.ToDouble(factura.ValorBaseAux.Replace(",", "").Replace(".", ","));
                    factura.ValorBase = valorBaseAux;

                    double valorIvaAux = Convert.ToDouble(factura.ValorIvaAux.Replace(",", "").Replace(".", ","));
                    factura.ValorIva = valorIvaAux;

                    if (!string.IsNullOrEmpty(factura.ValorCanceladoAux))
                    {
                        double valorCanceladoAux = Convert.ToDouble(factura.ValorCanceladoAux.Replace(",", "").Replace(".00", ""));
                        factura.ValorCancelado = valorCanceladoAux;
                    }
                    else
                        factura.ValorCancelado = 0;

                    // Se asocian los nuevos pagos a la factura
                    factura.PagosContrato = new List<PagosContrato>();
                    factura.PagosContrato.AddRange(pagosContrato);

                    db.Entry(factura).State = EntityState.Modified;

                    exito = (db.SaveChanges() > 0);
                }
                else
                {
                    CargarListados(factura.Estado_Id.ToString(), factura.Municipio_Id.ToString(), factura.Mes.ToString());
                    ViewBag.MensajeError = "No fue posible actualizar la factura";
                }
            }
            catch (Exception e)
            {
                CargarListados(form["Estado_Id"], form["Municipio_Id"], form["Mes"]);
                ViewBag.MensajeError = "Ha ocurrido un error al actualizar la factura: " + e.Message;
            }

            if (exito)
                return RedirectToAction("Index");
            else
                return View(factura);
        }

        // GET: Facturas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Facturas factura = db.Facturas.Find(id);

            if (factura == null)
                return HttpNotFound();

            return View(factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool exito = false;

            Facturas factura = db.Facturas.Find(id);
            if (factura == null) return HttpNotFound();

            try
            {
                db.Facturas.Remove(factura);
                exito = db.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                ViewBag.MensajeError = "Ha ocurrido un error al eliminar la factura: " + e.Message;
            }

            if (exito)
                return RedirectToAction("Index");
            else
                return View(factura);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();

            base.Dispose(disposing);
        }
    }
}
