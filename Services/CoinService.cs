using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace API.Services;

/// <summary>
/// Serviço de integração com a CoinGecko API para cotações em tempo real
/// </summary>
public class CoinService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CoinService> _logger;

    private static readonly Dictionary<string, string> SimboloParaId = new(StringComparer.OrdinalIgnoreCase)
    {
        { "btc", "bitcoin" },
        { "eth", "ethereum" },
        { "sol", "solana" },
        { "xrp", "ripple" },
        { "ada", "cardano" },
        { "doge", "dogecoin" },
        { "dot", "polkadot" },
        { "matic", "polygon" },
        { "link", "chainlink" },
        { "usdt", "tether" },
    };

    private const string CoinGeckoBaseUrl = "https://api.coingecko.com/api/v3/simple/price";

    public CoinService(HttpClient httpClient, ILogger<CoinService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "RecifeCryptoAPI/2.0");
    }

    /// <summary>
    /// Obtém os preços de uma criptomoeda em USD e BRL
    /// </summary>
    /// <param name="simbolo">Símbolo da criptomoeda (ex: btc, eth, sol)</param>
    /// <returns>Dicionário com preços ou null se não encontrado</returns>
    public async Task<Dictionary<string, decimal>?> GetPricesAsync(string simbolo)
    {
        var cryptoId = NormalizarSimbolo(simbolo);

        if (string.IsNullOrEmpty(cryptoId))
        {
            _logger.LogWarning("Símbolo inválido ou não suportado: {Simbolo}", simbolo);
            return null;
        }

        try
        {
            _logger.LogInformation("Consultando preço para {CryptoId} (símbolo: {Simbolo})", cryptoId, simbolo);

            var url = $"{CoinGeckoBaseUrl}?ids={cryptoId.ToLowerInvariant()}&vs_currencies=usd,brl";

            var response = await _httpClient.GetFromJsonAsync<Dictionary<string, CoinGeckoPrice>>(url);

            if (response is null || !response.TryGetValue(cryptoId.ToLowerInvariant(), out var preco))
            {
                _logger.LogWarning("Criptomoeda {CryptoId} não encontrada na CoinGecko", cryptoId);
                return null;
            }

            var resultado = new Dictionary<string, decimal>
            {
                ["usd"] = preco.Usd,
                ["brl"] = preco.Brl
            };

            _logger.LogInformation(
                "Preço obtido com sucesso para {CryptoId}: USD={Usd}, BRL={Brl}",
                cryptoId, preco.Usd, preco.Brl);

            return resultado;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de rede ao consultar CoinGecko para {CryptoId}", cryptoId);
            return null;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout ao consultar CoinGecko para {CryptoId}", cryptoId);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao consultar CoinGecko para {CryptoId}", cryptoId);
            return null;
        }
    }

    /// <summary>
    /// Converte símbolo para ID usado na CoinGecko
    /// </summary>
    private static string NormalizarSimbolo(string simbolo)
    {
        if (string.IsNullOrWhiteSpace(simbolo))
            return string.Empty;

        return SimboloParaId.TryGetValue(simbolo.Trim(), out var id)
            ? id
            : simbolo.Trim().ToLowerInvariant();
    }

    /// <summary>
    /// Modelo interno para desserialização da resposta da CoinGecko
    /// </summary>
    private sealed class CoinGeckoPrice
    {
        [JsonPropertyName("usd")]
        public decimal Usd { get; set; }

        [JsonPropertyName("brl")]
        public decimal Brl { get; set; }
    }
}
