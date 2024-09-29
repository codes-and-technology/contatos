#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/delete-worker/regional-contacts-worker-delete/src/Delete.Worker/Delete.Worker.csproj", "src/Delete.Worker/"]
COPY ["/delete-worker/regional-contacts-worker-delete/src/DeleteController/DeleteController.csproj", "src/DeleteController/"]
COPY ["/delete-worker/regional-contacts-worker-delete/src/DeleteEntitys/DeleteEntitys.csproj", "src/DeleteEntitys/"]
COPY ["/delete-worker/regional-contacts-worker-delete/src/DeleteInterface/DeleteInterface.csproj", "src/DeleteInterface/"]
COPY ["/delete-worker/regional-contacts-worker-delete/src/Presenters/Presenters.csproj", "src/Presenters/"]
COPY ["/delete-worker/regional-contacts-worker-delete/src/DeleteUseCases/DeleteUseCases.csproj", "src/DeleteUseCases/"]
COPY ["/delete-worker/regional-contacts-worker-delete/src/DataBase/DataBase.csproj", "src/DataBase/"]
COPY ["/delete-worker/regional-contacts-worker-delete/src/Rabbit.Consumer.Delete/Rabbit.Consumer.Delete.csproj", "src/Rabbit.Consumer.Delete/"]
RUN dotnet restore "./src/Delete.Worker/Delete.Worker.csproj"
COPY ["/delete-worker/regional-contacts-worker-delete", "/src"]
WORKDIR "/src/src/Delete.Worker"
RUN dotnet build "./Delete.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Delete.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Delete.Worker.dll"]