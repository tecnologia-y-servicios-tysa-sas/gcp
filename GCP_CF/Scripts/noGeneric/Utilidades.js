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