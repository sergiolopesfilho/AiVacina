using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class CarteiraVacinacao
    {
        [Display(Name = "N° Carteira: ")]
        public int id { get; set; }

        public string numCartaoCidadao { get; set; }

        [Required(ErrorMessage ="Nome completo é obrigatório.")]
        [Display(Name = "Nome Completo: ")]
        public string nome { get; set; }

        [Required(ErrorMessage = "Data de nascimento é obrigatório.")]
        [Display(Name = "Data de Nascimento: ")]
        public string dataNascimento { get; set; }

        public DateTime dataCadastro { get; set; }
        public Posto Posto { get; set; }
        public List<VacinaAplicada> minhasVacinas{ get; set; }
    }
}