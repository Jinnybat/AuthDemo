using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogContext : ICatalogContext
{
    private readonly IMongoClient mongoClient;

    public CatalogContext(IConfiguration configuration)
    {
        this.mongoClient = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = mongoClient.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));

        CatalogContextSeed.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; }
}
