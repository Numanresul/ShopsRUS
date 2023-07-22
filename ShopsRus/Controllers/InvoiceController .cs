using Microsoft.AspNetCore.Mvc;
using ShopsRus.Models;
using ShopsRus.Services.Classes;

namespace ShopsRus.Controllers
{
    [ApiController]

    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        [Route("api/CalculateInvoice")]
        public IActionResult CalculateInvoice([FromBody] InvoiceRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request model.");
            }

            try
            {
                var invoice = _invoiceService.CalculateInvoice(requestModel);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet]
        [Route("api/GetAllGeneratedCustomerList")]
        public IActionResult GetAllGeneratedCustomerList()
        {
            try
            {
                var result = _invoiceService.GetAllGeneratedCustomers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }
    }
}
