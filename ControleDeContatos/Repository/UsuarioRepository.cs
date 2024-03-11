using ControleDeContatos.Data;
using ControleDeContatos.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeContatos.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;
        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public UsuarioModel BuscarPorLogin(string login)
        {
            return _context.Usuarios.FirstOrDefault(l => l.Login.ToUpper() == login.ToUpper());
        }

        public UsuarioModel BuscarPorEmailELogin(string email, string login)
        {
            return _context.Usuarios.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper() && x.Login.ToUpper() == login.ToUpper());
        }

        public UsuarioModel BuscarPorId(int id)
        {
            return _context.Usuarios.FirstOrDefault(c => c.Id == id);
        }

        public List<UsuarioModel> BuscarTodos()
        {
            return _context.Usuarios.Include(y => y.Contatos).ToList();
        }

        public UsuarioModel Adicionar(UsuarioModel usuario)
        {
            usuario.DataCadastro = DateTime.Now;
            usuario.SetSenhaHash();
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario;
        }

        public UsuarioModel Atualizar(UsuarioModel usuario)
        {
            var usuarioDB = BuscarPorId(usuario.Id);

            if(usuarioDB is null) 
            {
                throw new Exception("Houve um erro ao atualizar o Usuario!");
            }

            usuarioDB.Nome = usuario.Nome;
            usuarioDB.Email = usuario.Email;
            usuarioDB.Login = usuario.Login;
            usuarioDB.Perfil = usuario.Perfil;
            usuario.DataAtualizacao = DateTime.Now;

            _context.Update(usuarioDB);
            _context.SaveChanges();

            return usuarioDB;
        }

        public UsuarioModel AlterarSenha(AlterarSenhaModel alterarSenhaModel)
        {
            UsuarioModel usuarioDB = BuscarPorId(alterarSenhaModel.Id);

            if (usuarioDB == null) throw new Exception("Houve um erro na atualização da senha, usuário não encontrado!");

            if (!usuarioDB.SenhaValida(alterarSenhaModel.SenhaAtual)) throw new Exception("Senha atual não confere!");

            if (usuarioDB.SenhaValida(alterarSenhaModel.NovaSenha)) throw new Exception("Nova senha deve ser diferente da senha atual!");

            usuarioDB.SetNovaSenha(alterarSenhaModel.NovaSenha);
            usuarioDB.DataAtualizacao = DateTime.Now;

            _context.Usuarios.Update(usuarioDB);
            _context.SaveChanges();

            return usuarioDB;
        }

        public bool Apagar(int id)
        {
            var usuarioDB = BuscarPorId(id);

            if (usuarioDB is null)
            {
                throw new Exception("Houve um erro ao apagar o Usuario!");
            }

            _context.Usuarios.Remove(usuarioDB);
            _context.SaveChanges();

            return true;
        }
    }
}
