using ZDZCode.Domain.Entities.Common;

namespace ZDZCode.Domain.Entities;

public class Produto : EntityBase
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }

    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
}
