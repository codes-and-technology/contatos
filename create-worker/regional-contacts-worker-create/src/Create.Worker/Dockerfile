#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/create-worker/regional-contacts-worker-create/src/Create.Worker/Create.Worker.csproj", "src/Create.Worker/"]
COPY ["/create-worker/regional-contacts-worker-create/src/CreateController/CreateController.csproj", "src/CreateController/"]
COPY ["/create-worker/regional-contacts-worker-create/src/CreateEntitys/CreateEntitys.csproj", "src/CreateEntitys/"]
COPY ["/create-worker/regional-contacts-worker-create/src/CreateInterface/CreateInterface.csproj", "src/CreateInterface/"]
COPY ["/create-worker/regional-contacts-worker-create/src/Presenters/Presenters.csproj", "src/Presenters/"]
COPY ["/create-worker/regional-contacts-worker-create/src/CreateUseCases/CreateUseCases.csproj", "src/CreateUseCases/"]
COPY ["/create-worker/regional-contacts-worker-create/src/DataBase/DataBase.csproj", "src/DataBase/"]
COPY ["/create-worker/regional-contacts-worker-create/src/Rabbit.Consumer.Create/Rabbit.Consumer.Create.csproj", "src/Rabbit.Consumer.Create/"]

RUN dotnet restore "./src/Create.Worker/Create.Worker.csproj"

COPY ["/create-worker/regional-contacts-worker-create", "/src"]

WORKDIR "/src/src/Create.Worker"
RUN dotnet build "./Create.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Create.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Create.Worker.dll"]