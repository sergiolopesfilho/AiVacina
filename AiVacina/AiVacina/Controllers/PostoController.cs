using AiVacina.DAL;
using AiVacina.Models;
using AiVacina.Validação;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                PacienteLogin dbAdm = DataBase.GetLoginPacienteCPF(adm.cpfAdmPosto);
                if (dbAdm.senha.Equals(adm.senha) && dbAdm.perfil.Equals("Administrador", StringComparison.InvariantCultureIgnoreCase) 
                    && dbAdm.numCartaoCidadao.Equals(adm.cpfAdmPosto))
                {
                    System.Web.Security.FormsAuthentication.SetAuthCookie(dbAdm.nome, false);
                    Session["Nome"] = dbAdm.nome;
                    Session["Cartao"] = dbAdm.numCartaoCidadao;
                    Session["Perfil"] = dbAdm.perfil;
                    Session["CNPJ"] = dbAdm.perfil;

                    return RedirectToAction("CadastrarVacinas");
                }
                else
                {
                    ModelState.AddModelError("", "Senha incorreta, tente novamente");
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
                return RedirectToAction("Entrar", "Home");
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
                return RedirectToAction("Entrar", "Home");
            }
        }

        // POST: Posto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastrarVacinas(Vacina vacina)
        {
            try
            {
                vacina.loteVacina = vacina.loteVacina.ToUpper();
                if (ValidaVacinas(vacina))
                {
                    if ((DateTime.Today.CompareTo(Convert.ToDateTime(vacina.dataValidade))) >= 0)
                        throw new Exception("Data de validade não pode ser menor que o dia atual.");
                    String[] data = vacina.dataValidade.Split('/');
                    DateTime dataUS = DateTime.Parse(data[1] + "/" + data[0] + "/" + data[2]);
                    vacina.dataValidade = (data[2] + "-" + data[1] + "-" + data[0]);
                    DataBase.CadastrarVacina(vacina, dataUS);
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
                return RedirectToAction("Entrar", "Home");
            }
        }

        // POST: Posto/CadastroAdministrador/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastroAdministrador(Posto posto)
        {
            try
            {
                if (posto.idEstabelecimento <= 0)
                {
                    ModelState.AddModelError("", "Favor escolha o posto a ser atualizado.");
                    return View(posto);
                }
                else if (String.IsNullOrEmpty(posto.cpfAdmPosto)
                    || String.IsNullOrEmpty(posto.admPosto))
                {
                    ModelState.AddModelError("", "Favor inserir o nume e cpf do novo administrador.");
                    return View(posto);
                }

                DataBase.AtualizarAdmPosto(posto);
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {

                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Cadastro", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(posto);
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
    }
}
