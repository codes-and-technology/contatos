#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Create.Worker/Create.Worker.csproj", "src/Create.Worker/"]
COPY ["src/CreateController/CreateController.csproj", "src/CreateController/"]
COPY ["src/CreateEntitys/CreateEntitys.csproj", "src/CreateEntitys/"]
COPY ["src/CreateInterface/CreateInterface.csproj", "src/CreateInterface/"]
COPY ["src/Presenters/Presenters.csproj", "src/Presenters/"]
COPY ["src/CreateUseCases/CreateUseCases.csproj", "src/CreateUseCases/"]
COPY ["src/DataBase/DataBase.csproj", "src/DataBase/"]
COPY ["src/Rabbit.Consumer.Create/Rabbit.Consumer.Create.csproj", "src/Rabbit.Consumer.Create/"]
RUN dotnet restore "./src/Create.Worker/Create.Worker.csproj"
COPY . .
WORKDIR "/src/src/Create.Worker"
RUN dotnet build "./Create.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Create.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Create.Worker.dll"]