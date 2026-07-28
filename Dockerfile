# ────────────────────────────────────────────
#  STAGE 1: BUILD
# ────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia arquivos de projeto e restaura (cache layer)
COPY *.csproj .
RUN dotnet restore

# Copia o resto e compila
COPY . .
RUN dotnet publish -c Release -o /app --no-restore

# ────────────────────────────────────────────
#  STAGE 2: RUNTIME
# ────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Cria usuário não-root para segurança
RUN addgroup --system --gid 1000 appgroup \
    && adduser --system --uid 1000 --ingroup appgroup appuser

COPY --from=build /app .

# Health check
HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1

USER appuser
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "API.dll"]
