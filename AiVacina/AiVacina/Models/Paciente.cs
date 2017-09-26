using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class Paciente
    {
        public int numCartaoCidadao { get; set; }

        public string nome { get; set; }

        public DateTime dataNascimento { get; set; }

        public Endereco endereco { get; set; }

        public string senha { get; set; }

        public string confSenha { get; set; }

        public List<Vacina> vacinasTomadas { get; set; }
    }
}