using ZDZCode.Application.Interfaces;
using ZDZCode.Application.Services;
using ZDZCode.Domain.Interfaces.Repositories;
using ZDZCode.Infrastructure.Repositories;

namespace ZDZCode.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            #region repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            #endregion

            #region services
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            #endregion

            return services;
        }
    }
}
