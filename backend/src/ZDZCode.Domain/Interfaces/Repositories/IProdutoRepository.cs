using ZDZCode.Domain.Entities;

namespace ZDZCode.Domain.Interfaces.Repositories;

public interface IProdutoRepository : IGenericRepository<Produto>
{
    Task<IEnumerable<Produto>> GetAllWithCategoriaAsync();
    Task<Produto?> GetByIdWithCategoriaAsync(int id);
}
