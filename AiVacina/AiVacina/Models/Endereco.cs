using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class Endereco
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Rua:")]
        public string rua { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Bairro:")]
        public string bairro { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Cidade:")]
        public string cidade { get; set; }
    }
}