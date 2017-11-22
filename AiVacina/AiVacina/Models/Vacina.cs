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
        public string codVacina { get; set; }

        [Display(Name = "Lote da Vacina")]
        public string loteVacina { get; set; }

        [Display(Name = "Nome da Vacina")]
        public string nomeVacina { get; set; }

        [Display(Name = "Quantidade")]
        public int quantidade { get; set; }

        [Display(Name = "Validade da Vacina")]
        public String dataValidade { get; set; }

        [Display(Name = "Grupo Alvo da Vacina")]
        public string grupoAlvo { get; set; }

        [Display(Name = "CNPJ do Posto")]
        public String postoCNPJ { get; set; }

    }

        public class VacinaAplicada
    {
            
            public int idVacinaAplicada { get; set; }

            [Display(Name = "Vacina")]
            public string vacina { get; set; }

            [Display(Name = "Data do Reforço")]
            public DateTime dataReforco { get; set; }

            [Display(Name = "Data Vacinacao")]
            public DateTime dataVacinação { get; set; }

            [Display(Name = "N° Carteira de Vacinação")]
            public int idCarteira { get; set; }

            [Display(Name = "Posto")]
            public string posto { get; set; }

    }
    }