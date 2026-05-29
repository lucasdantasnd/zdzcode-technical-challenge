using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ZDZCode.API.Configurations;
using ZDZCode.API.Middleware;
using ZDZCode.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "ZDZCode API";
        document.Info.Version = "v1";
        document.Info.Description = "API do desafio técnico ZDZCode";
        return Task.CompletedTask;
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.ResolveDependencies(builder.Configuration);

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

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "ZDZCode API";
    options.Theme = ScalarTheme.DeepSpace;
    options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// app.UseHttpsRedirection();
app.UseCors("NuxtPolicy");
app.MapControllers();
app.Run();
