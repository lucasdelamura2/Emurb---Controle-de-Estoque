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
            
            var viewModel = new UsuarioEditViewModel
            {
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Email,
                FuncionarioId = usuario.FuncionarioId,
                IsAdmin = usuario.IsAdmin,
                IsAtivo = usuario.IsAtivo
            };
            
            PrepararViewBagFuncionarios(); 
            return View(viewModel); 
        }

        [HttpPost]
        public IActionResult Edit(int id, UsuarioEditViewModel viewModel) 
        {
            viewModel.IdUsuario = id;

            if (string.IsNullOrEmpty(viewModel.NovaSenha))
            {
                ModelState.Remove("NovaSenha");
                ModelState.Remove("ConfirmarSenha");
            }

            if (!ModelState.IsValid)
            {
                PrepararViewBagFuncionarios();
                return View(viewModel); 
            }

            var usuarioExistente = _usuarioRepository.GetById(id);
            if (usuarioExistente == null)
            {
                return NotFound();
            }
            
            if (usuarioExistente.Email != viewModel.Email && _usuarioRepository.EmailExists(viewModel.Email))
            {
                ModelState.AddModelError("Email", "Este e-mail já está em uso por outra conta.");
                PrepararViewBagFuncionarios();
                return View(viewModel);
            }
            
            usuarioExistente.Email = viewModel.Email;
            usuarioExistente.FuncionarioId = viewModel.FuncionarioId;
            usuarioExistente.IsAdmin = viewModel.IsAdmin;
            usuarioExistente.IsAtivo = viewModel.IsAtivo;

            if (!string.IsNullOrEmpty(viewModel.NovaSenha))
            {
                usuarioExistente.Senha = viewModel.NovaSenha;
            }
            _usuarioRepository.Update(usuarioExistente);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            _usuarioRepository.Delete(id); 
            return RedirectToAction(nameof(Index));
        }

    } 
}