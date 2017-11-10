(function () {
    $.ajax({
        url: 'GetProximoAgendamento',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            console.log(result);
            if (result.texto != null)
            {
                $("#proximoTexto").html(result.texto);
                $("#proximaData").html("Data: " + result.data);
                $("#proximoVacina").html("Vacina: " + result.nomeVacina);
                $("#proximoLugar").html("Posto: " + result.nomeEstabelecimento);
                $(".divProximaVacina").toggle("slow");
            }
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR);
            console.log(exception);
        }
    });
})();

$(document).ready(function () {
    $("#fecharProximaVacina").click(function () {
        $(".divProximaVacina").toggle("slow");
        $("#mostrarProximaVacina").toggle();
    });


    $("#mostrarProximaVacina").click(function () {
        $(".divProximaVacina").toggle("slow");
        $("#mostrarProximaVacina").toggle("slow");
    });


    $("#selectPosto").change(function () {
        //var cnpj = $(this).find("#cnpjEstabelecimento").val();
        var cnpj = $(this).val();
        adicionaVacinas(cnpj);
    });
});

var adicionaVacinas = function (cnpj) {
    console.log(cnpj);
    $.ajax({
        url: 'VacinasPorPosto',
        type: 'POST',
        data: JSON.stringify({ cnpj }),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            console.log(result);
            if(result != null)
                $("#vacinas").html(result);
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR);
            console.log(exception);
        }
    });

}