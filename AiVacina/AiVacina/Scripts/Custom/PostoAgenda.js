
(function () {
    $.ajax({
        url: 'GetVacinasPosto',
        type: 'POST',
        data: JSON.stringify({ cnpj: '22.323.458/0001-79' }),
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            console.log(response);
            if (response != null)
            {
                $("#agendamentos").html(response);
            }
            else
            {
                var noAswer = "<h2>Não existem vacinas agendadas.</h2>"
                $("#agendamentos").html(noAswer);
            }
        },
        error: function(jqXHR, exception)
        {
            var noAswer = "<h2>Não foi possível encontrar vacinas agendadas para esta data.</h2>"
            $("#agendamentos").html(noAswer);
        }
    });
})();

(function () {
    
    $("#datepicker").datepicker({
        dateFormat: 'dd/mm/yyyy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior',
        onSelect: function (dateText, inst) {
            var fullDate = $(this).datepicker('getDate');
            //var dataEscolhida = 
            filtraAgendamentos(fullDate);

        }
    });

    $(".datepickerCancelamento").datepicker({
        minDate: 0,
        dateFormat: 'dd/mm/yyyy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior',
        onSelect: function (dateText, inst) {
            var fullDate = $(this).datepicker('getDate');
            var data = fullDate.getDate() + "/" + (fullDate.getMonth() + 1) + "/" + fullDate.getFullYear();
            $("#dataCancelamento").val(data);

        }
    });
})();

$(document).ready(function () {

    $("#linkMostrarBloqueio").click(function () {
        $("#divBloqueio").toggle("slow");
        //$("#divBloqueio").removeClass("hidden");
    });


    $("#btnBloquearAgendamento").click(function () {
        var bloquearDia = $("#dataCancelamento").val();
        var bloquearHorarios = '';

        $("input:checkbox[name=horarioCancelado]:checked").each(function(){
            bloquearHorarios = (bloquearHorarios + $(this).val() + ";");
        });

        if ((bloquearDia == undefined) || (bloquearDia == '')
            || (bloquearHorarios.length <= 0) || (bloquearHorarios == '')) {
            $("label.errorCancelamento").text("Por favor selecionar um dia e pelo menos um horário para ser bloqueado");
        }
        else
        {
            bloquearAgenda(bloquearDia, bloquearHorarios);
        }
    });
});

function filtraAgendamentos(dataEscolhida) {
    $.ajax({
        url: 'GetVacinasPosto',
        type: 'POST',
        data: JSON.stringify({ cnpj: '22.323.458/0001-79', data: dataEscolhida }),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            console.log(result);
            if (result != null) {
                $("#agendamentos").html(result);
            }
            else {
                var noAswer = "<h2>Não existem vacinas agendadas.</h2>"
                $("#agendamentos").html(noAswer);
            }
        },
        error: function (jqXHR, exception) {
            var noAswer = "<h2>Não foi possível encontrar vacinas agendadas para esta data.</h2>"
            $("#agendamentos").html(noAswer);
        }
    });

};

function bloquearAgenda(dia, horarios) {
    $.ajax({
        url: 'BloquearHorarios',
        type: 'POST',
        data: JSON.stringify({ diaBloquear: dia, horaBloquear: horarios }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
           
            if (response != null && response.success) {
                alert("Horarios bloqueados com sucesso!");
            }
            else {
                alert("Horarios não bloqueados, contate o gerente por favor.");
            }
        },
        error: function (jqXHR, exception) {
            alert("Não foi possível realizar o bloqueio, por favor contate o gerente.");
        }
    });

};