using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senai.Financas.Mvc.Web.Models;
using Senai.Financas.Mvc.Web.Repositorios;

namespace Senai.Financas.Mvc.Web.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult Cadastro(){
            return View();
        }

        [HttpPost]
        public ActionResult Cadastro(IFormCollection form){
            UsuarioModel usuarioModel = new UsuarioModel(nome:form["nome"], email:form["email"], senha:form["senha"], dataNascimento:DateTime.Parse(form["dataNascimento"]));

            usuarioModel.Nome = form["nome"];
            usuarioModel.Email = form["email"];
            usuarioModel.DataNascimento = DateTime.Parse(form["dataNascimento"]);
            usuarioModel.Senha = form["senha"];
            
            UsuarioRepositorio usuarioRepositorio = new UsuarioRepositorio();
            usuarioRepositorio.Cadastrar(usuarioModel);

            ViewBag.Mensagem = "Usuário Cadastrado";

            return View();
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(IFormCollection form)
        {
            
            //Pega os dados do POST
            UsuarioModel usuario = new UsuarioModel
            (
                email: form["email"],
                senha: form["senha"]
            );

            //Verificar se o usuário possuí acesso para realizazr login
            UsuarioRepositorio usuarioRep = new UsuarioRepositorio();
            
            UsuarioModel usuarioModel = usuarioRep.BuscarPorEmailESenha(usuario.Email, usuario.Senha);

            if (usuarioModel != null)
            {
                HttpContext.Session.SetString("idUsuario", usuarioModel.Email.ToString());

                ViewBag.Mensagem = "Login realizado com sucesso!";

                return RedirectToAction("Cadastrar", "Transacao");
            }
            else
            {
                ViewBag.Mensagem = "Acesso negado!";
            }

            return View();
        }
        
        /// <summary>
        /// Lista todos os usuários cadastrados no sistema
        /// </summary>
        /// <returns>A view do usuário</returns>
        [HttpGet]
        public IActionResult Listar(){
            UsuarioRepositorio rep = new UsuarioRepositorio();

            //Buscando os dados do repositório e aplicando no view bag
            // ViewBag.Usuarios = rep.Listar();
            ViewData["Usuarios"] = rep.Listar();
            

            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id){
            UsuarioRepositorio rep = new UsuarioRepositorio();
            rep.Excluir(id);

            TempData["Mensagem"] = "Usuário excluído";
            
            return RedirectToAction("Listar");
        }

        [HttpGet]
        public IActionResult Editar (int id){
            

            if(id == 0){
                TempData["Mensagem"] = "Informe um id";
                return RedirectToAction("Listar");
            }

            UsuarioRepositorio usuarioRepositorio = new UsuarioRepositorio();
            UsuarioModel usuario = usuarioRepositorio.BuscarPorId(id);

            if(usuario != null){
                ViewBag.Usuario = usuario;
                return View();
            }
            
            TempData["Mensagem"] = "Usuário não encontrado";
            return RedirectToAction("Listar");
        }

        [HttpPost]
        public IActionResult Editar (IFormCollection form)
        {
            UsuarioModel usuario = new UsuarioModel(id:int.Parse(form["id"]), nome:form["nome"], email:form["email"], senha:form["senha"], dataNascimento:DateTime.Parse(form["dataNascimento"]));
            usuario.ID = int.Parse(form["id"]);
            usuario.Nome = form["nome"];
            usuario.Email = form["email"];
            usuario.Senha = form["senha"];
            usuario.DataNascimento = DateTime.Parse(form["dataNascimento"]);

            UsuarioRepositorio usuarioRepositorio = new UsuarioRepositorio();
            usuarioRepositorio.Editar(usuario);
            
            TempData["Mensagem"] = "Usuário editado com sucesso";
            return RedirectToAction("Listar");
        }
    }
}