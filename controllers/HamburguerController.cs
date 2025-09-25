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
        [FromQuery] bool? ativo,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sort = null)
    {
        if (page <= 0 || pageSize <= 0) return BadRequest("page e pageSize devem ser positivos.");
        if (pageSize > 200) pageSize = 200; // hard cap saudável

        var q = db.Hamburgueres.AsNoTracking().AsQueryable();

        if (ativo.HasValue)
            q = q.Where(h => h.Ativo == ativo.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = $"%{search.Trim()}%";
            q = q.Where(h =>
                EF.Functions.Like(h.Nome, s) ||
                (h.Descricao != null && EF.Functions.Like(h.Descricao, s)));
        }

        q = sort?.ToLower() switch
        {
            "preco:desc" => q.OrderByDescending(h => h.Preco),
            "preco:asc"  => q.OrderBy(h => h.Preco),
            "nome:desc"  => q.OrderByDescending(h => h.Nome),
            _            => q.OrderBy(h => h.Nome)
        };

        var total = await q.CountAsync();
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return Ok(new
        {
            page,
            pageSize,
            total,
            data = items.Select(i => i.ToReadDto())
        });
    }

    // GET api/hamburguer/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<HamburguerReadDto>> GetById(int id)
    {
        var e = await db.Hamburgueres.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return e is null ? NotFound() : Ok(e.ToReadDto());
    }

    // POST api/hamburguer
    [HttpPost]
    public async Task<ActionResult<HamburguerReadDto>> Create(HamburguerCreateDto input)
    {
        var entity = input.ToEntity();
        db.Hamburgueres.Add(entity);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToReadDto());
    }

    // PUT api/hamburguer/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, HamburguerUpdateDto input)
    {
        if (id != input.Id) return BadRequest("Id da rota difere do corpo.");

        var entity = await db.Hamburgueres.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null) return NotFound();

        entity.Apply(input);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // PATCH api/hamburguer/5/ativar?ativo=true
    [HttpPatch("{id:int}/ativar")]
    public async Task<IActionResult> ToggleAtivo(int id, [FromQuery] bool ativo = true)
    {
        var e = await db.Hamburgueres.FindAsync(id);
        if (e is null) return NotFound();
        e.Ativo = ativo;
        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/hamburguer/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await db.Hamburgueres.FindAsync(id);
        if (e is null) return NotFound();

        // opcional: apagar composições relacionadas
        var rels = db.HamburguerIngredientes.Where(x => x.HamburguerId == id);
        db.HamburguerIngredientes.RemoveRange(rels);

        db.Hamburgueres.Remove(e);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ====== COMPOSIÇÃO (ingredientes do hambúrguer) ======

    // GET api/hamburguer/5/ingredientes
    [HttpGet("{id:int}/ingredientes")]
    public async Task<ActionResult<object>> GetIngredientes(int id)
    {
        var exists = await db.Hamburgueres.AsNoTracking().AnyAsync(x => x.Id == id);
        if (!exists) return NotFound();

        var itens = await db.HamburguerIngredientes
            .AsNoTracking()
            .Where(x => x.HamburguerId == id)
            .Join(db.Ingredientes.AsNoTracking(),
                  x => x.IngredienteId,
                  i => i.Id,
                  (x, i) => new { i.Id, i.Nome, x.Quantidade })
            .OrderBy(x => x.Nome)
            .ToListAsync();

        return Ok(itens);
    }

    // POST api/hamburguer/5/ingredientes  (define composição completa)
    // body: [{ "ingredienteId": 1, "quantidade": 1.0 }, ...]
    public record ComposicaoItemDto(int IngredienteId, decimal Quantidade);

    [HttpPost("{id:int}/ingredientes")]
    public async Task<IActionResult> SetIngredientes(int id, List<ComposicaoItemDto> itens)
    {
        var burger = await db.Hamburgueres.FindAsync(id);
        if (burger is null) return NotFound();

        // apaga composição atual
        var existentes = db.HamburguerIngredientes.Where(x => x.HamburguerId == id);
        db.HamburguerIngredientes.RemoveRange(existentes);

        if (itens is { Count: > 0 })
        {
            var novos = itens
                .Where(x => x.Quantidade > 0)
                .Select(x => new HamburguerIngrediente
                {
                    HamburguerId = id,
                    IngredienteId = x.IngredienteId,
                    Quantidade = x.Quantidade
                });
            await db.HamburguerIngredientes.AddRangeAsync(novos);
        }

        await db.SaveChangesAsync();
        return NoContent();
    }
}
