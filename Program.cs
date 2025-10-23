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

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();