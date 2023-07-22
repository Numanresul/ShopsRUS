using MongoDB.Driver;
using ShopsRus.Data;
using ShopsRus.Models;
using ShopsRus.Services.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopsRusNUnitTest.ShopsRUSTests
{
    [TestFixture]
    public class InvoiceServiceTests
    {
        private IInvoiceService _invoiceService;
        private IMongoCollection<Customer> _customerCollection;
        private IMongoCollection<Discount> _discountCollection;
        private IMongoCollection<Invoice> _invoiceCollection;

        // Test için InMemory Mongo veritabanını hazırlayın
        private string _connectionString = "mongodb+srv://TestMongo:TestMongo2023@cluster0.efdfu8s.mongodb.net/?retryWrites=true&w=majority";
        private string _databaseName = "ShopsRUsTest";


        [SetUp]
        public void Setup()
        {
            // Test için InMemory Mongo veritabanını hazırlayın
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);

            // Test için Customer ve Discount koleksiyonlarını oluşturma
            _customerCollection = database.GetCollection<Customer>("Customers");
            //_discountCollection = database.GetCollection<Discount>("Discounts");
            _invoiceCollection = database.GetCollection<Invoice>("Invoices");

            // InvoiceService'i test için hazırlayın
            _invoiceService = new InvoiceService(new AppDbContext(_connectionString, _databaseName));
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()// tüm testler için geçerli bir discount veri listesi ekleme
        {
            // Test için InMemory Mongo veritabanını hazırlayın
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);

            // Test için Discount koleksiyonlarını oluşturma
            _discountCollection = database.GetCollection<Discount>("Discounts");

            var discounts = new List<Discount>
            {
                new Discount { Type = DiscountType.Employee, Percentage = 30 },
                new Discount { Type = DiscountType.Affiliate, Percentage = 10 },
                new Discount { Type = DiscountType.Loyalty, Percentage = 5 },
                new Discount { Type = DiscountType.AmountBased, Percentage = 0 }
            };
            Console.WriteLine("Test Discount Object Created.");
            _discountCollection.InsertManyAsync(discounts);
        }

        [TearDown]
        public void Cleanup()// her test metodu tamamlıktan sonra eklenen veriyi temizliyoruz.
        {
            // Test sonrası veritabanı temizleme
            _customerCollection.DeleteMany(_ => true);
            _invoiceCollection.DeleteMany(_ => true);
        }

        [OneTimeTearDown]           
        public void OneTimeTearDown()// Test sınıfı için genel temizlik yapılır (tüm testlerden sonra bir kez çalışır)
        {
            _discountCollection.DeleteMany(_ => true);
        }

        [Test]
        public async Task CalculateInvoiceShouldCalculateTotalAmount_WithDiscounts()
        {
            // Test müşterisi ve indirimleri ekleme
            int ExpectedDiscountedAmount = 65;
            var customer = new Customer { Name = "Melih Kul", Type = CustomerType.Employee, JoinDate = DateTime.Now.AddYears(-3) };
            await _customerCollection.InsertOneAsync(customer);
            Console.WriteLine("Test Customer Added To MongoDB");

            // Test verileri ile bir InvoiceRequestModel oluşturma
            var requestModel = new InvoiceRequestModel
            {
                CustomerId = customer.CustomerId,
                TotalAmount = 100,
                IsGrocery = false
            };
            // CalculateInvoice metodu ile faturayı hesaplama
            var invoice = _invoiceService.CalculateInvoice(requestModel);
            Console.WriteLine("CalculateInvoice Method Executed.");

            // Doğru indirimin kontrolü

            Console.WriteLine("Discounted Amount Given By CalculateInvoice: " + invoice.DiscountedAmount);
            Console.WriteLine("Expected Discounted Amount: " + ExpectedDiscountedAmount);

            Assert.AreEqual(ExpectedDiscountedAmount, invoice.DiscountedAmount);

        }

        [Test]
        public async Task CalculateInvoiceShouldCalculateTotalAmount_WithEmployeeDiscount()
        {
            int ExpectedDiscountedAmount = 65;
            var customer = new Customer { Name = "Numan Kul", Type = CustomerType.Employee, JoinDate = DateTime.Now.AddYears(-3) };
            await _customerCollection.InsertOneAsync(customer);
            Console.WriteLine("Test Customer Object Created.");

            var requestModel = new InvoiceRequestModel
            {
                CustomerId = customer.CustomerId,
                TotalAmount = 100,
                IsGrocery = false
            };

            var invoice = _invoiceService.CalculateInvoice(requestModel);

            Console.WriteLine("Discounted Amount Given By CalculateInvoice: " + invoice.DiscountedAmount);
            Console.WriteLine("Expected Discounted Amount: " + ExpectedDiscountedAmount);

            Assert.AreEqual(ExpectedDiscountedAmount, invoice.DiscountedAmount); //Her 100 dolar için 5 dolar ve Employee üyelik için  %30 indirim bekleniyor
        }

        [Test]
        public async Task CalculateInvoiceShouldCalculateTotalAmount_WithAffiliateDiscount()
        {
            int ExpectedDiscountedAmount = 85;
            var affiliate = new Customer { Name = "Emre Kul", Type = CustomerType.Affiliate, JoinDate = DateTime.Now.AddYears(-2) };
            await _customerCollection.InsertOneAsync(affiliate);
            Console.WriteLine("Test Customer Object Created.");

            var requestModel = new InvoiceRequestModel
            {
                CustomerId = affiliate.CustomerId,
                TotalAmount = 100,
                IsGrocery = false
            };

            var invoice = _invoiceService.CalculateInvoice(requestModel);

            Console.WriteLine("Discounted Amount Given By CalculateInvoice: " + invoice.DiscountedAmount);
            Console.WriteLine("Expected Discounted Amount: " + ExpectedDiscountedAmount);

            Assert.AreEqual(ExpectedDiscountedAmount, invoice.DiscountedAmount); //Affiliate için %10  2 yılı aşkın üyelik için %5(Diğer yüzdeden küçük olduğu için uygulanmayacak)  Her 100 dolar için 5 dolar indirim bekleniyor
        }

        [Test]
        public async Task CalculateInvoiceShouldCalculateTotalAmount_WithLoyaltyDiscount()
        {
            int ExpectedDiscountedAmount = 90;
            var loyalCustomer = new Customer { Name = "Aydın Kul", Type = CustomerType.Regular, JoinDate = DateTime.Now.AddYears(-3) };
            await _customerCollection.InsertOneAsync(loyalCustomer);
            Console.WriteLine("Test Customer Object Created.");

            var requestModel = new InvoiceRequestModel
            {
                CustomerId = loyalCustomer.CustomerId,
                TotalAmount = 100,
                IsGrocery = false
            };

            var invoice = _invoiceService.CalculateInvoice(requestModel);
            Console.WriteLine("Discounted Amount Given By CalculateInvoice: " + invoice.DiscountedAmount);
            Console.WriteLine("Expected Discounted Amount: " + ExpectedDiscountedAmount);

            Assert.AreEqual(ExpectedDiscountedAmount, invoice.DiscountedAmount); //Her 100 dolar için 5 dolar ve 2 Yılı aşkın üyelik için toplam %10 indirim bekleniyor
        }

        [Test]
        public async Task CalculateInvoiceShouldCalculateTotalAmount_WithGroceryDiscount()
        {
            int ExpectedDiscountedAmount = 95;
            var regularCustomer = new Customer { Name = "Recep Çevik", Type = CustomerType.Regular, JoinDate = DateTime.Now.AddYears(-1) };
            await _customerCollection.InsertOneAsync(regularCustomer);
            Console.WriteLine("Test Customer Object Created.");

            var requestModel = new InvoiceRequestModel
            {
                CustomerId = regularCustomer.CustomerId,
                TotalAmount = 95,
                IsGrocery = true
            };

            var invoice = _invoiceService.CalculateInvoice(requestModel);
            Console.WriteLine("Discounted Amount Given By CalculateInvoice: " + invoice.DiscountedAmount);
            Console.WriteLine("Expected Discounted Amount: " + ExpectedDiscountedAmount);

            Assert.AreEqual(ExpectedDiscountedAmount, invoice.DiscountedAmount); // Grocery indirimi yok, toplam tutar değişmemeli
        }

        [Test]
        public async Task CalculateInvoiceShouldCalculateTotalAmountWith_AmountBasedDiscount()
        {
            int ExpectedDiscountedAmount = 145;
            var regularCustomer = new Customer { Name = "Ali Cabbar", Type = CustomerType.Regular, JoinDate = DateTime.Now.AddYears(-1) };
            await _customerCollection.InsertOneAsync(regularCustomer);
            Console.WriteLine("Test Customer Object Created.");

            var requestModel = new InvoiceRequestModel
            {
                CustomerId = regularCustomer.CustomerId,
                TotalAmount = 150,
                IsGrocery = false
            };

            var invoice = _invoiceService.CalculateInvoice(requestModel);
            Console.WriteLine("Discounted Amount Given By CalculateInvoice: " + invoice.DiscountedAmount);
            Console.WriteLine("Expected Discounted Amount: " + ExpectedDiscountedAmount);

            Assert.AreEqual(ExpectedDiscountedAmount, invoice.DiscountedAmount); //150 için 5 dolar indirim bekleniyor
        }

    }
}
