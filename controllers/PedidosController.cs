using HamburguerApi.Data;
using HamburguerApi.Dtos;
using HamburguerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HamburguerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController(AppDbContext db) : ControllerBase
{
    /// <summary>Cria um pedido com itens, remoções de ingredientes e extras.</summary>
    /// <remarks>
    /// Exemplo:
    /// {
    ///   "clienteId": 1,
    ///   "itens": [
    ///     { "hamburguerId": 1, "qtde": 2, "precoUnit": 22.9, "removerIngredientesIds": [3,5] },
    ///     { "hamburguerId": 2, "qtde": 1, "precoUnit": 27.5 }
    ///   ],
    ///   "extras": [
    ///     { "tipo": "Bebida", "nome": "Coca 350ml", "qtde": 2, "precoUnit": 6.00 },
    ///     { "tipo": "Acompanhamento", "nome": "Batata M", "qtde": 1, "precoUnit": 12.00 }
    ///   ]
    /// }
    /// </remarks>
    [HttpPost]
    public async Task<ActionResult<PedidoReadDto>> Create(PedidoCreateDto dto)
    {
        if (dto.Itens is null || dto.Itens.Count == 0)
            return BadRequest("Itens obrigatórios.");

        // cria pedido + itens
        var pedido = new Pedido { ClienteId = dto.ClienteId };
        foreach (var i in dto.Itens)
        {
            if (i.Qtde <= 0) return BadRequest("Quantidade inválida.");
            if (i.PrecoUnit < 0) return BadRequest("Preço unitário inválido.");

            var item = new PedidoItem
            {
                HamburguerId = i.HamburguerId,
                Qtde = i.Qtde,
                PrecoUnit = i.PrecoUnit
            };
            pedido.Itens.Add(item);
        }

        // total inicial só dos burgers
        pedido.ValorTotal = dto.Itens.Sum(x => x.PrecoUnit * x.Qtde);

        db.Pedidos.Add(pedido);
        await db.SaveChangesAsync(); // gera Ids dos itens

        // remoções de ingredientes por item (se houver)
        foreach (var (iDto, idx) in dto.Itens.Select((x, idx) => (x, idx)))
        {
            if (iDto.RemoverIngredientesIds is { Length: > 0 })
            {
                var itemEntity = pedido.Itens[idx]; // mesmo índice da criação
                var rems = iDto.RemoverIngredientesIds.Distinct()
                    .Select(ingId => new PedidoItemRemocaoIngrediente
                    {
                        PedidoItemId = itemEntity.Id,
                        IngredienteId = ingId
                    });
                db.PedidoItemRemocoes.AddRange(rems);
            }
        }

        // extras (bebida / acompanhamento / sobremesa)
        if (dto.Extras is { Count: > 0 })
        {
            foreach (var e in dto.Extras)
            {
                if (e.Qtde <= 0) return BadRequest("Quantidade de extra inválida.");
                if (e.PrecoUnit < 0) return BadRequest("Preço de extra inválido.");
                if (!Enum.TryParse<TipoExtra>(e.Tipo, ignoreCase: true, out var tipo))
                    return BadRequest("Tipo de extra inválido. Use: Acompanhamento | Bebida | Sobremesa");

                db.PedidoExtras.Add(new PedidoExtra
                {
                    PedidoId = pedido.Id,
                    Tipo = tipo,
                    Nome = e.Nome,
                    Qtde = e.Qtde,
                    PrecoUnit = e.PrecoUnit
                });

                pedido.ValorTotal += e.PrecoUnit * e.Qtde;
            }
        }

        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = pedido.Id },
            new PedidoReadDto(pedido.Id, pedido.ClienteId, pedido.Status.ToString(), pedido.ValorTotal, pedido.CriadoEm));
    }

    /// <summary>Busca pedido por ID (retorna detalhes: itens, remoções e extras).</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<object>> GetById(int id)
    {
        var p = await db.Pedidos
            .AsNoTracking()
            .Include(x => x.Itens)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (p is null) return NotFound();

        // carrega remoções e extras
        var itemIds = p.Itens.Select(i => i.Id).ToArray();
        var removals = await db.PedidoItemRemocoes
            .AsNoTracking()
            .Where(r => itemIds.Contains(r.PedidoItemId))
            .ToListAsync();

        var extras = await db.PedidoExtras
            .AsNoTracking()
            .Where(e => e.PedidoId == id)
            .ToListAsync();

        var dto = new
        {
            p.Id,
            p.ClienteId,
            Status = p.Status.ToString(),
            p.ValorTotal,
            p.CriadoEm,
            Itens = p.Itens.Select(i => new
            {
                i.Id,
                i.HamburguerId,
                i.Qtde,
                i.PrecoUnit,
                RemoverIngredientesIds = removals
                    .Where(r => r.PedidoItemId == i.Id)
                    .Select(r => r.IngredienteId)
                    .ToArray()
            }),
            Extras = extras.Select(e => new
            {
                e.Id,
                Tipo = e.Tipo.ToString(),
                e.Nome,
                e.Qtde,
                e.PrecoUnit
            })
        };

        return Ok(dto);
    }

    /// <summary>Lista pedidos por status (opcional) com paginação.</summary>
    [HttpGet]
    public async Task<ActionResult<object>> List(
        [FromQuery] StatusPedido? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page <= 0 || pageSize <= 0) return BadRequest("Página e pageSize devem ser positivos.");

        var q = db.Pedidos.AsNoTracking().OrderByDescending(p => p.CriadoEm).AsQueryable();
        if (status.HasValue) q = q.Where(p => p.Status == status.Value);

        var total = await q.CountAsync();
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize)
                           .Select(p => new PedidoReadDto(p.Id, p.ClienteId, p.Status.ToString(), p.ValorTotal, p.CriadoEm))
                           .ToListAsync();

        return Ok(new { page, pageSize, total, data = items });
    }

    /// <summary>Define o status do pedido (ex.: EmPreparo, Pronto, SaiuEntrega...).</summary>
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> SetStatus(int id, [FromQuery] StatusPedido novo)
    {
        var p = await db.Pedidos.FindAsync(id);
        if (p is null) return NotFound();
        p.Status = novo;
        await db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove um pedido (e seus filhos) definitivamente.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await db.Pedidos
            .Include(x => x.Itens)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (p is null) return NotFound();

        // apaga filhos dependentes
        var itemIds = p.Itens.Select(i => i.Id).ToArray();
        var rems = db.PedidoItemRemocoes.Where(r => itemIds.Contains(r.PedidoItemId));
        var extras = db.PedidoExtras.Where(e => e.PedidoId == id);

        db.PedidoItemRemocoes.RemoveRange(rems);
        db.PedidoExtras.RemoveRange(extras);
        db.PedidoItens.RemoveRange(p.Itens);
        db.Pedidos.Remove(p);

        await db.SaveChangesAsync();
        return NoContent();
    }
}
