using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class Paciente
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Cartão Cidadão:")]
        public string numCartaoCidadao { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome:")]
        public string nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Data de Nascimento:")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",ApplyFormatInEditMode = true,NullDisplayText = "Estoque vazio")]
        public DateTime dataNascimento { get; set; }
        
        public Endereco endereco { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Senha:")]
        [DataType(DataType.Password)]
        public string senha { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Confirmação senha:")]
        [DataType(DataType.Password)]
        public string confSenha { get; set; }
         
        public List<Vacina> vacinasTomadas { get; set; }
    }
}