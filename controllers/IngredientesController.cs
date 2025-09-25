using HamburguerApi.Data;
using HamburguerApi.Dtos;
using HamburguerApi.Mappings;
using HamburguerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HamburguerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientesController(AppDbContext db) : ControllerBase
{
    /// <summary>Lista ingredientes com filtro e paginação.</summary>
    [HttpGet]
    public async Task<ActionResult<object>> GetAll(
        [FromQuery] bool? ativo, [FromQuery] string? search,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? sort = null)
    {
        var q = db.Ingredientes.AsQueryable();
        if (ativo.HasValue) q = q.Where(i => i.Ativo == ativo.Value);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(i => i.Nome.Contains(search));

        q = sort?.ToLower() switch
        {
            "custo:asc"  => q.OrderBy(i => i.Custo),
            "custo:desc" => q.OrderByDescending(i => i.Custo),
            _            => q.OrderBy(i => i.Nome)
        };

        var total = await q.CountAsync();
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return Ok(new { page, pageSize, total, data = items.Select(x => x.ToReadDto()) });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IngredienteReadDto>> GetById(int id)
    {
        var e = await db.Ingredientes.FindAsync(id);
        return e is null ? NotFound() : Ok(e.ToReadDto());
    }

    [HttpPost]
    public async Task<ActionResult<IngredienteReadDto>> Create(IngredienteCreateDto input)
    {
        var e = input.ToEntity();
        db.Ingredientes.Add(e);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = e.Id }, e.ToReadDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, IngredienteUpdateDto input)
    {
        if (id != input.Id) return BadRequest("Id da rota difere do corpo.");
        var e = await db.Ingredientes.FindAsync(id);
        if (e is null) return NotFound();
        e.Apply(input);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id:int}/ativar")]
    public async Task<IActionResult> ToggleAtivo(int id, [FromQuery] bool ativo = true)
    {
        var e = await db.Ingredientes.FindAsync(id);
        if (e is null) return NotFound();
        e.Ativo = ativo;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await db.Ingredientes.FindAsync(id);
        if (e is null) return NotFound();
        db.Ingredientes.Remove(e);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
