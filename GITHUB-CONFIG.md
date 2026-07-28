# 🔧 Configuração do Repositório GitHub

## Informações do Repositório

### Nome
```
ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live
```

### Descrição (para GitHub)
```
🪙 API RESTful em ASP.NET Core para cotações de Bitcoin em USD e BRL em tempo real. 
Desenvolvida com .NET 8+, Entity Framework Core, Swagger e documentação completa.
```

### URL
```
https://github.com/RafaelBatistaDev/ASP.NET-Core-Web-API-Bitcoin-USD-BRL-Live
```

---

## Topics/Tags

Adicione estes topics no repositório para melhor descoberta:

```
✅ dotnet
✅ aspnetcore
✅ webapi
✅ bitcoin
✅ cryptocurrency
✅ rest-api
✅ entity-framework-core
✅ swagger
✅ open-source
✅ mit-license
```

---

## Configuração Recomendada

### Geral
- ✅ **Public** (visível para todos)
- ✅ **Template repository**: Não
- ✅ **Default branch**: `main`

### Issues
- ✅ **Habilitadas**: Sim
- ✅ **Templates**: Feature Request + Bug Report

### Pull Requests
- ✅ **Habilitadas**: Sim
- ✅ **Require pull request reviews**: 1
- ✅ **Require status checks to pass**: GitHub Actions

### Branch Protection (main)
- ✅ **Require pull request reviews**: True
- ✅ **Dismiss stale pull request approvals**: True
- ✅ **Require code owner reviews**: False
- ✅ **Require status checks**: True
- ✅ **Require branches to be up to date**: True

### Segurança
- ✅ **Secret scanning**: Habilitado
- ✅ **Dependabot alerts**: Habilitado
- ✅ **Code scanning with CodeQL**: Habilitado

---

## Badges para README

```markdown
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0%2B-blue)](https://dotnet.microsoft.com)
[![C#](https://img.shields.io/badge/C%23-12-green)](https://docs.microsoft.com/dotnet/csharp)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-purple)](https://aspnet.core)
```

---

## Discussões & Comunidade

- **Discussions**: Habilitadas para Q&A
- **Projects**: Para roadmap
- **Wiki**: Para documentação adicional

---

## GitHub Actions

Arquivo: `.github/workflows/dotnet.yml`

```yaml
name: .NET CI/CD

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

env:
  DOTNET_VERSION: "8.0.x"

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}
      
      - name: Restore NuGet packages
        run: dotnet restore
      
      - name: Build
        run: dotnet build --configuration Release --no-restore
      
      - name: Test
        run: dotnet test --configuration Release --no-build --verbosity detailed
      
      - name: Code coverage
        run: dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

---

## Integração com Ferramentas

### Codacy
```
Conectar para análise de qualidade de código
Importar configuração: .editorconfig
```

### Codecov
```
Tracking de cobertura de testes
Mínimo recomendado: 80%
```

### Dependabot
```
Atualizações automáticas de dependências
Daily updates
Group minor/patch updates
```

---

## Roadmap Público

Visível em: **Projects** > **Roadmap**

### Versão 1.0.0 ✅
- [x] Endpoints CRUD básicos
- [x] Documentação Swagger
- [x] Entity Framework Core
- [x] MIT License

### Versão 1.1.0 🚧
- [ ] Autenticação JWT
- [ ] Rate limiting
- [ ] Cache com Redis
- [ ] Testes de integração
- [ ] Docker Compose

### Versão 2.0.0 📋
- [ ] Webhooks para cotações
- [ ] Mais exchanges
- [ ] GraphQL API
- [ ] Mobile App
- [ ] Analytics

---

## Releases

### Template para Release Notes

```markdown
## 🎉 Release v1.0.0

### ✨ Novidades
- Endpoints CRUD para moedas
- Integração com CoinService
- Swagger/OpenAPI interativo

### 🐛 Correções
- Nenhuma (primeira release)

### 📚 Documentação
- README completo
- DEVELOPMENT guide
- API documentation

### 🙏 Agradecimentos
Obrigado a todos contribuintes!

**Download:**
- [Source code (zip)](link)
- [Source code (tar.gz)](link)
```

---

## SEO & Descoberta

### Keywords
```
ASP.NET Core Web API
Bitcoin API
REST API C#
.NET 8 API
Entity Framework Core
Cryptocurrency API
Real-time Bitcoin Quotes
Open Source .NET
```

### GitHub Pages (opcional)

Habilitar GitHub Pages em `docs/` para documentação adicional.

---

## Comunicação

### Resposta a Issues
Tempo máximo: 48 horas

**Template para resposta:**
```markdown
Obrigado por reportar! 

**Versão afetada:** [versão]
**Comportamento:** [descrição]
**Esperado:** [o que deveria acontecer]

Investigarei e volto para você.
```

### Code Review Checklist
- [ ] Código segue guidelines C#
- [ ] Testes adicionados/atualizados
- [ ] Documentação atualizada
- [ ] SEM breaking changes
- [ ] Commit message é semântica
- [ ] Cobertura de testes > 80%

---

## Estatísticas & Métricas

Monitorar regularmente:
- Stars ⭐
- Forks 🍴
- Issues abertas
- PRs em review
- Contributors 👥
- Cobertura de código
- Build status

---

**Última atualização:** 4 de abril de 2026
