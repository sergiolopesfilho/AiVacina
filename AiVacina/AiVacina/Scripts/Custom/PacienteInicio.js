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
    });;
});