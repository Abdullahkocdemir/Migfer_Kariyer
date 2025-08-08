using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;

namespace Migfer_Kariyer.Controllers
{
    public class FeatureController : Controller
    {
        private readonly ApplicationContext _context;

        public FeatureController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Features.OrderBy(f => f.Name).ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feature);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Özellik eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(feature);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var feature = await _context.Features.FindAsync(id);
            if (feature == null) return NotFound();
            return View(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Feature feature)
        {
            if (id != feature.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(feature);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Özellik güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var feature = await _context.Features.FindAsync(id);
            if (feature != null)
            {
                _context.Features.Remove(feature);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Özellik silindi.";
            }
            return RedirectToAction(nameof(Index));
        }
    }

    public class SpecializationController : Controller
    {
        private readonly ApplicationContext _context;

        public SpecializationController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.FieldofSpecializations.OrderBy(s => s.Name).ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] FieldofSpecialization specialization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specialization);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Uzmanlık alanı eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(specialization);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var spec = await _context.FieldofSpecializations.FindAsync(id);
            if (spec == null) return NotFound();
            return View(spec);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FieldofSpecializationId,Name")] FieldofSpecialization specialization)
        {
            if (id != specialization.FieldofSpecializationId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(specialization);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Uzmanlık alanı güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(specialization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var spec = await _context.FieldofSpecializations.FindAsync(id);
            if (spec != null)
            {
                _context.FieldofSpecializations.Remove(spec);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Uzmanlık alanı silindi.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
