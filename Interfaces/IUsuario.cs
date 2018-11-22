using System.Collections.Generic;
using Senai.Financas.Mvc.Web.Models;

namespace FinancasMVC.Interfaces
{
    public interface IUsuario
    {
         UsuarioModel Cadastrar(UsuarioModel usuario);
         List<UsuarioModel> Listar();

         void Excluir(int id);

         UsuarioModel Editar(UsuarioModel usuario);

         UsuarioModel BuscarPorEmailESenha(string email, string senha);
         
         UsuarioModel BuscarPorId(int id);
    }
}