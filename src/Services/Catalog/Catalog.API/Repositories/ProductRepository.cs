using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext context;

    public ProductRepository(ICatalogContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await context
                 .Products
                 .Find(_ => true)
                 .ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetProduct(string id)
    {
        return await context
                 .Products
                 .Find(p => p.Id == id)
                 .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string productName)
    {
        // FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name, productName);

        return await context
                        .Products
                        .Find(p => p.Name.Contains(productName))
                        .ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        // FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

        return await context
                        .Products
                        .Find(p => p.Category == categoryName)
                        .ToListAsync();
    }
    public async Task CreateProduct(Product product)
    {
        await context
                 .Products
                 .InsertOneAsync(product);
    }

    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        DeleteResult result = await context
                                        .Products
                                        .DeleteOneAsync(filter);

        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await context
                                    .Products
                                    .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }
}

