$body = $(".partialAgendamento");
$('#modalAgendamento').modal({
    show: 'false'
});


$(document).on({
    ajaxStart: function () { $body.addClass("loading"); },
    ajaxStop: function () { $body.removeClass("loading"); }
});

$(document).ready(function () {

    $(".cancelar").click(function (event) {
        var idAgendamento = $("#data2610").attr("idValue");

        $.ajax({
            url: 'DeleteAgendamento',
            type: 'POST',
            data: JSON.stringify({ id: idAgendamento }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                console.log(result.success);
                $("#textModal").text(result.success);
                $('#modalAgendamento').modal('toggle');
            },
            erro: function () { },
            async: true,
        });

    });

});
