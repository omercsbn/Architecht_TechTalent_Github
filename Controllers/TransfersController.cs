// Controllers/TransfersController.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinansalYonetimWebApi.Data;
using FinansalYonetimWebApi.Models;

namespace FinansalYonetimWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransfersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Transfers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transfer>>> GetTransfers()
        {
            return await _context.Transfers.ToListAsync();
        }

        // GET: api/Transfers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transfer>> GetTransfer(int id)
        {
            var transfer = await _context.Transfers.FindAsync(id);

            if (transfer == null)
            {
                return NotFound();
            }

            return transfer;
        }

        // POST: api/Transfers
        [HttpPost]
        public async Task<ActionResult<Transfer>> PostTransfer(Transfer transfer)
        {
            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransfer), new { id = transfer.TransferId }, transfer);
        }

        // PUT: api/Transfers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransfer(int id, Transfer transfer)
        {
            if (id != transfer.TransferId)
            {
                return BadRequest();
            }

            _context.Entry(transfer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransferExists(id))
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

        // DELETE: api/Transfers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransfer(int id)
        {
            var transfer = await _context.Transfers.FindAsync(id);
            if (transfer == null)
            {
                return NotFound();
            }

            _context.Transfers.Remove(transfer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransferExists(int id)
        {
            return _context.Transfers.Any(e => e.TransferId == id);
        }
    }
}
