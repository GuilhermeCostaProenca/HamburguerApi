using HamburguerApi.Data;
using HamburguerApi.Dtos;
using HamburguerApi.Mappings;
using HamburguerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HamburguerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AcompanhamentosController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<object>> GetAll([FromQuery] bool? ativo)
    {
        var q = db.Acompanhamentos.AsNoTracking().AsQueryable();
        if (ativo.HasValue) q = q.Where(x => x.Ativo == ativo.Value);
        var list = await q.OrderBy(x => x.Nome).ToListAsync();
        return Ok(new { total = list.Count, data = list.Select(x => x.ToReadDto()) });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AcompanhamentoReadDto>> GetById(int id)
    {
        var e = await db.Acompanhamentos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return e is null ? NotFound() : Ok(e.ToReadDto());
    }

    [HttpPost]
    public async Task<ActionResult<AcompanhamentoReadDto>> Create(AcompanhamentoCreateDto dto)
    {
        var e = dto.ToEntity();
        db.Acompanhamentos.Add(e);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = e.Id }, e.ToReadDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, AcompanhamentoUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var e = await db.Acompanhamentos.FirstOrDefaultAsync(x => x.Id == id);
        if (e is null) return NotFound();
        e.Apply(dto);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id:int}/ativar")]
    public async Task<IActionResult> ToggleAtivo(int id, [FromQuery] bool ativo = true)
    {
        var e = await db.Acompanhamentos.FindAsync(id);
        if (e is null) return NotFound();
        e.Ativo = ativo;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await db.Acompanhamentos.FindAsync(id);
        if (e is null) return NotFound();
        db.Acompanhamentos.Remove(e);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
