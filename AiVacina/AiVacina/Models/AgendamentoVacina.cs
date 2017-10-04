using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class AgendamentoVacina
    {
        public int id { get; set; }
        public int idPosto { get; set; }
        public int idVacina { get; set; }
        public string cartaocidadao { get; set; }
        public DateTime dataAgendamento { get; set; }
    }
    
}