using EmurbEstoque.Repositories; 

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IFuncionarioRepository>(_ => new FuncionarioDatabaseRepository(connectionString));
builder.Services.AddTransient<IProdutoRepository>(_ => new ProdutoDatabaseRepository(connectionString));
builder.Services.AddTransient<IFornecedorRepository>(_ => new FornecedorDatabaseRepository(connectionString));
builder.Services.AddTransient<IOrdemEntradaRepository>(_ => new OrdemEntradaDatabaseRepository(connectionString));
builder.Services.AddTransient<ILoteRepository>(_ => new LoteDatabaseRepository(connectionString));
builder.Services.AddTransient<ILocalRepository>(_ => new LocalDatabaseRepository(connectionString));
builder.Services.AddTransient<IAutorizacaoRepository>(_ => new AutorizacaoDatabaseRepository
(connectionString));
builder.Services.AddTransient<IAutorizadoRepository>(_ => new AutorizadoDatabaseRepository(connectionString));
builder.Services.AddTransient<IOrdemSaidaRepository>(_ => new OrdemSaidaDatabaseRepository(connectionString));
builder.Services.AddTransient<IItensOSRepository>(_ => new ItensOSDatabaseRepository(connectionString));
builder.Services.AddTransient<IPessoaRepository>(_ => new PessoaDatabaseRepository(connectionString));
builder.Services.AddTransient<IEstoqueRepository>(sp => 
    new EstoqueDatabaseRepository(
        sp.GetRequiredService<IProdutoRepository>(),
        sp.GetRequiredService<ILoteRepository>(),
        sp.GetRequiredService<IItensOSRepository>()
    )
);


// builder.Services.AddSingleton<IFuncionarioRepository, FuncionarioMemoryRepository>();
// builder.Services.AddSingleton<IProdutoRepository, ProdutoMemoryRepository>(); 
// builder.Services.AddSingleton<IFornecedorRepository, FornecedorMemoryRepository>();
// builder.Services.AddSingleton<IOrdemEntradaRepository, OrdemEntradaMemoryRepository>();
// builder.Services.AddSingleton<ILoteRepository, LoteMemoryRepository>();
// builder.Services.AddSingleton<ILocalRepository, LocalMemoryRepository>();              
// builder.Services.AddSingleton<IAutorizadoRepository, AutorizadoMemoryRepository>();    
 //builder.Services.AddSingleton<IAutorizacaoRepository, AutorizacaoMemoryRepository>();  
// builder.Services.AddSingleton<IOrdemSaidaRepository, OrdemSaidaMemoryRepository>();    
// builder.Services.AddSingleton<IItensOSRepository, ItensOSMemoryRepository>();
// builder.Services.AddSingleton<IEstoqueRepository, EstoqueMemoryRepository>();


var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); 
}
app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();