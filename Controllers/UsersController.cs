using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FinansalYonetimWebApi.Models;
using FinansalYonetimWebApi.Data;
using FinansalYonetimWebApi.Dtos;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public UserController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{userID}/namesurname")]
    public async Task<ActionResult<object>> GetNameSurnameById(Guid userID)
    {
        try
        {
            var user = await _dbContext.Users
                .Where(u => u.Id == userID)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var responseData = new
            {
                Name = user.Name,
                Surname = user.Surname
            };

            return Ok(responseData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST api/user/create
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(User user)
    {
        try
        {
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Surname) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "Name, surname, email, and password are required." });
            }

            // email validation
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User already exists with this email." });
            }

            // create user
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return StatusCode(201, new { message = "User created successfully." });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error creating user: {ex.Message}");
            return StatusCode(500, new { message = "Error creating user." });
        }
    }

    // GET api/user/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        try
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving user: {ex.Message}");
            return StatusCode(500, new { message = "Error retrieving user." });
        }
    }

    // PUT api/user/update/{id}
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, User updatedUser)
    {
        try
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            user.Name = updatedUser.Name;
            user.Surname = updatedUser.Surname;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;
            user.Address = updatedUser.Address;
            user.Phone = updatedUser.Phone;
            user.Balance = updatedUser.Balance;
            user.Role = updatedUser.Role;
            user.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "User updated successfully." });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error updating user: {ex.Message}");
            return StatusCode(500, new { message = "Error updating user." });
        }
    }

    // DELETE api/user/delete/{id}
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully." });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error deleting user: {ex.Message}");
            return StatusCode(500, new { message = "Error deleting user." });
        }
    }

    // POST api/user/login
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(UserLoginDTO userLogin)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // find user by email
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userLogin.Email);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // is password correct?
            if (!user.ComparePassword(userLogin.Password))
            {
                return Unauthorized(new { message = "Invalid password." });
            }

            var userID = user.Id;
            // JWT token generationt but dummy secret key
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authentication");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // token in HTTPOnly cookie
            Response.Cookies.Append("token", tokenString, new CookieOptions
            {
                HttpOnly = true
            });
            Response.Cookies.Append("userId", userID.ToString(), new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new { token = tokenString, userId = user.Id, message = "Login successful." });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error logging in user: {ex.Message}");
            return StatusCode(500, new { message = "Error logging in user." });
        }
    }


    // POST api/user/logout
    [HttpPost("logout")]
    public IActionResult LogoutUser()
    {
        try
        {
            // clear token cookie
            Response.Cookies.Delete("token");

            return Ok(new { message = "Logout successful." });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error logging out user: {ex.Message}");
            return StatusCode(500, new { message = "Error logging out user." });
        }
    }

    // GET api/user/getaccounts/{userid}
    [HttpGet("getaccounts/{userid}")]
    public async Task<IActionResult> GetAccounts(Guid userid)
    {
        try
        {
            var userAccounts = await _dbContext.Accounts.Where(a => a.UserID == userid).ToListAsync();

            var responseData = userAccounts.Select(account => new
            {
                account.AccountID,
                account.AccountName,
                account.AccountType,
                account.Iban,
                Balance = account.Balance,
                AvailableBalance = account.AvailableBalance,
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

    // GET api/user/getaccountbyid/{userid}/{accountid}
    [HttpGet("getaccountbyid/{userid}/{accountid}")]
    public async Task<IActionResult> GetAccountById(Guid userid, int accountid)
    {
        try
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountID == accountid && a.UserID == userid);

            if (account == null)
            {
                return UnprocessableEntity(new { message = "The specified account ID does not exist." });
            }

            var responseData = new
            {
                account.AccountName,
                account.AccountType,
                account.Iban,
                Balance = account.Balance,
                AvailableBalance = account.AvailableBalance,
                account.Currency,
                account.UserID,
                account.OpenDate
            };

            return Ok(new { message = "success", data = responseData });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving account by ID: {ex.Message}");
            return StatusCode(500, new { message = "An error occurred while retrieving account information." });
        }
    }


}
