# Dockerfile multi-stage para .NET 8 - build/publish da solution notifications-api.sln
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todo o contexto (ajuste para otimizar cache se desejar)
COPY . .

# Restore e build da solution (ajuste o caminho se sua .sln estiver em outro lugar)
RUN dotnet restore "src/notifications-api.sln"
RUN dotnet build "src/notifications-api.sln" -c Release -o /app/build

# Publica o projeto web (ajuste o caminho do csproj se necessário)
RUN dotnet publish "src/Service/Notifications/Notifications.API/Notifications.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Healthcheck opcional (mantém metadata; não falha no build se curl não existir)
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
  CMD curl -f http://localhost/health || exit 1

ENTRYPOINT ["dotnet", "Notifications.API.dll"]
