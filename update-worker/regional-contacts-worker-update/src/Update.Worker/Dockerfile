# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["/update-worker/regional-contacts-worker-update/src/Update.Worker/Update.Worker.csproj", "src/Update.Worker/"]
COPY ["/update-worker/regional-contacts-worker-update/src/UpdateController/UpdateController.csproj", "src/UpdateController/"]
COPY ["/update-worker/regional-contacts-worker-update/src/UpdateEntitys/UpdateEntitys.csproj", "src/UpdateEntitys/"]
COPY ["/update-worker/regional-contacts-worker-update/src/UpdateInterface/UpdateInterface.csproj", "src/UpdateInterface/"]
COPY ["/update-worker/regional-contacts-worker-update/src/Presenters/Presenters.csproj", "src/Presenters/"]
COPY ["/update-worker/regional-contacts-worker-update/src/UpdateUseCases/UpdateUseCases.csproj", "src/UpdateUseCases/"]
COPY ["/update-worker/regional-contacts-worker-update/src/DataBase/DataBase.csproj", "src/DataBase/"]
COPY ["/update-worker/regional-contacts-worker-update/src/Rabbit.Consumer.Update/Rabbit.Consumer.Update.csproj", "src/Rabbit.Consumer.Update/"]

# Restore dependencies
RUN dotnet restore "./src/Update.Worker/Update.Worker.csproj"

# Copy the rest of the source code
COPY ["/update-worker/regional-contacts-worker-update", "/src"]

# Build the application
WORKDIR "/src/src/Update.Worker"
RUN dotnet build "./Update.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "./Update.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage - use the ASP.NET Core runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Update.Worker.dll"]