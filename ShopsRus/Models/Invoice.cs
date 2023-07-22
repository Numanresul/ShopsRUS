using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ShopsRus.Models
{
    public class Invoice
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string InvoiceId { get; set; }
        public Customer Customer { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
