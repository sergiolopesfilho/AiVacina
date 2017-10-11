using AiVacina.DAL;
using AiVacina.Models;
using AiVacina.Validação;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AiVacina.Controllers
{
    public class PacienteController : Controller
    {

        // GET: Paciente/Agenda
        public ActionResult Agenda()
        {
            ///TODO:
            /// Deixar dinamico de acordo com o paciente
            IEnumerable<AgendaVacina> agendamentos =
                DataBase.AgendamentosVacina("123.4567.8913.2413");
            return View(agendamentos);
        }

        // GET: Paciente/Agendamento
        public ActionResult Agendamento()
        {
            return View();
        }

        // POST: Paciente/Agendamento
        [HttpPost]
        public ActionResult Agendamento(AgendamentoVacina agendamento)
        {
            ///TODO:
            /// Deixar dinamico de acordo com o paciente
            agendamento.cartaocidadao = "123.4567.8913.2413";
            DataBase.SalvaAgendamento(agendamento);
            return RedirectToAction("Agenda");
        }

        //GET: Paciente/CadastrarCarteira
        [HttpGet]
        public ActionResult CadastrarCarteira()
        {
            return View();
        }

        //POST: Paciente/CadastrarCarteira
        [HttpPost]
        public ActionResult CadastrarCarteira(CarteiraVacinacao carteira)
        {
            try
            {
                if (Valida.CartaoCidadao(carteira.numCartaoCidadao))
                {
                    DataBase.CadastraCarteiraVacinacao(carteira);
                    return RedirectToAction("MinhaCarteira");
                }
                else
                    throw new Exception("Número do cartão cidadão invalido!");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(carteira);
            }
        }

        //GET: Paciente/MinhaCarteira
        [HttpGet]
        public ActionResult MinhaCarteira()
        {
            return View();
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
            IEnumerable<Posto> postos = DataBase.ListaPostos();
            return PartialView("_Postos", postos);
        }

        [HttpPost]
        public JsonResult DeleteAgendamento(string id)
        {
            string resultado = string.Empty;
            try
            {
                int intId = Convert.ToInt32(id);
                if (intId > 0)
                {
                    if (DataBase.DeletaAgendamento(intId))
                        resultado = "Agendamento deletado com sucesso!";
                    else
                        resultado = "Agendamento não pôde ser deletado. Tente novamente mais tarde.";
                }
                else
                {
                    resultado = "Agendamento invalido.";
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
            }
            //return Json(new { success = resultado });
            return Json(new { success = resultado });
        }
    }
}