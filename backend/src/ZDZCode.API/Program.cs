using Microsoft.EntityFrameworkCore;
using ZDZCode.API.Middleware;
using ZDZCode.Application.Interfaces;
using ZDZCode.Application.Services;
using ZDZCode.Domain.Interfaces.Repositories;
using ZDZCode.Infrastructure.Context;
using ZDZCode.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra Repositórios
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

// Registra Serviços
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();

var nuxtOrigin = builder.Configuration["AllowedOrigins:Nuxt"]
                 ?? "http://localhost:3000";

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuxtPolicy", policy =>
    {
        policy.WithOrigins(nuxtOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseCors("NuxtPolicy");
app.MapControllers();
app.Run();
