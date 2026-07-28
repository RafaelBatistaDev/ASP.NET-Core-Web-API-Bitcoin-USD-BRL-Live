using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using API.Services;

namespace API.Controllers;

/// <summary>
/// Controlador de criptomoedas - cotações em tempo real USD/BRL
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MoedasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly CoinService _coinService;
    private readonly ILogger<MoedasController> _logger;

    public MoedasController(
        AppDbContext context,
        CoinService coinService,
        ILogger<MoedasController> logger)
    {
        _context = context;
        _coinService = coinService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as criptomoedas cadastradas no banco local
    /// </summary>
    /// <returns>Lista de criptomoedas</returns>
    /// <response code="200">Lista de moedas retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Moeda>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Moeda>>> GetMoedas()
    {
        var moedas = await _context.Moedas
            .OrderBy(m => m.Nome)
            .ToListAsync();

        _logger.LogInformation("Listadas {Count} moedas do banco local", moedas.Count);
        return Ok(moedas);
    }

    /// <summary>
    /// Obtém uma criptomoeda específica pelo ID
    /// </summary>
    /// <param name="id">ID da criptomoeda</param>
    /// <returns>Dados da criptomoeda</returns>
    /// <response code="200">Moeda encontrada</response>
    /// <response code="404">Moeda não encontrada</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Moeda), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Moeda>> GetMoeda(int id)
    {
        var moeda = await _context.Moedas.FindAsync(id);

        if (moeda is null)
        {
            _logger.LogWarning("Moeda com ID {Id} não encontrada", id);
            return NotFound(new { Message = $"Moeda com ID {id} não encontrada." });
        }

        return Ok(moeda);
    }

    /// <summary>
    /// Obtém o preço em tempo real de uma criptomoeda via CoinGecko
    /// </summary>
    /// <param name="simbolo">Símbolo da criptomoeda (btc, eth, sol, etc)</param>
    /// <returns>Cotação em USD e BRL</returns>
    /// <response code="200">Cotação obtida com sucesso</response>
    /// <response code="404">Criptomoeda não encontrada na CoinGecko</response>
    [HttpGet("live/{simbolo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLivePrice(string simbolo)
    {
        if (string.IsNullOrWhiteSpace(simbolo))
        {
            return BadRequest(new { Message = "O símbolo da criptomoeda é obrigatório." });
        }

        var precos = await _coinService.GetPricesAsync(simbolo);

        if (precos is null)
        {
            _logger.LogWarning("Preço não encontrado para símbolo: {Simbolo}", simbolo);
            return NotFound(new
            {
                Message = $"Cotação para '{simbolo.ToUpperInvariant()}' não encontrada.",
                Sugestao = "Verifique se o símbolo está correto. Exemplos: btc, eth, sol, xrp, ada, doge, dot"
            });
        }

        return Ok(new LivePriceResponse
        {
            Simbolo = simbolo.ToUpperInvariant(),
            PrecoUsd = precos["usd"],
            PrecoBrl = precos["brl"],
            Fonte = "CoinGecko",
            DataConsulta = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Obtém o preço em tempo real e salva no banco local
    /// </summary>
    /// <param name="simbolo">Símbolo da criptomoeda</param>
    /// <returns>Cotação salva no banco</returns>
    [HttpGet("live/{simbolo}/save")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLivePriceAndSave(string simbolo)
    {
        var precos = await _coinService.GetPricesAsync(simbolo);

        if (precos is null)
            return NotFound(new { Message = $"Cotação para '{simbolo}' não encontrada." });

        var moeda = new Moeda
        {
            Nome = simbolo.ToUpperInvariant(),
            Simbolo = simbolo.ToUpperInvariant(),
            Preco = precos["usd"]
        };

        _context.Moedas.Add(moeda);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Moeda {Simbolo} salva com preço USD {Preco}", simbolo, precos["usd"]);

        return CreatedAtAction(nameof(GetMoeda), new { id = moeda.Id }, new
        {
            moeda.Id,
            moeda.Nome,
            moeda.Simbolo,
            PrecoUsd = precos["usd"],
            PrecoBrl = precos["brl"],
            Fonte = "CoinGecko",
            DataConsulta = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Cadastra uma nova criptomoeda no banco local
    /// </summary>
    /// <param name="moeda">Dados da criptomoeda</param>
    /// <returns>Criptomoeda criada</returns>
    /// <response code="201">Criptomoeda criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(Moeda), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Moeda>> PostMoeda(Moeda moeda)
    {
        if (await _context.Moedas.AnyAsync(m => m.Simbolo == moeda.Simbolo))
        {
            return Conflict(new { Message = $"Já existe uma moeda cadastrada com o símbolo '{moeda.Simbolo}'." });
        }

        _context.Moedas.Add(moeda);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Moeda criada: {Nome} ({Simbolo}) - ID: {Id}",
            moeda.Nome, moeda.Simbolo, moeda.Id);

        return CreatedAtAction(nameof(GetMoeda), new { id = moeda.Id }, moeda);
    }

    /// <summary>
    /// Atualiza os dados de uma criptomoeda
    /// </summary>
    /// <param name="id">ID da criptomoeda</param>
    /// <param name="moeda">Dados atualizados</param>
    /// <response code="204">Atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Moeda não encontrada</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutMoeda(int id, Moeda moeda)
    {
        if (id != moeda.Id)
        {
            return BadRequest(new { Message = "O ID da URL não corresponde ao ID do corpo da requisição." });
        }

        var exists = await _context.Moedas.AnyAsync(m => m.Id == id);
        if (!exists)
        {
            return NotFound(new { Message = $"Moeda com ID {id} não encontrada." });
        }

        _context.Entry(moeda).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Moeda ID {Id} atualizada com sucesso", id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Erro de concorrência ao atualizar moeda ID {Id}", id);
            return Conflict(new { Message = "A moeda foi modificada por outro usuário. Recarregue e tente novamente." });
        }

        return NoContent();
    }

    /// <summary>
    /// Remove uma criptomoeda do banco local
    /// </summary>
    /// <param name="id">ID da criptomoeda</param>
    /// <response code="204">Removida com sucesso</response>
    /// <response code="404">Moeda não encontrada</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMoeda(int id)
    {
        var moeda = await _context.Moedas.FindAsync(id);
        if (moeda is null)
        {
            return NotFound(new { Message = $"Moeda com ID {id} não encontrada." });
        }

        _context.Moedas.Remove(moeda);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Moeda ID {Id} ({Nome}) removida", id, moeda.Nome);

        return NoContent();
    }
}

/// <summary>
/// Modelo de resposta para cotações em tempo real
/// </summary>
public sealed class LivePriceResponse
{
    public string Simbolo { get; set; } = string.Empty;
    public decimal PrecoUsd { get; set; }
    public decimal PrecoBrl { get; set; }
    public string Fonte { get; set; } = string.Empty;
    public DateTime DataConsulta { get; set; }
}
