using Microsoft.EntityFrameworkCore;
using ZDZCode.Domain.Entities;
using ZDZCode.Domain.Interfaces.Repositories;
using ZDZCode.Infrastructure.Context;

namespace ZDZCode.Infrastructure.Repositories;

public class ProdutoRepository : GenericRepository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Produto>> GetAllWithCategoriaAsync()
    {
        return await _dbSet.Include(p => p.Categoria).ToListAsync();
    }

    public async Task<Produto?> GetByIdWithCategoriaAsync(int id)
    {
        return await _dbSet.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
    }
}
