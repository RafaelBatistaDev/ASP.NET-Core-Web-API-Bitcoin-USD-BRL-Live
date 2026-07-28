# 🤝 Guia de Contribuição

Obrigado por se interessar em contribuir para o **Bitcoin Price API**! 🚀

## Código de Conduta

Este projeto segue um Código de Conduta respeitoso:

- ✅ Seja respeitoso com todos
- ✅ Críticas sobre código, não sobre pessoas
- ✅ Inclusão é de todos nós
- ✅ Reporte comportamentos inadequados

---

## Como Contribuir

### 1. Abra uma Issue

Antes de começar a desenvolver:

```markdown
**Descrição do Problema:**
Descrever o bug ou feature solicitada

**Contexto:**
Como reproduzir? Qual é o comportamento esperado?

**Versão .NET:**
dotnet --version

**Sistema:**
Windows / macOS / Linux
```

### 2. Fork & Clone

```bash
# Fork no GitHub (botão "Fork")
git clone https://github.com/SEU-USUARIO/ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live.git
cd ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live

# Adicionar remoto upstream
git remote add upstream https://github.com/RafaelBatistaDev/ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live.git
```

### 3. Criar Branch

```bash
# Atualizar main
git fetch upstream
git checkout main
git reset --hard upstream/main

# Criar feature branch
git checkout -b feature/descricao-curta

# Exemplos válidos:
# - feature/add-jwt-auth
# - bugfix/null-reference-exception
# - docs/update-api-docs
# - perf/optimize-db-queries
```

### 4. Fazer Mudanças

Siga os padrões:

```csharp
// ✅ BOM: Nullable reference types habilitado
#nullable enable

public class MoedaService
{
    private readonly ILogger<MoedaService> _logger;
    private readonly AppDbContext _context;
    
    public MoedaService(ILogger<MoedaService> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task<MoedaDto?> GetMoedaAsync(int id)
    {
        var moeda = await _context.Moedas.FindAsync(id);
        
        if (moeda is null)
            _logger.LogWarning("Moeda {Id} não encontrada", id);
        
        return moeda?.ToDto();
    }
}

// ❌ EVITAR: Sem type hints, sem async, sem logging
public MoedaDto GetMoeda(int id)
{
    return _context.Moedas.Find(id).ToDto();
}
```

### 5. Testes

```bash
# Escrever testes para toda lógica nova
dotnet test

# Verificar cobertura
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

**Exemplo de teste:**
```csharp
[Fact]
public async Task GetMoedaAsync_WithInvalidId_ReturnsNull()
{
    // Arrange
    var service = new MoedaService(_mockLogger.Object, _mockContext.Object);
    
    // Act
    var result = await service.GetMoedaAsync(-1);
    
    // Assert
    Assert.Null(result);
}
```

### 6. Commit

Use **Conventional Commits**:

```bash
# Feature nova
git commit -m "feat: adiciona autenticação JWT"

# Correção de bug
git commit -m "fix: corrige null reference na API de moedas"

# Documentação
git commit -m "docs: atualiza guia de setup"

# Refatoração
git commit -m "refactor: simplifica CoinService"

# Performance
git commit -m "perf: melhora query de moedas com índices"

# Testes
git commit -m "test: adiciona testes de integração"

# Formato: <type>(<scope>): <subject>
# Tipos: feat, fix, docs, refactor, perf, test, chore, ci
# Escopo: Controllers, Services, Models, Data, etc (opcional)
# Assunto: imperativo, sem ponto final, máx 50 chars
```

### 7. Push & Pull Request

```bash
# Sincronizar com upstream
git fetch upstream
git rebase upstream/main

# Push para seu fork
git push origin feature/descricao

# Abrir PR no GitHub
# - Título claro
# - Issue vinculada (#123)
# - Descrição detalhada
# - Screenshots se UI
```

**Template PR:**
```markdown
## Descrição
Resolve #123

Adiciona suporte a autenticação JWT no endpoint de moedas.

## Mudanças
- [ ] Controllers
- [ ] Services
- [ ] Testes
- [ ] Documentação

## Tipo de Mudança
- [x] Feature / Melhoria
- [ ] Bug fix
- [ ] Breaking change
- [ ] Documentation

## Checklist
- [x] Código segue style guidelines
- [x] Autotest passou com sucesso
- [x] Novos testes adicionados
- [x] Documentação atualizada
```

---

## Padrões de Código

### C# Style

```csharp
// Ordering: using → namespace → class
using System;
using Microsoft.EntityFrameworkCore;

namespace BitcoinApi.Services;

/// <summary>
/// Serviço para gerenciar moedas
/// </summary>
public class MoedaService
{
    // 1. Constantes
    private const int MaxPageSize = 100;
    
    // 2. Static fields
    private static readonly HttpClient Client = new();
    
    // 3. Fields
    private readonly ILogger<MoedaService> _logger;
    
    // 4. Constructor
    public MoedaService(ILogger<MoedaService> logger)
    {
        _logger = logger;
    }
    
    // 5. Properties
    public int TotalMoedas { get; set; }
    
    // 6. Public methods
    public async Task<MoedaDto?> GetAsync(int id)
    {
        return await GetInternalAsync(id);
    }
    
    // 7. Private methods
    private async Task<MoedaDto?> GetInternalAsync(int id)
    {
        return null;
    }
}
```

### Async/Await
```csharp
// ✅ BOM
public async Task<IActionResult> GetMoedas()
{
    var moedas = await _service.GetAllAsync();
    return Ok(moedas);
}

// ❌ ERRADO
public IActionResult GetMoedas()
{
    var moedas = _service.GetAllAsync().Result; // Deadlock!
    return Ok(moedas);
}
```

### Error Handling
```csharp
try
{
    var result = await _externalApi.GetCotacao();
    return Ok(result);
}
catch (HttpRequestException ex)
{
    _logger.LogError(ex, "Erro ao chamar API externa");
    return StatusCode(503, "Serviço temporariamente indisponível");
}
catch (Exception ex)
{
    _logger.LogError(ex, "Erro inesperado ao buscar cotação");
    return StatusCode(500, "Erro interno do servidor");
}
```

---

## Processso de Review

1. **Automático:** GitHub Actions roda testes
2. **Código:** Maintainer revisa código
3. **Testes:** Cobertura deve ser > 80%
4. **Comment:** Feedback ou aprovação
5. **Merge:** Squash + merge para main

---

## Comunicação

- 💬 Discussões: [GitHub Discussions](https://github.com/RafaelBatistaDev/ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live/discussions)
- 🐛 Issues: [GitHub Issues](https://github.com/RafaelBatistaDev/ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live/issues)
- 📧 Email: Contactar maintainer se necessário

---

## Licença

Ao contribuir, você concorda que suas contribuições serão licenciadas sob a mesma [MIT License](LICENSE).

---

## Dúvidas?

- Leia [DEVELOPMENT.md](DEVELOPMENT.md)
- Abra uma [Discussion](https://github.com/RafaelBatistaDev/ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live/discussions)

**Muito obrigado pela contribuição!** 🙏
