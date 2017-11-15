$body = $("body");

$(document).on({
    ajaxStart: function () { $body.addClass("loading"); },
    ajaxStop: function () { $body.removeClass("loading"); }
});

$(document).ready(function () {

    $(".cancelaAgendamento").click(function () {
        deletaAgendamento($(this).val());
    });


    $(".fechaModal").click(function () {
        location.reload();
    });
});


function deletaAgendamento(idAgendamento) {
    $.ajax({
        url: 'DeleteAgendamento',
        type: 'POST',
        data: JSON.stringify({ id: idAgendamento }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //alert(result.success);
            $("#modalSucesso").modal();
           
        },
        erro: function (qXHR, exception) {
            console.log(qXHR);
            console.log(exception);
        },
        async: true,
    });
}