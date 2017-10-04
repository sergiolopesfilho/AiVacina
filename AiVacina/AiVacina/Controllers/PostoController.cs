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
            IEnumerable<AgendaVacina> agendamentos =
                DataBase.AgendamentosVacina("123.1231.2312.1323");
            return View(agendamentos);
        }

        // GET: Posto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posto/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
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

        // GET: Posto/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Posto/Edit/5
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
    }
}
