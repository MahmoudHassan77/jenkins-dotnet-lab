using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductApi.Api.Models;
using Xunit;

namespace ProductApi.Tests;

public class ProductsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductsIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsSuccessStatusCode_WithProducts()
    {
        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.NotNull(products);
        Assert.NotEmpty(products);
    }

    [Fact]
    public async Task GetById_ExistingProduct_ReturnsProduct()
    {
        // Act
        var response = await _client.GetAsync("/api/products/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var product = await response.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(product);
        Assert.Equal(1, product.Id);
    }

    [Fact]
    public async Task GetById_NonExistingProduct_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/products/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ValidProduct_ReturnsCreatedProduct()
    {
        // Arrange
        var newProduct = new Product
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            Stock = 10
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", newProduct);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdProduct = await response.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(createdProduct);
        Assert.NotEqual(0, createdProduct.Id);
        Assert.Equal("Test Product", createdProduct.Name);
    }

    [Fact]
    public async Task Update_ExistingProduct_ReturnsUpdatedProduct()
    {
        // Arrange
        var updateProduct = new Product
        {
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 199.99m,
            Stock = 20
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/products/1", updateProduct);

        // Assert
        response.EnsureSuccessStatusCode();
        var updatedProduct = await response.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(updatedProduct);
        Assert.Equal("Updated Product", updatedProduct.Name);
    }

    [Fact]
    public async Task Update_NonExistingProduct_ReturnsNotFound()
    {
        // Arrange
        var updateProduct = new Product
        {
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 199.99m,
            Stock = 20
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/products/999", updateProduct);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingProduct_ReturnsNoContent()
    {
        // Arrange - Create a product first
        var newProduct = new Product
        {
            Name = "Product to Delete",
            Description = "Will be deleted",
            Price = 50m,
            Stock = 5
        };
        var createResponse = await _client.PostAsJsonAsync("/api/products", newProduct);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();

        // Act
        var response = await _client.DeleteAsync($"/api/products/{createdProduct!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_NonExistingProduct_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/products/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task FullCrudWorkflow_CreatesUpdatesDeletesProduct()
    {
        // 1. Create
        var newProduct = new Product
        {
            Name = "Workflow Product",
            Description = "Full CRUD test",
            Price = 100m,
            Stock = 15
        };

        var createResponse = await _client.PostAsJsonAsync("/api/products", newProduct);
        createResponse.EnsureSuccessStatusCode();
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(createdProduct);
        int productId = createdProduct.Id;

        // 2. Read
        var getResponse = await _client.GetAsync($"/api/products/{productId}");
        getResponse.EnsureSuccessStatusCode();
        var retrievedProduct = await getResponse.Content.ReadFromJsonAsync<Product>();
        Assert.Equal("Workflow Product", retrievedProduct!.Name);

        // 3. Update
        retrievedProduct.Name = "Updated Workflow Product";
        var updateResponse = await _client.PutAsJsonAsync($"/api/products/{productId}", retrievedProduct);
        updateResponse.EnsureSuccessStatusCode();
        var updatedProduct = await updateResponse.Content.ReadFromJsonAsync<Product>();
        Assert.Equal("Updated Workflow Product", updatedProduct!.Name);

        // 4. Delete
        var deleteResponse = await _client.DeleteAsync($"/api/products/{productId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // 5. Verify deletion
        var verifyResponse = await _client.GetAsync($"/api/products/{productId}");
        Assert.Equal(HttpStatusCode.NotFound, verifyResponse.StatusCode);
    }
}
