FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copie os arquivos .csproj com caminhos absolutos
COPY ["/consulting-api/regional-contacts-consulting-api/src/Consulting.Api/Consulting.Api.csproj", "src/Consulting.Api/"]
COPY ["/consulting-api/regional-contacts-consulting-api/src/CacheGateway/CacheGateway.csproj", "src/CacheGateway/"]
COPY ["/consulting-api/regional-contacts-consulting-api/src/ConsultingController/ConsultingController.csproj", "src/ConsultingController/"]
COPY ["/consulting-api/regional-contacts-consulting-api/src/ConsultingEntitys/ConsultingEntitys.csproj", "src/ConsultingEntitys/"]
COPY ["/consulting-api/regional-contacts-consulting-api/src/ConsultingInterface/ConsultingInterface.csproj", "src/ConsultingInterface/"]
COPY ["/consulting-api/regional-contacts-consulting-api/src/ConsultingUseCases/ConsultingUseCases.csproj", "src/ConsultingUseCases/"]
COPY ["/consulting-api/regional-contacts-consulting-api/src/DataBase/DataBase.csproj", "src/DataBase/"]
COPY ["/consulting-api/regional-contacts-consulting-api/src/DBGateway/DBGateway.csproj", "src/DBGateway/"]
COPY ["/consulting-api/regional-contacts-consulting-api/src/Presenters/Presenters.csproj", "src/Presenters/"]
COPY ["/consulting-api/regional-contacts-consulting-api/src/Redis/Redis.csproj", "src/Redis/"]

# Restore as dependências
RUN dotnet restore "./src/Consulting.Api/Consulting.Api.csproj"

# Copie o restante dos arquivos
COPY ["/consulting-api/regional-contacts-consulting-api", "/src"]

# Build do projeto
WORKDIR "/src/src/Consulting.Api"
RUN dotnet build "Consulting.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Consulting.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consulting.Api.dll"]