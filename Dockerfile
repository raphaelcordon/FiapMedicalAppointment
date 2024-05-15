# Stage 1: Build the backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
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
FROM backend-build AS backend-publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

# Stage 2: Build the frontend
FROM node:18-alpine AS frontend-build
WORKDIR /frontend

COPY src/Frontend/package.json ./package.json
COPY src/Frontend/package-lock.json ./package-lock.json

RUN npm install

COPY src/Frontend ./

RUN npm run build

# Final stage: Combine backend and frontend
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=backend-publish /app/publish .
COPY --from=frontend-build /frontend/dist /app/wwwroot
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 80
ENTRYPOINT ["dotnet", "Api.dll"]

# Production environment: use Nginx to serve frontend
FROM nginx:1.25 AS production
COPY --from=frontend-build /frontend/dist /usr/share/nginx/html
COPY nginx/default.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]