//$(document).ready(function () {

//    carregarPostos();
//});
(function () {
    $.ajax({
        url: 'GetPostos',
        type: 'GET',
        success: function (result) {
            console.log(result);
            $("#listaPostos").html(result);

            $("#divListaPostos a").each(function () {
                $(this).click(function () {
                    console.log($(this).find("#idEstabelecimento").val());
                    $("#Posto_idEstabelecimento").val($(this).find("#idEstabelecimento").val());
                });
            });
        }
    });
})();