using LernPfad.Models;
using LernPfad.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace LernPfad.Controllers
{
    public class LernpfadController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;


        public LernpfadController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(string? suchtext, string? autor)
        {
            var query = _context.Lernpfade.AsQueryable();

            // Testausgabe für Debug-Zwecke (optional)
            var test = _context.Lernpfade.Any(lp => lp.Id == 1);
            Console.WriteLine("Lernpfad mit ID=1 vorhanden: " + test);

            // Suchtext filtert nach Titel
            if (!string.IsNullOrEmpty(suchtext))
            {
                query = query.Where(lp => lp.Titel.Contains(suchtext));
            }

            // Autor-Filter
            if (!string.IsNullOrEmpty(autor))
            {
                query = query.Where(lp => lp.AutorId == autor);
            }

            // Für das Dropdown: Alle Autorennamen
            var autoren = await _context.Lernpfade
                .Select(lp => lp.AutorId)
                .Distinct()
                .ToListAsync();

            // Übergabe an die View
            ViewBag.Autoren = autoren;
            ViewBag.Suchtext = suchtext;
            ViewBag.AktiverAutor = autor;

            return View(await query.ToListAsync());
        }


        public IActionResult Create()
        {
            Console.WriteLine("Create-Methode wurde aufgerufen!");
            Console.WriteLine("ModelState valid: " + ModelState.IsValid);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lernpfad lernpfad, IFormFile? BildDatei)
        {
            if (ModelState.IsValid)
            {
                if (BildDatei != null && BildDatei.Length > 0)
                {
                    var wwwRootPath = _env.WebRootPath;
                    var uploadsFolder = Path.Combine(wwwRootPath, "uploads");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(BildDatei.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await BildDatei.CopyToAsync(fileStream);
                    }

                    lernpfad.BildPfad = "/uploads/" + uniqueName;
                }

                _context.Add(lernpfad);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(lernpfad);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var lernpfad = await _context.Lernpfade.FindAsync(id);
            if (lernpfad == null)
            {
                return NotFound();
            }
            return View(lernpfad);
        }

        public async Task<IActionResult> Details(int id)
        {
            var lernpfad = await _context.Lernpfade
                .Include(lp => lp.Schritte)
                .FirstOrDefaultAsync(lp => lp.Id == id);

            if (lernpfad == null)
            {
                return NotFound();
            }

            return View(lernpfad);
        }


        [HttpGet]
        public IActionResult CreateInteractive()
        {
            return View();
        }





        // GET: Lernpfad/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var lernpfad = await _context.Lernpfade.FindAsync(id);
            if (lernpfad == null)
            {
                return NotFound();
            }
            return View(lernpfad);
        }

        [Authorize]
        public async Task<IActionResult> Preview(int id = 1)
        {
            var lernpfad = await _context.Lernpfade
                .Include(lp => lp.Schritte)
                .FirstOrDefaultAsync(lp => lp.Id == id);

            if (lernpfad == null)
                return NotFound();

            return View(lernpfad);
        }





        // POST: Lernpfad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lernpfad = await _context.Lernpfade.FindAsync(id);
            if (lernpfad != null)
            {
                _context.Lernpfade.Remove(lernpfad);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lernpfad lernpfad, IFormFile? NeuesBild)
        {
            if (id != lernpfad.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var original = await _context.Lernpfade.FindAsync(id);
                    if (original == null)
                        return NotFound();

                    // 🔁 Wenn ein neues Bild hochgeladen wird → ersetzen
                    if (NeuesBild != null && NeuesBild.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        var uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(NeuesBild.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await NeuesBild.CopyToAsync(stream);
                        }

                        original.BildPfad = "/uploads/" + uniqueName;
                    }

                    // 🔁 Textdaten aktualisieren
                    original.Titel = lernpfad.Titel;
                    original.Beschreibung = lernpfad.Beschreibung;
                    original.AutorId = lernpfad.AutorId;

                    _context.Update(original);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Lernpfade.Any(e => e.Id == lernpfad.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(lernpfad);
        }

    }
}
