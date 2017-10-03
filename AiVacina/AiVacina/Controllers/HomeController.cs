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
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/Cadastro/
        public ActionResult Cadastro()
        {
            return View();
        }
        
        [HttpPost]
        // GET: Home/Cadastro/
        public ActionResult Cadastro(Paciente paciente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ValidaCadastro(paciente);

                    DataBase.CadastrarPaciente(paciente);
                    ///TODO:
                    ///Adicionar ao banco

                }
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View(paciente);
            }
        }

        // GET: Home/Entrar
        public ActionResult Entrar()
        {
            return View();
        }

        // POST: Home/Entrar
        [HttpPost]
        public ActionResult Entrar(Paciente paciente)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
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

        private bool ValidaCadastro(Paciente paciente)
        {
            bool valido = true;
            if (!Valida.Data(paciente.dataNascimento))
            {
                throw new Exception("Data invalida, por favor utilize uma data válida.");
            }
            //else if(!Valida.CartaoCidadao(paciente.numCartaoCidadao))
            //{
            //    throw new Exception("Número do Cartão Cidadão inválido.");
            //}

            return valido;
        }
    }
}
