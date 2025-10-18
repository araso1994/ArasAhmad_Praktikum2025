using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WuppertalMapMvc.Data;
using WuppertalMapMvc.Models;

namespace WuppertalMapMvc.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HaltestellenNetzController : ControllerBase
	{
		private readonly AppDbContext _context;

		public HaltestellenNetzController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var stops = await _context.Stops.ToListAsync();
			var connections = await _context.StopConnections.ToListAsync();

			return Ok(new StopNetworkResult
			{
				Stops = stops,
				Connections = connections
			});
		}
	}
}
