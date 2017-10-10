using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class Posto
    {
        public int idEstabelecimento { get; set; }

        [Display(Name ="Nome do posto: ")]
        public string nomeEstabelecimento { get; set; }

        public Endereco endereco { get; set; }

        [Display(Name = "Nome do administrador: ")]
        public string admPosto { get; set; }

        [Display(Name = "CPF do administrador: ")]
        public string cpfAdmPosto { get; set; }

        [Display(Name = "CNPJ do posto: ")]
        public string cnpj { get; set; }
    }
}