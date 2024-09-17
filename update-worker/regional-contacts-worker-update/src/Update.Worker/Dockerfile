#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Update.Worker/Update.Worker.csproj", "src/Update.Worker/"]
COPY ["src/UpdateController/UpdateController.csproj", "src/UpdateController/"]
COPY ["src/UpdateEntitys/UpdateEntitys.csproj", "src/UpdateEntitys/"]
COPY ["src/UpdateInterface/UpdateInterface.csproj", "src/UpdateInterface/"]
COPY ["src/Presenters/Presenters.csproj", "src/Presenters/"]
COPY ["src/UpdateUseCases/UpdateUseCases.csproj", "src/UpdateUseCases/"]
COPY ["src/DataBase/DataBase.csproj", "src/DataBase/"]
COPY ["src/Rabbit.Consumer.Update/Rabbit.Consumer.Update.csproj", "src/Rabbit.Consumer.Update/"]
RUN dotnet restore "./src/Update.Worker/Update.Worker.csproj"
COPY . .
WORKDIR "/src/src/Update.Worker"
RUN dotnet build "./Update.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Update.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Update.Worker.dll"]