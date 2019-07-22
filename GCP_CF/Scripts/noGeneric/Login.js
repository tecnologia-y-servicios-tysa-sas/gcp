var idMensaje = "bodyError";
var idPopup = "PopUPModalError";

$(function () {

    $("#usuario").blur(function () {
        validateRequired($(this), "Usuario");
    });

    $("#password").blur(function () {
        validateRequired($(this), "Contraseña");
    });

    if ($.type($(".validation-summary-errors").html()) === "undefined") return;

    var mensaje = "<strong>Error:</strong> " + $(".validation-summary-errors").html();
    MostrarMensajeValidacion(idMensaje, idPopup, mensaje, 5000);
});

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

$(function () {

    $("#btnIngreso").on("click", function (e) {

        e.preventDefault(); // Cancela la acción por defecto

        var esValido = validateRequired($("#usuario"), "Usuario");
        if (esValido) esValido = validateRequired($("#password"), "Contraseña");

        if (esValido) {
            $("form")[0].submit();
        }

    });

});
