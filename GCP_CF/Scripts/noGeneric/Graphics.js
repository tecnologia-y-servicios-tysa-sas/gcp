function CargarReporte(width, height, target, msgTarget) {

    var textoFallido = "No se pudo cargar la imagen";
    var url = "/Graphics/PresupuestoPorAnio";

    $('<img src="' + url + '">').on("load", function () {
        $(msgTarget).fadeOut();
        $(this).width(width).height(height).appendTo(target);
        $(target).fadeIn();
    });
}

$(document).ready(function () {
    CargarReporte(600, 400, "#imgReportePresupuesto", "#msgReportePresupuesto");
})