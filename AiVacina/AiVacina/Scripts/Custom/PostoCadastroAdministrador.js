/*PostoCadastroAdministrador*/

$(document).ready(function () {

    $("#atualizarCadastro").click(function () {
        var idEstabelecimento = $("#Posto_idEstabelecimento").val();
        var admPosto = $("#admPosto").val();
        var cpfAdmPosto = $("#cpfAdmPosto").val();
        var senha = $("#senha").val();
        var cnpj = $("#cnpj").val();

        var posto = { idEstabelecimento, admPosto, cpfAdmPosto, senha, cnpj };

        $.ajax({
            url: 'CadastroAdministrador',
            type: 'POST',
            data: JSON.stringify({ posto }),
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                console.log(result);
                if (result.success != null && result.success) {
                    if (result.sucess == "True") {
                        $("#modalSucesso").modal();
                    }
                    else {
                        $("#warning").text(result.success);
                    }
                }
                else
                {
                    alert("Houve um erro na comunicação com o servidor. Tente mais tarde");
                }
            },
            error: function (jqXHR, exception) {
                console.log(jqXHR);
                console.log(exception);
            }
        });
    });

    $(".fechaModal").click(function () {
        $("#modalSucesso").modal();
        window.location.href = '/Posto/CadastrarVacinas/';
    });
});