(function () {
    var horario;
    var data;

    $("#datepicker").datepicker({
        minDate: 0,
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior',
        onSelect: function (dateText, inst) {
            var fullDate = $(this).datepicker('getDate');
            var day = fullDate.getDate();

            //data = fullDate.getDate() + "/" + (fullDate.getMonth() + 1) + "/" + fullDate.getFullYear();
            data = (fullDate.getMonth() + 1) + "/" + fullDate.getDate() + "/" + fullDate.getFullYear();
            $("#dataDia").val(data);
            getHorariosBloqueados(data);
        }
    });

})();

$(document).ready(function () {

    $("#agendar").mouseover(function () {
        $("#dataAgendamento").val($("#dataDia").val() + " " + $("#horarioVacina").val() + ":00");
    });

    $("#selectListaVacinas").change(function () {
        var lote = $("#selectListaVacinas option:selected").val();
        $("#idVacina").val(lote);
        adicionaPostos(lote);
    });

});


var getHorariosBloqueados = function (dataConsulta) {
    $.ajax({
        url: 'GetHorariosBloqueados',
        type: 'POST',
        data: JSON.stringify({ dataTeste: dataConsulta }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result != null && result.success != null) {
                console.log(result.success);
                $('#horarioVacina').empty();
                $.each(result.success, function (key, value) {
                    $('#horarioVacina').append($("<option/>", {
                        value: value,
                        text: value
                    }));
                });
            }
            else {
            }
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR);
            console.log(exception);
        }
    });
}

var adicionaPostos = function (lote) {
    console.log(lote);
    $.ajax({
        url: 'GetPostosPorLote',
        type: 'POST',
        data: JSON.stringify({ lote: lote }),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            console.log(result);
            $("#listaPostos").html(result);


            $("#divListaPostos a").each(function () {
                $(this).click(function () {
                    console.log($(this).find("#idEstabelecimento").val());
                    $("#Posto_idEstabelecimento").val($(this).find("#idEstabelecimento").val());
                });
            });
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR);
            console.log(exception);
        }
    });
    
}