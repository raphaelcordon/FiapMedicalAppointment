# Stage 1: Build the backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/Api.csproj", "Api/"]
RUN dotnet restore "Api/Api.csproj"
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 2: Build the frontend
FROM node:18-alpine AS frontend-build
WORKDIR /frontend

COPY src/Frontend/package.json ./package.json
COPY src/Frontend/package-lock.json ./package-lock.json

RUN npm install

COPY src/Frontend ./

RUN npm run build

# Stage 3: Final stage to assemble the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

#COPY --from=publish /app/publish .
#COPY --from=frontend-build /frontend/dist /app/wwwroot

ENTRYPOINT ["dotnet", "Api.dll"]
