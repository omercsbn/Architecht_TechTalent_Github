using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using FinansalYonetimWebApi.Models;
using Microsoft.EntityFrameworkCore; // Altin modelini doğru namespace'e göre değiştirin
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

namespace FinansalYonetimWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AltinController : ControllerBase
    {
        private readonly AppDbContext _context; // DbContext adınızla değiştirin

        public AltinController(AppDbContext context)
        {
            _context = context;
        }

        // Tüm altın verilerini getiren bir endpoint
        [HttpGet]
        public IActionResult GetAllAltin()
        {
            try
            {
                var altinlar = _context.altins.ToList(); // Altin modelinin doğru şekilde kullanıldığından emin olun
                return Ok(altinlar);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Server error" });
            }
        }

        // Belirli bir altın verisini getiren bir endpoint
        [HttpGet("{altin_id}")]
        public async Task<IActionResult> GetAltinById(int altin_id)
        {
            try
            {
                var altin = await _context.altins.FindAsync(altin_id);
                if (altin == null)
                {
                    return NotFound(new { error = "Altin not found" });
                }
                return Ok(altin);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Server error" });
            }
        }
    }
}
