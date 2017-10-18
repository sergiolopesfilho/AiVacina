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
    }
}
