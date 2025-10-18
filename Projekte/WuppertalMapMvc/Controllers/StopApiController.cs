using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WuppertalMapMvc.Data;
using WuppertalMapMvc.Models;



[Route("api/[controller]")]
[ApiController]
public class StopApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public StopApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetStops()
    {
        var stops = await _context.Stops
            .Where(s => !s.IsDeleted)
            .Select(s => new
            {
                s.Id,
                Name = s.Name,
                Lines = s.Lines,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Description = s.Description,
                Image = s.ImagePath, 
                OpeningHours = s.OpeningHours
            })
            .ToListAsync();

        return Ok(new { stops });
    }

    [HttpPost]
    public async Task<IActionResult> CreateStop([FromBody] Stop stop)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Stops.Add(stop);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStops), new { id = stop.Id }, stop);
    }



}


