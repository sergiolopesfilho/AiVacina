$(document).ready(function () {

    $(".deletaVacina").click(function () {
       var lote = $(this).parent().prev().prev().text().trim();

        $.ajax({
            url: 'DeleteVacina',
            type: 'POST',
            data: JSON.stringify({ lote }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                alert(result.success);
                location.reload();
            },
            erro: function () { },
            async: true,
        });
    });
});