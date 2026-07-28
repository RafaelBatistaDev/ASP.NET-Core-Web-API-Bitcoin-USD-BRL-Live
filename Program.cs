using Microsoft.EntityFrameworkCore;
using Serilog;
using Scalar.AspNetCore;
using API.Data;
using API.Middleware;
using API.Services;

// ──────────────────────────────────────────────────
//  CONFIGURAÇÃO DO SERILOG (logging estruturado)
// ──────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/api-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "BitcoinPriceAPI")
    .CreateLogger();

try
{
    Log.Information("Inicializando a aplicação...");

    var builder = WebApplication.CreateBuilder(args);

    // ─── Serilog como logging provider ───
    builder.Host.UseSerilog();

    // ──────────────────────────────────────
    //  SERVIÇOS
    // ──────────────────────────────────────

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    // SQLite com Entity Framework Core
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? "Data Source=moedas.db"));

    // HttpClient com Polly retry policy para CoinService
    builder.Services.AddHttpClient<CoinService>(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(15);
    })
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)),
            onRetry: (outcome, timespan, retryAttempt, context) =>
            {
                Log.Warning(
                    "Tentativa {RetryAttempt} de 3 - Falha ao chamar CoinGecko. Retentando em {Timespan}ms",
                    retryAttempt, timespan.TotalMilliseconds);
            }));

    // Health checks
    builder.Services.AddHealthChecks();

    // CORS - liberado para desenvolvimento
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .WithExposedHeaders("X-Request-Id");
        });
    });

    // Response compression
    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
    });

    var app = builder.Build();

    // ──────────────────────────────────────
    //  PIPELINE (MIDDLEWARE)
    // ──────────────────────────────────────

    // Global Exception Handler (primeiro middleware)
    app.UseMiddleware<GlobalExceptionHandler>();

    app.UseResponseCompression();

    // Swagger / Scalar apenas em desenvolvimento
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("Bitcoin Price API")
                   .WithTheme(ScalarTheme.Purple)
                   .WithDarkModeToggle(true);
        });
    }

    // Health check endpoint (público)
    app.MapHealthChecks("/health");

    // Redireciona raiz para o Scalar
    app.MapGet("/", () => Results.Redirect("/scalar/v1"));

    app.UseCors("AllowAll");
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    // ──────────────────────────────────────
    //  MIGRAÇÃO AUTOMÁTICA (DEV)
    // ──────────────────────────────────────
    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }

    Log.Information("API iniciada com sucesso!");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Falha ao iniciar a aplicação");
}
finally
{
    Log.CloseAndFlush();
}
