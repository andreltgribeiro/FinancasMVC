using System;
using System.Collections.Generic;
using System.IO;
using FinancasMVC.Interfaces;
using Microsoft.AspNetCore.Http;
using Senai.Financas.Mvc.Web.Models;

namespace Senai.Financas.Mvc.Web.Repositorios
{
    public class UsuarioRepositorio : IUsuario
    {
        public UsuarioModel BuscarPorEmailESenha(string email, string senha)
        {
            List<UsuarioModel> usuariosCadastrados = CarregarDoCSV();

            //Percorro cada usuário da lista do CSV...
            foreach (UsuarioModel usuario in usuariosCadastrados)
            {
                if (usuario.Email == email && usuario.Senha == senha)
                {
                    return usuario;
                }
            }

            //Caso  sistema não encontre nenhuma combinação de email e senha retorna nulls
            return null;
        }

        public List<UsuarioModel> Listar() => CarregarDoCSV();

        /// <summary>
        /// Carrega a lista de usuários com os dados do CSV
        /// </summary>
        private List<UsuarioModel> CarregarDoCSV()
        {
            List<UsuarioModel> lsUsuarios = new List<UsuarioModel>();

            //Abre o stream de leitura do arquivo
            string[] linhas = File.ReadAllLines("usuarios.csv");

            //Lê cada registro no CSV
            foreach (string linha in linhas)
            {
                //Verificando se é uma linha vazia
                if(string.IsNullOrEmpty(linha)){
                    continue;//Pula para o proximo registro do laço
                }

                //Separa os dados da linha
                string[] dadosDaLinha = linha.Split(';');

                //Cria o objeto com os dados da linha do CSV
                UsuarioModel usuario = new UsuarioModel
                {
                    ID = int.Parse(dadosDaLinha[0]),
                    Nome = dadosDaLinha[1],
                    Email = dadosDaLinha[2],
                    Senha = dadosDaLinha[3],
                    DataNascimento = DateTime.Parse(dadosDaLinha[4])
                };

                //Adicionando o usuário na lista
                lsUsuarios.Add(usuario);
            }       
            return lsUsuarios;     
        }

        /// <summary>
        /// Exclui um registro do CSV
        /// </summary>  
        /// <param name="id">O ID do usuário Cadastrado</param>
        public void Excluir (int id){
            //Abre o stream de leitura do arquivo
            string[] linhas = File.ReadAllLines("usuarios.csv");

            //Lê cada registro no CSV
            for (int i = 0; i <linhas.Length; i++)
            {
                //Separa os dados da linha
                string[] dadosDaLinha = linhas[i].Split(';');

                if(id.ToString() == dadosDaLinha[0]) {
                    linhas[i] = "";
                    break;
                }
            }
            File.WriteAllLines("usuarios.csv", linhas);

        }

        public UsuarioModel Cadastrar(UsuarioModel usuario)
        {
            
            if(File.Exists("usuarios.csv")){
                usuario.ID = System.IO.File.ReadAllLines("usuarios.csv").Length+1;
            }else{
                usuario.ID = 1;   
            }
            using(StreamWriter sw = new StreamWriter("usuarios.csv",true)){
                sw.WriteLine($"{usuario.ID};{usuario.Nome};{usuario.Email};{usuario.Senha};{usuario.DataNascimento}");
            }
            return usuario;
        }

        public UsuarioModel Editar(UsuarioModel usuario)
        {
            string[] linhas = System.IO.File.ReadAllLines("usuarios.csv");

                for (int i = 0; i < linhas.Length; i++)
            {
                if(string.IsNullOrEmpty(linhas[i])){
                    continue;
                }
                string[] dados = linhas[i].Split(';');


                    if(dados[0] == usuario.ID.ToString()){
                    linhas[i] = $"{usuario.ID};{usuario.Nome};{usuario.Email};{usuario.Senha};{usuario.DataNascimento}";
                    break;
                    }                
            }
            System.IO.File.WriteAllLines("usuarios.csv", linhas);

            return usuario;
        }

        public UsuarioModel BuscarPorId(int id)
        {
            string[] linhas = System.IO.File.ReadAllLines ("usuarios.csv");

            for (int i = 0; i < linhas.Length; i++) {
                if (string.IsNullOrEmpty (linhas[i])) {
                    continue;
                }
                string[] dados = linhas[i].Split (';');

                if (dados[0] == id.ToString ()) {
                    UsuarioModel usuario = new UsuarioModel ();
                    usuario.ID = int.Parse (dados[0]);
                    usuario.Nome = dados[1];
                    usuario.Email = dados[2];
                    usuario.Senha = dados[3];
                    usuario.DataNascimento = DateTime.Parse (dados[4]);

                    return usuario;
                }

            }
            return null;
        }
    }
}