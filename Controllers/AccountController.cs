using FinansalYonetimWebApi.Data;
using FinansalYonetimWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using FinansalYonetimWebApi.Data;
using FinansalYonetimWebApi.Models;
using FinansalYonetimWebApi.DTOs;
using Microsoft.Identity.Client;
using System.Text.Json;

namespace FinansalYonetimWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAccountByIBAN")]
        public async Task<ActionResult<object>> GetAccountByIBAN(string iban)
        {
            try
            {
                var account = await _context.Accounts
                    .Include(a => a.UserID)
                    .FirstOrDefaultAsync(a => a.Iban == iban);

                if (account == null)
                {
                    return NotFound("Account not found.");
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == account.UserID);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var responseData = new
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    ReceiverAccountID = account.AccountID
                };

                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{accountID}")]
        public async Task<ActionResult> DeleteAccountById(int accountID)
        {
            try
            {
                var account = await _context.Accounts.FindAsync(accountID);
                if (account == null)
                {
                    return NotFound("Account not found.");
                }

                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Account deleted successfully.", accountID });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PATCH api/account/{accountID}
        [HttpPatch("{accountID}")]
        public async Task<ActionResult> UpdateAccountName(int accountID, [FromBody] JsonElement accountNameElement)
        {
            try
            {
                if (accountNameElement.ValueKind != JsonValueKind.Object)
                {
                    return BadRequest("Invalid JSON format. Expected an object.");
                }

                // Extracting the value of the "accountName" key
                string accountName = accountNameElement.GetProperty("accountName").GetString();

                var account = await _context.Accounts.FindAsync(accountID);
                if (account == null)
                {
                    return NotFound("Account not found.");
                }

                account.AccountName = accountName;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Account name updated successfully.", accountID, accountName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PATCH api/account/{accountID}/updateBalance
        [HttpPatch("{accountID}/updateBalance")]
        public async Task<IActionResult> UpdateAccountBalance(int accountID, [FromBody] JsonElement balanceElement)
        {
            try
            {
                if (balanceElement.ValueKind != JsonValueKind.Object)
                {
                    return BadRequest("Invalid JSON format. Expected an object.");
                }

                // Extracting the value of the "balance" key
                decimal newBalance = balanceElement.GetProperty("balance").GetDecimal();

                var account = await _context.Accounts.FindAsync(accountID);
                if (account == null)
                {
                    return NotFound("Account not found.");
                }

                account.Balance = newBalance;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Account balance updated successfully.", accountID, newBalance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public async Task<bool> IsAccountIDUnique(int accountID)
        {
            var existingAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountID == accountID);

            return existingAccount == null;
        }

        public async Task<string> GenerateIBAN(int accountID)
        {
            const string countryCode = "TR";
            const string controlDigits = "96";
            const string bankCode = "12345";
            const string reserveArea = "0";

            // Generate a unique account ID
            int newAccountID = await GenerateUniqueAccountID();
            string accountIDString = newAccountID.ToString().PadLeft(16, '0');

            // Construct IBAN
            string iban = $"{countryCode}{controlDigits}{bankCode}{reserveArea}{accountIDString}";

            return iban;
        }

        private async Task<int> GenerateUniqueAccountID()
        {
            // Generate a new unique account ID (example implementation)
            Random random = new Random();
            int newAccountID = random.Next(100000000, 999999999); // Generate random ID (adjust as needed)

            // Check if this ID is already in use, if so, recursively call until unique
            while (await _context.Accounts.AnyAsync(a => a.AccountID == newAccountID))
            {
                newAccountID = random.Next(100000000, 999999999);
            }

            return newAccountID;
        }

        [HttpPost("createDepositAccount")]
        public async Task<ActionResult<int>> CreateDepositAccount(Account account)
        {
            try
            {
                Guid userID = GetUserIDFromCookie(Request.Headers["Cookie"]);

                // Generate unique IBAN
                string iban = await GenerateUniqueIBAN();
                account.UserID = userID;
                account.AccountType = "Deposit";
                account.OpenDate = DateTime.Now;
                account.Balance = 0;
                account.AvailableBalance = 0;
                account.Iban = iban;
                account.DepositOption = "1";

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateDepositAccount), new { accountID = account.AccountID });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("createGoldAccount")]
        public async Task<ActionResult<int>> CreateGoldAccount(Account account)
        {
            try
            {
                Guid userID = GetUserIDFromCookie(Request.Headers["Cookie"]);
                string iban = await GenerateUniqueIBAN();
                account.UserID = userID;
                account.AccountType = "Gold";
                account.OpenDate = DateTime.Now;
                account.Iban = iban;

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateGoldAccount), new { accountID = account.AccountID });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("createCheckingAccount")]
        public async Task<ActionResult<int>> CreateCheckingAccount(Account account)
        {
            try
            {
                Guid userID = GetUserIDFromCookie(Request.Headers["Cookie"]);
                string iban = await GenerateUniqueIBAN();
                account.UserID = userID;
                account.AccountType = "Checking";
                account.OpenDate = DateTime.Now;
                account.Balance = 0;
                account.AvailableBalance = 0;
                account.Iban = iban;
                account.DepositOption = "0";

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateCheckingAccount), new { accountID = account.AccountID });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        private async Task<string> GenerateUniqueIBAN()
        {
            const string countryCode = "TR";
            const string controlDigits = "96";
            const string bankCode = "12345";
            const string reserveArea = "0";

            // Generate a unique account ID
            int newAccountID = await GenerateUniqueAccountID();
            string accountIDString = newAccountID.ToString().PadLeft(16, '0');

            // Construct IBAN
            string iban = $"{countryCode}{controlDigits}{bankCode}{reserveArea}{accountIDString}";

            return iban;
        }

        // GET api/user/{userid}/accounts
        [HttpGet("{userid}/accounts")]
        public async Task<IActionResult> GetUserAccounts(Guid userid)
        {
            try
            {
                var userAccounts = await _context.Accounts
                    .Where(a => a.UserID == userid)
                    .ToListAsync();

                var responseData = userAccounts.Select(account => new
                {
                    account.AccountID,
                    account.AccountName,
                    account.AccountType,
                    account.Iban,
                    balance = account.Balance,
                    availableBalance = account.AvailableBalance,
                    account.Currency,
                    account.UserID,
                    account.OpenDate
                }).ToList();

                return Ok(new { message = "success", data = responseData });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving user accounts: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving account information." });
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

        [HttpGet("totalAccountCount")]
        public async Task<ActionResult<int>> GetTotalAccountCount()
        {
            try
            {
                Guid userID = GetUserIDFromCookie(Request.Headers["Cookie"]);

                if (userID == null)
                {
                    return BadRequest("User ID not found in cookies.");
                }

                // Kullanıcıya ait hesap sayısını al
                var totalAccountCount = await _context.Accounts
                    .Where(a => a.UserID == userID)
                    .CountAsync();

                return Ok(totalAccountCount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }


    }
}
