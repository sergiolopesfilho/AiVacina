using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class Posto
    {
        public int idEstabelecimento { get; set; }

        public string nomeEstacionamento { get; set; }

        public Endereco endereco { get; set; } 
         
        public string admPosto { get; set; }

        public string cpfAdmPosto { get; set; }

        public string cnpj { get; set; }
    }
}