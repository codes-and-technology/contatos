#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["/update-api/regional-contacts-api-update/src/Update.Api/Update.Api.csproj", "src/Update.Api/"]
COPY ["/update-api/regional-contacts-api-update/src/UpdateController/UpdateController.csproj", "src/UpdateController/"]
COPY ["/update-api/regional-contacts-api-update/src/UpdateEntitys/UpdateEntitys.csproj", "src/UpdateEntitys/"]
COPY ["/update-api/regional-contacts-api-update/src/UpdateInterface/UpdateInterface.csproj", "src/UpdateInterface/"]
COPY ["/update-api/regional-contacts-api-update/src/UpdateUseCases/UpdateUseCases.csproj", "src/UpdateUseCases/"]
COPY ["/update-api/regional-contacts-api-update/src/External.Interfaces/External.Interfaces.csproj", "src/External.Interfaces/"]
COPY ["/update-api/regional-contacts-api-update/src/ExternalInterfaceGateway/ExternalInterfaceGateway.csproj", "src/ExternalInterfaceGateway/"]
COPY ["/update-api/regional-contacts-api-update/src/Presenters/Presenters.csproj", "src/Presenters/"]
COPY ["/update-api/regional-contacts-api-update/src/QueueGateway/QueueGateway.csproj", "src/QueueGateway/"]
COPY ["/update-api/regional-contacts-api-update/src/Rabbit.Producer.Update/Rabbit.Producer.Update.csproj", "src/Rabbit.Producer.Update/"]

RUN dotnet restore "./src/Update.Api/Update.Api.csproj"

COPY ["/update-api/regional-contacts-api-update", "/src"]

WORKDIR "/src/src/Update.Api"
RUN dotnet build "Update.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Update.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Update.Api.dll"]