using Microsoft.EntityFrameworkCore;
using WakeCommerceAPI.Data;
using WakeCommerceAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProdutoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = Path.Combine(AppContext.BaseDirectory, "ProjetoWakeCommerce.xml");
    c.IncludeXmlComments(xmlFile);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ProdutoDbContext>();
    context.Database.Migrate();
    SeedDatabase(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
void SeedDatabase(ProdutoDbContext context)
{
    if (!context.Produtos.Any())
    {
        var produtos = new List<Produto>
        {
            new Produto { Nome = "Produto 1", Estoque = 10, Valor = 100 },
            new Produto { Nome = "Produto 2", Estoque = 5, Valor = 50 },
            new Produto { Nome = "Produto 3", Estoque = 20, Valor = 200 },
            new Produto { Nome = "Produto 4", Estoque = 15, Valor = 150 },
            new Produto { Nome = "Produto 5", Estoque = 8, Valor = 80 }
        };

        context.Produtos.AddRange(produtos);
        context.SaveChanges();
    }
}
public partial class Program { }
