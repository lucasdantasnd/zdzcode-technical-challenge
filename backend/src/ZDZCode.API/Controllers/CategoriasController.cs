using Microsoft.AspNetCore.Mvc;
using ZDZCode.Application.DTOs;
using ZDZCode.Application.Interfaces;

namespace ZDZCode.API.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categorias = await _categoriaService.GetAllAsync();
        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _categoriaService.GetByIdAsync(id);
        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoriaRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _categoriaService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoriaRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _categoriaService.UpdateAsync(id, dto);
        
        var response = await _categoriaService.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoriaService.DeleteAsync(id);
        return NoContent();
    }
}
