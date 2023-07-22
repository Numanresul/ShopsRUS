using Microsoft.Extensions.Hosting;
using ShopsRus.Data;
using ShopsRus.Services.Classes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDB baðlantýsýný yapýlandýrýn ve DbContext'i ekleyin
string connectionString = "mongodb+srv://TestMongo:TestMongo2023@cluster0.efdfu8s.mongodb.net/?retryWrites=true&w=majority";
string databaseName = "ShopsRUs";
builder.Services.AddSingleton(new AppDbContext(connectionString, databaseName));

// Invoice servisini ekleyin
builder.Services.AddScoped<IInvoiceService, InvoiceService>();



var app = builder.Build();

// Seed verilerini eklemek için SeedDataGenerator sýnýfýný kullanýn
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
    var customerCollection = dbContext.Customers;
    var discountCollection = dbContext.Discounts;

    var seedDataGenerator = new SeedDataGenerator(customerCollection, discountCollection);
    seedDataGenerator.SeedCustomers();
    seedDataGenerator.SeedDiscounts();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
