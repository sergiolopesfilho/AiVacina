//$(document).ready(function () {
//    carregarVacinas();


//});

(function () {
    $.ajax({
        url: 'GetVacinas',
        type: 'GET',
        success: function (result) {
            $("#listaVacinas").html(result);
            $("#selectListaVacinas").change(function () {
                $("#idVacina").val($(this).val());
            });
        }
    });

})();