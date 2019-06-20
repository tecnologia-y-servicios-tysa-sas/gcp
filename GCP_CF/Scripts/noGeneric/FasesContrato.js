function VerFasesContrato(IdContrato) {
    console.log("Solicitando fases para el contrato " + IdContrato);
    $(location).attr("href", "/Seguimiento/FasesContrato?idContrato=" + IdContrato);
}

function ObtenerListadoFasesContrato(idContrato) {

    var tblListadoFases = $("#tblListadoFases");
    $(tblListadoFases).html("");
    $(tblListadoFases).append("<tbody><tr><td class=\"alert-warning\">Cargando fases...</td></tr><tbody>");

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
                    + "<th class=\"col-md-1\">ID</th>"
                    + "<th class=\"col-md-10\">Fase</th>"
                    + "<th class=\"col-md-1 text-center\">Acciones</th>"
                    + "</tr></thead>"
                    + "<tbody>";
                $("#tblListadoFases").append(tblHeader);
                var i = 0;
                $.each(data.fases, function (key, entry) {
                    i++;
                    var tblRow = "<tr>"
                        + "<td class=\"col-md-1\">" + i + "</td>"
                        + "<td class=\"col-md-10\"><span id=\"fc_descripcion_" + entry.fase_Id + "\">" + entry.Descripcion.trim() + "</span></td>"
                        + "<td class=\"col-md-1 text-center\">"
                        + "<a class=\"btn-details\" href=\"javascript:void(0)\" title=\"Agregar una actividad\" onclick=\"AgregarActividadFase(" + idContrato + ", " + entry.fase_Id + ")\">Actividades</a>"
                        + "<a class=\"btn-delete\" href=\"javascript:void(0)\" title=\"Eliminar\" onclick=\"EliminarFaseContrato(" + idContrato + ", " + entry.fase_Id + ")\">Eliminar</a>"
                        + "</td>"
                        + "</tr>";
                    $(tblListadoFases).append(tblRow);
                });
                $(tblListadoFases).append($("</tbody>"));
            } else {
                $(tblListadoFases).html("");
                $(tblListadoFases).append($("<tbody>"));
                $(tblListadoFases).append($("<tr><td class=\"alert-info\">" + data.mensaje + "</td></tr>"));
                $(tblListadoFases).append($("</tbody>"));
                console.log(data.detalle);
            }
        },
        error: function () {
            $(tblListadoFases).html("");
            $(tblListadoFases).append($("<tbody>"));
            $(tblListadoFases).append($("<tr><td class=\"alert-danger\">Ha ocurrido un error al cargar las fases del contrato</td></tr>"));
            $(tblListadoFases).append($("</tbody>"));
        }
    });
}

function AgregarFaseContrato(idContrato) {
    $("#agregarFaseContrato").modal("show");
    CargarFasesDisponibles(idContrato);
}

function VerActividadesFase(idContrato) {
    NoImplementado();
}

function AgregarActividadFase(idContrato, idFase) {
    $("#addAct_IdContrato").val(idContrato);
    $("#addAct_IdFase").val(idFase);
    $("#agregarActividadFase").modal("show");
}

function GuardarActividadFase() {

    //debugger

    var mensaje = "";
    var claseMensaje = "";

    $("#mensajeAccion").hide();
    $("#textoMensaje").removeClass("alert-info").removeClass("alert-warning").text("");

    var item = $("#item").val();
    var descripcion = $("#descripcion").val();
    var diasHabiles = $("#diasHabiles").val();
    var fechaInicio = $("#fechaInicio").val();
    var fechaFin = $("#fechaFinal").val();
    var estadoActividad = $("#estadoActividad").val();
    var idContrato = $("#addAct_IdContrato").val();
    var idFase = $("#addAct_IdFase").val();

    if (confirm("¿Está seguro de agregar la actividad a la fase seleccionada?")) {
        $.ajax({
            async: false,
            url: "/Seguimiento/GuardarActividadFase",
            type: "POST",
            data: JSON.stringify({
                idContrato: idContrato, idFase: idFase, item: item, descripcion: descripcion,
                diasHabiles: diasHabiles, fechaInicio: fechaInicio, fechaFin: fechaFin,
                estadoActividad: estadoActividad
            }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                jsonData = jQuery.parseJSON(data);
                console.log(jsonData);
                if (jsonData.error.trim().length > 0) {
                    mensaje = jsonData.error.trim();
                    claseMensaje = "alert-danger";
                } else {
                    mensaje = jsonData.mensaje.trim();
                    claseMensaje = "alert-info";
                }
            }, error: function (xhr, textStatus, errorThrown) {
                console.log("ERROR: " + textStatus + " - " + errorThrown);
                mensaje = "Ha ocurrido un error interno al intentar eliminar la fase seleccionada.";
                claseMensaje = "alert-danger";
            }
        });
    }

    ObtenerListadoFasesContrato(idContrato); // Refrescar el listado
    CerrarModalActividadFase();
    $("#mensajeAccion").addClass(claseMensaje).fadeIn();
    $("#textoMensaje").html(mensaje);
}

function EliminarFaseContrato(idContrato, idFase) {

    var mensaje = "";
    var claseMensaje = "";

    $("#mensajeAccion").hide();
    $("#textoMensaje").removeClass("alert-info").removeClass("alert-warning").text("");

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
                        claseMensaje = "alert-danger";
                    } else {
                        mensaje = jsonData.mensaje.trim();
                        claseMensaje = "alert-info";
                    }
                }, error: function () {
                    mensaje = "Ha ocurrido un error interno al intentar eliminar la fase seleccionada.";
                    claseMensaje = "alert-danger";
                }
            })

            ObtenerListadoFasesContrato(idContrato); // Refrescar el listado
            $("#mensajeAccion").addClass(claseMensaje).fadeIn();
            $("#textoMensaje").html(mensaje);
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
    // Limpiar formulario
    $("#agregarActividadFase").modal("hide");
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

    $("#mensajeAccion").hide();
    $("#textoMensaje").removeClass("alert-info").removeClass("alert-warning").text("");

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
                        claseMensaje = "alert-danger";
                    } else {
                        mensaje = jsonData.mensaje.trim();
                        claseMensaje = "alert-info";
                    }
                }
            });

            ObtenerListadoFasesContrato(idContrato); // Refrescar el listado
            CerrarModalFaseContrato();
            $("#mensajeAccion").addClass(claseMensaje).fadeIn();
            $("#textoMensaje").html(mensaje);
        }
    }
}

function NoImplementado() {
    alert("Función no implementada por el momento");
}