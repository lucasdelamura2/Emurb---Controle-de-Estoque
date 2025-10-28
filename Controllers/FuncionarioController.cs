using EmurbEstoque.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers;

public class FuncionarioController : Controller
{
    private static readonly List<Funcionario> _mem = new();
    private static int _nextId = 1;

    public IActionResult Index()
    {
        return View(_mem.OrderBy(f => f.IdFuncionario).ToList());
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Funcionario funcionario)
    {
        funcionario.IdFuncionario = _nextId++;
        _mem.Add(funcionario);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int id)
    {
        var f = _mem.FirstOrDefault(x => x.IdFuncionario == id);
        if (f is null) return NotFound();
        return View(f);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var f = _mem.FirstOrDefault(x => x.IdFuncionario == id);
        if (f is null) return NotFound();
        return View(f);
    }

    [HttpPost]
    public IActionResult Edit(int id, Funcionario dados)
    {
        var f = _mem.FirstOrDefault(x => x.IdFuncionario == id);
        if (f is null) return NotFound();

        f.Nome = dados.Nome;
        f.CpfCnpj = dados.CpfCnpj;
        f.Email = dados.Email;
        f.Telefone = dados.Telefone;
        f.Cargo = dados.Cargo;
        f.Setor = dados.Setor;

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var f = _mem.FirstOrDefault(x => x.IdFuncionario == id);
        if (f is null) return NotFound();
        _mem.Remove(f);
        return RedirectToAction(nameof(Index));
    }
}