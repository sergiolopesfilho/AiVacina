using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class CarteiraVacinacao
    {
        public Vacina vacina { get; set; }
        public string numCartaoCidadao { get; set; }
        public DateTime dataVacinacao { get; set; }
        public int idPosto { get; set; }
    }
}