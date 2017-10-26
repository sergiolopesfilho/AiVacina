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
   
    public class PostoController : Controller
    {
        // GET: Posto
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Agenda()
        {
            ///TODO:
            ///Atualizar o Script dessa pagina
            ///assim que for selecionado um dia no datepicker
            ///deve aparecer só as vacinas daquele dia
            IEnumerable<AgendaVacina> agendamentos =
                DataBase.AgendamentosPosto("22.323.458/0001-79");
            return View(agendamentos);
        }

        // GET: Posto/Create
        
        public ActionResult CadastrarVacinas()
        {
            return View();
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
                        throw new Exception("Data inválida, insira uma nova data.");
                    DataBase.CadastrarVacina(vacina);
                }
                return RedirectToAction("Vacinas");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vacina);
            }
        }

        // GET: Posto/Create
        [HttpGet]
        public ActionResult CadastroAdministrador()
        {
            return View();
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
                    throw new Exception("Favor escolha o posto a ser atualizado.");
                }
                else if (String.IsNullOrEmpty(posto.cpfAdmPosto)
                    || String.IsNullOrEmpty(posto.admPosto))
                {
                    throw new Exception("Favor inserir o nume e cpf do novo administrador.");
                }

                DataBase.AtualizarAdmPosto(posto);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View(posto);
            }
        }

        // GET: Posto/Vacinas/
        public ActionResult Vacinas()
        {
            ///TODO:
            ///Pegar o cnpj do posto
            IEnumerable<Vacina> vacinasPosto = DataBase.ListaVacinas("22.323.458/0001-79");
            return View(vacinasPosto);
        }

        // GET: Posto/Vacinas/
        [ChildActionOnly]
        public ActionResult VacinasAjax()
        {
            ///TODO:
            ///Pegar o cnpj do posto
            IEnumerable<Vacina> vacinas = DataBase.ListaVacinas("22.323.458/0001-79");
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
            string resultado = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(lote))
                {
                    if (DataBase.DeletaVacina(lote, "22.323.458/0001-79"))
                        resultado = "Vacina deletada com sucesso!";
                    else
                        resultado = "A vacina não pôde ser deletada. Tente novamente mais tarde.";
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
