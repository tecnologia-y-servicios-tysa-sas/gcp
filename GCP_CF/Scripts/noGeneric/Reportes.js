$(function () {
    $("#filtrosReporteContratosForm").submit(function () {
        NoDisponible();
        return false;
    });
});

function NoDisponible() {
    alert("Por ahora esta funcionalidad no se encuentra disponible.");
}