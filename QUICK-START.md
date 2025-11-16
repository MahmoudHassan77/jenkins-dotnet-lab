# Quick Start Guide

## 🚀 Run the API Locally

```bash
# Build the solution
dotnet build

# Run tests
dotnet test

# Run the API
dotnet run --project ProductApi.Api

# Access the API
# HTTP: http://localhost:5025
# Swagger: http://localhost:5025/swagger
```

## 🧪 Test the API

```bash
# Get all products
curl http://localhost:5025/api/products

# Get product by ID
curl http://localhost:5025/api/products/1

# Create a product
curl -X POST http://localhost:5025/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"New Item","description":"Test","price":99.99,"stock":10}'

# Update a product
curl -X PUT http://localhost:5025/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{"name":"Updated","description":"Modified","price":199.99,"stock":20}'

# Delete a product
curl -X DELETE http://localhost:5025/api/products/1
```

## 🐳 Run with Docker

```bash
# Build and run with Docker Compose
docker-compose up --build

# Access at http://localhost:8080
# API endpoint: http://localhost:8080/api/products

# Stop containers
docker-compose down
```

## 🔧 Jenkins Pipeline

The `Jenkinsfile` includes:
- ✅ Checkout code
- ✅ Restore dependencies
- ✅ Build solution
- ✅ Run all tests
- ✅ Publish application
- ✅ Archive artifacts
- ✅ **Docker Build & Push** (pushes to `atwa77/productapi`)

### Run from Docker Hub

After Jenkins builds and pushes the image:
```bash
# Pull the latest image
docker pull atwa77/productapi:latest

# Run the container
docker run -d -p 8080:8080 --name productapi atwa77/productapi:latest

# Test the API
curl http://localhost:8080/api/products
```

## 📊 Test Results

- **17 tests total** (8 unit + 9 integration)
- **100% passing**
- Test coverage includes all CRUD operations

## 📁 Project Structure

```
jenkins-dotnet-lab/
├── ProductApi.Api/              # Main API application
│   ├── Controllers/             # REST API controllers
│   ├── Models/                  # Data models
│   └── Services/                # Business logic & repositories
├── ProductApi.Tests/            # Test project
├── Jenkinsfile                  # CI/CD pipeline definition
├── Dockerfile                   # Container configuration
├── docker-compose.yml           # Multi-container orchestration
├── api-requests.http            # Sample API requests
└── README.md                    # Full documentation
```

## 🎯 Key Features

- ✅ RESTful CRUD operations
- ✅ Swagger/OpenAPI documentation
- ✅ Unit & integration tests
- ✅ Jenkins CI/CD ready
- ✅ Docker support
- ✅ Static in-memory data
- ✅ Logging & error handling

## 📝 Sample Data

5 pre-loaded products:
1. Laptop - $1299.99
2. Mouse - $29.99
3. Keyboard - $89.99
4. Monitor - $399.99
5. Headphones - $249.99
