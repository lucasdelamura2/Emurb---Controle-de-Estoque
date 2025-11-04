using EmurbEstoque.Models;
using EmurbEstoque.Models.ViewModels;
using EmurbEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmurbEstoque.Controllers
{
    public class UsuarioController : Controller 
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository, IFuncionarioRepository funcionarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _funcionarioRepository = funcionarioRepository;
        }

        private void PrepararViewBagFuncionarios()
        {
            ViewBag.ListaFuncionarios = new SelectList(
                _funcionarioRepository.Read(), "IdPessoa", "Nome"
            );
        }

        public IActionResult Index()
        {
            ViewBag.NomesFuncionarios = _funcionarioRepository.Read()
                .ToDictionary(f => f.IdPessoa, f => f.Nome);
            
            var usuarios = _usuarioRepository.GetAll();
            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PrepararViewBagFuncionarios();
            return View(new UsuarioCreateViewModel());
        }

        [HttpPost]
        public IActionResult Create(UsuarioCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                PrepararViewBagFuncionarios();
                return View(viewModel);
            }

            if (_usuarioRepository.EmailExists(viewModel.Email))
            {
                ModelState.AddModelError("Email", "Este e-mail já está em uso.");
                PrepararViewBagFuncionarios();
                return View(viewModel);
            }

            Usuario novoUsuario = viewModel; 
            _usuarioRepository.Create(novoUsuario);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var usuario = _usuarioRepository.GetById(id);
            if (usuario == null)
            {
                return NotFound();
            }
            
            PrepararViewBagFuncionarios(); 
            return View(usuario); 
        }

        [HttpPost]
        public IActionResult Edit(int id, Usuario usuario)
        {
            usuario.IdUsuario = id;
            ModelState.Remove("Senha");

            if (!ModelState.IsValid)
            {
                PrepararViewBagFuncionarios();
                return View(usuario);
            }

            var usuarioExistente = _usuarioRepository.GetById(id);
            if (usuarioExistente == null)
            {
                return NotFound();
            }
            
            usuario.Senha = usuarioExistente.Senha;
            
            if (usuarioExistente.Email != usuario.Email && _usuarioRepository.EmailExists(usuario.Email))
            {
                ModelState.AddModelError("Email", "Este e-mail já está em uso por outra conta.");
                PrepararViewBagFuncionarios();
                return View(usuario);
            }

            _usuarioRepository.Update(usuario);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ResetPassword(int id)
        {
            var usuario = _usuarioRepository.GetById(id);
            if (usuario == null)
            {
                return NotFound();
            }

            var viewModel = new ResetPasswordViewModel
            {
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Email
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var usuario = _usuarioRepository.GetById(viewModel.IdUsuario);
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Senha = viewModel.NovaSenha;
            _usuarioRepository.Update(usuario);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            _usuarioRepository.Delete(id); 
            return RedirectToAction(nameof(Index));
        }

    } 
}