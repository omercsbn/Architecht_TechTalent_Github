using FinansalYonetimWebApi.Models;

public interface IInvoiceService
{
    Task<int> CreateInvoiceAsync(InvoiceDto invoiceDto);
    Task<List<InvoiceDto>> GetInvoicesAsync();
    Task MarkInvoiceAsPaidAsync(string invoiceNumber);
    Task<int> GetTotalInvoiceNumberAsync();
    Task<int> GetTotalInvoiceCountAsync();
}
