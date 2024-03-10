using ControleDeContatos.Filters;
using ControleDeContatos.Models;
using ControleDeContatos.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    [PaginaRestritaSomenteAdmin]
    public class UsuarioController : Controller
    {

        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public IActionResult Index()
        {
            var usuarios = _usuarioRepository.BuscarTodos();
            return View(usuarios);
        }

        public IActionResult Criar()
        {
            return View();
        } 
        
        public IActionResult Editar(int id)
        {
            var usuario = _usuarioRepository.BuscarPorId(id);
            return View(usuario);
        }

        public IActionResult ApagarConfirmacao(int id)
        {
            var usuario = _usuarioRepository.BuscarPorId(id);
            return View(usuario);
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                var apagado = _usuarioRepository.Apagar(id);
                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Usuario apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = $"Não foi possível apagar o usuario, tente novamente";
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Não foi possível apagar o usuario, tente novamente, detalhe do erro: {e.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usuarioRepository.Adicionar(usuario);
                    TempData["MensagemSucesso"] = "Usuario cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(usuario);

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Não foi possível cadastrar o usuario, tente novamente, detalhe do erro: {e.Message}";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public IActionResult Editar(UsuarioSemSenhaModel usuarioSemSenha)
        {
            try
            {
                UsuarioModel usuario = null;
                if (ModelState.IsValid)
                {
                    usuario = new UsuarioModel()
                    {
                        Id = usuarioSemSenha.Id,
                        Nome = usuarioSemSenha.Nome,
                        Login = usuarioSemSenha.Login,
                        Email = usuarioSemSenha.Email,
                        Perfil = usuarioSemSenha.Perfil
                    };
                    _usuarioRepository.Atualizar(usuario);
                    TempData["MensagemSucesso"] = "Contato atualizado com sucesso!";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Não foi possível atualizar o contato, tente novamente, detalhe do erro: {e.Message}";
                return RedirectToAction("Index");
            }

        }
    }
}
