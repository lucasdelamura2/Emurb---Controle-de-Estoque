using EmurbEstoque.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmurbEstoque.Models.ViewModels
{
    // Este Model especial carrega TODOS os dados que a tela de "Details" precisa
    public class OrdemEntradaDetailsViewModel
    {
        // 1. Os dados da Ordem (cabeçalho)
        public OrdemEntrada Ordem { get; set; }
        
        // 2. O nome do Fornecedor (para exibição)
        public string NomeFornecedor { get; set; }

        // 3. A lista de Lotes/Itens já adicionados
        public List<Lote> ItensDaOrdem { get; set; }
        
        // 4. O objeto de formulário para adicionar um NOVO lote
        public Lote NovoLoteForm { get; set; }

        // 5. A lista de produtos para o dropdown
        public SelectList ListaProdutos { get; set; }
    }
}