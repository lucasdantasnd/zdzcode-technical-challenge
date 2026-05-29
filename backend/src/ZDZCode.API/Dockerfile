# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files for all projects
COPY ["src/ZDZCode.Domain/ZDZCode.Domain.csproj", "src/ZDZCode.Domain/"]
COPY ["src/ZDZCode.Application/ZDZCode.Application.csproj", "src/ZDZCode.Application/"]
COPY ["src/ZDZCode.Infrastructure/ZDZCode.Infrastructure.csproj", "src/ZDZCode.Infrastructure/"]
COPY ["src/ZDZCode.API/ZDZCode.API.csproj", "src/ZDZCode.API/"]

# Restore dependencies targeting the API project (which pulls transitively)
RUN dotnet restore "src/ZDZCode.API/ZDZCode.API.csproj"

# Copy the entire src folder
COPY src/ ./src/

# Publish the API project
WORKDIR "/src/src/ZDZCode.API"
RUN dotnet publish "ZDZCode.API.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose HTTP port
EXPOSE 8080

ENTRYPOINT ["dotnet", "ZDZCode.API.dll"]
