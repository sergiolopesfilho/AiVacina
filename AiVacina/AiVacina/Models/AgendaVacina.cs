using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class AgendaVacina
    {

        public int id { get; set; }

        [Display(Name ="Posto de Saúde")]
        public string nomeEstabelecimento { get; set; }

        [Display(Name = "Endereço")]
        public string rua { get; set; }

        [Display(Name = "Bairro")]
        public string bairro { get; set; }

        [Display(Name ="Vacina")]
        public string nomeVacina { get; set; }

        [Display(Name = "Data")]
        public DateTime dataAgendamento
        {
            get { return longDate; }
            set{ shorDate = value.ToShortDateString();
                 longDate = value;   }
        }

        public string shorDate;
        private DateTime longDate;
    }
}