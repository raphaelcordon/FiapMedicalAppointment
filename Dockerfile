# Stage 1: Build the backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /.
COPY ["src/Api/Api.csproj", "src/Api/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
RUN dotnet restore "src/Api/Api.csproj"
COPY . .
WORKDIR /src/Api
RUN dotnet build "Api.csproj" -c Release -o /build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /publish

# Stage 2: Build the frontend
# Installing node.js
RUN apt-get update
RUN apt-get upgrade -y
RUN apt-get install curl -y
RUN curl https://deb.nodesource.com/setup_18.x | bash - && apt-get install -y nodejs

WORKDIR /frontend
COPY src/Frontend/package.json /frontend/package.json
COPY src/Frontend/package-lock.json /frontend/package-lock.json
RUN npm install

# Copying frontend files and building application
COPY src/Frontend /frontend
RUN npm run build

# Stage 3: Final stage to assemble the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /publish .

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
