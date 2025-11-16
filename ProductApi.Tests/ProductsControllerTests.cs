using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductApi.Api.Controllers;
using ProductApi.Api.Models;
using ProductApi.Api.Services;
using Xunit;

namespace ProductApi.Tests;

public class ProductsControllerTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly Mock<ILogger<ProductsController>> _mockLogger;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockLogger = new Mock<ILogger<ProductsController>>();
        _controller = new ProductsController(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public void GetAll_ReturnsOkResult_WithListOfProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.99m, Stock = 100 },
            new Product { Id = 2, Name = "Product2", Description = "Desc2", Price = 20.99m, Stock = 200 }
        };
        _mockRepository.Setup(repo => repo.GetAll()).Returns(products);

        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Equal(2, returnedProducts.Count());
    }

    [Fact]
    public void GetById_ReturnsOkResult_WithProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product1", Description = "Desc1", Price = 10.99m, Stock = 100 };
        _mockRepository.Setup(repo => repo.GetById(1)).Returns(product);

        // Act
        var result = _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(1, returnedProduct.Id);
        Assert.Equal("Product1", returnedProduct.Name);
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetById(999)).Returns((Product?)null);

        // Act
        var result = _controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public void Create_ReturnsCreatedAtAction_WithCreatedProduct()
    {
        // Arrange
        var newProduct = new Product { Name = "NewProduct", Description = "New Desc", Price = 15.99m, Stock = 50 };
        var createdProduct = new Product { Id = 1, Name = "NewProduct", Description = "New Desc", Price = 15.99m, Stock = 50 };
        _mockRepository.Setup(repo => repo.Create(It.IsAny<Product>())).Returns(createdProduct);

        // Act
        var result = _controller.Create(newProduct);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedProduct = Assert.IsType<Product>(createdAtActionResult.Value);
        Assert.Equal(1, returnedProduct.Id);
        Assert.Equal("NewProduct", returnedProduct.Name);
    }

    [Fact]
    public void Update_ReturnsOkResult_WithUpdatedProduct()
    {
        // Arrange
        var updateProduct = new Product { Name = "Updated", Description = "Updated Desc", Price = 25.99m, Stock = 75 };
        var updatedProduct = new Product { Id = 1, Name = "Updated", Description = "Updated Desc", Price = 25.99m, Stock = 75 };
        _mockRepository.Setup(repo => repo.Update(1, It.IsAny<Product>())).Returns(updatedProduct);

        // Act
        var result = _controller.Update(1, updateProduct);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal("Updated", returnedProduct.Name);
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var updateProduct = new Product { Name = "Updated", Description = "Updated Desc", Price = 25.99m, Stock = 75 };
        _mockRepository.Setup(repo => repo.Update(999, It.IsAny<Product>())).Returns((Product?)null);

        // Act
        var result = _controller.Update(999, updateProduct);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public void Delete_ReturnsNoContent_WhenProductIsDeleted()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.Delete(1)).Returns(true);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.Delete(999)).Returns(false);

        // Act
        var result = _controller.Delete(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
