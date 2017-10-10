using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AiVacina.Validação
{
    public static class Valida
    {
        public static bool Data(string data)
        {
            bool resultado = false;
            Regex rxData = new Regex(@"^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$");
            //resultado = rxData.IsMatch(data.ToShortDateString());
            resultado = rxData.IsMatch(data);
            return resultado;
        }

        public static bool CartaoCidadao(string numCartao)
        {
            bool resultado = false;
            Regex rxData = new Regex(@"^([0-9]{3}.[0-9]{4}.[0-9]{4}.[0-9]{4})$");
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
        
        public static Boolean LoteVacina(String lote)
        {
            bool resultado = false;
            Regex rxLote = new Regex(@"[A-Z]{5}[0-9]{3}[A-Z]{2}$");
            resultado = rxLote.IsMatch(lote);
            return resultado;
        }

        public static Boolean CodVacina(String cod)
        {
            bool resultado = false;
            Regex rxCod = new Regex(@"[0-9]{5,8}$");
            resultado = rxCod.IsMatch(cod);
            return resultado;
        }

    }
}