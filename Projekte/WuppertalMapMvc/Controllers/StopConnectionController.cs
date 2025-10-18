
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WuppertalMapMvc.Data;
using WuppertalMapMvc.Models;

namespace WuppertalMapMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StopConnectionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StopConnectionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetConnections()
        {
            var connections = await _context.StopConnections
                .Select(sc => new {
                    fromStopId = sc.FromStopId,
                    toStopId = sc.ToStopId
                })
                .ToListAsync();

            return Ok(connections);
        }
    }
}
