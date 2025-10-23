using EmurbEstoque.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmurbEstoque.Models.ViewModels
{
    public class OrdemSaidaDetailsViewModel
    {
        public OrdemSaida Ordem { get; set; }
        public string NomeFuncionario { get; set; }
        public string DescricaoAutorizacao { get; set; }
        public List<ItensOS> ItensDaOrdem { get; set; }
        public ItensOS NovoItemForm { get; set; }
        public SelectList ListaLotesDisponiveis { get; set; }
    }
}