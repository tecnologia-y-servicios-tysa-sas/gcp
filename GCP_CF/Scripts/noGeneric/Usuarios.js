var idMensaje = "bodyError";
var idPopup = "PopUPModalError";

$(function () {

    if ($("#mensajeErrorOperacion") != null && $("#mensajeErrorOperacion").text().trim().length > 0) {
        MostrarMensajeValidacion(idMensaje, idPopup, $("#mensajeErrorOperacion").text(), 5000);
    }

    $("#esSuperUsuario").change(function () {
        if ($(this).is(":checked")) {
            $("#tipoPermiso").prop("checked", false);
            $("#tipoPermiso").prop("disabled", true);
            $("#todosLosContratos").prop("checked", false);
            $("#todosLosContratos").prop("disabled", true);
            $("#seccionContratos").fadeOut();
        } else {
            $("#tipoPermiso").prop("disabled", false);
            $("#todosLosContratos").prop("disabled", false);
            $("#seccionContratos").fadeIn();
        }
    });

    $("#todosLosContratos").change(function () {
        if ($(this).is(":checked")) {
            $("#seccionContratos").fadeOut();
        } else {
            $("#seccionContratos").fadeIn();
        }
    });

    if ($("#esSuperUsuario").is(":checked")) {
        $("#tipoPermiso").prop("checked", false);
        $("#tipoPermiso").prop("disabled", true);
        $("#todosLosContratos").prop("checked", false);
        $("#todosLosContratos").prop("disabled", true);
        $("#seccionContratos").fadeOut();
    } else {
        $("#seccionContratos").fadeIn();
    }

    $("#btnGuardarUsuario").on("click", function (e) {

        e.preventDefault(); // Cancela la acción por defecto

        var esValido = validarTodo();
        console.log("¿Es válido? " + esValido);

        if (esValido) {
            $("form")[0].submit();
        }

    });

});

function validarNombres() {
    return validateRequired($("#nombres"), "Nombres");
}

function validarApellidos() {
    return validateRequired($("#apellidos"), "Apellidos");
}

function validarCorreoElectronico() {
    if (validateRequired($("#correo"), "Correo Electrónico")) {
        return IsEmail($("#correo"));
    }
}

function validarUsuario() {
    return validateRequired($("#usuario"), "Usuario");
}

function validarPassword() {
    if ($("#idUsuario").val().trim() == "")
        return validateRequired($("#password"), "Contraseña");

    return true;
}

function validarTodo() {
    var esValido = validarNombres();
    if (esValido) esValido = validarApellidos();
    if (esValido) esValido = validarCorreoElectronico();
    if (esValido) esValido = validarUsuario();
    if (esValido) esValido = validarPassword();
    return esValido;
}