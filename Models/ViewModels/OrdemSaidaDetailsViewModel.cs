using EmurbEstoque.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmurbEstoque.Models.ViewModels
{
    public class OrdemSaidaDetailsViewModel
    {
        // 1. Dados do Cabeçalho da Ordem
        public OrdemSaida Ordem { get; set; }

        // 2. Descrições (Nomes) para exibição
        public string NomeFuncionario { get; set; }
        public string DescricaoAutorizacao { get; set; }

        // 3. Lista de Itens (ItensOS) já adicionados a esta Ordem
        public List<ItensOS> ItensDaOrdem { get; set; }

        // 4. Objeto para o formulário "Adicionar Item"
        public ItensOS NovoItemForm { get; set; }

        // 5. Lista de Lotes disponíveis (com saldo) para o dropdown
        public SelectList ListaLotesDisponiveis { get; set; }
    }
}
