using System;
using System.Collections.Generic;
using System.IO;
using Senai.Financas.Mvc.Web.Models;

namespace Senai.Financas.Mvc.Web.Repositorios
{
    public class UsuarioRepositorio
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
    }
}