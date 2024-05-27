# Stage 1: Build the frontend
FROM node:18 AS frontend-build
WORKDIR /app
COPY ./src/Frontend/package*.json ./
RUN npm install
COPY ./src/Frontend .
RUN npm run build

# Stage 2: Publish the backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src
COPY ./src .
RUN dotnet restore "Api/Api.csproj"
RUN dotnet publish "Api/Api.csproj" -c Release -o /app/publish

# Stage 3: Run migrations
FROM publish AS migrations
WORKDIR /src
RUN dotnet ef database update --project Infrastructure

# Stage 4: Final stage to assemble the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=frontend-build /app/dist /frontend_build

# Set environment variables
ARG ASPNETCORE_ENVIRONMENT
ARG JWT_KEY
ARG SQLSERVER_CONNECTIONSTRING
ARG EMAIL_SERVICE

ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
ENV JWT_KEY=${JWT_KEY}
ENV SQLSERVER_CONNECTIONSTRING=${SQLSERVER_CONNECTIONSTRING}
ENV EMAIL_SERVICE=${EMAIL_SERVICE}

ENTRYPOINT ["dotnet", "Api.dll"]
