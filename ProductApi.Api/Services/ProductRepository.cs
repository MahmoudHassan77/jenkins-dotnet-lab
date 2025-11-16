using ProductApi.Api.Models;

namespace ProductApi.Api.Services;

public class ProductRepository : IProductRepository
{
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 1299.99m, Stock = 50 },
        new Product { Id = 2, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m, Stock = 200 },
        new Product { Id = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 89.99m, Stock = 150 },
        new Product { Id = 4, Name = "Monitor", Description = "27-inch 4K monitor", Price = 399.99m, Stock = 75 },
        new Product { Id = 5, Name = "Headphones", Description = "Noise-cancelling headphones", Price = 249.99m, Stock = 100 }
    };
    private static int _nextId = 6;

    public IEnumerable<Product> GetAll()
    {
        return _products.ToList();
    }

    public Product? GetById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public Product Create(Product product)
    {
        product.Id = _nextId++;
        _products.Add(product);
        return product;
    }

    public Product? Update(int id, Product product)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Id == id);
        if (existingProduct == null)
            return null;

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;

        return existingProduct;
    }

    public bool Delete(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return false;

        _products.Remove(product);
        return true;
    }
}
