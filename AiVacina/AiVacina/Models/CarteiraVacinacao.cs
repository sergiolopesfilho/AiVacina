using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class CarteiraVacinacao
    {
        public int id { get; set; }
        //public Vacina vacina { get; set; }
        public string numCartaoCidadao { get; set; }
        //public DateTime dataVacinacao { get; set; }
        public Posto Posto { get; set; }
        public List<Vacina> minhasVacinas{ get; set; }
    }
}