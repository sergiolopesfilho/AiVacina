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
        private const string horarios = "8:00;8:30;09:00;09:30;10:00;10:30;11:00;11:30;12:00;12:30;13:00;13:30;14:00;14:30;15:00;15:30;";

        // GET: Home/Inicio/
        [Authorize]
        public ActionResult Inicio(Paciente paciente)
        {
            return View();
        }

        // GET: Home/MeusDados/
        [Authorize]
        public ActionResult MeusDados(Paciente paciente)
        {
            if (paciente == null)
            {
                ModelState.AddModelError("", "Por favor, realize seu cadastro.");
                return RedirectToAction("Cadastro", "Home");

            }
            else
            {
                return View(paciente);
            }
        }

        // GET: Paciente/Agenda
        [Authorize]
        public ActionResult Agenda()
        {
            ///TODO:
            /// Deixar dinamico de acordo com o paciente

            try
            {
                String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
                if (String.IsNullOrEmpty(perfil))
                {
                    return RedirectToAction("Entrar", "Home");
                }
                else if (perfil.Equals("Paciente", StringComparison.InvariantCultureIgnoreCase))
                {
                    String cartao = Session["Cartao"] == null ? String.Empty : Session["Cartao"].ToString();
                    if (String.IsNullOrEmpty(cartao))
                    {

                        return RedirectToAction("Entrar", "Home");
                    }
                    else
                    {
                        IEnumerable<AgendaVacina> agendamentos =
                        //DataBase.AgendamentosVacina("123.4567.8913.2413");
                        DataBase.AgendamentosVacina(cartao);
                        return View(agendamentos);
                    }
                }
                else
                {
                    throw new Exception("Você não está autorizado a acessar essa página.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();

            }
        }

        // GET: Paciente/Agendamento
        [Authorize]
        public ActionResult Agendamento()
        {
            String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
            if (String.IsNullOrEmpty(perfil))
            {
                return RedirectToAction("Entrar", "Home");
            }
            else if (perfil.Equals("Paciente", StringComparison.InvariantCultureIgnoreCase))
            {
                return View();
            }
            else
            {
                ModelState.AddModelError("","Por favor, realizeseu cadastro para fazer um agendamento.");
                return RedirectToAction("Entrar", "Home");
            }
        }

        // POST: Paciente/Agendamento
        [HttpPost]
        public ActionResult Agendamento(AgendamentoVacina agendamento)
        {
            try
            {
                String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
                if (perfil.Equals("Paciente", StringComparison.InvariantCultureIgnoreCase))
                {
                    String cartao = Session["Cartao"] == null ? String.Empty : Session["Cartao"].ToString();
                    if (String.IsNullOrEmpty(cartao))
                    {
                        return RedirectToAction("Entrar", "Home");
                    }
                    else
                    {
                        agendamento.cartaocidadao = cartao;
                        DataBase.SalvaAgendamento(agendamento);
                    }
                }
                return RedirectToAction("Agenda");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(agendamento);
            }
        }

        //GET: Paciente/CadastrarCarteira
        [HttpGet]
        [Authorize]
        public ActionResult CadastrarCarteira()
        {
            String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
            if (String.IsNullOrEmpty(perfil))
            {
                return RedirectToAction("Entrar", "Home");
            }
            else if (perfil.Equals("Paciente", StringComparison.InvariantCultureIgnoreCase))
            {
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Por favor, realizeseu cadastro para fazer um agendamento.");
                return RedirectToAction("Entrar", "Home");
            }
        }

        //POST: Paciente/CadastrarCarteira
        [HttpPost]
        public ActionResult CadastrarCarteira(CarteiraVacinacao carteira)
        {
            try
            {
                carteira.numCartaoCidadao = Session["Cartao"] == null ? String.Empty : Session["Cartao"].ToString();
                if (Valida.CartaoCidadao(carteira.numCartaoCidadao))
                {
                    carteira.dataCadastro = DateTime.Now;

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
        [Authorize]
        public ActionResult MinhaCarteira()
        {
            String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
            if (String.IsNullOrEmpty(perfil))
            {
                return RedirectToAction("Entrar", "Home");
            }
            else if (perfil.Equals("Paciente", StringComparison.InvariantCultureIgnoreCase))
            {
                String cartao = Session["Cartao"] == null ? String.Empty : Session["Cartao"].ToString();
                CarteiraVacinacao carteira = DataBase.GetCarteiraVacinacao(cartao);
                return View(carteira);
            }
            else
            {
                ModelState.AddModelError("", "Por favor, realize seu cadastro para fazer um agendamento.");
                return RedirectToAction("Entrar", "Home");
            }
        }

        // GET: Paciente/GetVacinas
        public ActionResult GetVacinas()
        {
            IEnumerable<Vacina> vacinas = DataBase.ListaVacinas();
            return PartialView("_Vacinas", vacinas);
        }

        // GET: Paciente/GetVacinas
        public ActionResult GetVacinasPortPosto(string idEstabelecimento)
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

        // GET: Paciente/GetPostos
        public ActionResult GetSelectPostos()
        {
            IEnumerable<Posto> postos = DataBase.ListaPostos();
            return PartialView("_SelectPostos", postos);
        }

        // POST: Paciente/GetPostos
        [HttpPost]
        public ActionResult GetPostosPorLote(string lote)
        {
            try
            {
                IEnumerable<Posto> postos = DataBase.ListaPostosPorVacina(lote);

                return PartialView("_Postos", postos);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
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
                        resultado = "Agendamento cancelado com sucesso!";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetHorariosBloqueados(string dataTeste)
        {
            try
            {
                if (!String.IsNullOrEmpty(dataTeste))
                {
                    var resultado = DataBase.GetHorariosBloqueados(dataTeste).Split(';');
                    if (resultado.Count() > 0)
                    {
                        var horariosDisponiveis = horarios.Split(';').Except(resultado);
                        return Json(new { success = horariosDisponiveis });
                    }
                    else
                    {
                        var horariosDisponiveis = horarios.Split(';');
                        return Json(new { success = horariosDisponiveis });
                    }
                }
                else
                    return Json(new { success = String.Empty });
            }
            catch (Exception ex)
            {
                return Json(new { success = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetProximoAgendamento()
        {
            try
            {
                AgendaVacina proximo = DataBase.GetProximoAgendamento("123.4567.8913.2413");
                if (proximo != null)
                {
                    var faltam = proximo.dataAgendamento.Subtract(DateTime.Now);
                    string texto = String.Empty;
                    if (faltam.Days > 0)
                    {
                        texto = "Sua próxima vacina será em " + proximo.dataAgendamento.ToShortDateString() + ". Visite sua agenda!";
                    }
                    else
                    {
                        texto = "Faltam apenas " + faltam.Hours + " horas para seu próximo agendamento!";
                    }

                    return Json(new { proximo.nomeVacina, proximo.nomeEstabelecimento, texto, data = proximo.dataAgendamento.ToLongDateString() }, JsonRequestBehavior.AllowGet);
                }
                string semAgendamento = "Você não tem agendamentos cadastrados!";
                return Json(new { resposta = semAgendamento }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string semAgendamento = "Você não tem agendamentos cadastrados!";
                return Json(new { resposta = semAgendamento }, JsonRequestBehavior.AllowGet);

            }
        }

        // GET: Posto/Vacinas/
        [HttpPost]
        public ActionResult VacinasPorPosto(string cnpj)
        {
            IEnumerable<Vacina> vacinas = DataBase.ListaVacinas(cnpj);
            return PartialView("_VacinasPorPosto", vacinas);
        }
    }
}