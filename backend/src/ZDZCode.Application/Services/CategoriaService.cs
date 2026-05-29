using ZDZCode.Application.DTOs;
using ZDZCode.Application.Exceptions;
using ZDZCode.Application.Interfaces;
using ZDZCode.Domain.Entities;
using ZDZCode.Domain.Interfaces.Repositories;

namespace ZDZCode.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<IEnumerable<CategoriaResponseDto>> GetAllAsync()
    {
        var categorias = await _categoriaRepository.GetAllAsync();
        return categorias.Select(c => new CategoriaResponseDto
        {
            Id = c.Id,
            Nome = c.Nome,
            Descricao = c.Descricao
        });
    }

    public async Task<CategoriaResponseDto> GetByIdAsync(int id)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id);
        if (categoria == null)
            throw new NotFoundException($"Categoria com ID {id} não encontrada.");

        return new CategoriaResponseDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Descricao = categoria.Descricao
        };
    }

    public async Task<CategoriaResponseDto> CreateAsync(CategoriaRequestDto dto)
    {
        var categoria = new Categoria
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao
        };

        var created = await _categoriaRepository.AddAsync(categoria);

        return new CategoriaResponseDto
        {
            Id = created.Id,
            Nome = created.Nome,
            Descricao = created.Descricao
        };
    }

    public async Task UpdateAsync(int id, CategoriaRequestDto dto)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id);
        if (categoria == null)
            throw new NotFoundException($"Categoria com ID {id} não encontrada.");

        categoria.Nome = dto.Nome;
        categoria.Descricao = dto.Descricao;

        await _categoriaRepository.UpdateAsync(categoria);
    }

    public async Task DeleteAsync(int id)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id);
        if (categoria == null)
            throw new NotFoundException($"Categoria com ID {id} não encontrada.");

        bool hasProdutos = await _categoriaRepository.HasProdutosAsync(id);
        if (hasProdutos)
            throw new ConflictException("Não é possível excluir a categoria pois existem produtos vinculados a ela.");

        await _categoriaRepository.DeleteAsync(categoria);
    }
}
