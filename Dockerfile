FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY src/Domain/Domain.csproj ./Domain/
COPY src/Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY src/Api/Api.csproj ./Api/
RUN dotnet restore "Api/Api.csproj"

# Copy everything else and build
COPY src/Domain ./Domain/
COPY src/Infrastructure ./Infrastructure/
COPY src/Api ./Api/
WORKDIR /src/Api

# Build the project
RUN dotnet build "Api.csproj" -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Run migrations at runtime (not at build time)
ENTRYPOINT ["dotnet", "Api.dll"]
