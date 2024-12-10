FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app
USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Service.Analise/ .
RUN dotnet restore "Service.Analise.csproj"  # Caminho corrigido
WORKDIR /src
RUN dotnet build "Service.Analise.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Service.Analise.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service.Analise.dll"]