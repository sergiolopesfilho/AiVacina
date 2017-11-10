$(document).ready(function () {

    $("#btnCadastrarVacina").click(function (e) {
        var cartaoPaciente = $("#txtCartaoCidadao").val();

        if (!cartaoPaciente) {
            alert("Insira o número do cartão cidadão do paciente.");
        }
        else {
            $("#modalCadastroVacina").modal();
        }
    });


    $("#btnCadastrarAplicacao").click(function () {
        var cartao = $("#txtCartaoCidadao").val();
        var vacina = $("#txtNomeVacinaAplicada").val();
        var aplicacao = $("#txtDataAplicada").val();
        var reforco = $("#txtDataReforço").val();

        if (cartao && vacina && aplicacao && reforco)
        {
            realizarCadastroAplicacal(cartao, vacina, aplicacao, reforco);
        }
        else
        {
            alert("Todos os campos são obrigatórios.");
        }
    });
});


function realizarCadastroAplicacal(cartao, vacina, aplicacao, reforco) {
    $.ajax({
        url: 'AdicionaVacinaAplicada',
        type: 'POST',
        data: JSON.stringify({ cartao, vacina, aplicacao, reforco }),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            console.log(result);
            if (result.success != null && result.success) {
                alert(result.success);
                $("#modalCadastroVacina").modal();
                location.reload();
            }
            else
            {
                alert("Houve um erro no cadastro da vacina, contate o administrador.");
            }
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR);
            console.log(exception);
        }
    });
}