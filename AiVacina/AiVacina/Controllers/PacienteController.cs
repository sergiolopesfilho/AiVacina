using AiVacina.DAL;
using AiVacina.Models;
using AiVacina.Validação;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
                return RedirectToAction("Entrar", "Home");
            }
          
        }

        // GET: Home/MeusDados/
        [Authorize]
        public ActionResult MeusDados()
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
                    return RedirectToAction("Entrar", "Home");
                try
                {
                    Paciente paciente = DataBase.GetDadosPaciente(cartao);
                    return View(paciente);
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Entrar", "Home");
            }

        }

        // GET: Paciente/Agenda
        [Authorize]
        public ActionResult Agenda()
        {

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

                        String to = Session["Email"] == null ? String.Empty : Session["Email"].ToString();

                        if (!String.IsNullOrEmpty(to))
                        { 
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress("aivacina.puc@gmail.com");
                            mail.To.Add(new MailAddress(to));
                            mail.Subject = "Agendamento de Vacina";
                            mail.Body = "Você tem uma vacina agendada para " +agendamento.dataAgendamento.ToShortDateString()
                                        + ". A vacina a ser tomada é "+ agendamento.vacina;

                            SendEmail(mail);
                        }
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
                String cartao = Session["Cartao"] == null ? String.Empty : Session["Cartao"].ToString();

                if (!String.IsNullOrEmpty(cartao) && DataBase.CarteiraVacinacaoCadastrada(cartao))
                    return RedirectToAction("MinhaCarteira");
                else
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

        //GET: Paciente/CadastrarVacina
        [Authorize]
        public ActionResult CadastrarVacina()
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

        // POST: Paciente/CadastrarVacina
        [HttpPost]
        public ActionResult CadastrarVacina(VacinaAplicada aplicada)
        {
            try
            {
                String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
                if (String.IsNullOrEmpty(perfil))
                {
                    return RedirectToAction("Entrar", "Home");
                }
                else if (String.IsNullOrEmpty(aplicada.vacina))
                {
                    ModelState.AddModelError("", "Por favor selecionar a vacina que foi aplicada.");
                    return View();
                }

                else if (aplicada.dataVacinação == null || aplicada.dataVacinação == DateTime.MinValue
                    || aplicada.dataVacinação == DateTime.MaxValue)
                {
                    ModelState.AddModelError("", "Data invalida, por favor selecione uma valida.");
                    return View();
                }
                else if (perfil.Equals("Paciente", StringComparison.InvariantCultureIgnoreCase))
                {
                    String cartao = Session["Cartao"] == null ? String.Empty : Session["Cartao"].ToString();

                    if (!String.IsNullOrEmpty(cartao))
                    {
                        aplicada.posto = String.IsNullOrEmpty(aplicada.posto) ? String.Empty : aplicada.posto;
                        bool salva = DataBase.SalvaVacinaAplicada(aplicada.vacina, aplicada.dataVacinação, cartao, aplicada.posto);
                        if (salva)
                        {
                            return RedirectToAction("MinhaCarteira", "Paciente");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Ocorreu um erro, por favor tente adicionar vacinas mais tarde.");
                            return View();
                        }
                    }
                    else
                        return View();
                }
                else
                {
                    ModelState.AddModelError("", "Por favor, realizeseu cadastro para fazer um agendamento.");
                    return RedirectToAction("Entrar", "Home");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Não foi possível cadastrar a vacina.");
                return View();
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
                try
                {
                    CarteiraVacinacao carteira = DataBase.GetCarteiraVacinacao(cartao);
                    return View(carteira);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("CadastrarCarteira","Paciente");
                }
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
                String cartao = Session["Cartao"] == null ? String.Empty : Session["Cartao"].ToString();
                AgendaVacina proximo = DataBase.GetProximoAgendamento(cartao);
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

        private void SendEmail(MailMessage mail)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("aivacina.puc@gmail.com", "senha12345");

                client.Send(mail);
            }
            catch (Exception e)
            {

            }
        }
    }
}