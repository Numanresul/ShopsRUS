// SeedDataGenerator.cs
using System;
using System.Collections.Generic;
using MongoDB.Driver;
using ShopsRus.Models;

public class SeedDataGenerator
{
    private readonly IMongoCollection<Customer> _customerCollection;
    private readonly IMongoCollection<Discount> _discountCollection;

    public SeedDataGenerator(IMongoCollection<Customer> customerCollection, IMongoCollection<Discount> discountCollection)
    {
        _customerCollection = customerCollection;
        _discountCollection = discountCollection;
    }

    public void SeedCustomers()
    {
        if (_customerCollection.CountDocuments(FilterDefinition<Customer>.Empty) == 0)
        {
            var customers = new List<Customer>
            {
                new Customer { Name = "John Doe", Type = CustomerType.Employee, JoinDate = DateTime.Now.AddYears(-3)},
                new Customer { Name = "Alice Smith", Type = CustomerType.Affiliate, JoinDate = DateTime.Now.AddYears(-2) },
                new Customer { Name = "Bob Johnson", Type = CustomerType.Regular, JoinDate = DateTime.Now.AddYears(-1) }
            };
            _customerCollection.InsertMany(customers);
        }
    }

    public void SeedDiscounts()
    {
        if (_discountCollection.CountDocuments(FilterDefinition<Discount>.Empty) == 0)
        {
            var discounts = new List<Discount>
            {
                new Discount { Type = DiscountType.Employee, Percentage = 30 },
                new Discount { Type = DiscountType.Affiliate, Percentage = 10 },
                new Discount { Type = DiscountType.Loyalty, Percentage = 5 },
                new Discount { Type = DiscountType.AmountBased, Percentage = 0, MaxAmount = 5 }
            };
            _discountCollection.InsertMany(discounts);
        }
    }
}
