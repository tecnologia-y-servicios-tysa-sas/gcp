$(function () {
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || moment(value, true).isValid();
    }
    var fechaActual = new Date();

    $('#datetimeFechaPago').datetimepicker({
        locale: 'es',
        format: 'DD/MM/YYYY',
        useCurrent: false
    });

    $("#numeroContrato").blur(function () {
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
    });

    $("#valorHonorarios").blur(function () {
        CalcularValoresIva();
        AplicarFormatoNumerico("valorHonorarios");
    });

    $("#porcentajeIva").blur(function () {
        CalcularValoresIva();
    });

    $("#valorCancelado").blur(function () {
        AplicarFormatoNumerico("valorCancelado");
    });

    if ($("#mensajeErrorCreacion") != null && $("#mensajeErrorCreacion").text().trim().length > 0) {
        MostrarMensajeAccion($("#mensajeErrorCreacion").text(), "alert-Error", 5000);
    }

});

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

    var valorSinFormato = RestablecerFormato("valorHonorarios");
    var valorIvaSinFormato = RestablecerFormato("valorIVA");

    if (isNaN(valorSinFormato)) {
        MostrarMensajeAccion("Valor de honorarios incorrecto", "alert-Error", 5000);
        return;
    }

    if (isNaN(valorIvaSinFormato)) {
        MostrarMensajeAccion("Valor del porcentaje de IVA incorrecto", "alert-Error", 5000);
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
    var valorSinFormato = RestablecerFormato(campoId);
    console.log(valorActual + ", " + valorSinFormato);
    if (valorSinFormato.trim().length > 0) {
        var valorNumerico = parseFloat(valorSinFormato, 10).toFixed(2);
        $("#" + campoId).val(valorNumerico.replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
    } else {
        $("#" + campoId).val("");
    }
}

function RestablecerFormato(campoId) {
    var valorActual = $("#" + campoId).val();
    return valorActual.replace(/,/gi, "").toString();
}