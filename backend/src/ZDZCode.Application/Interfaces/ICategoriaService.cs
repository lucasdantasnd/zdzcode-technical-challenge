using ZDZCode.Application.DTOs;

namespace ZDZCode.Application.Interfaces;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaResponseDto>> GetAllAsync();
    Task<CategoriaResponseDto> GetByIdAsync(int id);
    Task<CategoriaResponseDto> CreateAsync(CategoriaRequestDto dto);
    Task UpdateAsync(int id, CategoriaRequestDto dto);
    Task DeleteAsync(int id);
}
