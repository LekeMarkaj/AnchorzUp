# Use the official .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["server/AnchorzUp.API/AnchorzUp.API.csproj", "server/AnchorzUp.API/"]
COPY ["server/AnchorzUp.Application/AnchorzUp.Application.csproj", "server/AnchorzUp.Application/"]
COPY ["server/AnchorzUp.Domain/AnchorzUp.Domain.csproj", "server/AnchorzUp.Domain/"]
COPY ["server/AnchorzUp.Infrastructure/AnchorzUp.Infrastructure.csproj", "server/AnchorzUp.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "server/AnchorzUp.API/AnchorzUp.API.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/server/AnchorzUp.API"
RUN dotnet build "AnchorzUp.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "AnchorzUp.API.csproj" -c Release -o /app/publish

# Build frontend with Node.js
FROM node:18-alpine AS frontend-build
WORKDIR /frontend

# Copy frontend package files
COPY frontend/package*.json ./
RUN npm ci

# Copy frontend source and build
COPY frontend/ .
RUN npm run build

# Final stage - simple .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy API
COPY --from=publish /app/publish .

# Copy built frontend
COPY --from=frontend-build /frontend/build ./wwwroot

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "AnchorzUp.API.dll"]
