using AiVacina.DAL;
using AiVacina.Models;
using AiVacina.Validação;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AiVacina.Controllers
{

    public class PostoController : Controller
    {
        // GET: Posto
        public ActionResult Index()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Index(Posto adm)
        {
            if (ModelState.IsValid && !String.IsNullOrEmpty(adm.cpfAdmPosto)
                && !String.IsNullOrEmpty(adm.senha))
            {
                try {
                    PacienteLogin dbAdm = DataBase.GetLoginPacienteCPF(adm.cpfAdmPosto);
                    if (dbAdm.senha.Equals(adm.senha) && dbAdm.perfil.Equals("Administrador", StringComparison.InvariantCultureIgnoreCase)
                        && dbAdm.numCartaoCidadao.Equals(adm.cpfAdmPosto))
                    {
                        System.Web.Security.FormsAuthentication.SetAuthCookie(dbAdm.nome, false);
                        Session["Nome"] = dbAdm.nome;
                        Session["Cartao"] = dbAdm.numCartaoCidadao;
                        Session["Perfil"] = dbAdm.perfil;
                        Session["CNPJ"] = dbAdm.cnpj;

                        return RedirectToAction("CadastrarVacinas");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Senha incorreta, tente novamente");
                        return View(adm);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(adm);
                }
            }
            else
            {
                ModelState.AddModelError("", "Algo deu errado, confirme seus dados por favor.");
                return View(adm);
            }
        }

        public ActionResult Agenda()
        {
            String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
            if (String.IsNullOrEmpty(perfil))
            {
                return RedirectToAction("Index");
            }
            else if (perfil.Equals("Administrador", StringComparison.InvariantCultureIgnoreCase))
            {
                String cnpj = Session["CNPJ"] == null ? String.Empty : Session["CNPJ"].ToString();

                IEnumerable<AgendaVacina> agendamentos =
                    DataBase.AgendamentosPosto(cnpj);
                return View(agendamentos);
            }
            else
            {
                ModelState.AddModelError("", "Você não está autorizado a acessar essa pagina.");
                return RedirectToAction("Index", "Posto");
            }
        }

        // GET: Posto/Create
        public ActionResult CadastrarVacinas()
        {
            String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
            if (String.IsNullOrEmpty(perfil))
            {
                return RedirectToAction("Index");
            }
            else if (perfil.Equals("Administrador", StringComparison.InvariantCultureIgnoreCase))
            {
                String cnpj = Session["CNPJ"] == null ? String.Empty : Session["CNPJ"].ToString();

                return View();
            }
            else
            {
                ModelState.AddModelError("", "Você não está autorizado a acessar essa pagina.");
                return RedirectToAction("Index", "Posto");
            }
        }

        // POST: Posto/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CadastrarVacinas(Vacina vacina)
        {
            try
            {
                vacina.loteVacina = vacina.loteVacina.ToUpper();
                if (ValidaVacinas(vacina))
                {
                    String[] dataSplit = vacina.dataValidade.Split('/');
                    if(Convert.ToInt32(dataSplit[1]) == 2 && Convert.ToInt32(dataSplit[0]) > 28)
                        throw new Exception("O mês comercial de Fevereiro vai ate dia 28.");

                    if ((DateTime.Today.CompareTo(DateTime.ParseExact(vacina.dataValidade + " 00:00",
                         "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture))) >= 0)
                        throw new Exception("Data de validade não pode ser menor que o dia atual.");

                    vacina.postoCNPJ = Session["CNPJ"] == null ? String.Empty : Session["CNPJ"].ToString();
                    vacina.dataValidade = dataSplit[1] + "/" + dataSplit[0] + "/" + dataSplit[2];
                    DataBase.CadastrarVacina(vacina);
                }
                return RedirectToAction("Vacinas");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vacina);
            }
        }

        // GET: Posto/Create
        [HttpGet]
        public ActionResult CadastroAdministrador()
        {

            String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
            if (String.IsNullOrEmpty(perfil))
            {
                return RedirectToAction("Index");
            }
            else if (perfil.Equals("Administrador", StringComparison.InvariantCultureIgnoreCase))
            {
                String cnpj = Session["CNPJ"] == null ? String.Empty : Session["CNPJ"].ToString();

                return View();
            }
            else
            {
                ModelState.AddModelError("", "Você não está autorizado a acessar essa pagina.");
                return RedirectToAction("Index", "Posto");
            }
        }

        // POST: Posto/CadastroAdministrador/
        [HttpPost]
        public JsonResult CadastroAdministrador(Posto posto)
        {
            String resultado = String.Empty;
            try
            {
                String cnpj = Session["CNPJ"] != null? Session["CNPJ"].ToString() : String.Empty;
                if (String.IsNullOrEmpty(posto.cnpj))
                {
                    ModelState.AddModelError("", "Favor inserir o CNPJ do posto.");
                    resultado = "Favor inserir CPF do administrador.";
                }
                else if (String.IsNullOrEmpty(posto.cpfAdmPosto)
                    || String.IsNullOrEmpty(posto.admPosto))
                {
                    ModelState.AddModelError("", "Favor inserir CPF do administrador e o nome do administrador.");
                    resultado = "Favor inserir CPF do administrador.";
                }
                else if (String.IsNullOrEmpty(posto.senha))
                {
                    ModelState.AddModelError("", "Favor inserir uma senha para o administrador.");
                    resultado = "Favor inserir CPF do administrador.";
                }
                else
                {
                    DataBase.AtualizarAdmPosto(posto);
                    resultado = "True";
                }
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", ex.Message);
                resultado = ex.Message;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                resultado = ex.Message;
            }
            return Json(new { success = resultado });
        }

        // GET: Posto/CarteiraPaciente
        [HttpGet]
        public ActionResult CarteiraPaciente()
        {
            String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
            if (String.IsNullOrEmpty(perfil))
            {
                return RedirectToAction("Index");
            }
            else if (perfil.Equals("Administrador", StringComparison.InvariantCultureIgnoreCase))
            {
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Você não está autorizado a acessar essa pagina.");
                return RedirectToAction("Index", "Posto");
            }
        }

        [HttpPost]
        public ActionResult CarteiraPaciente(string cartaoCidadao)
        {
         
            String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
            if (String.IsNullOrEmpty(perfil))
            {
                return RedirectToAction("Index");
            }
            else if (perfil.Equals("Administrador", StringComparison.InvariantCultureIgnoreCase))
            {
                CarteiraVacinacao carteira = DataBase.GetCarteiraVacinacao(cartaoCidadao);
                return View(carteira);
            }
            else
            {
                ModelState.AddModelError("", "Você não está autorizado a acessar essa pagina.");
                return RedirectToAction("Index", "Posto");
            }
        }

        // GET: Posto/Vacinas/
        public ActionResult Vacinas()
        {

            String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
            if (String.IsNullOrEmpty(perfil))
            {
                return RedirectToAction("Entrar", "Home");
            }
            else if (perfil.Equals("Administrador", StringComparison.InvariantCultureIgnoreCase))
            {
                String cnpj = Session["CNPJ"] == null ? String.Empty : Session["CNPJ"].ToString();

                IEnumerable<Vacina> vacinasPosto = DataBase.ListaVacinas(cnpj);
                return View(vacinasPosto);
            }
            else
            {
                ModelState.AddModelError("", "Você não está autorizado a acessar essa pagina.");
                return RedirectToAction("Entrar", "Home");
            }
        }

        // GET: Posto/Vacinas/
        [ChildActionOnly]
        public ActionResult VacinasAjax()
        {
             String cnpj = Session["CNPJ"] == null ? String.Empty : Session["CNPJ"].ToString();
            IEnumerable<Vacina> vacinas = DataBase.ListaVacinas(cnpj);
            return PartialView("_Vacinas", vacinas);
        }

        [HttpPost]
        public JsonResult BloquearHorarios(string diaBloquear, string horaBloquear)
        {
            HorariosBloqueados horarios = new HorariosBloqueados()
            {
                diaBloqueado = diaBloquear,
                horariosBloqueados = horaBloquear
            };

            try
            {
                if (DataBase.AdicionarHorariosBloqueados(horarios))
                {

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = true });
            }
        }

        // GET: Paciente/GetPostos
        public ActionResult GetPostos()
        {
            IEnumerable<Posto> postos = DataBase.ListaPostos();
            return PartialView("~/Views/Shared/_Postos.cshtml", postos);
        }

        // GET: Paciente/GetPostos
        public ActionResult GetVacinasPosto(string cnpj, string data)
        {
            IEnumerable<AgendaVacina> vacinas;
            cnpj = Session["CNPJ"] == null ? String.Empty : Session["CNPJ"].ToString();
            if (!string.IsNullOrEmpty(data))
            {
                vacinas = DataBase.AgendamentosPosto(cnpj, Convert.ToDateTime(data));
            }
            else
            {
                vacinas = DataBase.AgendamentosPosto(cnpj);
            }
            return PartialView("~/Views/Shared/_Agendamentos.cshtml", vacinas);
        }

        private bool ValidaVacinas(Vacina vacina)
        {
            bool valido = true;
            if (!Valida.Data(vacina.dataValidade))
            {
                //throw new Exception("Data invalida, por favor utilize uma data válida.");
            }
            else if (!Valida.CodVacina(vacina.codVacina))
            {
                throw new Exception("Código inválido, por favor tente novamente.");
            }
            else if (!Valida.LoteVacina(vacina.loteVacina))
            {
                throw new Exception("Formato do Lote inválido, por favor insira um válido.");
            }

            return valido;
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
        public JsonResult DeleteVacina(string lote)
        {
            String resultado = String.Empty;
            try
            {
                if (!String.IsNullOrEmpty(lote))
                {

                    String cnpj = Session["CNPJ"] == null ? String.Empty : Session["CNPJ"].ToString();
                    if (!String.IsNullOrEmpty(cnpj))
                    {
                        if (DataBase.DeletaVacina(lote, cnpj))
                            resultado = "Vacina deletada com sucesso!";
                        else
                            resultado = "A vacina não pôde ser deletada. Tente novamente mais tarde.";
                    }
                    else
                    {
                        resultado = "Favor fazer login antes de executar essa ação.";
                    }
                }
                else
                {
                    resultado = "Vacina inválida.";
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
            }
            return Json(new { success = resultado });
        }

        [HttpPost]
        public JsonResult AdicionaVacinaAplicada(string cartao, string vacina, string aplicacao, string reforco)
        {
            String resultado = String.Empty;
            try
            {
                if (!String.IsNullOrEmpty(vacina) && !String.IsNullOrEmpty(aplicacao)
                    && !String.IsNullOrEmpty(reforco) && !String.IsNullOrEmpty(cartao))
                {

                    String perfil = Session["Perfil"] == null ? String.Empty : Session["Perfil"].ToString();
                    if (!String.IsNullOrEmpty(perfil))
                    {
                        DateTime dataAplic = DateTime.Parse(aplicacao);
                        DateTime dataRef = DateTime.Parse(reforco);

                        if (DataBase.SalvaVacinaAplicada(vacina, dataAplic, dataRef, cartao))
                            resultado = "Vacina adicionada com sucesso!";
                        else
                            resultado = "A vacina não pôde ser adicionada. Tente novamente mais tarde.";
                    }
                    else
                    {
                        resultado = "Favor fazer login antes de executar essa ação.";
                    }
                }
                else
                {
                    resultado = "Informações inválidas, verifique os dados inseridos.";
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
            }
            return Json(new { success = resultado });
        }

    }
}
