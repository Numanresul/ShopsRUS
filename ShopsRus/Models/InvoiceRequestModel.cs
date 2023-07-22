namespace ShopsRus.Models
{
    public class InvoiceRequestModel
    {
        public string CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsGrocery { get; set; }
    }

}
