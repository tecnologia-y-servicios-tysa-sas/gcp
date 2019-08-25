var idMensaje = "textoMensaje";
var idPopup = "mensajeAccion";

$(function () {
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || moment(value, true).isValid();
    }


    $('#datetimeFechaPago').datetimepicker({
        locale: 'es',
        format: 'DD/MM/YYYY',
        useCurrent: false
    });

    /*$("#numeroContrato").blur(function () {
        var numeroContrato = $("#numeroContrato").val();
        $("#idContrato").val("");
        var mensaje = "";
        $.ajax({
            async: false,
            url: "/Facturas/ConsultarIdContrato",
            type: "POST",
            data: JSON.stringify({ numeroContrato: numeroContrato }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                console.log(data);
                if (data.error.trim().length > 0) {
                    mensaje = data.error.trim();
                    claseMensaje = "alert-Error";
                } else {
                    $("#idContrato").val(data.id.trim());
                }
            }, error: function () {
                mensaje = "Ha ocurrido un error interno al consultar el identificador del contrato.";
                claseMensaje = "alert-danger";
            }
        });

        if (mensaje != "") {
            MostrarMensajeAccion(mensaje, claseMensaje, 5000); // Mostrar mensaje
        }
    });*/

    /*$("#valorHonorarios").blur(function () {
        CalcularValoresIva();
        AplicarFormatoNumerico("valorHonorarios");
    });

    $("#porcentajeIva").blur(function () {
        CalcularValoresIva();
    });*/

    $("#valorCancelado").blur(function () {
        AplicarFormatoNumerico("valorCancelado");
    });

    if ($("#porcentajeIva").val().trim() == "") {
        $("#porcentajeIva").val($("#defaultPorcentajeIva").val());
    }

    if ($("#mensajeErrorCreacion") != null && $("#mensajeErrorCreacion").text().trim().length > 0) {
        MostrarMensajeValidacion(idMensaje, idPopup, $("#mensajeErrorCreacion").text(), 5000);
    }

});

function ConsultarContrato() {
    var mensaje = "";
    var numeroContrato = $("#numeroContrato").val();
    var regionPagos = $("#regionPagos");
    var alertaPagos = $("#alertaPagos");

    if (numeroContrato.trim() != "") {

        regionPagos.fadeOut();
        alertaPagos.fadeIn();

        $.ajax({
            async: false,
            url: "/Facturas/ConsultarIdContrato",
            type: "POST",
            data: JSON.stringify({ numeroContrato: numeroContrato }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                console.log(data);
                if (data.error.trim().length > 0) {
                    mensaje = data.error.trim();
                    claseMensaje = "alert-Error";
                } else {
                    $("#idContrato").val(data.id.trim());
                    if (data.pagos != null && data.pagos.length > 0) {
                        // TODO: Llenar la tabla de selección de pagos
                        alertaPagos.fadeOut();
                        LlenarTablaPagos(data.pagos);
                        regionPagos.fadeIn();
                    }
                }
            }, error: function () {
                mensaje = "Ha ocurrido un error interno al consultar el identificador del contrato.";
                regionPagos.fadeOut();
                alertaPagos.fadeIn();
            }
        });
    } else {
        mensaje = "Debe digitar el número del contrato";
        regionPagos.fadeOut();
        alertaPagos.fadeIn();
    }

    if (mensaje != "") {
        MostrarMensajeValidacion(idMensaje, idPopup, mensaje, 5000); // Mostrar mensaje
    }
}

function LlenarTablaPagos(pagos) {
    var filasPagos = $("#tablaPagos tbody");
    for (var i = 0; i < pagos.length; i++) {
        var fila = '<tr class="filaPago" id="filaPago_' + pagos[i].id + '">'
            + '<td class="border-cell col-md-1 text-center" style = "width: 75px">'
            + '<input type="checkbox" class="chkPagos" name="idPagoContrato" id="idPagoContrato_' + pagos[i].id + '" value="' + pagos[i].id + '" onchange="'
            + ((i + 1) < pagos.length ? 'HabilitarSiguientePago(' + pagos[i].id + ', ' + pagos[i + 1].id + ');' : 'ActualizarValorHonorarios(' + pagos[i].id + ', -1)') + '"'
            + ((i != 0) ? ' disabled' : '')
            + '>'
            + '</td>'
            + '<td class="border-cell col-md-2 text-right">' + pagos[i].numero + '</td>'
            + '<td class="border-cell col-md-4 text-center">' + pagos[i].fecha
            + '<input type="hidden" class="valorFecha" id="valorFecha_' + pagos[i].id + '" value="' + pagos[i].fecha + '" />'
            + '</td>'
            + '<td class="border-cell col-md-5 text-right">' + AplicarFormatoNumericoValor(pagos[i].valor)
            + '<input type="hidden" class="valorPago" id="valorPago_' + pagos[i].id + '" value="' + pagos[i].valor + '" />'
            + '</td>'
            + '</tr>';
        filasPagos.append(fila);
    }
}

function HabilitarSiguientePago(idPago, idSiguientePago) {

    var id1 = "#idPagoContrato_" + idPago;
    var id2 = "#idPagoContrato_" + idSiguientePago;
    console.log("HabilitarSiguientePago(" + idPago + ", " + idSiguientePago + ")");

    if (!validateFechaPago()) {
        $(".chkPagos").prop("checked", false);
        return;
    }

    if (!$(id1).is(":checked")) {
        $(".chkPagos").prop("disabled", true);
        $(".chkPagos").prop("checked", false);
        $(id1).prop("disabled", false);
    } else {
        $(id2).prop("disabled", false);
    }

    ActualizarValorHonorarios(id1, id2);
}

function ActualizarValorHonorarios(idPago, idSiguientePago) {

    var valorAcumulado = 0;
    var honorarios = $("#valorHonorarios");
    var cValorBase = $("#valorBase");
    var cValorIva = $("#valorIVA");
    var porcentajeIva = $("#porcentajeIva");
    var filasPago = $(".filaPago");

    var fechaValida = ValidarMaximaFechaPagoSeleccionada();

    if (fechaValida) {

        if (filasPago != null && filasPago.length > 0) {
            for (var i = 0; i <= filasPago.length; i++) {
                var chkPago = $(filasPago[i]).find(".chkPagos");
                if ($(chkPago).is(":checked")) {
                    valorAcumulado += Number($(filasPago[i]).find(".valorPago").val());
                }
            }

            valorIva = (valorAcumulado * porcentajeIva.val() / 100).toFixed(2);
            valorBase = valorAcumulado - valorIva;

            honorarios.val(valorAcumulado);
            cValorIva.val(valorIva);
            cValorBase.val(valorBase);

        } else {
            honorarios.val(0);
            cValorIva.val(0);
            cValorBase.val(0);
        }

        AplicarFormatoNumerico("valorHonorarios");
        AplicarFormatoNumerico("valorBase");
        AplicarFormatoNumerico("valorIVA");

    } else {
        var id1 = "#idPagoContrato_" + idPago;
        $(id1).prop("checked", false);

        var id2 = idSiguientePago > -1 ? "#idPagoContrato_" + idSiguientePago : null;
        if (id2 != null) {
            $(id2).prop("checked", false);
            $(id2).prop("disabled", true);
        }
    }
}

function ValidarMaximaFechaPagoSeleccionada() {

    if (!validateFechaPago()) {
        return false;
    }

    var fechaPago = $("#fechaPago");
    var timeout = 5000;
    var filasPago = $(".filaPago");

    var strFechaPago = fechaPago.val().split("/");
    var dtFechaPago = new Date(Number(strFechaPago[2]), Number(strFechaPago[1]) - 1, Number(strFechaPago[0]), 0, 0, 0);
    var fechaMaxima = dtFechaPago;

    if (filasPago != null && filasPago.length > 0) {
        for (var i = 0; i <= filasPago.length; i++) {
            var valorFecha = $(filasPago[i]).find(".valorFecha");
            var chkPago = $(filasPago[i]).find(".chkPagos");
            if ($(chkPago).is(":checked")) {
                var laFecha = valorFecha.val().split("/");
                var fecha = new Date(Number(laFecha[2]), Number(laFecha[1]) - 1, Number(laFecha[0]), 0, 0, 0);
                console.log("- Fecha Pago: " + fecha);
                if (fechaMaxima.getTime() < fecha.getTime()) {
                    fechaMaxima = fecha;
                }
            }
        }
    }

    console.log("Fecha Pago: " + dtFechaPago + ", Fecha Máxima: " + fechaMaxima);

    if (dtFechaPago.getTime() < fechaMaxima.getTime()) {
        MostrarMensajeValidacion(idMensaje, idPopup, "La fecha del pago seleccionado es mayor a la fecha de la factura", timeout);
        return false;
    }

    return true;
}

function MostrarMensajeAccion(mensaje, claseMensaje, timeout) {
    $("#mensajeAccion > div.modal-dialog").addClass(claseMensaje);
    $("#mensajeAccion").modal("show");
    $("#textoMensaje").html(mensaje);
    setTimeout(function () {
        $('#mensajeAccion').modal('hide');
    }, timeout);
}

function CalcularValoresIva() {

    var valor = $("#valorHonorarios").val();
    var porcentajeIva = $("#porcentajeIva").val();

    var valorSinFormato = RestablecerFormatoId("valorHonorarios");
    var valorIvaSinFormato = RestablecerFormatoId("valorIVA");

    if (isNaN(valorSinFormato)) {
        MostrarMensajeValidacion(idMensaje, idPopup, "Valor de honorarios incorrecto", 5000);
        return;
    }

    if (isNaN(valorIvaSinFormato)) {
        MostrarMensajeValidacion(idMensaje, idPopup, "Valor del porcentaje de IVA incorrecto", 5000);
        return;
    }

    if (valor.trim() != "" && porcentajeIva.trim() != "") {
        var valorIva = Number(valorSinFormato) * Number(porcentajeIva) / 100;
        $("#valorIVA").val(valorIva);
        $("#valorBase").val(valorSinFormato - valorIva);
        AplicarFormatoNumerico("valorIVA");
        AplicarFormatoNumerico("valorBase");
    } else {
        $("#valorIVA").val("");
        $("#valorBase").val("");
    }
}

function AplicarFormatoNumerico(campoId) {
    var valorActual = $("#" + campoId).val();
    var valorSinFormato = RestablecerFormatoId(campoId);
    console.log(valorActual + ", " + valorSinFormato);
    if (valorSinFormato.trim().length > 0) {
        var valorNumerico = parseFloat(valorSinFormato, 10).toFixed(2);
        $("#" + campoId).val(valorNumerico.replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
    } else {
        $("#" + campoId).val("");
    }
}

function AplicarFormatoNumericoValor(valor) {
    var valorConFormato = "0,00";
    var valorSinFormato = valor.toString().replace(/,/gi, "");
    if (valor.toString().trim().length > 0) {
        var valorNumerico = parseFloat(valorSinFormato, 10).toFixed(2);
        valorConFormato = valorNumerico.replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
    }

    return valorConFormato;

}

function RestablecerFormatoId(campoId) {
    var valorActual = $("#" + campoId).val();
    return valorActual.replace(/,/gi, "").toString();
}

function validateNumeroFactura() {
    return validateRequired($("#numero"), "Número Factura");
}

function validateAnio() {
    return validateRequired($("#anio"), "Año");
}

function validateMes() {
    return validateRequired($("#mes"), "Mes");
}

function validateEstado() {
    return validateRequired($("#idEstado"), "Estado");
}

function validateFechaPago() {
    return validateRequired($("#fechaPago"), "Fecha de Pago");
}

function validateEntidad() {
    return validateRequired($("#idMunicipio"), "Municipio o Entidad");
}

function validateObjeto() {
    return validateRequired($("#objeto"), "Objeto");
}

function validateNumeroContrato() {
    return validateRequired($("#numeroContrato"), "Contrato");
}

function validatePagos() {
    var exito = true;
    var mensaje = "";
    var filasPago = $(".filaPago");

    if (filasPago == null || filasPago.length == 0) {
        mensaje = "Debe existir un listado de pagos para seleccionar";
        exito = false;
    } else {
        var chkPago = $(".chkPagos:checked");
        if (chkPago == null || chkPago.length == 0) {
            mensaje = "Debe seleccionar al menos un pago";
            exito = false;
        }
    }

    if (!exito) {
        MostrarMensajeValidacion(idMensaje, idPopup, mensaje, 5000);
        return false;
    }

    return true;
}

function validateHonorarios() {
    return validateNumeric($("#valorHonorarios"), "Honorarios");
}

function validatePorcentajeIva() {
    return validateNumeric($("#porcentajeIva"), "Porcentaje IVA");
}

function validateValorCancelado() {
    if ($("#valorCancelado").val().trim() != "") {
        return validateNumeric($("#valorCancelado"), "Valor Cancelado");
    }
    return true;
}

function validateObservaciones() {
    return validateRequired($("#observaciones"), "Observaciones");
}

function validarTodo() {

    var esValido = validateNumeroFactura();
    if (esValido) esValido = validateAnio();
    if (esValido) esValido = validateMes();
    if (esValido) esValido = validateEstado();
    if (esValido) esValido = validateFechaPago();
    if (esValido) esValido = validateEntidad();
    if (esValido) esValido = validateObjeto();
    if (esValido) esValido = validateNumeroContrato();
    if (esValido) esValido = validatePagos();
    if (esValido) esValido = validateHonorarios();
    if (esValido) esValido = validatePorcentajeIva();
    if (esValido) esValido = validateValorCancelado();
    if (esValido) esValido = validateObservaciones();

    return esValido;
}

$(function () {

    $("#btnEnviar").on("click", function (e) {

        e.preventDefault(); // Cancela la acción por defecto

        var esValido = validarTodo();
        console.log("¿Es válido? " + esValido);

        if (esValido) {
            $("form")[0].submit();
        }

    });

});