
# 🪙 Bitcoin & Crypto Price API

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-blue)](https://dotnet.microsoft.com)
[![C#](https://img.shields.io/badge/C%23-13-green)](https://docs.microsoft.com/dotnet/csharp)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-purple)](https://aspnet.core)
[![CI/CD](https://github.com/RafaelBatistaDev/ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live/actions/workflows/dotnet.yml/badge.svg)](https://github.com/RafaelBatistaDev/ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live/actions)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)](https://docker.com)

API RESTful profissional em ASP.NET Core 10 para cotações de criptomoedas (Bitcoin, Ethereum, Solana e mais) em USD e BRL em **tempo real** via CoinGecko.

---

## ✨ Funcionalidades

✅ **Cotações em tempo real** — Bitcoin, Ethereum, Solana, Ripple, Cardano e +10 criptos  
✅ **USD + BRL** — Preços em dólar e real simultaneamente  
✅ **CRUD completo** — Gerencie suas criptomoedas no banco local  
✅ **Documentação interativa** — Scalar API UI (/scalar/v1)  
✅ **Health checks** — Monitoramento (/health)  
✅ **Logging estruturado** — Serilog com console e arquivo  
✅ **Retry automático** — Polly para resiliência em falhas de rede  
✅ **Docker** — Multi-stage build pronto para produção  
✅ **CI/CD** — GitHub Actions com build, teste e análise de segurança  

---

## 🚀 Quick Start

### Pré-requisitos

- [.NET SDK 10.0](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com)

### 1. Clone e execute

```bash
git clone https://github.com/RafaelBatistaDev/ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live.git
cd ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live
dotnet run --launch-profile https
```

Acesse: **https://localhost:7004/scalar/v1** 🎉

### 2. Com Docker

```bash
docker build -t bitcoin-price-api .
docker run -p 8080:8080 bitcoin-price-api
```

Acesse: **http://localhost:8080/scalar/v1**

---

## 📡 Endpoints

### Cotações em Tempo Real

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/moedas/live/{simbolo}` | Cotação ao vivo (USD + BRL) |
| `GET` | `/api/moedas/live/{simbolo}/save` | Cotação ao vivo + salva no banco |

**Símbolos suportados:** `btc`, `eth`, `sol`, `xrp`, `ada`, `doge`, `dot`, `matic`, `link`, `usdt`

**Exemplo:**
```bash
curl https://localhost:7004/api/moedas/live/btc
```

**Resposta:**
```json
{
  "simbolo": "BTC",
  "precoUsd": 67523.45,
  "precoBrl": 345678.90,
  "fonte": "CoinGecko",
  "dataConsulta": "2026-07-28T12:00:00Z"
}
```

### CRUD - Banco Local

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/moedas` | Lista todas as moedas |
| `GET` | `/api/moedas/{id}` | Obtém moeda por ID |
| `POST` | `/api/moedas` | Cadastra nova moeda |
| `PUT` | `/api/moedas/{id}` | Atualiza moeda |
| `DELETE` | `/api/moedas/{id}` | Remove moeda |

### Utilitários

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/health` | Health check da API |
| `GET` | `/scalar/v1` | Documentação interativa |
| `GET` | `/` | Redireciona para documentação |

---

## 🏗️ Estrutura do Projeto

```
├── Controllers/
│   └── MoedasController.cs       # Endpoints da API
├── Data/
│   └── AppDbContext.cs            # Contexto do EF Core
├── Middleware/
│   └── GlobalExceptionHandler.cs  # Tratamento global de erros
├── Migrations/
│   └── (migrações do banco)
├── Models/
│   └── Moeda.cs                   # Modelo de criptomoeda
├── Services/
│   └── CoinService.cs             # Integração CoinGecko
├── Properties/
│   └── launchSettings.json        # Config de inicialização
├── Program.cs                     # Entry point e configuração
├── appsettings.json               # Configurações gerais
├── appsettings.Development.json   # Configurações de dev
├── Dockerfile                     # Build Docker multi-stage
└── .github/workflows/
    └── dotnet.yml                 # CI/CD pipeline
```

---

## 🧪 Testes

```bash
# Executar testes (quando implementados)
dotnet test

# Verificar cobertura
dotnet test /p:CollectCoverage=true
```

---

## 🛠️ Stack Tecnológica

| Tecnologia | Versão | Propósito |
|------------|--------|-----------|
| .NET | 10.0 | Runtime principal |
| ASP.NET Core | 10.0 | Framework web |
| Entity Framework Core | 10.0 | ORM / SQLite |
| Serilog | 9.x | Logging estruturado |
| Polly | 8.x | Resiliência / Retry |
| Scalar | 2.x | Documentação API |
| SQLite | — | Banco de dados |

---

## 🐳 Docker

```bash
# Build
docker build -t bitcoin-price-api .

# Executar
docker run -d -p 8080:8080 --name bitcoin-api bitcoin-price-api

# Ver logs
docker logs -f bitcoin-api

# Health check
curl http://localhost:8080/health
```

---

## 🤝 Contribuindo

1. **Fork** o repositório
2. **Crie um branch** (`git checkout -b feature/MinhaFeature`)
3. **Commit** (`git commit -m 'feat: adiciona MinhaFeature'`)
4. **Push** (`git push origin feature/MinhaFeature`)
5. **Abra uma Pull Request** 🚀

### Padrões
- ✅ Commits semânticos (feat:, fix:, chore:, docs:, refactor:)
- ✅ Async/await em toda a API
- ✅ C# moderno com nullable habilitado
- ✅ Logging em operações críticas

---

## 📜 Licença

MIT © [Rafael Batista](https://github.com/RafaelBatistaDev)

---

## 📞 Contato

🐛 [Issues](https://github.com/RafaelBatistaDev/ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live/issues)  
💬 [Discussions](https://github.com/RafaelBatistaDev/ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live/discussions)  
🔗 [LinkedIn](https://linkedin.com/in/rafaelbatistadev)
