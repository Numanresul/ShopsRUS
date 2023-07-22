using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ShopsRus.Models
{
    // Models/Customer.cs
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public CustomerType Type { get; set; }
        public DateTime JoinDate { get; set; }
    }

    public enum CustomerType
    {
        Employee,
        Affiliate,
        Regular
    }


}
