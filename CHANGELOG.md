# Changelog

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/),
e este projeto segue [Semantic Versioning](https://semver.org/).

---

## [2.0.0] - 2026-07-28

### ✨ Added
- Middleware global de tratamento de exceções (`GlobalExceptionHandler`)
- Logging estruturado com **Serilog** (console + arquivo rotativo)
- Política de retry com **Polly** para chamadas à CoinGecko (3 tentativas com backoff exponencial)
- Health checks (`GET /health`)
- CORS configurado globalmente
- Response compression
- Dockerfile multi-stage para produção
- `.dockerignore` para builds otimizados
- `appsettings.Development.json` com configurações de dev
- Suporte a 10+ criptomoedas (BTC, ETH, SOL, XRP, ADA, DOGE, DOT, MATIC, LINK, USDT)
- Endpoint `GET /api/moedas/live/{simbolo}/save` (cotação + persistência)
- Endpoints `GET /api/moedas/{id}`, `PUT /api/moedas/{id}`, `DELETE /api/moedas/{id}`
- Propriedades `DataCadastro` e `DataAtualizacao` no modelo Moeda
- Índice único no campo Simbolo (EF Core)
- Configuração de perfil Docker no `launchSettings.json`

### 🔧 Changed
- Atualização para **.NET 10.0** (target framework)
- CI/CD atualizado para .NET 10.x
- README.md reescrito com documentação atualizada (Scalar API, Docker, endpoints reais)
- `CoinService.cs` refatorado com `ILogger`, dicionário de símbolos, modelo de resposta tipado
- `MoedasController.cs` refatorado com logging, validações e CRUD completo
- `AppDbContext.cs` com Fluent API e índice único
- `API.csproj` com metadados e novos pacotes (Serilog, Polly)
- `.editorconfig` expandido com regras de estilo C# moderno
- `.gitignore` atualizado para incluir logs do Serilog

### 🐛 Fixed
- Tratamento de exceções vazio no `CoinService` — agora com logging específico por tipo de erro
- Timeout configurado para chamadas HTTP externas (15s)

### 🗑️ Removed
- `WeatherForecastController.cs` (template leftover)
- `WeatherForecast.cs` (modelo não utilizado)
- `Produto.cs` (modelo não relacionado ao propósito da API)
- `Produtos` DbSet do `AppDbContext`
- Migrações antigas substituídas por migration inicial limpa

### 📚 Documentation
- README.md atualizado com .NET 10, Scalar API, endpoints reais
- API.http reescrito com todos os endpoints atuais

---

## [1.0.0] - 2026-04-04

### ✨ Added
- Endpoints CRUD para gerenciamento de moedas (GET, POST, PUT, DELETE)
- Integração com CoinService para buscar cotações em tempo real
- Documentação Swagger/OpenAPI interativa
- Entity Framework Core com migrations do SQLite
- Modelos de dados: Moeda, Produto
- Controllers profissionais com validação
- Arquivo API.http para testes de endpoints
- Testes preparados para integração

### 🔧 Changed
- Configuração de startup modernizada
- appsettings.json organizado e documentado

### 🔒 Security
- SQL Injection prevenido com EF Core
- CORS configurável
- Validação de entrada nos endpoints

### 📚 Documentation
- README.md completo
- DEVELOPMENT.md com guias de setup
- CONTRIBUTING.md com padrões de PR
- GITHUB-CONFIG.md com configuração recomendada
- Comentários de XML em classes e métodos

### 🚀 Infrastructure
- GitHub Actions CI/CD configurado
- .editorconfig para padrões de código
- .gitignore otimizado para .NET
- MIT License adicionada

---

## Padrões de Versionamento

- **MAJOR**: Breaking changes (1.0.0 → 2.0.0)
- **MINOR**: Nova funcionalidade backwards-compatible (1.0.0 → 1.1.0)
- **PATCH**: Bug fixes (1.0.0 → 1.0.1)

---

**Versão Atual:** [2.0.0]  
**Data:** 28 de julho de 2026  
**Status:** Production Ready ✅
