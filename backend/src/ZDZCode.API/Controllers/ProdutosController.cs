using Microsoft.AspNetCore.Mvc;
using ZDZCode.Application.DTOs;
using ZDZCode.Application.Interfaces;

namespace ZDZCode.API.Controllers;

[ApiController]
[Route("api/produtos")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var produtos = await _produtoService.GetAllAsync();
        return Ok(produtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var produto = await _produtoService.GetByIdAsync(id);
        return Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProdutoRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _produtoService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProdutoRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _produtoService.UpdateAsync(id, dto);
        
        var response = await _produtoService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _produtoService.DeleteAsync(id);
        return NoContent();
    }
}
