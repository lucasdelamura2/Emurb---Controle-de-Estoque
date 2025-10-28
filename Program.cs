using EmurbEstoque.Repositories; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IFuncionarioRepository, FuncionarioMemoryRepository>();
builder.Services.AddSingleton<IFornecedorRepository, FornecedorMemoryRepository>();
builder.Services.AddSingleton<IProdutoRepository, ProdutoMemoryRepository>();
builder.Services.AddSingleton<IOrdemEntradaRepository, OrdemEntradaMemoryRepository>();
builder.Services.AddSingleton<ILoteRepository, LoteMemoryRepository>();
builder.Services.AddSingleton<ILocalRepository, LocalMemoryRepository>();              
builder.Services.AddSingleton<IAutorizadoRepository, AutorizadoMemoryRepository>();    
builder.Services.AddSingleton<IAutorizacaoRepository, AutorizacaoMemoryRepository>();  
builder.Services.AddSingleton<IOrdemSaidaRepository, OrdemSaidaMemoryRepository>();
builder.Services.AddSingleton<IItensOSRepository, ItensOSMemoryRepository>();
builder.Services.AddSingleton<IEstoqueRepository, EstoqueMemoryRepository>();
builder.Services.AddSingleton<IPessoaRepository, PessoaMemoryRepository>();
builder.Services.AddSingleton<IUsuarioRepository, UsuarioMemoryRepository>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Login/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();