using HamburguerApi.Data;
using HamburguerApi.Dtos;
using HamburguerApi.Mappings;
using HamburguerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HamburguerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HamburguerController(AppDbContext db) : ControllerBase
{
    // GET api/hamburguer?ativo=true&search=cheese&page=1&pageSize=20&sort=Preco:desc
    [HttpGet]
    public async Task<ActionResult<object>> GetAll(
        [FromQuery] bool? ativo, [FromQuery] string? search,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? sort = null)
    {
        var q = db.Hamburgueres.AsQueryable();
        if (ativo.HasValue) q = q.Where(h => h.Ativo == ativo.Value);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(h => h.Nome.Contains(search) || (h.Descricao != null && h.Descricao.Contains(search)));

        // sorting
        q = sort?.ToLower() switch
        {
            "preco:desc" => q.OrderByDescending(h => h.Preco),
            "preco:asc"  => q.OrderBy(h => h.Preco),
            _            => q.OrderBy(h => h.Nome)
        };

        var total = await q.CountAsync();
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return Ok(new
        {
            page, pageSize, total,
            data = items.Select(i => i.ToReadDto())
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<HamburguerReadDto>> GetById(int id)
    {
        var e = await db.Hamburgueres.FindAsync(id);
        return e is null ? NotFound() : Ok(e.ToReadDto());
    }

    [HttpPost]
    public async Task<ActionResult<HamburguerReadDto>> Create(HamburguerCreateDto input)
    {
        var entity = input.ToEntity();
        db.Hamburgueres.Add(entity);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToReadDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, HamburgurguerUpdateDto input)
    {
        if (id != input.Id) return BadRequest("Id da rota difere do corpo.");
        var entity = await db.Hamburgueres.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Apply(input);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id:int}/ativar")]
    public async Task<IActionResult> ToggleAtivo(int id, [FromQuery] bool ativo = true)
    {
        var e = await db.Hamburgueres.FindAsync(id);
        if (e is null) return NotFound();
        e.Ativo = ativo;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await db.Hamburgueres.FindAsync(id);
        if (e is null) return NotFound();
        db.Hamburgueres.Remove(e);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
