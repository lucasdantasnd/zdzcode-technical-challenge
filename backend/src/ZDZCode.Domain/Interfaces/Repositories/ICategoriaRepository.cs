using ZDZCode.Domain.Entities;

namespace ZDZCode.Domain.Interfaces.Repositories;

public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    Task<bool> HasProdutosAsync(int id);
}
