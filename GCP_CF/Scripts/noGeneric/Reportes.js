function ExportarContratos() {
    var newUrl = "/Reportes/ExportarReporteContratos";
    var oldUrl = "/Reportes/Contratos";
    var form = $("#filtrosReporteContratosForm");

    $(form).attr("action", newUrl);
    $(form).submit();

    $(form).attr("action", oldUrl);
}

function NoDisponible() {
    alert("Por el momento esta funcionalidad no se encuentra disponible.");
}