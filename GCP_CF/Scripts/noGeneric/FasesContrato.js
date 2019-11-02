var idMensaje = "textoMensaje";
var idPopup = "mensajeAccion";

function MostrarMensajeAccion(mensaje, claseMensaje, timeout) {
    $("#mensajeAccion > div.modal-dialog").addClass(claseMensaje);
    $("#mensajeAccion").modal("show");
    $("#textoMensaje").html(mensaje);
    setTimeout(function () {
        $('#mensajeAccion').modal('hide');
    }, timeout);
}

function ResetMensajeAccion() {
    $("#mensajeAccion > div.modal-dialog").removeClass("alert-Succes").removeClass("alert-Information");
}

function VerFasesContrato(IdContrato) {
    console.log("Solicitando fases para el contrato " + IdContrato);
    $(location).attr("href", "/Seguimiento/FasesContrato?idContrato=" + IdContrato);
}

function ObtenerListadoFasesContrato(idContrato, puedeEscribir) {

    var cargaFases = $("#cargaFases");
    var listadoFases = $("#listadoFases");
    var tblListadoFases = $("#tblListadoFases");

    $(listadoFases).hide();
    $(cargaFases).show();

    $.ajax({
        async: false,
        url: "/Seguimiento/ObtenerListadoFasesContrato",
        type: "POST",
        data: JSON.stringify({ idContrato: idContrato }),
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.fases && data.fases.length > 0) {       
                $(tblListadoFases).html("");
                var tblHeader = "<thead><tr>"
                    + "<th class=\"col-md-1 col-sm-1 col-xs-1\">No.</th>"
                    + "<th class=\"col-md-10 col-sm-10 col-xs-10\">Fase</th>"
                    //+ (puedeEscribir ? "<th class=\"col-md-1 col-sm-1 col-xs-1 text-center\">Acciones</th>" : "")
                    + "<th class=\"col-md-1 col-sm-1 col-xs-1 text-center\">Acciones</th>"
                    + "</tr></thead>"
                    + "<tbody>";
                $("#tblListadoFases").append(tblHeader);
                var i = 0;
                $.each(data.fases, function (key, entry) {
                    i++;
                    var tblRow = "<tr>"
                        + "<td class=\"col-md-1 col-sm-1 col-xs-1\" style=\"vertical-align: top\"><span style=\"padding: 6px 1px\">" + i + "</span></td>"
                        + "<td class=\"col-md-10 col-sm-10 col-xs-10\" style=\"vertical-align: top\">"
                        + "<div class=\"row row-no-gutters\">"
                        + "<div class=\"col-md-1 col-sm-1 col-xs-1\" style=\"width: 26px; display: table-cell\">"
                        + "<button style =\"padding: 1px 2px; margin: 3px 0px\" type=\"button\" class=\"btn btn-secondary btn-sm\" title=\"Mostrar actividades\" onclick=\"VerActividadesFase(" + idContrato + ", " + entry.Id + ")\">"
                        + "<i class=\"glyphicon glyphicon-plus-sign\" id=\"toggle_" + idContrato + "_" + entry.Id + "\"></i></button>"
                        + "</div>"
                        + "<div id=\"fc_descripcion_" + entry.Id + "\" style=\"margin: 4px 0px\">" + entry.Descripcion.trim() + "</div>"
                        + "</div>"
                        + "<div class=\"row row-no-gutters\">"
                        + "<div id=\"regionActividadesC" + idContrato + "F" + entry.Id + "\" style=\"display: none\">";

                    if (entry.Actividades && entry.Actividades.length > 0) {
                        tblRow += ProcesarActividadesFase(entry.Actividades, idContrato, entry.Id, puedeEscribir)
                    } else {
                        tblRow += "<div class=\"alert alert-warning\" style=\"padding: 4px; margin-bottom: 6px\">No hay actividades asociadas a esta fase</div>";
                    }

                    tblRow += "</div>"
                        + "</td>";

                    //if (puedeEscribir) {
                        tblRow += "<td class=\"col-md-1 col-sm-1 col-xs-1 text-center\">"
                            + "<a class=\"btn-details\" href=\"javascript:void(0)\" title=\"Agregar una actividad\" onclick=\"AgregarActividadFase(" + idContrato + ", " + entry.Id + ")\">Actividades</a>"
                            + "<a class=\"btn-delete\" href=\"javascript:void(0)\" title=\"Eliminar\" onclick=\"EliminarFaseContrato(" + idContrato + ", " + entry.Id + ")\">Eliminar</a>"
                            + "</td>";
                   // }

                    tblRow += "</tr>";
                    $(tblListadoFases).append(tblRow);
                });
                $(tblListadoFases).append($("</tbody>"));
            } else {
                $(tblListadoFases).html("");
                $(tblListadoFases).append($("<tbody>"));
                $(tblListadoFases).append($("<tr><td class=\"alert-info\">" + data.mensaje + "</td></tr>"));
                $(tblListadoFases).append($("</tbody>"));
                //console.log(data.detalle);
            }
        },
        error: function () {
            $(tblListadoFases).html("");
            $(tblListadoFases).append($("<tbody>"));
            $(tblListadoFases).append($("<tr><td class=\"alert-danger\">Ha ocurrido un error al cargar las fases del contrato</td></tr>"));
            $(tblListadoFases).append($("</tbody>"));
        }
    });

    $(cargaFases).hide();
    $(listadoFases).fadeIn();
}

function VerActividadesFase(idContrato, idFase) {

    var toggleButton = $("#toggle_" + idContrato + "_" + idFase);
    var toggleRegion = $("#regionActividadesC" + idContrato + "F" + idFase);

    if ($(toggleRegion).css("display") != "block") {
        $(toggleButton).attr("title", "Ocultar actividades");
        $(toggleButton).removeClass("glyphicon-plus-sign");
        $(toggleButton).addClass("glyphicon-minus-sign");
    } else {
        $(toggleButton).attr("title", "Mostrar actividades");
        $(toggleButton).removeClass("glyphicon-minus-sign");
        $(toggleButton).addClass("glyphicon-plus-sign");
    }

    $(toggleRegion).toggle(500);
}

function ProcesarActividadesFase(actividades, idContrato, idFase, puedeEscribir) {

    var tblActividades = "<table width=\"100%\" id=\"tblActividadesFase_C" + idContrato + "_F" + idFase + "\">"
        + "<thead>"
        + "<tr>"
        + "<th class=\"col-md-1 col-sm-1 col-xs-1\">No.</th>"
        + "<th class=\"col-md-4 col-sm-4 col-xs-4\">Descripción</th>"
        + "<th class=\"col-md-2 col-sm-2 col-xs-2\">Fecha Inicio</th>"
        + "<th class=\"col-md-2 col-sm-2 col-xs-2\">Fecha Final</th>"
        + "<th class=\"col-md-2 col-sm-2 col-xs-2\">Estado</th>"
        + (puedeEscribir ? "<th class=\"col-md-1 col-sm-2 col-xs-1\" style=\"padding: 4px\">Acciones</th>" : "")
        + "</tr>"
        + "</thead>"
        + "<tbody>";

    for (var i = 1; i <= actividades.length; i++) {
        var ac = actividades[i - 1];
        var formatFechaInicio = FormatoFecha(ac.FechaInicio, "/");
        var formatFechaFinal = FormatoFecha(ac.FechaFinal, "/");

        tblActividades += "<tr id=\"rowActividadFase_C" + idContrato + "_F" + idFase + "_A" + ac.Id + "\">"
            + "<td class=\"col-md-1 col-sm-1 col-xs-1\">" + i + "</td>"
            + "<td class=\"col-md-5 col-sm-5 col-xs-5\" id=\"descripcion_act_" + ac.Id + "\">" + ac.Descripcion + "</td>"
            + "<td class=\"col-md-2 col-sm-2 col-xs-2\">" + formatFechaInicio + "</td>"
            + "<td class=\"col-md-2 col-sm-2 col-xs-2\">" + formatFechaFinal + "</td>"
            + "<td class=\"col-md-2 col-sm-2 col-xs-2\">" + ac.DescripcionEstado + "</td>";

        if (puedeEscribir) {
            tblActividades += "<td class=\"col-md-1 col-sm-1 col-xs-1\" style=\"padding: 4px\">"
                + "<a class=\"btn-edit\" href=\"javascript:void(0)\" title=\"Editar actividad\" onclick=\"EditarActividadFase(" + ac.Id + ", " + ac.IdContrato + ", " + ac.IdFase + ")\">Editar</a>"
                + "<a class=\"btn-delete\" href=\"javascript:void(0)\" title=\"Eliminar\" onclick=\"EliminarActividadFase(" + ac.Id + ", " + ac.IdContrato + ", " + ac.IdFase + ")\">Eliminar</a>"
                + "</td>"
        }

        tblActividades += "</tr>";
    }

    tblActividades += "</tbody>"
        + "</table>";

    return tblActividades;
}

function AgregarFaseContrato(idContrato) {
    $("#agregarFaseContrato").modal("show");
    CargarFasesDisponibles(idContrato);
}

function AgregarActividadFase(idContrato, idFase) {
  
    $("#addAct_IdActividad").val("-1");
    $("#addAct_IdContrato").val(idContrato);
    $("#addAct_IdFase").val(idFase);
    $("#ActividadesEtapasId").val(null);
    $("#ActividadesEtapasId").empty();
    $("#ActividadesEtapasId").append('<option value="0">Seleccione...</option>');
    $.ajax({
    type: "POST",
    url: "/Seguimiento/GetActividades",
    dataType: 'json',
    data: { Fase: idFase },   
    success: function (data) {
        $.each(data, function (i, data) {
            $("#ActividadesEtapasId").append('<option value='
                + data.ActividadesEtapasId + '>'
                + data.Descripción + '</option>');
        });

    }, error: function () {
        mensaje = "Ha ocurrido un error interno al intentar eliminar la fase seleccionada.";
        claseMensaje = "alert-danger";
    }
})

    $("#agregarActividadFase").modal("show");
}

function GuardarActividadFase() {

    var mensaje = "";
    var claseMensaje = "";

    ResetMensajeAccion();

    var item = $("#item").val();
    var descripcion = $("#descripcion").val();
    var diasHabiles = $("#diasHabiles").val();
    var fechaInicio = $("#fechaInicio").val();
    var fechaFin = $("#fechaFinal").val();
    var estadoActividad = $("#estadoActividad").val();
    var idActividad = $("#addAct_IdActividad").val();
    var idContrato = $("#addAct_IdContrato").val();
    var idFase = $("#addAct_IdFase").val();
    var ActividadesEtapasId =$("#ActividadesEtapasId").val()

    if ($("#AgregarActividadFaseForm").valid()) {

        var mensajeConfirmacion = "¿Está seguro de agregar la actividad a la fase seleccionada?";
        if (idActividad != "-1") {
            mensajeConfirmacion = "¿Está seguro de actualizar la actividad seleccionada?"
        }

        if (confirm(mensajeConfirmacion)) {

            console.log("OK. Se procede a enviar los datos...");
            CerrarModalActividadFase();

            $.ajax({
                async: false,
                url: "/Seguimiento/GuardarActividadFase",
                type: "POST",
                data: JSON.stringify({
                    idActividad: idActividad, idContrato: idContrato, idFase: idFase, item: item,
                    descripcion: descripcion, diasHabiles: diasHabiles, fechaInicio: fechaInicio,
                    fechaFin: fechaFin, estadoActividad: estadoActividad, ActividadesEtapasId: ActividadesEtapasId
                }),
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    $("#loadingMessage").text("Guardando la información...");
                    $("#popUpModalMessage").modal("show");
                }, 
                complete: function () {
                    $("#popUpModalMessage").modal("hide");
                }, 
                success: function (data) {
                    jsonData = jQuery.parseJSON(data);
                    console.log(jsonData);
                    if (jsonData.error.trim().length > 0) {
                        mensaje = jsonData.error.trim();
                        claseMensaje = "alert-Error";
                    } else {
                        mensaje = jsonData.mensaje.trim();
                        claseMensaje = "alert-Succes";
                    }
                }, error: function (xhr, textStatus, errorThrown) {
                    console.log("ERROR: " + textStatus + " - " + errorThrown);
                    mensaje = "Ha ocurrido un error interno al intentar eliminar la fase seleccionada.";
                    claseMensaje = "alert-Error";
                }
            });

            ObtenerListadoFasesContrato(idContrato); // Refrescar el listado
            MostrarMensajeAccion(mensaje, claseMensaje, 5000); // Mostrar mensaje
        }
    }
}

function EditarActividadFase(idActividad, idContrato, idFase) {

    LimpiarModalActividadFase();

    var mensaje = "";
    var claseMensaje = "";

    ResetMensajeAccion();

    $.ajax({
        async: false,
        url: "/Seguimiento/ObtenerActividadFase",
        type: "POST",
        data: JSON.stringify({
            idActividad: idActividad
        }),
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.actividadFase != null) {
                // Llenar los datos de la modal
                $("#item").val(data.actividadFase.Item);
                $("#descripcion").val(data.actividadFase.Descripcion);
                $("#fechaInicio").val(FormatoFecha(data.actividadFase.FechaInicio, "/"));
                $("#fechaFinal").val(FormatoFecha(data.actividadFase.FechaFinal, "/"));
                $("#diasHabiles").val(data.actividadFase.DiasEntreFechas);
                $("#estadoActividad").val(data.actividadFase.CodigoEstado);
                $("#addAct_IdActividad").val(data.actividadFase.Id);
                $("#addAct_IdContrato").val(idContrato);
                $("#addAct_IdFase").val(idFase);
                $("#agregarActividadFase").modal("show");
            } else {
                mensaje = "La actividad seleccionada no existe.";
                claseMensaje = "alert-Information";
            }
        }, error: function (xhr, textStatus, errorThrown) {
            console.log("ERROR: " + textStatus + " - " + errorThrown);
            mensaje = "Ha ocurrido un error interno al consultar la actividad seleccionada.";
            claseMensaje = "alert-Error";
        }
    });

    if (mensaje != "") {
        MostrarMensajeAccion(mensaje, claseMensaje, 5000); // Mostrar mensaje
    }
}

function EliminarActividadFase(idActividad, idContrato, idFase) {

    if (confirm("¿Está seguro de eliminar la actividad " + $("#descripcion_act_" + idActividad).text().trim() + " de la fase actual?")) {

        ResetMensajeAccion();

        var mensaje = "";
        var claseMensaje = "";

        var idTabla = "#tblActividadesFase_C" + idContrato + "_F" + idFase;
        var idFila = "#rowActividadFase_C" + idContrato + "_F" + idFase + "_A" + idActividad;
        var emptyDiv = "#empty_C" + idContrato + "_F" + idFase;
        
        $.ajax({
            async: false,
            url: "/Seguimiento/EliminarActividadFase",
            type: "POST",
            data: JSON.stringify({ idActividad: idActividad }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                jsonData = jQuery.parseJSON(data);
                console.log(jsonData);
                if (jsonData.error.trim().length > 0) {
                    mensaje = jsonData.error.trim();
                    claseMensaje = "alert-Error";
                } else {
                    mensaje = jsonData.mensaje.trim();
                    claseMensaje = "alert-Succes";
                    $(idFila).fadeOut();
                    $(idFila).remove();
                    if ($(idTabla).find("> tbody > tr").length == 0) {
                        var newEmptyDiv = "<div id=\"empty_C" + idContrato + "_F" + idFase + "\" class=\"alert alert-warning\" style=\"padding: 4px; margin-bottom: 6px; display: none\">No hay actividades asociadas a esta fase</div>";
                        $(idTabla).parent().html(newEmptyDiv);
                        $(idTabla).fadeOut();
                        $(idTabla).remove();
                        $(emptyDiv).fadeIn();
                    }
                }
            }, error: function () {
                mensaje = "Ha ocurrido un error interno al intentar eliminar la actividad seleccionada.";
                claseMensaje = "alert-Error";
            }
        });

        if (mensaje != "") {
            MostrarMensajeAccion(mensaje, claseMensaje, 5000); // Mostrar mensaje
        }
    }
}

function EliminarFaseContrato(idContrato, idFase) {

    var mensaje = "";
    var claseMensaje = "";

    ResetMensajeAccion();

    if (idFase == "" || idFase == "-1") {
        alert("Por favor seleccione una fase para eliminar del contrato");
        return false;
    } else {
        if (confirm("¿Está seguro de eliminar la fase " + $("#fc_descripcion_" + idFase).text().trim() + " del contrato actual?")) {
            $.ajax({
                async: false,
                url: "/Seguimiento/EliminarFaseContrato",
                type: "POST",
                data: JSON.stringify({ idContrato: idContrato, idFase: idFase }),
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    jsonData = jQuery.parseJSON(data);
                    console.log(jsonData);
                    if (jsonData.error.trim().length > 0) {
                        mensaje = jsonData.error.trim();
                        claseMensaje = "alert-Error";
                    } else {
                        mensaje = jsonData.mensaje.trim();
                        claseMensaje = "alert-Succes";
                    }
                }, error: function () {
                    mensaje = "Ha ocurrido un error interno al intentar eliminar la fase seleccionada.";
                    claseMensaje = "alert-danger";
                }
            })

            ObtenerListadoFasesContrato(idContrato); // Refrescar el listado
            MostrarMensajeAccion(mensaje, claseMensaje, 5000); // Mostrar mensaje
        }
    }
}

function LimpiarModalFaseContrato() {
    $("#selectGroup").hide();
    $("#mensajeError").text("");
    $("#mensajeError").hide();
    $("#idFaseDisponible option").remove();
}

function CerrarModalFaseContrato() {
    LimpiarModalFaseContrato();
    $("#agregarFaseContrato").modal("hide");
}

function CerrarModalActividadFase() {
    LimpiarModalActividadFase();
    $("#agregarActividadFase").modal("hide");
}

function LimpiarModalActividadFase() {
    $("#item").val("");
    $("#descripcion").val("");
    $("#fechaInicio").val("");
    $("#fechaFinal").val("");
    $("#diasHabiles").val("");
    $("#estadoActividad").val("");
    $("#addAct_IdContrato").val("");
    $("#addAct_IdFase").val("");
}

function CargarFasesDisponibles(idContrato) {

    LimpiarModalFaseContrato();

    selectGroup = $("#selectGroup");
    fasesDisponibles = $("#idFaseDisponible");
    mensajeError = $("#mensajeError");

    $.ajax({
        async: false,
        url: "/Seguimiento/ObtenerFasesNoAsociadas",
        type: "POST",
        data: JSON.stringify({ idContrato: idContrato }),
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            jsonData = jQuery.parseJSON(data);
            if (jsonData.fases.length > 0 && jsonData.fases[0].fase_Id) {
                $(fasesDisponibles).append($("<option></option>").attr("value", "-1").text("Seleccione..."));
                $(selectGroup).fadeIn();
                $.each(jsonData.fases, function (key, entry) {
                    $(fasesDisponibles).append($("<option></option>").attr("value", entry.fase_Id).text(entry.Descripcion));
                });
            } else {
                console.log("AQUI");
                $(mensajeError).text("No hay más fases para agregar a este contrato.");
                $(mensajeError).fadeIn();
            }
        }, 
        error: function () {
            $(mensajeError).text("Ha ocurrido un error al cargar la lista.");
            $(mensajeError).fadeIn();
        }
    });
}

function AgregarFaseSeleccionada(idContrato) {

    var mensaje = "";
    var claseMensaje = "";
    var idFase = $("#idFaseDisponible option:selected").val();
    var textoFase = $("#idFaseDisponible option:selected").text();

    ResetMensajeAccion();

    if (idFase == "" || idFase == "-1") {
        alert("Por favor seleccione una fase para agregar al contrato");
        return false;
    } else {
        if (confirm("¿Está seguro de agregar la fase " + textoFase.trim() + " al contrato actual?")) {
            $.ajax({
                async: false,
                url: "/Seguimiento/GuardarFaseContrato",
                type: "POST",
                data: JSON.stringify({ idContrato: idContrato, idFase: idFase }),
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    jsonData = jQuery.parseJSON(data);
                    console.log(jsonData);
                    if (jsonData.error.trim().length > 0) {
                        mensaje = jsonData.error.trim();
                        claseMensaje = "alert-Error";
                    } else {
                        mensaje = jsonData.mensaje.trim();
                        claseMensaje = "alert-Succes";
                    }
                }
            });

            ObtenerListadoFasesContrato(idContrato); // Refrescar el listado
            CerrarModalFaseContrato();
        }
    }

    if (mensaje != "") {
        MostrarMensajeAccion(mensaje, claseMensaje, 5000); // Mostrar mensaje
    }
}

function NoImplementado() {
    alert("Función no implementada por el momento");
}

$(function () {
    $.validator.addMethod(
        "notEqualTo",
        function (value, element, arg) {
            return arg != value;
        },
        "Valor no debe ser igual al argumento."
    );

    $("#AgregarActividadFaseForm").validate({
        rules: {
            "actividadFase.Item": {
                required: true
            },
            "actividadFase.Descripción": {
                required: true
            },
            "actividadFase.FechaInicio": {
                required: true
            },
            "actividadFase.FechaFinal": {
                required: true
            },
            "actividadFase.DiasHabiles": {
                required: true,
                digits: true
            },
            "EstadoActividad_Id": {
                notEqualTo: ""
            }
        },
        messages: {
            "actividadFase.Item": {
                required: "Por favor escriba el ítem."
            },
            "actividadFase.Descripción": {
                required: "Por favor escriba la descripción."
            },
            "actividadFase.FechaInicio": {
                required: "Debe seleccionar la fecha inicial."
            },
            "actividadFase.FechaFinal": {
                required: "Debe seleccionar la fecha final."
            },
            "actividadFase.DiasHabiles": {
                digits: "Solamente debe ingresar dígitos."
            },
            "EstadoActividad_Id": {
                notEqualTo: "Por favor seleccione un estado."
            }
        }
    });
    
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || moment(value, true).isValid();
    }

    var fechaActual = new Date();
    fechaActual.setHours(23, 59, 59, 0);

    $('#datetimeFechaInicio').datetimepicker({
        locale: 'es',
        format: 'DD/MM/YYYY',
        useCurrent: false
    });

    $('#datetimeFechaFinal').datetimepicker({
        locale: 'es',
        format: 'DD/MM/YYYY',
        useCurrent: false
    });

    $("#fechaFinal").blur(function () {
        var start = moment(stringToDate($("#fechaInicio").val(), "dd/MM/yyyy", "/"));
        var end = moment(stringToDate($("#fechaFinal").val(), "dd/MM/yyyy", "/"));
        $("#diasHabiles").val(end.diff(start, "days"));
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
        var formattedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
        return formattedDate;
    }

    $("#valorEjecutado").blur(function () {
        if ($(this).val() != "" && !isNaN($(this).val())) {
            cambiarFormatoNumerico($("#valorEjecutado"));
        }
    });
});

// Actualizar valor ejecutado
$("#btnActualizarEjecucion").click(function () {

    var porcentajeEjecucion = 0;
    var fmtValorEjecutado = $("#valorEjecutado").val();
    var idContrato = $("#idContratoValorEjecutado").val();
    var mensaje = "";
    var claseMensaje = "";

    ResetMensajeAccion();

    if (validateNumeric($("#valorEjecutado"), "Valor Ejecutado")) {
        var valorEjecutado = fmtValorEjecutado.replace(/,/gi, "").toString();
        console.log(idContrato + ", " + valorEjecutado);
        $.ajax({
            async: false,
            url: "/Seguimiento/ActualizarValorEjecutado",
            type: "POST",
            data: JSON.stringify({ idContrato: idContrato, valorEjecutado: Number(valorEjecutado) }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                jsonData = jQuery.parseJSON(data);
                console.log(jsonData);
                if (jsonData.error != null && jsonData.error.length > 0) {
                    mensaje = jsonData.error;
                    claseMensaje = "alert-Error";
                } else {
                    porcentajeEjecucion = jsonData.porcentajeEjecucion;
                    mensaje = jsonData.mensaje;
                    claseMensaje = "alert-Succes";
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                mensaje = "Ha ocurrido un error al intentar actualizar el valor ejecutado del contrato: " + errorThrown;
                claseMensaje = "alert-Error";
            }
        });

        $("#textoPorcentajeEjecutado").text(porcentajeEjecucion + "%");
        $("#porcentajeValorEjecutado").attr("aria-valuenow", porcentajeEjecucion);
        $("#porcentajeValorEjecutado").attr("style", "width: " + porcentajeEjecucion + "%");

        if (mensaje != "") {
            MostrarMensajeAccion(mensaje, claseMensaje, 5000); // Mostrar mensaje
        }
    }
});