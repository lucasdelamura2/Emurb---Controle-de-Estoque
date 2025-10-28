using EmurbEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly IEstoqueRepository _repository;

        public EstoqueController(IEstoqueRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var model = _repository.GetEstoqueConsolidado();
            return View(model);
        }
    }
}