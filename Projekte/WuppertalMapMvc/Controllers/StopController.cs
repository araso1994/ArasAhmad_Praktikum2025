using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WuppertalMapMvc.Data;
using WuppertalMapMvc.Models;
using System.Linq;
using System.Threading.Tasks;

namespace WuppertalMapMvc.Controllers
{
    public class StopController : Controller
    {
        private readonly AppDbContext _context;

        public StopController(AppDbContext context)
        {
            _context = context;
        }

        // Übersicht
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stops.ToListAsync());
        }

        // Neue Haltestelle – Formular
        public IActionResult Create()
        {
            return View();
        }

        // Neue Haltestelle – speichern
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Stop stop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stop);
        }

        // Haltestelle bearbeiten – Formular
        public async Task<IActionResult> Edit(int id)
        {
            var stop = await _context.Stops.FindAsync(id);
            if (stop == null) return NotFound();
            return View(stop);
        }

        // Haltestelle bearbeiten – speichern
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Stop stop)
        {
            if (id != stop.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(stop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stop);
        }

        // Haltestelle löschen – Bestätigung
        public async Task<IActionResult> Delete(int id)
        {
            var stop = await _context.Stops.FindAsync(id);
            if (stop == null) return NotFound();
            return View(stop);
        }

        // Haltestelle löschen – ausführen
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stop = await _context.Stops.FindAsync(id);
            if (stop != null)
            {
                _context.Stops.Remove(stop);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // HALTESTELLE ZWISCHEN ZWEI ANDEREN EINFÜGEN
        [HttpGet]
        public IActionResult InsertBetween()
        {
            var stops = _context.Stops.ToList();

            var viewModel = new InsertBetweenViewModel
            {
                StopList = stops.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertBetween(InsertBetweenViewModel model)
        {

            
            if (!ModelState.IsValid)
            {
                model.StopList = _context.Stops
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    }).ToList();

                return View(model);
            }

            double minLng = 7.12, maxLng = 7.22;
            double minLat = 51.23, maxLat = 51.28;
            int svgWidth = 800, svgHeight = 500;

            int x = (int)((model.Longitude - minLng) / (maxLng - minLng) * svgWidth);
            int y = (int)((1 - (model.Latitude - minLat) / (maxLat - minLat)) * svgHeight);


            var stop = new Stop
            {
                Name = model.Name,
                Lines = model.Lines.Split(',').Select(l => l.Trim()).ToArray(),
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                X =x,
                Y =y
            };

            _context.Stops.Add(stop);
            await _context.SaveChangesAsync();

            // Neue Verbindungen erstellen
            _context.Connections.Add(new Connection
            {
                FromStopId = model.StartStopId,
                ToStopId = stop.Id,
                Line = "zwischen"
            });

            _context.Connections.Add(new Connection
            {
                FromStopId = stop.Id,
                ToStopId = model.EndStopId,
                Line = "zwischen"
            });

            // alte Direktverbindung löschen
            var existing = await _context.Connections
                .FirstOrDefaultAsync(c =>
                    c.FromStopId == model.StartStopId &&
                    c.ToStopId == model.EndStopId);

            if (existing != null)
            {
                _context.Connections.Remove(existing);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
