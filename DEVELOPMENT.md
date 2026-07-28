# 🔨 Guia de Desenvolvimento

## Ambiente Recomendado

### IDE/Editor
- **Visual Studio 2022** (recomendado para Windows)
- **Visual Studio Code** + C# DevKit
- **JetBrains Rider** (pago)

### Extensões VS Code
```json
{
  "recommendations": [
    "ms-dotnettools.csharp",
    "ms-dotnettools.vscode-dotnet-runtime",
    "eamodio.gitlens",
    "EditorConfig.EditorConfig"
  ]
}
```

---

## Setup Inicial

### 1. Clone e Dependências
```bash
git clone https://github.com/RafaelBatistaDev/ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live.git
cd ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live
dotnet restore
```

### 2. Configurar Banco de Dados
```bash
# Criar migrações
dotnet ef migrations add InitialCreate

# Aplicar migrações
dotnet ef database update

# Ver status
dotnet ef migrations list
```

### 3. Build Local
```bash
# Debug (mais lento, melhor para dev)
dotnet build

# Release (otimizado)
dotnet build -c Release
```

### 4. Run em Watch Mode
```bash
# Recompila automaticamente ao salvar
dotnet watch run

# Especificar projeto
dotnet watch --project API.csproj run
```

### 5. Debug
```bash
# VS Code: Pressione F5
# Visual Studio: Pressione F5
# Swagger UI: https://localhost:7001/swagger
```

---

## Workflow de Feature

### Exemplo: Adicionar Novo Endpoint

```bash
# 1. Criar branch
git checkout -b feature/novo-endpoint

# 2. Criar controller/service
# → Controllers/NovoController.cs
[ApiController]
[Route("api/[controller]")]
public class NovoController : ControllerBase
{
    private readonly IMyService _service;
    
    public NovoController(IMyService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dto>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }
}

# 3. Registrar DI no Program.cs
builder.Services.AddScoped<IMyService, MyService>();

# 4. Testar localmente
dotnet run
# Acessar: https://localhost:7001/api/novo

# 5. Rodar testes
dotnet test

# 6. Commit e push
git add .
git commit -m "feat: adiciona novo endpoint"
git push origin feature/novo-endpoint

# 7. Abrir Pull Request no GitHub
```

---

## Commands Úteis

### Diagnosticar
```bash
# Informações do projeto
dotnet project-info

# Versão SDK
dotnet --version

# Listar templates disponíveis
dotnet new list
```

### Banco de Dados
```bash
# Recriar migrations
dotnet ef migrations add [MigrationName]

# Reverter última migração
dotnet ef migrations remove

# Update banco
dotnet ef database update

# Drop banco
dotnet ef database drop

# Script SQL
dotnet ef migrations script
```

### Limpeza
```bash
# Remover build artifacts
dotnet clean

# Remover NuGet cache
dotnet nuget locals all --clear
```

---

## Padrões de Código

### Naming Conventions
- **Classes**: `PascalCase` → `MoedasController`, `CoinService`
- **Methods**: `PascalCase` → `GetMoedas()`, `UpdateCotacao()`
- **Properties**: `PascalCase` → `NomeMoeda`, `PrecoUsd`
- **Private fields**: `_camelCase` → `_logger`, `_service`
- **Constants**: `UPPER_CASE` → `API_KEY`, `MAX_ITEMS`

### Async/Await Pattern
```csharp
// ✅ Bom: Async/await correto
[HttpGet("{id}")]
public async Task<ActionResult<MoedaDto>> GetMoeda(int id)
{
    var moeda = await _service.GetMoedaAsync(id);
    if (moeda == null)
        return NotFound();
    
    return Ok(moeda);
}

// ❌ Evitar: Sync over async
[HttpGet("{id}")]
public ActionResult<MoedaDto> GetMoeda(int id)
{
    var moeda = _service.GetMoedaAsync(id).Result; // NUNCA!
    return Ok(moeda);
}
```

### Dependency Injection
```csharp
// Program.cs - Registrar services
builder.Services.AddScoped<IMoedaService, MoedaService>();
builder.Services.AddScoped<ICoinService, CoinService>();

// Controller - Injetar
public class MoedasController : ControllerBase
{
    private readonly IMoedaService _service;
    
    public MoedasController(IMoedaService service)
    {
        _service = service;
    }
}
```

### Error Handling
```csharp
// ✅ Bom: Try-catch apropriado
try
{
    var result = await _coinService.GetCotacaoAsync();
    return Ok(result);
}
catch (HttpRequestException ex)
{
    _logger.LogError(ex, "Erro ao buscar cotação");
    return StatusCode(500, "Erro ao buscar cotação externa");
}
catch (Exception ex)
{
    _logger.LogError(ex, "Erro inesperado");
    return StatusCode(500, "Erro interno do servidor");
}
```

---

## Testing

### Criar Testes Unitários
```bash
dotnet new xunit -n API.Tests
dotnet add API.Tests reference API
```

### Exemplo de Teste
```csharp
[Fact]
public async Task GetMoeda_WithValidId_ReturnsOkResult()
{
    // Arrange
    var mockService = new Mock<IMoedaService>();
    mockService.Setup(s => s.GetMoedaAsync(1))
        .ReturnsAsync(new MoedaDto { Id = 1, Nome = "Bitcoin" });
    
    var controller = new MoedasController(mockService.Object);
    
    // Act
    var result = await controller.GetMoeda(1);
    
    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    assert.NotNull(okResult.Value);
}
```

### Rodar Testes
```bash
# Todos os testes
dotnet test

# Com verbose
dotnet test --verbosity detailed

# Com cobertura
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

---

## Troubleshooting

### "Port 7001 already in use"
```bash
# Listar processos
lsof -i :7001  # macOS/Linux
netstat -ano | findstr :7001  # Windows

# Matar processo
kill -9 <PID>

# Usar porta diferente
dotnet run --urls "https://localhost:5002"
```

### "Migration not found"
```bash
# Restaurar migrations
dotnet ef migrations remove --force
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### "Database connection error"
```bash
# Verificar connection string em appsettings.json
# SQL Server: Server=.;Database=DbName;Trusted_Connection=true;
# SQLite: Data Source=app.db;
```

---

## Git Workflow

### Branching Strategy
```
main (production)
  ↑
  ├── release/* (versões de release)
  └── develop (staging)
        ↑
        ├── feature/* (novas features)
        ├── bugfix/* (correções)
        └── hotfix/* (emergências)
```

### Commit Pattern
```bash
feat:      Nova funcionalidade
fix:       Correção de bug
docs:      Mudanças na documentação
refactor:  Refatoração de código
perf:      Melhoria de performance
test:      Testes
chore:     Tarefas de manutenção
```

---

## Recursos

- 📖 [ASP.NET Core Docs](https://learn.microsoft.com/aspnet/core/)
- 🔗 [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- 📘 [C# Documentation](https://learn.microsoft.com/dotnet/csharp/)
- 🧪 [xUnit Testing](https://xunit.net/)
- 🔍 [Swagger/OpenAPI](https://swagger.io/)

---

**Boa sorte desenvolvendo!** 🚀
