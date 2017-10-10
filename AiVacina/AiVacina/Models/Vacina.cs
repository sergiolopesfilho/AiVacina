using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class Vacina
    {
        [Display(Name = "Código da Vacina")]
        public int codVacina { get; set; }

        [Display(Name = "Lote da Vacina")]
        public string loteVacina { get; set; }

        [Display(Name = "Nome da Vacina")]
        public string nomeVacina { get; set; }

        [Display(Name = "Quantidade")]
        public int quantidade { get; set; }

        [Display(Name = "Validade da Vacina")]
        public DateTime dataValidade { get; set; }

        [Display(Name = "Grupo Alvo da Vacina")]
        public string grupoAlvo { get; set; }

    }
}