using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinansalYonetimWebApi.Models;
using FinansalYonetimWebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace FinansalYonetimWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FirmsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FirmsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/firms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Firm>>> GetAllFirms()
        {
            try
            {
                var firms = await _context.Firms.ToListAsync();
                return Ok(firms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/firms/filter
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<Firm>>> GetFilteredFirms([FromBody] FilterModel filterModel)
        {
            try
            {
                var query = _context.Firms.AsQueryable();

                if (filterModel.Id.HasValue)
                {
                    query = query.Where(f => f.id == filterModel.Id);
                }

                if (filterModel.SubscriptionId.HasValue)
                {
                    query = query.Where(f => f.subscription_id == filterModel.SubscriptionId);
                }

                var filteredFirms = await query.ToListAsync();
                return Ok(filteredFirms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        public class FilterModel
        {
            public int? Id { get; set; }
            public int? SubscriptionId { get; set; }
        }
    }
}
