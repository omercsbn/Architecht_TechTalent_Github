using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinansalYonetimWebApi.Models;
using FinansalYonetimWebApi.Data;
using FinansalYonetimWebApi.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Numerics;

namespace FinansalYonetimWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<InvoicesController> _logger;

        public InvoicesController(AppDbContext context, ILogger<InvoicesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/invoices
        [HttpGet]
        public ActionResult<IEnumerable<Invoice>> GetInvoices(Guid userId)
        {
            var invoices = _context.invoices.Where(i => i.userID == userId).ToList();
            return Ok(invoices);
        }

        // POST: api/invoices
        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice(Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Log the invoice data
            _logger.LogInformation("Creating invoice with data: {@Invoice}", invoice);

            _context.invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.invoiceID }, invoice);
        }

        // PUT: api/invoices/{id}/mark-paid
        [HttpPut("{id}")]
        public async Task<IActionResult> MarkInvoiceAsPaid(int id)
        {
            var invoice = await _context.invoices.FindAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            var userAccount = await _context.Accounts.FindAsync(Int32.Parse(invoice.senderAccountID));

            if (userAccount == null)
            {
                return NotFound("Sender account not found");
            }

            if (string.IsNullOrEmpty(invoice.receiverAccountID))
            {
                if (!invoice.receiverAccountID.StartsWith("TR")) // Assuming "TR" indicates an IBAN
                {
                    var firm = await _context.Firms.FirstOrDefaultAsync(f => f.company_name == invoice.receiverAccountID);

                    if (firm == null)
                    {
                        return NotFound("Firm not found");
                    }

                    // Deduct amount from user account
                    if (userAccount.Balance < invoice.amount)
                    {
                        return BadRequest("Insufficient funds");
                    }

                    userAccount.Balance -= invoice.amount;

                    // Mark the firm as paid if necessary
                    firm.payment_status = "Paid";

                    _context.Firms.Update(firm);
                }
                else if (int.TryParse(invoice.receiverAccountID, out int receiverAccountID))
                {
                    var receiverAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountID == receiverAccountID);

                    if (receiverAccount == null)
                    {
                        return NotFound("Receiver account not found or IBAN does not match");
                    }

                    // Deduct amount from user account
                    if (userAccount.Balance < invoice.amount)
                    {
                        return BadRequest("Insufficient funds");
                    }

                    userAccount.Balance -= invoice.amount;

                    // Add amount to receiver account
                    receiverAccount.Balance += invoice.amount;

                    _context.Accounts.Update(receiverAccount);
                }
                else
                {
                    return BadRequest("Invalid receiver account ID format");
                }
            }

            // Mark the invoice as paid
            invoice.paid = true;
            invoice.UpdatedAt = DateTime.UtcNow;

            _context.invoices.Update(invoice);
            _context.Accounts.Update(userAccount);

            await _context.SaveChangesAsync();

            return Ok(new { id = invoice.invoiceID, paid = invoice.paid });
        }



        // GET: api/invoices/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoiceById(int id)
        {
            var invoice = await _context.invoices.FindAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }

        // GET: api/invoices/totalcount
        [HttpGet("totalcount")]
        public ActionResult<int> GetTotalInvoiceCount()
        {
            var totalInvoiceCount = _context.invoices.Count();
            return Ok(totalInvoiceCount);
        }
        [HttpGet("account/{iban}")]
        public async Task<object> GetAccountByIBAN(string iban)
        {
            try
            {
                // retreive account information by iban from the database
                var account = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.Iban == iban);

                // account not found return null
                if (account == null)
                {
                    return null;
                }

                // if user found, return the account and user information
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == account.UserID);

                // user not found return null
                if (user == null)
                {
                    return null;
                }

                // if account is found, return the account and user information
                var responseData = new
                {
                    name = user.Name,
                    surname = user.Surname,
                    receiverAccountID = account.AccountID
                };

                return responseData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/invoices/totalcount
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalInvoiceCount(Guid userId)
        {
            try
            {
                Guid userID = GetUserIDFromCookie(Request.Headers["Cookie"]);
                // get total invoice count for the user
                var totalInvoiceCount = await _context.invoices
                    .Where(i => i.userID == userId)
                    .CountAsync();

                return Ok(totalInvoiceCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private Guid GetUserIDFromCookie(string cookie)
        {
            if (string.IsNullOrWhiteSpace(cookie))
            {
                throw new Exception("Cookie is empty.");
            }

            var cookieParts = cookie.Split(';');
            foreach (var part in cookieParts)
            {
                var keyValuePair = part.Split('=');
                if (keyValuePair.Length == 2 && keyValuePair[0].Trim() == "userId")
                {
                    if (Guid.TryParse(keyValuePair[1].Trim(), out Guid userId))
                    {
                        return userId;
                    }
                    else
                    {
                        throw new Exception("Invalid format for userID in cookie.");
                    }
                }
            }

            throw new Exception("UserID not found in cookie.");
        }
    }
}