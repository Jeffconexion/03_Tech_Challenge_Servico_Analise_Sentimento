#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Service.Analise/Service.Analise.csproj", "Service.Analise/"]
RUN dotnet restore "./Service.Analise/Service.Analise.csproj"
COPY . .
WORKDIR "/src/Service.Analise"
RUN dotnet build "./Service.Analise.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Service.Analise.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./Service.Analise/feedback_sentiment_base.csv /app/feedback_sentiment_base.csv
ENTRYPOINT ["dotnet", "Service.Analise.dll"]