using ZDZCode.Application.DTOs;

namespace ZDZCode.Application.Interfaces;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoResponseDto>> GetAllAsync();
    Task<ProdutoResponseDto> GetByIdAsync(int id);
    Task<ProdutoResponseDto> CreateAsync(ProdutoRequestDto dto);
    Task UpdateAsync(int id, ProdutoRequestDto dto);
    Task DeleteAsync(int id);
}
