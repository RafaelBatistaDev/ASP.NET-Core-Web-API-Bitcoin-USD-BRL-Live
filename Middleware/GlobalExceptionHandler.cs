using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Middleware;

/// <summary>
/// Middleware global de tratamento de exceções
/// Captura exceções não tratadas e retorna uma resposta JSON padronizada
/// </summary>
public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 499;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exceção não tratada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.ContentType = "application/json";

        var (statusCode, title, detail) = exception switch
        {
            ArgumentNullException or ArgumentException => (
                (int)HttpStatusCode.BadRequest,
                "Parâmetro inválido",
                exception.Message
            ),
            UnauthorizedAccessException => (
                (int)HttpStatusCode.Unauthorized,
                "Acesso não autorizado",
                "Você não tem permissão para acessar este recurso."
            ),
            KeyNotFoundException => (
                (int)HttpStatusCode.NotFound,
                "Recurso não encontrado",
                exception.Message
            ),
            InvalidOperationException => (
                (int)HttpStatusCode.Conflict,
                "Operação inválida",
                exception.Message
            ),
            _ => (
                (int)HttpStatusCode.InternalServerError,
                "Erro interno do servidor",
                "Ocorreu um erro inesperado. Tente novamente mais tarde."
            )
        };

        context.Response.StatusCode = statusCode;

        var response = new ErrorResponse
        {
            StatusCode = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path,
            TraceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonContext.Default.ErrorResponse));
    }
}

/// <summary>
/// Modelo de resposta de erro padronizado
/// </summary>
public sealed class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string? Instance { get; set; }
    public string? TraceId { get; set; }
}

/// <summary>
/// JsonSerializerContext para source generation do ErrorResponse
/// </summary>
[JsonSerializable(typeof(ErrorResponse))]
internal sealed partial class JsonContext : JsonSerializerContext
{
}
