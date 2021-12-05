
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddDbContext<Contexto>
//    (options => options.UseSqlServer(
//        "Data Source=DESKTOP-HVNTI80\\DESENVOLVIMENTO;Initial Catalog=MINIMA_API_AULA;Integrated Security=False;User ID=sa;Password=1234;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False"));

//builder.Services.AddDbContext<Contexto>
//    (options => options.UseMySql(
//        "server=localhost;initial catalog=MINIMA_API_AULA;uid=root;pwd=1234", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.25-mysql")));

//Install-Package Npgsql.EntityFrameworkCore.PostgreSQL

builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<Contexto>
    (option => 
    option.UseNpgsql("Host=localhost;Port=5432;Pooling=true;Database=MINIMA_API_AULA;User Id=postgres;Password=1234;"));

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.MapPost("AdicionaProduto", async (Produto produto, Contexto contexto) =>
{
    contexto.Produto.Add(produto);
    await contexto.SaveChangesAsync();
});

app.MapPost("ExcluirProduto/{id}", async (int id, Contexto contexto) =>
{
    var produtoExcluir = await contexto.Produto.FirstOrDefaultAsync(p => p.Id == id);
    if (produtoExcluir != null)
    {
        contexto.Produto.Remove(produtoExcluir);
        await contexto.SaveChangesAsync();
    }
});

app.MapPost("ListarProdutos", async (Contexto contexto) =>
{
    return await contexto.Produto.ToListAsync();
});

app.MapPost("ObterProduto/{id}", async (int id, Contexto contexto) =>
{
    return await contexto.Produto.FirstOrDefaultAsync(p => p.Id == id);
});


app.UseSwaggerUI();
app.Run();
