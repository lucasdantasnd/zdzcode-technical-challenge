using Microsoft.EntityFrameworkCore;
using ZDZCode.Domain.Entities;
using ZDZCode.Domain.Interfaces.Repositories;
using ZDZCode.Infrastructure.Context;

namespace ZDZCode.Infrastructure.Repositories;

public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context) { }

    public async Task<bool> HasProdutosAsync(int id)
    {
        return await _context.Produtos.AnyAsync(p => p.CategoriaId == id);
    }
}
