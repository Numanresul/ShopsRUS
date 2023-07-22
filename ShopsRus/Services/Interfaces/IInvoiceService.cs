using ShopsRus.Models;

namespace ShopsRus.Services.Interfaces
{
    public interface IInvoiceService
    {
        Invoice CalculateInvoice(InvoiceRequestModel requestModel);
        List<Customer> GetAllGeneratedCustomers();
    }
}
