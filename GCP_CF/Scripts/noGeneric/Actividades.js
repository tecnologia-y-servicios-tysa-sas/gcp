function VerFases(idContrato) {
    $.ajax({
        async: false,
        url: "/Actividades/ObtenerFasesContratos",
        type: "POST",
        data: JSON.stringify({ idContrato: idContrato }),
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data, status, j) {
            $("#tableFases").html(data.html);
            $("#divFases").show();
            $("#texto1").html('<h3 style="float:left;padding-left:13px">Fases del Contrato ' + data.numContrato + '</h3>');
            
            
        }
    });
}
var idContrato;
var idFase;
function VerActividadesFaseContrato(idContrato,idFase) {
    $.ajax({
        async: false,
        url: "/Actividades/ObtenerActividadesFasesContratos",
        type: "POST",
        data: JSON.stringify({ idContrato: idContrato, idFase: idFase }),
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data, status, j) {
            $("#tableActividadesFases").html(data.html);
            $("#divActividadesFases").show();
            $("#texto2").html('<h3 style="float:left;padding-left:13px">Actividades de la fase ' + data.numFase + '  del Contrato ' + data.numContrato + '</h3>');
            
            $("#hfvContrato").val(data.idContrato);
            $("#hfvFase").val(data.idFase);
            
        }
    });
}

function GuardarActividad() {
    debugger
    var item = $("#Item").val();
    var descripcion = $("#Descripci_n").val();
    var diashabiles = $("#DiasHabiles").val();
    var finicio = $("#FechaInicio").val();
    var ffin = $("#FechaFinal").val();
    var estadoactividad = $("#EstadoActividad_Id").val();
    var idContrato = $("#hfvContrato").val();
    var idFase = $("#hfvFase").val();
    $.ajax({
        async: false,
        url: "/Actividades/GuardarActividadFase",
        type: "POST",
        data: JSON.stringify({
            idContrato: idContrato, idFase: idFase, item: item, descripcion: descripcion, diashabiles: diashabiles, finicio: finicio,
            ffin: ffin, estadoactividad: estadoactividad
        }),
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data, status, j) {
            alert('guardo');
        }
    });
}
