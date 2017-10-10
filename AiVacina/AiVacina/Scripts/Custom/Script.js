$(document).ready(function () {
    $('.cartao-cidadao').mask('000.0000.0000.0000', { reverse: true });
    $('.money').mask('000.000.000.000.000,00', { reverse: true });
    $('.data-mask').mask('00/00/0000');
    $('.cpf-mask').mask('000.000.000-00');
    $('.cnpj-mask').mask('00.000.000/0000-00');

});