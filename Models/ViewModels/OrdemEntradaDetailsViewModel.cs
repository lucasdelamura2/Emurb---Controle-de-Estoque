using EmurbEstoque.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmurbEstoque.Models.ViewModels
{
    public class OrdemEntradaDetailsViewModel
    {
        public OrdemEntrada Ordem { get; set; }
        public string NomeFornecedor { get; set; }
        public List<Lote> ItensDaOrdem { get; set; }
        public Lote NovoLoteForm { get; set; }
        public SelectList ListaProdutos { get; set; }
    }
}