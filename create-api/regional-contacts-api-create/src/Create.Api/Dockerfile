#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["/create-api/regional-contacts-api-create/src/Create.Api/Create.Api.csproj", "src/Create.Api/"]
COPY ["/create-api/regional-contacts-api-create/src/CreateController/CreateController.csproj", "src/CreateController/"]
COPY ["/create-api/regional-contacts-api-create/src/CreateEntitys/CreateEntitys.csproj", "src/CreateEntitys/"]
COPY ["/create-api/regional-contacts-api-create/src/CreateInterface/CreateInterface.csproj", "src/CreateInterface/"]
COPY ["/create-api/regional-contacts-api-create/src/CreateUseCases/CreateUseCases.csproj", "src/CreateUseCases/"]
COPY ["/create-api/regional-contacts-api-create/src/External.Interfaces/External.Interfaces.csproj", "src/External.Interfaces/"]
COPY ["/create-api/regional-contacts-api-create/src/ExternalInterfaceGateway/ExternalInterfaceGateway.csproj", "src/ExternalInterfaceGateway/"]
COPY ["/create-api/regional-contacts-api-create/src/Presenters/Presenters.csproj", "src/Presenters/"]
COPY ["/create-api/regional-contacts-api-create/src/QueueGateway/QueueGateway.csproj", "src/QueueGateway/"]
COPY ["/create-api/regional-contacts-api-create/src/Rabbit.Producer.Create/Rabbit.Producer.Create.csproj", "src/Rabbit.Producer.Create/"]

RUN dotnet restore "./src/Create.Api/Create.Api.csproj"

COPY ["/create-api/regional-contacts-api-create", "/src"]

WORKDIR "/src/src/Create.Api"
RUN dotnet build "Create.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Create.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Create.Api.dll"]