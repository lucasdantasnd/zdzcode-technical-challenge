using ZDZCode.Domain.Entities.Common;

namespace ZDZCode.Domain.Entities;

public class Categoria : EntityBase
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public ICollection<Produto> Produtos { get; set; } = [];
}
