using FinansalYonetimWebApi.Data;
using FinansalYonetimWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FinansalYonetimWebApi.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;

        public InvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateInvoiceAsync(InvoiceDto invoiceDto)
        {
            var newInvoice = new Invoice
            {
                userID = invoiceDto.userID,
                senderAccountID = invoiceDto.senderAccountID,
                receiverAccountID = invoiceDto.receiverAccountID,
                receiverName = invoiceDto.receiverName,
                transactionType = invoiceDto.transactionType,
                description = invoiceDto.description,
                amount = invoiceDto.amount,
                invoiceDate = invoiceDto.invoiceDate,
                dueDate = invoiceDto.dueDate,
                paid = false, // Assuming initially unpaid
                date = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.invoices.Add(newInvoice);
            await _context.SaveChangesAsync();

            return newInvoice.invoiceID;
        }

        public async Task<List<InvoiceDto>> GetInvoicesAsync()
        {
            var invoices = await _context.invoices.ToListAsync();

            return invoices.Select(i => new InvoiceDto
            {
                userID = i.userID,
                senderAccountID = i.senderAccountID,
                receiverAccountID = i.receiverAccountID,
                receiverName = i.receiverName,
                transactionType = i.transactionType,
                description = i.description,
                amount = i.amount,
                invoiceDate = i.invoiceDate,
                dueDate = i.dueDate,
                paid = i.paid,
            }).ToList();
        }

        public async Task MarkInvoiceAsPaidAsync(string invoiceNumber)
        {
            var invoice = await _context.invoices.FirstOrDefaultAsync(i => i.invoiceID.ToString() == invoiceNumber);
            if (invoice != null)
            {
                invoice.paid = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalInvoiceNumberAsync()
        {
            var totalInvoices = await _context.invoices.CountAsync();
            return totalInvoices + 1;
        }

        public async Task<int> GetTotalInvoiceCountAsync()
        {
            var totalInvoices = await _context.invoices.CountAsync();
            return totalInvoices;
        }
    }
}
