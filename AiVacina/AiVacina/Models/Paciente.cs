using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    [Table("pacientes")]
    public class Paciente
    {
        [Key]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Cartão Cidadão:")]
        public string numCartaoCidadao { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Nome:")]
        public string nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Data de Nascimento:")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",ApplyFormatInEditMode = true,NullDisplayText = "Data de nascimento obrigatória.")]
        public string dataNascimento { get; set; }

        public DateTime data { get; set; }

        public Endereco endereco { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Senha:")]
        [DataType(DataType.Password)]
        public string senha { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Confirmação senha:")]
        [DataType(DataType.Password)]
        [Compare("senha", ErrorMessage = "As senhas devem ser iguais.")]
        public string confSenha { get; set; }

        public string perfil { get; set; }


        [Display(Name = "CPF:")]
        public string cpfAdmPosto { get; set; }

        public List<Vacina> vacinasTomadas { get; set; }
    }

    public class PacienteLogin
    {
        [Key]
        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Cartão Cidadão:")]
        public string numCartaoCidadao { get; set; }
        
        public string perfil { get; set; }

        public string nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Display(Name = "Senha:")]
        [DataType(DataType.Password)]
        public string senha { get; set; }
    }
}