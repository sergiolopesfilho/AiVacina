using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AiVacina.Validação
{
    public static class Valida
    {
        public static bool Data(DateTime data)
        {
            bool resultado = false;
            Regex rxData = new Regex(@"^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$");
            resultado = rxData.IsMatch(data.ToShortDateString());
            return resultado;
        }

        public static bool CartaoCidadao(string numCartao)
        {
            bool resultado = false;
            Regex rxData = new Regex(@"^(\d{3}\s\d{4}\s\d{4}\s\d{4})$");
            resultado = rxData.IsMatch(numCartao);
            return resultado;
        }

        public static Boolean Email(String email)
        {
            bool resultado = false;
            Regex rxEmail = new Regex(@"^\w+([-+.']\w+)@\w+([-.]\w+)\.\w+([-.]\w+)*$");
            resultado = rxEmail.IsMatch(email);
            return resultado;
        }
        
        public static Boolean Nome(String nomeAtelie)
        {
            bool resultado = false;
            Regex rxNomeAtelie = new Regex(@"^\w[A-Za-z ]{4,50}$");
            resultado = rxNomeAtelie.IsMatch(nomeAtelie);
            return resultado;
        }

    }
}