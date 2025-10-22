using EmurbEstoque.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmurbEstoque.Controllers;

public class FornecedorController : Controller
{
    public static readonly List<Fornecedor> _mem = new();
    private static int _nextId = 1;

    public IActionResult Index()
    {
        return View(_mem.OrderBy(f => f.Id).ToList());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] 
    public IActionResult Create(Fornecedor fornecedor)
    {
        if (!ModelState.IsValid)
        {
            return View(fornecedor);
        }

        fornecedor.Id = _nextId++;
        _mem.Add(fornecedor);
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
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Fornecedor dados)
    {
        if (!ModelState.IsValid)
        {
            return View(dados);
        }
        
        var f = _mem.FirstOrDefault(x => x.Id == id);
        if (f is null) return NotFound();

        f.Nome = dados.Nome;
        f.CpfCnpj = dados.CpfCnpj;
        f.Email = dados.Email;
        f.Telefone = dados.Telefone;
        f.InscricaoEstadual = dados.InscricaoEstadual;

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