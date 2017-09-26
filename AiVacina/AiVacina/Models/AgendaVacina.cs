using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AiVacina.Models
{
    public class AgendaVacina
    {
        public Vacina vacina { get; set; }

        public Posto posto { get; set; }

        public DateTime data { get; set; }

        public int quantidade { get; set; }

        public Paciente paciente { get; set; }
    }
}