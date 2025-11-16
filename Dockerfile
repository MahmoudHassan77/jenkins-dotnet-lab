# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ProductApi.sln ./
COPY ProductApi.Api/ProductApi.Api.csproj ProductApi.Api/
COPY ProductApi.Tests/ProductApi.Tests.csproj ProductApi.Tests/

# Restore dependencies
RUN dotnet restore

# Copy remaining source code
COPY . .

# Build and test
RUN dotnet build -c Release --no-restore
RUN dotnet test -c Release --no-build --verbosity normal

# Publish the API
RUN dotnet publish ProductApi.Api/ProductApi.Api.csproj -c Release -o /app/publish --no-build

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "ProductApi.Api.dll"]
