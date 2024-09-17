#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Delete.Worker/Delete.Worker.csproj", "src/Delete.Worker/"]
COPY ["src/DeleteController/DeleteController.csproj", "src/DeleteController/"]
COPY ["src/DeleteEntitys/DeleteEntitys.csproj", "src/DeleteEntitys/"]
COPY ["src/DeleteInterface/DeleteInterface.csproj", "src/DeleteInterface/"]
COPY ["src/Presenters/Presenters.csproj", "src/Presenters/"]
COPY ["src/DeleteUseCases/DeleteUseCases.csproj", "src/DeleteUseCases/"]
COPY ["src/DataBase/DataBase.csproj", "src/DataBase/"]
COPY ["src/Rabbit.Consumer.Delete/Rabbit.Consumer.Delete.csproj", "src/Rabbit.Consumer.Delete/"]
RUN dotnet restore "./src/Delete.Worker/Delete.Worker.csproj"
COPY . .
WORKDIR "/src/src/Delete.Worker"
RUN dotnet build "./Delete.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Delete.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Delete.Worker.dll"]