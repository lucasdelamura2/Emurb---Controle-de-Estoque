using EmurbEstoque.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers;

public class FuncionarioController : Controller
{
    private static readonly List<Funcionario> _mem = new();
    private static int _nextId = 1;

    public IActionResult Index()
    {
        return View(_mem.OrderBy(f => f.Id).ToList());
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Funcionario funcionario)
    {
        funcionario.Id = _nextId++;
        _mem.Add(funcionario);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int id)
    {
        var f = _mem.FirstOrDefault(x => x.Id == id);
        if (f is null) return NotFound();
        return View(f);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var f = _mem.FirstOrDefault(x => x.Id == id);
        if (f is null) return NotFound();
        return View(f);
    }

    [HttpPost]
    public IActionResult Edit(int id, Funcionario dados)
    {
        var f = _mem.FirstOrDefault(x => x.Id == id);
        if (f is null) return NotFound();

        f.Nome = dados.Nome;
        f.CPF = dados.CPF;
        f.DataNascimento = dados.DataNascimento;

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var f = _mem.FirstOrDefault(x => x.Id == id);
        if (f is null) return NotFound();
        _mem.Remove(f);
        return RedirectToAction(nameof(Index));
    }
}