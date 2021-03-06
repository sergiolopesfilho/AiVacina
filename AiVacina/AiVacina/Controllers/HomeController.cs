﻿using AiVacina.DAL;
using AiVacina.Models;
using AiVacina.Validação;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
        
        // GET: Home/Cadastro/
        [HttpPost]
        public ActionResult Cadastro(Paciente paciente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ValidaCadastro(paciente);

                    String[] data = paciente.dataNascimento.Split('/');
                    paciente.data = DateTime.Parse((data[1] + "/" + data[0] + "/" + data[2]));
                    //paciente.data = DateTime.Parse(paciente.dataNascimento);
                    if (DataBase.CadastrarPaciente(paciente))
                    {
                        FormsAuthentication.SetAuthCookie(paciente.nome, false);
                        Session["Nome"] = paciente.nome;
                        Session["Cartao"] = paciente.numCartaoCidadao;
                        Session["Perfil"] = "Paciente";
                        Session["Email"] = paciente.email;
                        return RedirectToAction("Inicio", "Paciente");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Alguma coisa deu errado, tente novamente mais tarde.");
                        return View(paciente);
                    }
                }

                else
                {
                    ModelState.AddModelError("", "Por favor, certifique-se de que todos os campos estão sendo preenchidos corretamente.");
                    return View(paciente);
                }
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
        //[ValidateAntiForgeryToken]
        public ActionResult Entrar(PacienteLogin paciente)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(paciente.numCartaoCidadao) || !String.IsNullOrEmpty(paciente.senha))
                {
                    try
                    {
                        PacienteLogin dbPaciente = DataBase.GetLoginPaciente(paciente.numCartaoCidadao);
                        if (dbPaciente != null && !String.IsNullOrEmpty(paciente.numCartaoCidadao)
                            && !String.IsNullOrEmpty(paciente.senha))
                        {
                            if (paciente.senha.Equals(dbPaciente.senha))
                            {
                                FormsAuthentication.SetAuthCookie(paciente.nome, false);
                                Session["Nome"] = dbPaciente.nome;
                                Session["Cartao"] = dbPaciente.numCartaoCidadao;
                                Session["Perfil"] = dbPaciente.perfil;
                                Session["Email"] = dbPaciente.email;
                                Session["CarteiraVacinacao"] = DataBase.CarteiraVacinacaoCadastrada(dbPaciente.numCartaoCidadao);

                                return RedirectToAction("Inicio", "Paciente");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Usuário ou senha incorretos.");
                                return View(paciente);
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "Usuário não existe, favor cadastre-se.");
                            return View(paciente);
                        }
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                        return View(paciente);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Por favor, forneça o número de seu cartão cidadão e sua senha para logar.");
                    return View(paciente);
                }
            }
            else
            {
                ModelState.AddModelError("", "Não foi possível realizar seu login. Tente novamente mais tarde.");
                return View(paciente);
            }
        }

        public ActionResult Logoff()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }

        private bool ValidaCadastro(Paciente paciente)
        {
            bool valido = true;
            if (!Valida.Data(paciente.dataNascimento))
            {
                throw new Exception("Data invalida, por favor utilize uma data válida.");
            }
            else if (!Valida.CartaoCidadao(paciente.numCartaoCidadao))
            {
                throw new Exception("Número do Cartão Cidadão inválido.");
            }

            return valido;
        }
        // GET: Home
        public ActionResult DadosTeste()
        {
            return View();
        }
    }
}
