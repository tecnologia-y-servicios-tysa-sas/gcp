function validarAnio() {
    if ($('#anioContrato').val() == "") {
        $('#anioContrato').css('border-color', 'red');
        $("#mensaje").html('<div><strong>¡Error!</strong> Debe seleccionar un año</div>');
        $("#modalValidacion").modal('show');
        setTimeout(function () {
            $('#modalValidacion').modal('hide');
        }, 5000);
        return false;
    }
}

$(function () {

    $("#anioContrato").change(function () {
        $("#anioContrato").css('border-color', $("#anioContrato").val() == "" ? 'red' : "green");
    });

    $("#filtrosReporteContratosForm").submit(function () {
        return validarAnio();
    });
});

function ExportarContratos() {
    NoDisponible();
}

function NoDisponible() {
    alert("Por el momento esta funcionalidad no se encuentra disponible.");
}