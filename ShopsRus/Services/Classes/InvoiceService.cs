using MongoDB.Driver;
using ShopsRus.Data;
using ShopsRus.Models;
using System.Linq;

namespace ShopsRus.Services.Classes
{
    public interface IInvoiceService
    {
        Invoice CalculateInvoice(InvoiceRequestModel requestModel);
        List<Customer> GetAllGeneratedCustomers();
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _dbContext;

        public InvoiceService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Invoice CalculateInvoice(InvoiceRequestModel requestModel)
        {
            // Müşteri ve indirim bilgilerini veritabanından alın
            var customerCollection = _dbContext.Customers;
            Customer customer = customerCollection.Find(c => c.CustomerId == requestModel.CustomerId).FirstOrDefault();
            var discountCollection = _dbContext.Discounts;
            List<Discount> discounts = discountCollection.Find(_ => true).ToList();
            decimal totalAmount = requestModel.TotalAmount;

            // İndirimleri uygula
            decimal discountedAmount = ApplyDiscounts(customer, discounts, totalAmount, requestModel.IsGrocery);

            // Fatura nesnesini oluştur ve kaydet
            var invoice = new Invoice
            {
                Customer = customer,
                TotalAmount = totalAmount,
                DiscountedAmount = discountedAmount,
                InvoiceDate = DateTime.Now
            };

            var invoiceCollection = _dbContext.Invoices;
            invoiceCollection.InsertOne(invoice);

            return invoice;
        }

        public List<Customer> GetAllGeneratedCustomers()
        {
            var customerCollection = _dbContext.Customers;
            return customerCollection.Find(_ => true).ToList();
        }


        private decimal ApplyDiscounts(Customer customer, List<Discount> discounts, decimal totalAmount, bool isGrocery)
        {
            decimal discountAmount = 0;

            // En iyi indirimi bul ve uygula
            if (!isGrocery)
            {
                decimal bestDiscount = GetBestDiscount(customer, discounts);
                discountAmount = totalAmount * bestDiscount / 100;
            }

            // Her 100 dolar için 5 dolar indirimi uygula
            discountAmount += (int)(totalAmount / 100) * 5;

            return totalAmount - discountAmount;
        }

        private decimal GetBestDiscount(Customer customer, List<Discount> discounts)
        {
            decimal maxDiscount = 0;

            foreach (var discount in discounts)
            {
                if (IsApplicableDiscount(customer, discount) && discount.Percentage > maxDiscount)
                {
                    maxDiscount = discount.Percentage;
                }
            }

            return maxDiscount;
        }

        private bool IsApplicableDiscount(Customer customer, Discount discount)
        {
            switch (discount.Type)
            {
                case DiscountType.Employee:
                    return customer.Type == CustomerType.Employee;
                case DiscountType.Affiliate:
                    return customer.Type == CustomerType.Affiliate;
                case DiscountType.Loyalty:
                    return (DateTime.Now - customer.JoinDate).TotalDays >= 365 * 2;
                default:
                    return false;
            }
        }
    }

}
