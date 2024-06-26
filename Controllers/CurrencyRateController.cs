using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using FinansalYonetimWebApi.Models;
using FinansalYonetimWebApi.Data;

namespace FinansalYonetimWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyRateController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CurrencyRateController(AppDbContext context)
        {
            _context = context;
        }

        // GET - Tüm döviz kurlarını getir
        [HttpGet]
        public IActionResult GetAllCurrencyRates()
        {
            try
            {
                var currencyRates = _context.CurrencyRates.ToList();
                return Ok(currencyRates);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
