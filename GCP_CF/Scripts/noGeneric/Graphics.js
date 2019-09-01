function CargarReporte(url, width, height, target, msgTarget) {

    var textoFallido = "No se pudo cargar la imagen";

    $('<img src="' + url + '">').on("load", function () {
        $(msgTarget).fadeOut();
        $(this).width(width).height(height).appendTo(target);
        $(target).fadeIn();
    });
}

$(document).ready(function () {
    CargarReporte("/Graphics/PresupuestoPorAnio", 600, 400, "#imgReportePresupuesto", "#msgReportePresupuesto");
    CargarReporte("/Graphics/ValorContratadoDirectamentePorAnio", 600, 400, "#imgReporteValorContratado", "#msgReporteValorContratado");
    CargarReporte("/Graphics/PresupuestoPorMunicipio", 600, 400, "#imgReportePresupuestoMpio", "#msgReportePresupuestoMpio");
})