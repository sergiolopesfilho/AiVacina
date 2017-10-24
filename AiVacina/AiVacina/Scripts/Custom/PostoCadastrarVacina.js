(function () {
    $.ajax({
        url: 'VacinasAjax',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            console.log(result);
            if (result != null) {
                $("#listaVacinas").html(result);
            }
            else
            {
                $("#listaVacinas").html("<h2>Não existem vacinas cadastradas neste posto.</h2>");
            }
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR);
            console.log(exception);
        },
        async: true,
    });

})();