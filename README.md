# Product API - .NET CRUD Application

A simple RESTful API built with .NET 9 for managing products with CRUD operations. This project includes comprehensive unit and integration tests, and is designed for Jenkins CI/CD integration.

## Project Structure

```
jenkins-dotnet-lab/
├── ProductApi.Api/              # Main API project
│   ├── Controllers/             # API controllers
│   ├── Models/                  # Data models
│   └── Services/                # Business logic and repositories
├── ProductApi.Tests/            # Test project
│   ├── ProductsControllerTests.cs      # Unit tests
│   └── ProductsIntegrationTests.cs     # Integration tests
├── Jenkinsfile                  # Jenkins pipeline configuration
└── ProductApi.sln               # Solution file
```

## Features

- ✅ RESTful API with CRUD operations (Create, Read, Update, Delete)
- ✅ In-memory data storage with static data
- ✅ Swagger/OpenAPI documentation
- ✅ Comprehensive unit tests (8 tests)
- ✅ Integration tests (9 tests)
- ✅ Jenkins CI/CD pipeline
- ✅ Logging with structured output
- ✅ Error handling with appropriate HTTP status codes

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- (Optional) Docker for containerized deployment
- (Optional) Jenkins for CI/CD

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd jenkins-dotnet-lab
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Solution

```bash
dotnet build
```

### 4. Run Tests

```bash
dotnet test
```

### 5. Run the API

```bash
cd ProductApi.Api
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

## API Endpoints

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get a product by ID |
| POST | `/api/products` | Create a new product |
| PUT | `/api/products/{id}` | Update an existing product |
| DELETE | `/api/products/{id}` | Delete a product |

### Product Model

```json
{
  "id": 1,
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 1299.99,
  "stock": 50
}
```

## Sample Data

The API comes with 5 pre-loaded products:

1. Laptop - $1299.99
2. Mouse - $29.99
3. Keyboard - $89.99
4. Monitor - $399.99
5. Headphones - $249.99

## Testing

### Run All Tests

```bash
dotnet test
```

### Run Tests with Detailed Output

```bash
dotnet test --verbosity detailed
```

### Run Only Unit Tests

```bash
dotnet test --filter "FullyQualifiedName~ProductsControllerTests"
```

### Run Only Integration Tests

```bash
dotnet test --filter "FullyQualifiedName~ProductsIntegrationTests"
```

### Test Coverage

- **Unit Tests**: Test individual controller methods with mocked dependencies
- **Integration Tests**: Test the entire API pipeline including HTTP requests and responses

## Using the API

### Using Swagger UI

1. Run the application: `dotnet run --project ProductApi.Api`
2. Open browser: `https://localhost:5001/swagger`
3. Interact with the API through the Swagger interface

### Using cURL

#### Get All Products
```bash
curl -X GET https://localhost:5001/api/products -k
```

#### Get Product by ID
```bash
curl -X GET https://localhost:5001/api/products/1 -k
```

#### Create a Product
```bash
curl -X POST https://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"New Product","description":"Test","price":99.99,"stock":10}' \
  -k
```

#### Update a Product
```bash
curl -X PUT https://localhost:5001/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{"name":"Updated Product","description":"Updated","price":199.99,"stock":20}' \
  -k
```

#### Delete a Product
```bash
curl -X DELETE https://localhost:5001/api/products/1 -k
```

## Jenkins Integration

### Pipeline Stages

1. **Checkout**: Retrieve code from source control
2. **Restore Dependencies**: Restore NuGet packages
3. **Build**: Compile the solution
4. **Test**: Run all tests and publish results
5. **Publish**: Create deployment artifacts
6. **Archive**: Archive build artifacts
7. **Docker Build & Push**: Build Docker image and push to Docker Hub (atwa77/productapi)

### Setting Up Jenkins

1. **Install Jenkins** and required plugins:
   - Pipeline plugin
   - MSTest plugin
   - .NET SDK
   - Docker Pipeline plugin (for Docker support)

2. **Set Up Docker Hub Credentials**:
   - Navigate to: Manage Jenkins → Manage Credentials
   - Select the appropriate domain (usually "Global")
   - Click "Add Credentials"
   - Kind: Username with password
   - Username: `atwa77` (or your Docker Hub username)
   - Password: Your Docker Hub password or access token
   - ID: `docker-hub-credentials` (must match the Jenkinsfile)
   - Description: Docker Hub Credentials
   - Click "Create"

3. **Create a New Pipeline Job**:
   - New Item → Pipeline
   - Configure → Pipeline → Definition: Pipeline script from SCM
   - SCM: Git
   - Repository URL: `<your-repo-url>`
   - Script Path: `Jenkinsfile`

4. **Configure Build Triggers** (optional):
   - Poll SCM: `H/5 * * * *` (every 5 minutes)
   - Or use webhooks for immediate builds

5. **Run the Pipeline**:
   - Click "Build Now"
   - View console output and test results
   - Check Docker Hub for the pushed images: `https://hub.docker.com/r/atwa77/productapi`

### Jenkins Environment

The Jenkinsfile includes environment variables for .NET and Docker:
```groovy
// .NET Configuration
DOTNET_CLI_HOME = '/tmp/dotnet'
DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
DOTNET_NOLOGO = 'true'

// Docker Configuration
DOCKER_HUB_USERNAME = 'atwa77'
DOCKER_IMAGE_NAME = 'productapi'
DOCKER_IMAGE_TAG = "${BUILD_NUMBER}"  // Uses Jenkins build number for versioning
```

### Docker Images

After successful pipeline execution, Docker images will be available at:
- **Latest**: `atwa77/productapi:latest`
- **Tagged**: `atwa77/productapi:<build-number>`

Pull and run the image:
```bash
docker pull atwa77/productapi:latest
docker run -p 8080:8080 atwa77/productapi:latest
```

## Project Configuration

### API Configuration (`appsettings.json`)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Test Configuration

- **Framework**: xUnit
- **Mocking**: Moq 4.18.4
- **Integration Testing**: Microsoft.AspNetCore.Mvc.Testing 9.0.0

## Development

### Adding New Features

1. Create/modify models in `ProductApi.Api/Models/`
2. Update repository in `ProductApi.Api/Services/`
3. Modify controller in `ProductApi.Api/Controllers/`
4. Add tests in `ProductApi.Tests/`

### Code Quality

- Follow .NET naming conventions
- Add XML documentation comments
- Write tests for new features
- Keep controllers thin, move logic to services

## Troubleshooting

### Common Issues

1. **Port Already in Use**
   - Change port in `Properties/launchSettings.json`
   - Or kill the process using the port

2. **Certificate Issues**
   - Trust the development certificate: `dotnet dev-certs https --trust`
   - Or use `-k` flag with cURL to skip certificate validation

3. **Test Failures**
   - Ensure no other instance is running
   - Check logs in test output
   - Run tests individually to isolate issues

## Technologies Used

- **.NET 9**: Framework
- **ASP.NET Core**: Web API
- **Swashbuckle**: API documentation
- **xUnit**: Testing framework
- **Moq**: Mocking library
- **Jenkins**: CI/CD

## License

This project is for educational purposes.

## Contact

For questions or issues, please create an issue in the repository.
