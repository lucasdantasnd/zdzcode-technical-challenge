using ZDZCode.Application.DTOs;
using ZDZCode.Application.Exceptions;
using ZDZCode.Application.Interfaces;
using ZDZCode.Domain.Entities;
using ZDZCode.Domain.Interfaces.Repositories;

namespace ZDZCode.Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public ProdutoService(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<IEnumerable<ProdutoResponseDto>> GetAllAsync()
    {
        var produtos = await _produtoRepository.GetAllWithCategoriaAsync();
        return produtos.Select(p => new ProdutoResponseDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            Preco = p.Preco,
            CategoriaId = p.CategoriaId,
            CategoriaNome = p.Categoria?.Nome ?? string.Empty
        });
    }

    public async Task<ProdutoResponseDto> GetByIdAsync(int id)
    {
        var produto = await _produtoRepository.GetByIdWithCategoriaAsync(id);
        if (produto == null)
            throw new NotFoundException($"Produto com ID {id} não encontrado.");

        return new ProdutoResponseDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            CategoriaId = produto.CategoriaId,
            CategoriaNome = produto.Categoria?.Nome ?? string.Empty
        };
    }

    public async Task<ProdutoResponseDto> CreateAsync(ProdutoRequestDto dto)
    {
        if (!await _categoriaRepository.ExistsAsync(dto.CategoriaId))
            throw new NotFoundException($"Categoria com ID {dto.CategoriaId} não encontrada.");

        var produto = new Produto
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            CategoriaId = dto.CategoriaId
        };

        var created = await _produtoRepository.AddAsync(produto);
        created = await _produtoRepository.GetByIdWithCategoriaAsync(created.Id) ?? created;

        return new ProdutoResponseDto
        {
            Id = created.Id,
            Nome = created.Nome,
            Descricao = created.Descricao,
            Preco = created.Preco,
            CategoriaId = created.CategoriaId,
            CategoriaNome = created.Categoria?.Nome ?? string.Empty
        };
    }

    public async Task UpdateAsync(int id, ProdutoRequestDto dto)
    {
        var produto = await _produtoRepository.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Produto com ID {id} não encontrado.");

        if (produto.CategoriaId != dto.CategoriaId)
        {
            if (!await _categoriaRepository.ExistsAsync(dto.CategoriaId))
                throw new NotFoundException($"Categoria com ID {dto.CategoriaId} não encontrada.");
        }

        produto.Nome = dto.Nome;
        produto.Descricao = dto.Descricao;
        produto.Preco = dto.Preco;
        produto.CategoriaId = dto.CategoriaId;

        await _produtoRepository.UpdateAsync(produto);
    }

    public async Task DeleteAsync(int id)
    {
        var produto = await _produtoRepository.GetByIdAsync(id) 
            ?? throw new NotFoundException($"Produto com ID {id} não encontrado.");

        await _produtoRepository.DeleteAsync(produto);
    }
}
