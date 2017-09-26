using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class Vacina
    {
        public int codVacina { get; set; }

        public int loteVacina { get; set; }

        public string nomeVacina { get; set; }

        public int quantidade { get; set; }

        public DateTime dataValidade { get; set; }

        public string grupoAlvo { get; set; }

    }
}