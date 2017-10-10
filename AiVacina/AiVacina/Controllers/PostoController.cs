using AiVacina.DAL;
using AiVacina.Models;
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
                DataBase.AgendamentosVacina("123.1231.2312.1323");
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
                // TODO: Add insert logic here

                return RedirectToAction("Vacinas");
            }
            catch
            {
                return View();
            }
        }


        // GET: Posto/Create
        [HttpGet]
        public ActionResult CadastroAdministrador()
        {
            return View();
        }

        // POST: Posto/Edit/5
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

        // GET: Posto/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Posto/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Paciente/GetPostos
        public ActionResult GetPostos()
        {
            IEnumerable<Posto> postos = DataBase.ListaPostos();
            return PartialView("~/Views/Shared/_Postos.cshtml", postos);
        }
    }
}
