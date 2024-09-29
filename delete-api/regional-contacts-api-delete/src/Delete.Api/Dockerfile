#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["/delete-api/regional-contacts-api-delete/src/Delete.Api/Delete.Api.csproj", "src/Delete.Api/"]
COPY ["/delete-api/regional-contacts-api-delete/src/DeleteController/DeleteController.csproj", "src/DeleteController/"]
COPY ["/delete-api/regional-contacts-api-delete/src/DeleteEntitys/DeleteEntitys.csproj", "src/DeleteEntitys/"]
COPY ["/delete-api/regional-contacts-api-delete/src/DeleteInterface/DeleteInterface.csproj", "src/DeleteInterface/"]
COPY ["/delete-api/regional-contacts-api-delete/src/DeleteUseCases/DeleteUseCases.csproj", "src/DeleteUseCases/"]
COPY ["/delete-api/regional-contacts-api-delete/src/External.Interfaces/External.Interfaces.csproj", "src/External.Interfaces/"]
COPY ["/delete-api/regional-contacts-api-delete/src/ExternalInterfaceGateway/ExternalInterfaceGateway.csproj", "src/ExternalInterfaceGateway/"]
COPY ["/delete-api/regional-contacts-api-delete/src/Presenters/Presenters.csproj", "src/Presenters/"]
COPY ["/delete-api/regional-contacts-api-delete/src/QueueGateway/QueueGateway.csproj", "src/QueueGateway/"]
COPY ["/delete-api/regional-contacts-api-delete/src/Rabbit.Producer.Delete/Rabbit.Producer.Delete.csproj", "src/Rabbit.Producer.Delete/"]

RUN dotnet restore "./src/Delete.Api/Delete.Api.csproj"

COPY ["/delete-api/regional-contacts-api-delete", "/src"]

WORKDIR "/src/src/Delete.Api"
RUN dotnet build "Delete.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Delete.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Delete.Api.dll"]