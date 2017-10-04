using AiVacina.DAL;
using AiVacina.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AiVacina.Controllers
{
    public class PacienteController : Controller
    {
        // GET: Paciente/Agendamento
        public ActionResult Agendamento()
        {
            return View();
        }
        
        // POST: Paciente/Agendamento
        [HttpPost]
        public ActionResult Agendamento(AgendamentoVacina agendamento)
        {
            agendamento.cartaocidadao = "123.1231.2312.1323";
            DataBase.SalvaAgendamento(agendamento);
            return RedirectToAction("Agenda");
        }

        // GET: Paciente/Agenda
        public ActionResult Agenda()
        {
            IEnumerable<AgendaVacina> agendamentos = 
                DataBase.AgendamentosVacina("123.1231.2312.1323");
            return View(agendamentos);
        }


        // GET: Paciente/GetVacinas
        public ActionResult GetVacinas()
        {
            IEnumerable<Vacina> vacinas = DataBase.ListaVacinas();
            return PartialView("_Vacinas", vacinas);
        }

        // GET: Paciente/GetPostos
        public ActionResult GetPostos()
        {
            IEnumerable<Posto> postos= DataBase.ListaPostos();
            return PartialView("_Postos", postos);
        }
    }
}