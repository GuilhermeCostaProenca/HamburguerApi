using HamburguerApi.Data;
using HamburguerApi.Dtos;
using HamburguerApi.Mappings;
using HamburguerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HamburguerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController(AppDbContext db) : ControllerBase
{
    /// <summary>Lista clientes com busca e paginação.</summary>
    [HttpGet]
    public async Task<ActionResult<object>> GetAll(
        [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var q = db.Clientes.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(c => c.Nome.Contains(search) || (c.Email != null && c.Email.Contains(search)));

        var total = await q.CountAsync();
        var items = await q.OrderByDescending(c => c.CriadoEm)
                           .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return Ok(new { page, pageSize, total, data = items.Select(x => x.ToReadDto()) });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteReadDto>> GetById(int id)
    {
        var e = await db.Clientes.FindAsync(id);
        return e is null ? NotFound() : Ok(e.ToReadDto());
    }

    [HttpPost]
    public async Task<ActionResult<ClienteReadDto>> Create(ClienteCreateDto input)
    {
        var e = input.ToEntity();
        db.Clientes.Add(e);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = e.Id }, e.ToReadDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ClienteUpdateDto input)
    {
        if (id != input.Id) return BadRequest("Id da rota difere do corpo.");
        var e = await db.Clientes.FindAsync(id);
        if (e is null) return NotFound();
        e.Apply(input);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await db.Clientes.FindAsync(id);
        if (e is null) return NotFound();
        db.Clientes.Remove(e);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
