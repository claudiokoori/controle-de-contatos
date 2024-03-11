using ControleDeContatos.Data;
using ControleDeContatos.Models;

namespace ControleDeContatos.Repository
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly AppDbContext _context;
        public ContatoRepository(AppDbContext context)
        {
            _context = context;
        }

        public ContatoModel BuscarPorId(int id)
        {
            return _context.Contatos.FirstOrDefault(c => c.Id == id);
        }

        public List<ContatoModel> BuscarTodos(int usuarioId)
        {
            return _context.Contatos.Where(y => y.UsuarioId == usuarioId).ToList();
        }

        public ContatoModel Adicionar(ContatoModel contato)
        {
            _context.Contatos.Add(contato);
            _context.SaveChanges();
            return contato;
        }

        public ContatoModel Atualizar(ContatoModel contato)
        {
            var contatoDB = BuscarPorId(contato.Id);

            if(contatoDB is null) 
            {
                throw new Exception("Houve um erro ao atualizar o contato!");
            }

            contatoDB.Nome = contato.Nome;
            contatoDB.Email = contato.Email;
            contatoDB.Celular = contato.Celular;

            _context.Update(contatoDB);
            _context.SaveChanges();

            return contatoDB;
        }

        public bool Apagar(int id)
        {
            var contatoDB = BuscarPorId(id);

            if (contatoDB is null)
            {
                throw new Exception("Houve um erro ao apagar o contato!");
            }

            _context.Contatos.Remove(contatoDB);
            _context.SaveChanges();

            return true;
        }
    }
}
