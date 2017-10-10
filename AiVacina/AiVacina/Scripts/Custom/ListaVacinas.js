$(document).ready(function () {
    carregarVacinas();
    //carregarPostos();

    var horario;
    var data;

    $("#datepicker").datepicker({
        minDate: 0,
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior',
        onSelect: function (dateText, inst)
        {
            var fullDate =  $(this).datepicker('getDate');
            var day = fullDate.getDate();
            //$("#dataAgendamento").val(fullDate.getDate() + "/" + fullDate.getMonth() + "/" + fullDate.getFullYear() + " " + horario);
            data = fullDate.getDate() + "/" + fullDate.getMonth() + "/" + fullDate.getFullYear();
        }
    });

    $("#horarioVacina").change(function () {
        //console.log($(this).val());
        horario = $(this).val() + ":00";
    });

    $("#agendar").mouseover(function () {
        $("#dataAgendamento").val(data + " " + horario );
    });
});

function carregarVacinas()
{
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

}

//function carregarPostos() {
//    $.ajax({
//        url: 'GetPostos',
//        type: 'GET',
//        success: function (result) {
//            console.log(result);
//            $("#listaPostos").html(result);

//            $("#divListaPostos a").each(function () {
//                $(this).click(function () { 
//                    console.log($(this).find("#idEstabelecimento").val());
//                    $("#idPosto").val($(this).find("#idEstabelecimento").val());
//                });
//            });
//        }
//    });
//}