using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinansalYonetimWebApi.Data;
using FinansalYonetimWebApi.Models;
using FinansalYonetimWebApi.Dtos;

namespace FinansalYonetimWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions(Guid? userID)
        {
            IQueryable<Transaction> query = _context.Transactions;

            if (userID.HasValue)
            {
                query = query.Where(t => t.UserID == userID);
            }

            var transactions = await query
                .Select(t => new TransactionDto
                {
                    TransactionID = t.TransactionID,
                    Description = t.Description,
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    TransactionType = t.TransactionType,
                    IsHide = t.IsHide,
                    UserID = t.UserID,
                    SenderAccountID = t.SenderAccountID,
                    ReceiverAccountID = t.ReceiverAccountID
                })
                .ToListAsync();

            return transactions;
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // GET: api/Transactions/filter?field=value
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Transaction>>> FilterTransactions(string filterType, string filterValue)
        {
            IQueryable<Transaction> query = _context.Transactions;

            switch (filterType.ToLower())
            {
                case "senderaccountıd":
                    query = query.Where(t => t.SenderAccountID.ToString() == filterValue);
                    break;
                case "receiveraccountıd":
                    query = query.Where(t => t.ReceiverAccountID.ToString() == filterValue);
                    break;
                case "transactiontype":
                    query = query.Where(t => t.TransactionType == filterValue);
                    break;
                default:
                    return BadRequest("Invalid field for filtering transactions.");
            }

            var transactions = await query.ToListAsync();
            return transactions;
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionID }, transaction); 
        }

        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.TransactionID)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionID == id);
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

        // GET: api/transactions/weekly
        [HttpGet("weekly")]
        public async Task<ActionResult<IEnumerable<decimal>>> GetWeeklyTransactions()
        {
            try
            {
                Guid userID = GetUserIDFromCookie(Request.Headers["Cookie"]);

                // get weekly transactions
                DateTime today = DateTime.Now;
                List<decimal> weeklyTransactions = new List<decimal>();

                // fetch transactions for the last 7 days
                for (int i = 0; i < 7; i++)
                {
                    DateTime date = today.AddDays(-i);
                    string formattedDate = date.ToString("yyyy-MM-dd");

                    // get user accounts
                    var userAccounts = await _context.Accounts
                        .Where(a => a.UserID == userID)
                        .Select(a => a.AccountID)
                        .ToListAsync();

                    // fetch daily transactions
                    var dailyTransactions = await _context.Transactions
                        .Where(t => userAccounts.Contains(t.SenderAccountID) || userAccounts.Contains(t.ReceiverAccountID))
                        .Where(t => EF.Functions.DateDiffDay(t.TransactionDate, date) == 0)
                        .ToListAsync();

                    // calculate daily total
                    decimal dailyTotal = 0;
                    foreach (var transaction in dailyTransactions)
                    {
                        if (transaction.SenderAccountID.ToString() == userID.ToString())
                        {
                            dailyTotal -= transaction.Amount;
                        }
                        else
                        {
                            dailyTotal += transaction.Amount;
                        }
                    }

                    // sum of daily transactions
                    weeklyTransactions.Insert(0, dailyTotal);
                }

                return weeklyTransactions;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error fetching weekly transactions: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("totalTransactionCount")]
        public async Task<ActionResult<int>> GetTotalTransactionCount()
        {
            try
            {
                Guid userID = GetUserIDFromCookie(Request.Headers["Cookie"]);

                if (userID == null)
                {
                    return BadRequest("User ID not found in cookies.");
                }

                // get user accounts
                var userAccounts = await _context.Accounts
                    .Where(a => a.UserID == userID)
                    .Select(a => a.AccountID)
                    .ToListAsync();

                int totalTransactionCount = 0;

                // for each account, get the transaction count
                foreach (var accountId in userAccounts)
                {
                    var transactionCount = await _context.Transactions
                        .Where(t => t.SenderAccountID == accountId || t.ReceiverAccountID == accountId)
                        .CountAsync();

                    totalTransactionCount += transactionCount;
                }

                return Ok(totalTransactionCount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // PUT: api/Transactions/update
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTransaction(TransactionDto updatedTransactionDto)
        {
            try
            {
                // find the existing transaction
                var existingTransaction = await _context.Transactions.FindAsync(updatedTransactionDto.TransactionID);

                if (existingTransaction == null)
                {
                    return NotFound("Transaction not found");
                }

                // update the transactions properties
                existingTransaction.SenderAccountID = updatedTransactionDto.SenderAccountID;
                existingTransaction.ReceiverAccountID = updatedTransactionDto.ReceiverAccountID;
                existingTransaction.Amount = updatedTransactionDto.Amount;
                existingTransaction.TransactionType = updatedTransactionDto.TransactionType;
                existingTransaction.Description = updatedTransactionDto.Description;
                existingTransaction.TransactionDate = updatedTransactionDto.TransactionDate;
                existingTransaction.IsHide = updatedTransactionDto.IsHide;

                // save to the database
                _context.Entry(existingTransaction).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating transaction: {ex.Message}");
            }
        }


    }
}
