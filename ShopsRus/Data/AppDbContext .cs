using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using ShopsRus.Models;

namespace ShopsRus.Data
{
    public class AppDbContext
    {
        private readonly IMongoDatabase _database;

        public AppDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("Customers");
        public IMongoCollection<Invoice> Invoices => _database.GetCollection<Invoice>("Invoices");
        public IMongoCollection<Discount> Discounts => _database.GetCollection<Discount>("Discounts");
    }
}
