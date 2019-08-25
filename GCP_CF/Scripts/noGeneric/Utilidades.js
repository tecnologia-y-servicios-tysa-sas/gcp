function FormatoFecha(fecha, separador) {

    var soloFecha = fecha.split(" ")[0];
    var partes = soloFecha.split(separador);
    var dia = parseInt(partes[0], 10);
    var mes = parseInt(partes[1], 10);
    var anio = partes[2];

    return (dia < 10 ? "0" + dia : dia) + "/"
        + (mes < 10 ? "0" + mes : mes) + "/"
        + anio;
}

function MostrarMensajeValidacion(idMensaje, idPopup, mensaje, timeout) {

    var selectorIdMensaje = "#" + idMensaje;
    var selectorIdPopup = "#" + idPopup;

    $(selectorIdMensaje).html('<div>' + mensaje + '</div>');
    $(selectorIdPopup).modal('show');
    setTimeout(function () {
        $(selectorIdPopup).modal('hide');
    }, timeout);

}

// Validaciones

function cambiarFormatoNumerico(campo) {
    campo.val(parseFloat(campo.val(), 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
}

function RestablecerFormato(campo) {
    var valorActual = campo.val();
    return valorActual.replace(/,/gi, "").toString();
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