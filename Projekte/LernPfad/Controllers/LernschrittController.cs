using LernPfad.Models;
using LernPfad.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;


public class LernschrittController : Controller
{
    private readonly ApplicationDbContext _context;

    public LernschrittController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int lernpfadId)
    {
        var schritte = await _context.Lernschritte
            .Where(s => s.LernpfadId == lernpfadId)
            .OrderBy(s => s.Reihenfolge)
            .ToListAsync();

        ViewBag.LernpfadId = lernpfadId;
        return View(schritte);
    }

    public IActionResult Create(int lernpfadId)
    {
        var schritt = new Lernschritt { LernpfadId = lernpfadId };
        return View(schritt);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Lernschritt schritt)
    {
        if (ModelState.IsValid)
        {
            _context.Add(schritt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { lernpfadId = schritt.LernpfadId });
        }

        return View(schritt);
    }
}
