using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ShopsRus.Models
{
    public class Discount
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DiscountId { get; set; }
        public DiscountType Type { get; set; }
        public decimal Percentage { get; set; }
        public decimal MaxAmount { get; set; }
    }


    public enum DiscountType
    {
        Employee,
        Affiliate,
        Loyalty,
        AmountBased
    }
}
