var idMensaje = "bodyError";
var idPopup = "PopUPModalError";

$(function () {

    if ($("#mensajeErrorOperacion") != null && $("#mensajeErrorOperacion").text().trim().length > 0) {
        MostrarMensajeValidacion(idMensaje, idPopup, $("#mensajeErrorOperacion").text(), 5000);
    }

    /*$.validator.methods.date = function (value, element) {
        return this.optional(element) || moment(value, true).isValid();
    }*/

    var fechaActual = new Date();
    fechaActual.setHours(23, 59, 59, 0);

    $('#datetimeFechaInicio').datetimepicker({
        locale: 'es',
        format: 'DD/MM/YYYY',
        useCurrent: false
    });

    $('#datetimeFechaTerminacion').datetimepicker({
        locale: 'es',
        format: 'DD/MM/YYYY',
        minDate: fechaActual,
        useCurrent: false
    });

    $('#datetimeFechaActaInicio').datetimepicker({
        locale: 'es',
        format: 'DD/MM/YYYY',
        useCurrent: false
    });

    $('#datetimeFechaFirmaContrato').datetimepicker({
        locale: 'es',
        format: 'DD/MM/YYYY',
        useCurrent: false
    });

    $('#datetimeFechaCrp').datetimepicker({
        locale: 'es',
        format: 'DD/MM/YYYY',
        useCurrent: false
    });

    $('#datetimeFechaCdp').datetimepicker({
        locale: 'es',
        format: 'DD/MM/YYYY',
        useCurrent: false
    });

    $('#datetimeFechaPago_0').datetimepicker({
        locale: 'es',
        format: 'DD/MM/YYYY',
        useCurrent: false
    });

    $("#valorContrato").blur(function () {
        if ($(this).val() != "" && !isNaN($(this).val())) {
            cambiarFormatoNumerico($("#valorContrato"));
        }
    });

    $("#valorAdministrar").blur(function () {
        if ($(this).val() != "" && !isNaN($(this).val())) {
            cambiarFormatoNumerico($("#valorAdministrar"));
        }
    });

    $("#honorarios").blur(function () {
        if ($(this).val() != "" && !isNaN($(this).val())) {
            cambiarFormatoNumerico($("#honorarios"));
            calcularValorNetoHonorarios($(this), $("#porcentajeIvaHonorarios"));
        }
    });

    if ($("#valorNetoHonorarios").val().trim() != "") {
        cambiarFormatoNumerico($("#valorNetoHonorarios"));
    }

    $("#valorPoliza").blur(function () {
        if ($(this).val() != "" && !isNaN($(this).val())) {
            cambiarFormatoNumerico($("#valorPoliza"));
        }
    });

    // Para la parte de pagos, según si se está agregando o editando un contrato
    var tablaPagos = $("#tablaPagos");
    var filasPagos = tablaPagos.find("tbody tr");

    $(filasPagos).each(function (filaPago) {

        $("#valorPago_" + filaPago).blur(function () {
            if ($(this).val() != "" && !isNaN($(this).val())) {
                cambiarFormatoNumerico($(this));
                return validarPagos();
            }
        });

        $("#fechaPago_" + filaPago).blur(function () {
            if (validarFechaPago(filaPago)) {
                return validarPagos();
            }
        });

    });

    // Obtengo el numero del contrato
    $("#tipoContrato").change(function () {
        if ($(this).val() != "") {
            var url = "/Contratos/GetDocumento";
            $.ajax({
                type: 'POST',
                url: url,
                data: { id: $("#tipoContrato").val() },
                success: function (data) {
                    $("#numeroContrato").val(data);
                },
                error: function (ex) {
                    $("#numeroContrato").val("");
                }
            });
        } else {
            $("#numeroContrato").val("");
        }

        enableOrDisableNonCIADFields();
    });

});

function stringToDate(_date, _format, _delimiter) {
    var formatLowerCase = _format.toLowerCase();
    var formatItems = formatLowerCase.split(_delimiter);
    var dateItems = _date.split(_delimiter);
    var monthIndex = formatItems.indexOf("mm");
    var dayIndex = formatItems.indexOf("dd");
    var yearIndex = formatItems.indexOf("yyyy");
    var month = parseInt(dateItems[monthIndex]);
    month -= 1;
    var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
    return formatedDate;
}

// Validaciones

function cambiarFormatoNumerico(campo) {
    campo.val(parseFloat(campo.val(), 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
}

function validateRequired(campo, nombreCampo) {
    var mensaje = "<strong>Error:</strong> El campo " + nombreCampo + " es requerido";
    if (campo.val() == "") {
        campo.css("border-color", "red");
        MostrarMensajeValidacion(idMensaje, idPopup, mensaje, 5000);
        return false;
    } else {
        campo.css("border-color", "green");
        return true;
    }
}

function validateNumeric(campo, nombreCampo) {

    if (validateRequired(campo, nombreCampo)) {

        console.log(">> Nombre campo: " + nombreCampo + ", valor: " + campo.val());
        console.log(">> Test: " + /^([0-9.,])*$/.test(campo.val()));

        var mensaje = "<strong>Error:</strong> El campo " + nombreCampo + " debe ser un número";

        if (!/^([0-9.,])*$/.test(campo.val())) {
            campo.css('border-color', 'red');
            MostrarMensajeValidacion(idMensaje, idPopup, mensaje, 5000);
            return false;
        } else {
            campo.css('border-color', 'green');
            return true;
        }
    }

    return false;
}

$("#tipoContrato").blur(function () {
    validateTipoContrato();
});

$("#entidadContratante").blur(function () {
    validateEntidadContratante();
});

$("#fechaActa").blur(function () {
    validateFechaActa();
});

$("#fechaFirmaContrato").blur(function () {
    validateFechaFirmaContrato();
});

$("#estadoContrato").change(function () {
    validateEstadoContrato();
});

$("#fechaInicial").blur(function () {
    validateFechaInicial();
});

$("#fechaFinal").blur(function () {
    validateFechaFinal();
});

$("#supervisor").change(function () {
    validatesupervisor();
});

$("#abogado").change(function () {
    validateAbogado();
});

$("#supervisorTecnico").change(function () {
    validateSupervisorTecnico();
});

$("#crp").blur(function () {
    validateCrp();
});

$("#fechaCrp").blur(function () {
    validateFechaCrp();
});

$("#year").blur(function () {
    validateYear();
});

$("#cdp").blur(function () {
    validateCdp();
});

$("#fechaCdp").blur(function () {
    validateFechaCdp();
});

$("#valorContrato").blur(function () {
    validateValorContrato();
});

$("#valorAdministrar").blur(function () {
    validateValorAdministrar();
});

$("#honorarios").blur(function () {
    validateHonorarios();
});

$("#objeto").blur(function () {
    validateObjeto();
});

function calcularValorNetoHonorarios(honorarios, porcentajeIvaHonorarios) {
    var valorNeto = 0;
    var valorNetoHonorarios = $("#valorNetoHonorarios");
    var numHonorarios = RestablecerFormato($(honorarios));
    var ivaHonorarios = $(porcentajeIvaHonorarios).val();

    console.log(numHonorarios + ", IVA: " + ivaHonorarios);

    if (!isNaN(numHonorarios)) {
        valorNeto = Math.round(100 * (Number(numHonorarios) * Number(ivaHonorarios) / 100)) / 100;
    }

    $(valorNetoHonorarios).val(valorNeto);
    cambiarFormatoNumerico($(valorNetoHonorarios));
}

function validarTodosLosCamposGenerales() {

    var esValido = validateTipoContrato();
    console.log("-- 1... " + esValido);
    if (esValido) esValido = validateEntidadContratante();
    console.log("-- 2... " + esValido);
    if (esValido) esValido = validateFechaActa();
    console.log("-- 3... " + esValido);
    if (esValido) esValido = validateFechaFirmaContrato();
    console.log("-- 4... " + esValido);
    if (esValido) esValido = validateEstadoContrato();
    console.log("-- 5... " + esValido);
    if (esValido) esValido = validateFechaInicial();
    console.log("-- 6... " + esValido);
    if (esValido) esValido = validateFechaFinal();
    console.log("-- 7... " + esValido);
    if (esValido) esValido = validateValorContrato();
    console.log("-- 8... " + esValido);
    if (esValido) esValido = validateValorAdministrar();
    console.log("-- 9... " + esValido);
    if (esValido) esValido = validateHonorarios();
    console.log("-- 10... " + esValido);
    if (esValido) esValido = validateObjeto();
    console.log("-- 11... " + esValido);

    console.log("- Validar todos los campos... " + esValido);
    return esValido;

}

$("#observaciones").blur(function () {
    return validarTodosLosCamposGenerales();
});

//$("#abogado").change(function () {
//    validateTipoContrato();
//    validateEntidadContratante();
//    validateFechaActa();
//    validateEstadoContrato();
//    validateFechaInicial();
//    validateFechaFinal();
//    validatesupervisor();
//    validateAbogado();
//});

//$("#supervisorTecnico").change(function () {
//    validateTipoContrato();
//    validateEntidadContratante();
//    validateFechaActa();
//    validateEstadoContrato();
//    validateFechaInicial();
//    validateFechaFinal();
//    validatesupervisor();
//    validateAbogado();
//    validateSupervisorTecnico();
//});

$("#numeroPoliza").blur(function () {
    if (validarTodosLosCamposGenerales()) {
        return validarDatosPoliza();
    }
    return false;
});

$("#valorPoliza").blur(function () {
    if (validarTodosLosCamposGenerales()) {
        return validarDatosPoliza();
    }
    return false;
});

$("#nombreAseguradora").blur(function () {
    if (validarTodosLosCamposGenerales()) {
        return validarDatosPoliza();
    }
    return false;
});

$("#notasPoliza").blur(function () {
    if (validarTodosLosCamposGenerales()) {
        return validarDatosPoliza();
    }
    return false;
});

function validateTipoContrato() {

    if (validateRequired($("#tipoContrato"), "Tipo Contrato")) {
        enableOrDisableNonCIADFields();
        return true;
    }

    return false;
}

function enableOrDisableNonCIADFields() {

    var tipoContratoCIAD = $("#tipoContratoCiad").val();
    var disableFields = $("#tipoContrato").val() != tipoContratoCIAD;
    var panelPagos = $("#panelPagos");
    var tablaPagos = $("#tablaPagos");

    if (!disableFields) {
        // Se esconde la tabla de pagos
        panelPagos.fadeOut();

        var numRows = tablaPagos.find("tbody tr").length;
        for (var i = numRows; i > 1; i--) {
            var pos = i - 1;
            $("#pago_" + pos).remove();
            $("#numeroPagos").val(pos);
        }
    } else {
        // Se desactivan campos
        $("#honorarios").val("");
        $("#honorarios").css("border-color", "none");
        $("#valorNetoHonorarios").val("");
        panelPagos.fadeIn();
    }

    $("#honorarios").prop("disabled", disableFields);
    $("#valorNetoHonorarios").prop("disabled", disableFields);
    //$("#ContratoMarco_Id").prop("disabled", disableFields);
}

function validateEntidadContratante() {
    return validateRequired($("#entidadContratante"), "Entidad Contratante");
}

function validateFechaActa() {
    return validateRequired($("#fechaActa"), "Fecha de Firma de Acta");
}

function validateFechaFirmaContrato() {
    return validateRequired($("#fechaFirmaContrato"), "Fecha Firma Contrato");
}

function validateEstadoContrato() {
    return validateRequired($("#estadoContrato"), "Estado Contrato");
}

function validateFechaInicial() {
    return validateRequired($("#fechaInicial"), "Fecha de Inicio");
}

function validateFechaFinal() {

    if (validateRequired($("#fechaFinal"), "Fecha de Terminación")) {

        if (validateFechaInicial()) {
            var start = moment(stringToDate($("#fechaInicial").val(), "dd/MM/yyyy", "/"));
            var end = moment(stringToDate($("#fechaFinal").val(), "dd/MM/yyyy", "/"));

            if (start > end) {
                document.getElementById("fechaFinal").value = "";
                $('#fechaFinal').css('border-color', 'red');
                var mensaje = "<strong>Error!</strong> La Fecha Final debe ser mayor que la Fecha Inicial";
                MostrarMensajeValidacion(idMensaje, idPopup, mensaje, 5000);
                return false;
            } else {
                $('#fechaFinal').css('border-color', 'green');
                $("#plazo").val(end.diff(start, "days"));
                return true;
            }

        } else {
            $("#plazo").val("");
            return true;
        }

    } else {
        return false;
    }

}

function validatesupervisor() {
    $('#supervisor').css('border-color', 'green');
}

function validateAbogado() {
    $('#abogado').css('border-color', 'green');
}

function validateSupervisorTecnico() {
    $('#supervisorTecnico').css('border-color', 'green');
}

function validateCrp() {
    $('#crp').css('border-color', 'green');
}

function validateFechaCrp() {
    $('#fechaCrp').css('border-color', 'green');
}

function validateYear() {
    $('#year').css('border-color', 'green');
}

function validateCdp() {
    $('#cdp').css('border-color', 'green');
}

function validateFechaCdp() {
    $('#fechaCdp').css('border-color', 'green');
}

function validateValorContrato() {    
    return validateNumeric($("#valorContrato"), "Valor Contrato");
}

function validateValorAdministrar() {
    return validateNumeric($("#valorAdministrar"), "Recursos a Administrar");
}

function validateHonorarios() {
    var tipoContratoCIAD = $("#tipoContratoCiad").val();
    var esUnCIAD = $("#tipoContrato").val() == tipoContratoCIAD;
    console.log("--- " + tipoContratoCIAD + " == " + $("#tipoContrato").val() + "?");
    if (esUnCIAD) {
        if (validateNumeric($("#honorarios"), "Honorarios")) {
            calcularValorNetoHonorarios($("#honorarios"), $("#porcentajeIvaHonorarios"));
            return true;
        }
    } else {
        return true;
    }

    return false;
}

function validateObjeto() {
    return validateRequired($("#objeto"), "Objeto Contractual");
}

function validateObservaciones() {
    $('#observaciones').css('border-color', 'green');
}

function validarDatosPoliza() {
    
    var numeroPoliza = $("#numeroPoliza");
    var valorPoliza = $("#valorPoliza");
    var nombreAseguradora = $("#nombreAseguradora");
    var esValido = true;
    //var notasPoliza = $("#notasPoliza");

    if (numeroPoliza.val().trim() != "" || valorPoliza.val().trim() != "" || nombreAseguradora.val().trim() != "") {
        esValido = validateRequired(numeroPoliza, "Número de Póliza");

        if (esValido) {
            esValido = validateRequired(valorPoliza, "Valor de la Póliza");
        }

        if (esValido) {
            esValido = validateRequired(nombreAseguradora, "Aseguradora");
        }
    }

    console.log("- Validar datos póliza... " + esValido);

    return esValido;
}

function AgregarPago() {

    var tablaPagos = $("#tablaPagos");
    var numRows = tablaPagos.find("tbody > tr").length;
    var newIndex = numRows;
    var newRowId = "pago_" + newIndex;
    var valorPagoId = "valorPago_" + newIndex;
    var fechaPagoId = "fechaPago_" + newIndex;
    var datetimeFechaPagoId = "datetimeFechaPago_" + newIndex;
    var notasPagoId = "notasPago_" + newIndex;
    var eliminarPagoId = "eliminarPago_" + newIndex;
    var actionEliminarPago = "EliminarFilaPago(" + newIndex + ")";

    var htmlRow = '<tr id="' + newRowId + '">'
        + '<td class="border-cell col-md-1 text-right" style="font-weight: bold; font-size: 1.3em; padding-top: 10px; width: 70px">' + (newIndex + 1) + '</td>'
        + '<td class="border-cell col-md-3">'
        + '<input class="form-control" type="text" inputmode="numeric" name="' + valorPagoId + '" id="' + valorPagoId + '" />'
        + '</td>'
        + '<td class="border-cell col-md-2">'
        + '<div class="input-group date text-center" id="' + datetimeFechaPagoId + '" style="width: 280px">'
        + '<input class="form-control" type="text" name="' + fechaPagoId + '" id="' + fechaPagoId + '" autocomplete="off" />'
        + '<span class="input-group-addon">'
        + '<span class="glyphicon glyphicon-calendar"></span>'
        + '</span>'
        + '</div>'
        + '</td>'
        + '<td class="border-cell col-md-5">'
        + '<textarea class="form-control" name = "' + notasPagoId + '" id = "' + notasPagoId + '" style = "min-width: 100%"></textarea>'
        + '</td>'
        + '<td class="border-cell col-md-1" style="width: 25px" id="' + eliminarPagoId + '">'
        + '<a class="btn btn-delete" onclick="' + actionEliminarPago + '">Eliminar Pago</a>'
        + '</td>'
        + '</tr>';

    $(htmlRow).appendTo(tablaPagos);

    $("#" + datetimeFechaPagoId).datetimepicker({
        locale: 'es',
        format: 'DD/MM/YYYY',
        useCurrent: false
    });

    $("#" + valorPagoId).blur(function () {
        if ($(this).val() != "" && !isNaN($(this).val())) {
            cambiarFormatoNumerico($(this));
            return validarPagos();
        }
    });

    $("#" + fechaPagoId).blur(function () {
        if (validarFechaPago(newIndex)) {
            return validarPagos();
        }
    });

    $("#numeroPagos").val(numRows + 1);

}

function EliminarFilaPago(fila) {
    console.log("Se tratará de eliminar la fila " + fila);
    if (confirm('¿Está seguro de eliminar este pago?')) {
        var tablaPagos = $("#tablaPagos");
        var numRows = tablaPagos.find("tbody tr").length;
        $("#pago_" + fila).remove();
        $("#numeroPagos").val(numRows - 1);
    }
}

function validarPagos() {

    var esValido = true;
    var tipoContratoCIAD = $("#tipoContratoCiad").val();

    // Los pagos no pueden validarse para un contrato interadministrativo
    if ($("#tipoContrato").val() != tipoContratoCIAD) {

        console.log("Validando pagos...");

        var tablaPagos = $("#tablaPagos");
        var filasPagos = tablaPagos.find("tbody tr");
        var sumaValoresPago = 0;

        // Se valida si ya se ingresó el valor a administrar
        if (!validateValorAdministrar()) {
            $(filasPagos).each(function (filaPago) {
                $("#valorPago_" + filaPago).val("");
                $("#fechaPago_" + filaPago).val("");
            });
            return false;
        }

        // Se valida si las fechas iniciales y finales ya están
        if (!validateFechaInicial() || !validateFechaFinal()) {
            $(filasPagos).each(function (filaPago) {
                $("#valorPago_" + filaPago).val("");
                $("#fechaPago_" + filaPago).val("");
            });
            return false;
        }

        $(filasPagos).each(function (filaPago) {
            if (!validarCamposFilaPago(filaPago)) {
                esValido = false;
                return false; // Para salir del each
            } else {
                sumaValoresPago += Number($("#valorPago_" + filaPago).val().replace(/,/gi, ""));
            }
        });

        if (esValido) {

            var mensaje = "";
            var valorContrato = Number($("#valorContrato").val().replace(/,/gi, ""));

            console.log("Suma valores pago: " + sumaValoresPago);
            console.log("Valor contrato: " + valorContrato);

            if (sumaValoresPago > valorContrato) {
                $(filasPagos).each(function (filaPago) {
                    if (Number($("#valorPago_" + filaPago).val().replace(/,/gi, "")) > 0) {
                        $("#valorPago_" + filaPago).css('border-color', 'red');
                    }
                });
                mensaje = "<strong>Error!</strong> La sumatoria de valores de pago no debe ser mayor al valor del contrato";
                MostrarMensajeValidacion(idMensaje, idPopup, mensaje, 5000);
                esValido = false;
            } else {
                var hayVacios = false;
                $(filasPagos).each(function (filaPago) {
                    if ($("#valorPago_" + filaPago).val() == "") {
                        $("#valorPago_" + filaPago).css('border-color', 'red');
                        hayVacios = true;
                        return false;
                    }
                });
                if (hayVacios) {
                    mensaje = "<strong>Error!</strong> Existe al menos un valor de pago sin especificar.";
                    MostrarMensajeValidacion(idMensaje, idPopup, mensaje, 5000);
                    esValido = false;
                }
            }

        }

    }

    console.log("- Validar pagos... " + esValido);
    return esValido;
}

function validarCamposFilaPago(fila) {

    var valorPago = $("#valorPago_" + fila);
    var numeroFila = fila + 1;

    var esValido = validateNumeric(valorPago, "Valor del Pago " + numeroFila);
    if (esValido) esValido = validarFechaPago(fila);

    console.log("Validación de la fecha de pago: " + esValido);

    return esValido;
}

function validarFechaPago(fila) {

    var numeroFila = fila + 1;
    var fechaPago = $("#fechaPago_" + fila);
    var nombreCampo = "Fecha del Pago " + numeroFila;

    if (!validateRequired(fechaPago, nombreCampo)) return false;

    var esValida = true;
    var mensaje = "";
    
    var start = moment(stringToDate($("#fechaInicial").val(), "dd/MM/yyyy", "/"));
    var laFechaPago = moment(stringToDate(fechaPago.val(), "dd/MM/yyyy", "/"));
    var end = moment(stringToDate($("#fechaFinal").val(), "dd/MM/yyyy", "/"));

    if (laFechaPago < start || laFechaPago > end) {
        mensaje = "debe estar entre la Fecha Inicial y la Fecha Final del contrato";
        esValida = false;
    }

    if (esValida) {

        var tablaPagos = $("#tablaPagos");
        var filasPagos = tablaPagos.find("tbody tr");

        // Si la fecha está dentro del rango, se debe comparar con las demás fechas insertadas
        if (filasPagos.length > 1) {

            var fechasIguales = false;

            $(filasPagos).each(function (filaPago) {
                if (filaPago != fila) {
                    var fechaPagoActual = $("#fechaPago_" + filaPago);
                    var laFecha = moment(stringToDate(fechaPagoActual.val(), "dd/MM/yyyy", "/"));
                    if (fechaPago == laFecha) {
                        fechasIguales = true;
                        return false; // Para salir del each
                    }
                }
            });

            if (fechasIguales) {
                mensaje = "no debe ser igual a la fecha de otro pago.";
                esValida = false;
            }

            // Si no hay fechas iguales, se valida que la fecha seleccionada no sea menor a la anterior y mayor a la del siguiente pago
            if (esValida) {

                var fechaPagoAnterior = (fila == 0) ? null : $("#fechaPago_" + (fila - 1));
                var fechaSiguientePago = (fila >= filasPagos.length - 1) ? null : $("#fechaPago_" + (fila + 1));

                if (fechaPagoAnterior != null && fechaPagoAnterior.val() != "") {
                    var laFechaAnterior = moment(stringToDate(fechaPagoAnterior.val(), "dd/MM/yyyy", "/"));
                    if (laFechaPago <= laFechaAnterior) {
                        mensaje = "no debe ser menor o igual a la fecha del pago anterior.";
                        esValida = false;
                    }
                }

                if (esValida && fechaSiguientePago != null && fechaSiguientePago.val() != "") {
                    var laFechaSiguiente = moment(stringToDate(fechaSiguientePago.val(), "dd/MM/yyyy", "/"));
                    if (laFechaSiguiente <= laFechaPago) {
                        mensaje = "no debe ser mayor o igual a la fecha del siguiente pago.";
                        esValida = false;
                    }
                }

            }

        }

    }

    if (esValida) {
        fechaPago.css('border-color', 'green');
    } else {
        fechaPago.val("");
        fechaPago.css('border-color', 'red');
        mensaje = "<strong>Error!</strong> La " + nombreCampo + " " + mensaje;
        MostrarMensajeValidacion(idMensaje, idPopup, mensaje, 5000);
    }

    return esValida;
}

$(function () {

    $("#btnEnviar").on("click", function (e) {

        e.preventDefault(); // Cancela la acción por defecto

        var esValido = validarTodosLosCamposGenerales();
        if (esValido) esValido = validarPagos();
        if (esValido) esValido = validarDatosPoliza();

        console.log("¿Es válido? " + esValido);

        if (esValido) {
            $("form")[0].submit();
        }

    });

});
