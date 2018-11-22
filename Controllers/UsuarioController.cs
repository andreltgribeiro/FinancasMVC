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
            UsuarioModel usuarioModel = new UsuarioModel();

            usuarioModel.Nome = form["nome"];
            usuarioModel.Email = form["email"];
            usuarioModel.DataNascimento = DateTime.Parse(form["dataNascimento"]);
            usuarioModel.Senha = form["senha"];
            usuarioModel.ID = System.IO.File.ReadAllLines("usuarios.csv").Length+1;


            using(StreamWriter sw = new StreamWriter("usuarios.csv",true)){
                sw.WriteLine($"{usuarioModel.ID};{usuarioModel.Nome};{usuarioModel.Email};{usuarioModel.Senha};{usuarioModel.DataNascimento}");
            }

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
            {
                Email = form["email"],
                Senha = form["senha"]
            };

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
            string[] linhas = System.IO.File.ReadAllLines("usuarios.csv");

            if(id == 0){
                TempData["Mensagem"] = "Informe um id";
                return RedirectToAction("Listar");
            }

            for (int i = 0; i < linhas.Length; i++)
            {
                if(string.IsNullOrEmpty(linhas[i])){
                    continue;
                }
                string[] dados = linhas[i].Split(';');

                if(dados[0] == id.ToString()){
                    UsuarioModel usuario = new UsuarioModel();
                    usuario.ID = int.Parse(dados[0]);
                    usuario.Nome = dados[1];
                    usuario.Email = dados[2];
                    usuario.Senha = dados[3];
                    usuario.DataNascimento = DateTime.Parse(dados[4]);

                    ViewBag.Usuario = usuario;
                    return View();
                }
                
            }

            TempData["Mensagem"] = "Usuário não encontrado";
            return RedirectToAction("Listar");
        }

        [HttpPost]
        public IActionResult Editar (IFormCollection form)
        {
            string[] linhas = System.IO.File.ReadAllLines("usuarios.csv");

                for (int i = 0; i < linhas.Length; i++)
            {
                if(string.IsNullOrEmpty(linhas[i])){
                    continue;
                }
                string[] dados = linhas[i].Split(';');


                    if(dados[0] == form["id"]){
                    linhas[i] = $"{form["id"]};{form["nome"]};{form["email"]};{form["senha"]};{form["dataNascimento"]}";
                    break;
                    }                
            }
            System.IO.File.WriteAllLines("usuarios.csv", linhas);

            TempData["Mensagem"] = "Usuário editado com sucesso";
            return RedirectToAction("Listar");
        }
    }
}